using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Model;
using JetWallet.Resources;
using System.Globalization;

namespace JetWallet.Tools
{
    public class JetTextTools
    {
        public static string RetrieveStringFromResource(string id)
        {
            string NotFound = "NA";
            switch (JetGlobal.CFM.GetConfigLanguage())
            {
                case ConfigLanguage.English:
                    string eng = EnglishStringResource.ResourceManager.GetString(id);
                    if (eng == null) return NotFound;
                    return eng;
                default:
                    return "Language Not Set";

            }
        }

        public static string Base58Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Base58Check.Base58CheckEncoding.Encode(plainTextBytes);
        }

        public static string Base58Decode(string base58Encoded)
        {
            var base58EncodedBytes = Base58Check.Base58CheckEncoding.Decode(base58Encoded);
            return System.Text.Encoding.UTF8.GetString(base58EncodedBytes);
        }

        public static string FormatFullDate(DateTime date, string cultCode)
        {
            TimeZoneInfo local = TimeZoneInfo.Local;
            CultureInfo cult = new CultureInfo(cultCode);
            return TimeZoneInfo.ConvertTime(date, local).ToString("dd MMMM yyyy hh:mm tt", cult);
        }
    }
}
