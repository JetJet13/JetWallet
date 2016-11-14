using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.Protocol;
using JetWallet.Tools;
using JetWallet.Model;
using JetWallet.ViewModel;
using NUnit.Framework;
using System.Diagnostics;
using GalaSoft.MvvmLight.Threading;
using Moq;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace JetWallet.Tests.Tools
{
    [TestFixture(Author = "Johny Georges", Description = "Testing ConfigFileTools Methods")]
    public class ConfigFileToolsTest
    {
        ConfigFileTools _configtools;
        
        [SetUp]
        public void Init()
        {
            _configtools = new ConfigFileTools();
        }

        [TearDown]
        public void Clear()
        {
            string path = _configtools.GetConfigFilePath();
            File.Delete(path);
        }

        [Test]
        public void GetConfigFilePath()
        {
            Assert.DoesNotThrow(() => _configtools.GetConfigFilePath());
            Assert.True(Directory.Exists(App.AppDir));
        }

        [Test]
        public void CreateConfigFile()
        {
            _configtools.CreateConfigFile();
            string path = _configtools.GetConfigFilePath();
            Assert.True(File.Exists(path));
        }

        [Test]
        public void SaveConfigFile()
        {
            _configtools.CreateConfigFile();
            var configFile = _configtools.ParseConfigFile();
            Assert.True(_configtools.SaveConfigFile(configFile));
        }

        [Test]
        public void ParseConfigFile()
        {
            _configtools.CreateConfigFile();
            ConfigFile result = _configtools.ParseConfigFile();

            Assert.AreEqual("None", result.WalletPath);
            Assert.AreEqual(MaterialColorThemes.BlueGrey.ToString(), result.ColorScheme);
            Assert.AreEqual(ConfigLanguage.English, result.Language);
            Assert.AreEqual(ConfigCurrency.USD, result.Currency);
        }

        [Test]
        public void ParseConfigFileFail()
        {
            File.Delete(_configtools.GetConfigFilePath());
            Assert.Throws<Exception>(() => _configtools.ParseConfigFile());
        }

    }
}
