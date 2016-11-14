using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Tools;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;

namespace JetWallet.Model
{
    public class ConfigFileManager
    {
        private ConfigFile _config;
        private ConfigFileTools _tools;


        public ConfigFileManager()
        {
            Messenger.Default.Register<string>(this, "SetConfigWalletPath", (string s) => { _config.WalletPath = s; SaveConfig(); });
            Messenger.Default.Register<string>(this, "SetConfigColorScheme", (string s) => { _config.ColorScheme = s; SaveConfig(); });
            Messenger.Default.Register<ConfigLanguage>(this, "SetConfigLanguage", (ConfigLanguage l) => { _config.Language = l; SaveConfig(); });
            Messenger.Default.Register<ConfigCurrency>(this, "SetConfigCurrency", (ConfigCurrency c) => { _config.Currency = c; SaveConfig(); });

            _tools = new ConfigFileTools();
            Initialize();

        }

        private void Initialize()
        {
            var configExists = _tools.CheckConfigFile();
            if (configExists)
            {
                _config = _tools.ParseConfigFile();
                return;
            }

            _tools.CreateConfigFile();
            _config = _tools.ParseConfigFile();
            Messenger.Default.Send<string>("", "OpenSetLanguageView");

        }

        private void SaveConfig()
        {
            Trace.WriteLine("Saving ConfigFile");
            _tools.SaveConfigFile(_config);
        }


        public ConfigLanguage GetConfigLanguage()
        {
            return _config.Language;
        }
    }
}
