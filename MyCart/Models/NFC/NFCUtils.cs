using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigiFyy.Models.NFC
{
    public static class NFCUtils
    {
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

		public static string GetMessage(NFCNdefTypeFormat type, byte[] payload, string uri)
		{
			string message;
			if (!string.IsNullOrWhiteSpace(uri))
				message = uri;
			else
			{
				if (type == NFCNdefTypeFormat.WellKnown)
				{
					// NDEF_WELLKNOWN Text record
					var status = payload[0];
					var enc = status & 0x80;
					var languageCodeLength = status & 0x3F;
					if (enc == 0)
						message = Encoding.UTF8.GetString(payload, languageCodeLength + 1, payload.Length - languageCodeLength - 1);
					else
						message = Encoding.Unicode.GetString(payload, languageCodeLength + 1, payload.Length - languageCodeLength - 1);
				}
				else
				{
					// Other NDEF types
					message = Encoding.UTF8.GetString(payload, 0, payload.Length);
				}
			}
			return message;
		}
	}
}
