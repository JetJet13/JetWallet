using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Model
{
    public enum ConfigLanguage
    {
        English,
        French,
        Spanish,
        None // for testing purposes
    }

    public enum ConfigCurrency
    {
        USD,
        EUR,
        CAD,
        None // for testing purposes
    }

    public class ConfigFile
    {
        private string _walletpath;
        private string _colorscheme;
        private ConfigLanguage _language;
        private ConfigCurrency _currency;
        private string _cultcode;

        public string WalletPath
        {
            get { return _walletpath; }
            set { _walletpath = value; }
        }

        public string ColorScheme
        {
            get { return _colorscheme; }
            set { _colorscheme = value; }
        }

        public ConfigLanguage Language
        {
            get { return _language; }
            set { _language = value; SetCultureCode(); }
        }

        public ConfigCurrency Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public string CultCode
        {
            get { return _cultcode; }
            
        }

        public ConfigFile(string walletPath, string colorScheme, ConfigLanguage language, ConfigCurrency currency)
        {
            WalletPath = walletPath;
            ColorScheme = colorScheme;
            Language = language;
            Currency = currency;

        }

        public string GetSaveFormat()
        {
            return WalletPath
                 + Environment.NewLine
                 + ColorScheme
                 + Environment.NewLine
                 + Language.ToString()
                 + Environment.NewLine
                 + Currency.ToString();
        }

        private void SetCultureCode()
        {
            switch (Language)
            {
                case ConfigLanguage.English:
                    _cultcode = "en-US";
                    break;
                case ConfigLanguage.French:
                    _cultcode = "fr-FR";
                    break;
                case ConfigLanguage.Spanish:
                    _cultcode = "es-ES";
                    break;
                default:
                    _cultcode = "en-US";
                    break;
            }
        }
    }
}
