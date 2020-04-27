using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace DigiFyy.iOS.NFC
{
    public static class NdefMessageParser
    {
      

        public static void ParseTextPayload(byte[] payload, nuint length, out string langCode, out string text)
        {

            var isUTF16 = payload[0] & 0x80;
            var codeLength = payload[0] & 0x7f;
            langCode = "";
            text = "";

            if ((int)length < 1 + codeLength)
                return;

            langCode = (Foundation.NSString)NSData.FromStream(new MemoryStream(payload, 1, codeLength)).ToString();
            text = (Foundation.NSString)NSData.FromStream(new MemoryStream(payload, 1 + codeLength, (int)length - 1 - codeLength)).ToString(isUTF16 == 0 ? NSStringEncoding.UTF8 : NSStringEncoding.UTF16BigEndian);

        }

        public static void ParseUriPayload(byte[] payload, nuint length, out string text)
        {

            var code = payload[0];
            NSString originalText = (Foundation.NSString)NSData.FromStream(new MemoryStream(payload, 1, (int)length - 1)).ToString(NSStringEncoding.UTF8);
            text = (Foundation.NSString)(Utility.UriMap(code) + originalText);
            //switch (code)
            //{
            //    case 0x00:
            //        text = originalText;
            //        break;
            //    case 0x01:
            //        text = (Foundation.NSString)("http://www." + originalText);
            //        break;
            //    case 0x02:
            //        text = (Foundation.NSString)("https://www." + originalText);
            //        break;
            //    case 0x03:
            //        text = (Foundation.NSString)("http://" + originalText);
            //        break;
            //    default:
            //        text = originalText;
            //        break;
            //}
        }
    }


    public static class Utility
    {
        static Dictionary<byte, string> MAP = new Dictionary<byte, string>()
        {
            {(byte) 0x00, "" },
            {(byte)  0x01, "http://www."},
            {(byte)  0x02, "https://www."},
            {(byte)  0x03, "http://"},
            {(byte)  0x04, "https://"},
            {(byte)  0x05, "tel:"},
            {(byte)  0x06, "mailto:"},
            {(byte)  0x07, "ftp://anonymous:anonymous@"},
            {(byte)  0x08, "ftp://ftp."},
            {(byte)  0x09, "ftps://"},
            {(byte)  0x0A, "sftp://"},
            {(byte)  0x0B, "smb://"},
            {(byte)  0x0C, "nfs://"},
            {(byte)  0x0D, "ftp://"},
            {(byte)  0x0E, "dav://"},
            {(byte)  0x0F, "news:"},
            {(byte)  0x10, "telnet://"},
            {(byte)  0x11, "imap:"},
            {(byte)  0x12, "rtsp://"},
            {(byte)  0x13, "urn:"},
            {(byte)  0x14, "pop:"},
            {(byte)  0x15, "sip:"},
            {(byte)  0x16, "sips:"},
            {(byte)  0x17, "tftp:"},
            {(byte)  0x18, "btspp://"},
            {(byte)  0x19, "btl2cap://"},
            {(byte)  0x1A, "btgoep://"},
            {(byte)  0x1B, "tcpobex://"},
            {(byte)  0x1C, "irdaobex://"},
            {(byte)  0x1D, "file://"},
            {(byte)  0x1E, "urn:epc:id:"},
            {(byte)  0x1F, "urn:epc:tag:"},
            {(byte)  0x20, "urn:epc:pat:"},
            {(byte)  0x21, "urn:epc:raw:"},
            {(byte)  0x22, "urn:epc:"},
            {(byte)  0x23, "urn:nfc:"}
        };

        public static string UriMap(byte code)
        {
            MAP.TryGetValue(code, out string valore);
            return valore;
        }
    }
}