using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using JetWallet_v1.Resources;
using JetWallet_v1.Model;

namespace JetWallet_v1.Tools
{
    class TextTools
    {
        public static string RetrieveStringFromResource(string id)
        {
            string NotFound = "NA";
            switch (Global.VML.Language.ActiveLanguage)
            {
                case "English":
                    string eng = EnglishStringResource.ResourceManager.GetString(id);
                    if (eng == null) return NotFound;
                    return eng;
                default:
                    return "Language Not Set";

            }

        }

        public static string DecodeWalletName(string path)
        {
            // the wallet folder name is decoded to get the wallet displayname
            string[] directories = path.Split('\\');           
            string walletfile = directories[directories.Length - 2];

            // Decode walletName to readable text
            string name = Base58Decode(walletfile);
            return name;
        }

        public static string Base58Encode(string plainText)
        {           
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Base58Check.Base58CheckEncoding.Encode(plainTextBytes);
        }

        public static string Base58Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Base58Check.Base58CheckEncoding.Decode(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string FormatFullDate(DateTimeOffset date)
        {
            TimeZoneInfo local = TimeZoneInfo.Local;
            CultureInfo cult = new CultureInfo(GetCultureCode());
            return TimeZoneInfo.ConvertTime(date,local).ToString("dd MMMM yyyy hh:mm tt", cult);
        }

        public static string GetActionString(TxAction action)
        {
            if (action == TxAction.Sent)
            {
                return TextTools.RetrieveStringFromResource("Sent");
            }
            else if (action == TxAction.Received)
            {
                return TextTools.RetrieveStringFromResource("Received");
            }
            else
            {
                return "NA";
            }
        }

        
        public static string GetStateString(TxState state)
        {
            if (state == TxState.Confirmed)
            {
                return TextTools.RetrieveStringFromResource("Confirmed");
            }
            else if (state == TxState.Awaiting)
            {
                return TextTools.RetrieveStringFromResource("Awaiting");
            }
            else if (state == TxState.Unconfirmed)
            {
                return TextTools.RetrieveStringFromResource("Unconfirmed");
            }
            else
            {
                return "NA";
            }
        }

        public static string GetCultureCode()
        {
            switch (Global.VML.Language.ActiveLanguage)
            {
                case "English":
                    return "en-US";
                default:
                    return "en-US";
            }
        }
    }
}
