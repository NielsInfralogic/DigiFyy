using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CoreNFC;
using DigiFyy.Models.NFC;
using Foundation;
using UIKit;

namespace DigiFyy.iOS.NFC
{
	public static class NFCHelpers
	{
		/// <summary>
		/// Convert an iOS <see cref="NSData"/> into an array of bytes
		/// </summary>
		/// <param name="data">iOS <see cref="NSData"/></param>
		/// <returns>Array of bytes</returns>
		public static byte[] ToByteArray(this NSData data)
		{
			var bytes = new byte[data.Length];
			if (data.Length > 0) System.Runtime.InteropServices.Marshal.Copy(data.Bytes, bytes, 0, Convert.ToInt32(data.Length));
			return bytes;
		}

		/// <summary>
		/// Converte an iOS <see cref="NFCNdefMessage"/> into an array of bytes
		/// </summary>
		/// <param name="message">iOS <see cref="NFCNdefMessage"/></param>
		/// <returns>Array of bytes</returns>
		public static byte[] ToByteArray(this NFCNdefMessage message)
		{
			var records = message?.Records;

			// Empty message: single empty record
			if (records == null || records.Length == 0)
			{
				records = new NFCNdefPayload[] { null };
			}

			var m = new MemoryStream();
			for (var i = 0; i < records.Length; i++)
			{
				var record = records[i];
				var typeNameFormat = record?.TypeNameFormat ?? NFCTypeNameFormat.Empty;
				var payload = record?.Payload;
				var id = record?.Identifier;
				var type = record?.Type;

				var flags = (byte)typeNameFormat;

				// Message begin / end flags. If there is only one record in the message, both flags are set.
				if (i == 0)
					flags |= 0x80;      // MB (message begin = first record in the message)
				if (i == records.Length - 1)
					flags |= 0x40;      // ME (message end = last record in the message)

				// cf (chunked records) not supported yet

				// SR (Short Record)?
				if (payload == null || payload.Length < 255)
					flags |= 0x10;

				// ID present?
				if (id != null && id.Length > 0)
					flags |= 0x08;

				m.WriteByte(flags);

				// Type length
				if (type != null)
					m.WriteByte((byte)type.Length);
				else
					m.WriteByte(0);

				// Payload length 1 byte (SR) or 4 bytes
				if (payload == null)
				{
					m.WriteByte(0);
				}
				else
				{
					if ((flags & 0x10) != 0)
					{
						// SR
						m.WriteByte((byte)payload.Length);
					}
					else
					{
						// No SR (Short Record)
						var payloadLength = (uint)payload.Length;
						m.WriteByte((byte)(payloadLength >> 24));
						m.WriteByte((byte)(payloadLength >> 16));
						m.WriteByte((byte)(payloadLength >> 8));
						m.WriteByte((byte)(payloadLength & 0x000000ff));
					}
				}

				// ID length
				if (id != null && (flags & 0x08) != 0)
					m.WriteByte((byte)id.Length);

				// Type length
				if (type != null && type.Length > 0)
					m.Write(type.ToArray(), 0, (int)type.Length);

				// ID data
				if (id != null && id.Length > 0)
					m.Write(id.ToArray(), 0, (int)id.Length);

				// Payload data
				if (payload != null && payload.Length > 0)
					m.Write(payload.ToArray(), 0, (int)payload.Length);
			}

			return m.ToArray();
		}

		/// <summary>
		/// Returns complete URI of TNF_WELL_KNOWN, RTD_URI records.
		/// </summary>
		/// <returns><see cref="Uri"/></returns>
		private static Uri ParseWktUri(this NSData data)
		{
			var payload = data.ToByteArray();

			if (payload.Length < 2)
				return null;

			var prefixIndex = payload[0] & 0xFF;
			if (prefixIndex < 0 || prefixIndex >= _uri_Prefixes_Map.Length)
				return null;

			var prefix = _uri_Prefixes_Map[prefixIndex];
			var suffix = Encoding.UTF8.GetString(CopyOfRange(payload, 1, payload.Length));

			if (Uri.TryCreate(prefix + suffix, UriKind.Absolute, out var result))
				return result;

			return null;
		}

		/// <summary>
		/// Copy a range of an array into another array
		/// </summary>
		/// <param name="src">Array of <see cref="byte"/></param>
		/// <param name="start">Start</param>
		/// <param name="end">End</param>
		/// <returns>Array of <see cref="byte"/></returns>
		private static byte[] CopyOfRange(byte[] src, int start, int end)
		{
			var length = end - start;
			var dest = new byte[length];
			for (var i = 0; i < length; i++)
				dest[i] = src[start + i];
			return dest;
		}

		/// <summary>
		/// Returns NFC Tag identifier
		/// </summary>
		/// <param name="tag"><see cref="INFCNdefTag"/></param>
		/// <returns>Tag identifier</returns>
		public static byte[] GetTagIdentifier(INFCNdefTag tag, ref string technology)
		{
			byte[] identifier = null;
			if (tag is INFCMiFareTag mifareTag)
			{
				technology = "MiFare";
				identifier = mifareTag.Identifier.ToByteArray();
				NFCMiFareFamily f = mifareTag.GetMifareFamily();
				if (f == NFCMiFareFamily.Ultralight)
					technology = "MiFare Ultralight";
				else if (f == NFCMiFareFamily.Plus)
					technology = "MiFare Plus";
				else if (f == NFCMiFareFamily.DesFire)
					technology = "MiFare DesFire";
			}
			else if (tag is INFCFeliCaTag felicaTag)
			{
				technology = "Felica";
				identifier = felicaTag.CurrentIdm.ToByteArray();
			}
			else if (tag is INFCIso15693Tag iso15693Tag)
			{
				technology = "Iso15693";
				identifier = iso15693Tag.Identifier.ToByteArray();
			}
			else if (tag is INFCIso7816Tag iso7816Tag)
			{
				technology = "Iso7816";
				identifier = iso7816Tag.Identifier.ToByteArray();
			}


			return identifier;
		}


		/// <summary>
		/// Transforms an array of <see cref="NFCNdefPayload"/> into an array of <see cref="NFCNdefRecord"/>
		/// </summary>
		/// <param name="records">Array of <see cref="NFCNdefPayload"/></param>
		/// <returns>Array of <see cref="NFCNdefRecord"/></returns>
		public static List<string> GetRecords(NFCNdefPayload[] records)
		{
			List<string> textMessages = new List<string>();

			for (var i = 0; i < records.Length; i++)
			{
				var record = records[i];
				if (record != null)
				{
					var typeFormat = (NFCNdefTypeFormat)record.TypeNameFormat;
					var payload = ToByteArray(record.Payload);
					var mimeType = record.ToMimeType();
					var uri = record.ToUri()?.ToString();
					string textmessage = NFCUtils.GetMessage(typeFormat, payload, uri);
					if (string.IsNullOrEmpty(textmessage) == false)
						textMessages.Add(textmessage);

				}
			}

			return textMessages;
		}

		public static INFCNdefTag GetNdefTag(INFCTag tag)
		{
			if (tag == null /*|| !tag.Available*/)
				return null;
			NFCTagType tp = (NFCTagType)tag.Type;

			INFCNdefTag ndef;
			if (tag.GetNFCMiFareTag() != null)
				ndef = tag.GetNFCMiFareTag();
			else if (tag.GetNFCIso7816Tag() != null)
				ndef = tag.GetNFCIso7816Tag();
			else if (tag.GetNFCFeliCaTag() != null)
				ndef = tag.GetNFCFeliCaTag();
			else
				ndef = null;

			return ndef;
		}

		

		/// <summary>
		/// Transforms a string message into an array of bytes
		/// </summary>
		/// <param name="text">text message</param>
		/// <returns>Array of bytes</returns>
		public static byte[] EncodeToByteArray(string text) => Encoding.UTF8.GetBytes(text);



		/// <summary>
		/// Convert bytes array into hexadecimal string
		/// </summary>
		/// <param name="bytes">Bytes Array</param>
		/// <param name="separator">Separator</param>
		/// <returns>Hexadecimal string</returns>
		public static string ByteArrayToHexString(byte[] bytes, string separator = null)
		{
			return bytes == null ? string.Empty : string.Join(separator ?? string.Empty, bytes.Select(b => b.ToString("X2")));
		}


		public static string ToMimeType(this NFCNdefPayload payload)
		{
			switch (payload.TypeNameFormat)
			{
				case NFCTypeNameFormat.NFCWellKnown:
					if (payload.Type.ToString() == "T")
						return "text/plain";
					break;
				case NFCTypeNameFormat.Media:
					return payload.Type.ToString();
			}
			return null;
		}

		public static Uri ToUri(this NFCNdefPayload payload)
		{
			switch (payload.TypeNameFormat)
			{
				case NFCTypeNameFormat.NFCWellKnown:
					if (payload.Type.ToString() == "U")
					{
						var uri = payload.Payload.ParseWktUri();
						return uri;
					}
					break;
				case NFCTypeNameFormat.AbsoluteUri:
				case NFCTypeNameFormat.Media:
					var content = Encoding.UTF8.GetString(payload.Payload.ToByteArray());
					if (Uri.TryCreate(content, UriKind.RelativeOrAbsolute, out var result))
						return result;

					break;
			}
			return null;
		}

		private static readonly string[] _uri_Prefixes_Map = new string[] {
			"", // 0x00
            "http://www.", // 0x01
            "https://www.", // 0x02
            "http://", // 0x03
            "https://", // 0x04
            "tel:", // 0x05
            "mailto:", // 0x06
            "ftp://anonymous:anonymous@", // 0x07
            "ftp://ftp.", // 0x08
            "ftps://", // 0x09
            "sftp://", // 0x0A
            "smb://", // 0x0B
            "nfs://", // 0x0C
            "ftp://", // 0x0D
            "dav://", // 0x0E
            "news:", // 0x0F
            "telnet://", // 0x10
            "imap:", // 0x11
            "rtsp://", // 0x12
            "urn:", // 0x13
            "pop:", // 0x14
            "sip:", // 0x15
            "sips:", // 0x16
            "tftp:", // 0x17
            "btspp://", // 0x18
            "btl2cap://", // 0x19
            "btgoep://", // 0x1A
            "tcpobex://", // 0x1B
            "irdaobex://", // 0x1C
            "file://", // 0x1D
            "urn:epc:id:", // 0x1E
            "urn:epc:tag:", // 0x1F
            "urn:epc:pat:", // 0x20
            "urn:epc:raw:", // 0x21
            "urn:epc:", // 0x22
            "urn:nfc:", // 0x23
		};
	}
}