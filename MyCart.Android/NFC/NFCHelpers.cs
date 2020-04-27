using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DigiFyy.Models.NFC;

namespace DigiFyy.Droid.NFC
{
    public static class NFCHelpers
    {
        public static List<string>  GetRecords(NdefRecord[] records)
        {
            List<string> textMessages = new List<string>();
          
            for (var i = 0; i < records.Length; i++)
            {
                var record = records[i];
                if (record != null)
                {
                    var typeFormat = (NFCNdefTypeFormat)record.Tnf;
                    var uri = record.ToUri()?.ToString();
                    var mimeType = record.ToMimeType();
                    var payload = record.GetPayload();
                    string textmessage = NFCUtils.GetMessage(typeFormat, payload, uri);
                    if (string.IsNullOrEmpty(textmessage) == false)
                        textMessages.Add(textmessage);
              
                }
               
            }
            return textMessages;
        }

        public static bool IsNDEFTag(Tag tag)
        {
            bool returnVar = false;
            Ndef lNdefTag = Ndef.Get(tag);
            if (lNdefTag != null)
                returnVar = true;

            return returnVar;
        }

        public static string GetManufacturer(Tag tag)
        {
            string mifareInfo = "";

            string[] techtags = tag.GetTechList();
            foreach (string s in techtags)
            {

                if (s.ToLower().IndexOf("mifareultralight") != -1)
                    return "MifareUltralight";
                else if (s.ToLower().IndexOf("mifareclassic") != -1)
                {
                    mifareInfo = "Mifare Classic";
                    MifareClassic mifareClassicTag = MifareClassic.Get(tag); //Get MIFARE CLASSIC tag
                    if (mifareClassicTag != null)
                    {
                        if (mifareClassicTag.Type == MifareClassic.TypePlus)
                            mifareInfo = "Mifare  Plus";
                        else if (mifareClassicTag.Type == MifareClassic.TypePro)
                            mifareInfo = "Mifare  Pro";
                    }

                    return mifareInfo;
                }
                else if (s.ToLower().IndexOf("nfca") != -1)
                {
                    NfcA nfc_A = NfcA.Get(tag);
                    if (nfc_A != null)
                    {
                        byte[] atqa = nfc_A.GetAtqa();
                        if (atqa.Length >= 2)
                        {
                            Int32.TryParse(nfc_A.Sak.ToString(), out int Sak);
                            switch (atqa[1])
                            {
                                case 0x00:
                                    if (atqa[0] == 0x44 || atqa[0] == 0x42 || atqa[0] == 0x02 || atqa[0] == 0x04)
                                    {
                                        if (Sak == 0x20)
                                        {
                                            mifareInfo = "Mifare Plus SL3";
                                        }
                                        else if (Sak == 0x10)
                                        {
                                            mifareInfo = "Mifare Plus SL2";
                                        }

                                        else if (Sak == 0x00)
                                        {
                                            mifareInfo = "Mifare EM";
                                        }
                                    }
                                    break;
                                case 0x03:
                                    if (atqa[0] == 0x44 || atqa[0] == 0x4)
                                    {
                                        mifareInfo = "Mifare Desfire";
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else if (IsFelicaTag(tag))
                    mifareInfo = "Felica";

            }

            return mifareInfo;
        }

        public static bool IsFelicaTag(Tag tag)
        {
            NfcF mNfcF = NfcF.Get(tag);
            if (mNfcF == null)
                return false;

            byte[] mPMm = mNfcF.GetManufacturer();
            if (mPMm == null)
                return false;
            if (mPMm.Length < 2)
                return false;

            return ((mPMm[0] == 0x01 && mPMm[1] == 0x20) || (mPMm[0] == 0x03 && mPMm[1] == 0x32));
        }


    }
}