using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JetWallet.Model;
using GalaSoft.MvvmLight.Messaging;

namespace JetWallet.Tools
{
    public class ConfigFileTools
    {
        public string GetConfigFilePath()
        {
            var path = Path.Combine(App.AppDir, @"..\jet.conf");
            var fullPath = Path.GetFullPath(path);
            return fullPath;
        }

        public void CreateConfigFile()
        {
            string path = GetConfigFilePath();
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Default Settings
                string defaultPath = "None";
                string defaultColorScheme = MaterialColorThemes.BlueGrey.ToString();
                string defaultLanguage = ConfigLanguage.English.ToString();
                string defaultCurrency = ConfigCurrency.USD.ToString();

                writer.WriteLine(defaultPath);
                writer.WriteLine(defaultColorScheme);
                writer.WriteLine(defaultLanguage);
                writer.WriteLine(defaultCurrency);

            }
            
        }

        public ConfigFile ParseConfigFile()
        {
            
            string path = GetConfigFilePath();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {

                    string walletPath = reader.ReadLine();
                    string colorScheme = reader.ReadLine();
                    ConfigLanguage language = (ConfigLanguage) Enum.Parse(typeof(ConfigLanguage), reader.ReadLine());
                    ConfigCurrency currency = (ConfigCurrency) Enum.Parse(typeof(ConfigCurrency), reader.ReadLine());
                

                    return new ConfigFile(walletPath, colorScheme, language, currency);

                }
            }
            catch(Exception e)
            {
                JetLogger.LogException(e);
                throw e;

            }
            
        }

        public bool SaveConfigFile(ConfigFile conf)
        {
            string path = GetConfigFilePath();
            
            // clear config file content first
            File.WriteAllText(path, string.Empty);

            File.WriteAllText(path, conf.GetSaveFormat());
            return true;

        }

        public bool CheckConfigFile()
        {
            string path = GetConfigFilePath();
            if (File.Exists(path))
            {
                return true;
            }
            return false;
        }

    }
}
