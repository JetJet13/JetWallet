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

namespace JetWallet.Tests.Models
{
    [TestFixture(Author = "Johny Georges", Description = "Testing ConfigFile Methods")]
    public class ConfigFileTest
    {
        ConfigFile _configfile;

        [SetUp]
        public void Init()
        {
            // Default Settings
            string defaultPath = "None";
            string defaultColorScheme = MaterialColorThemes.BlueGrey.ToString();
            ConfigLanguage defaultLanguage = ConfigLanguage.English;
            ConfigCurrency defaultCurrency = ConfigCurrency.USD;
            _configfile = new ConfigFile(defaultPath, defaultColorScheme, defaultLanguage, defaultCurrency);
        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void SaveFormat()
        {
            var result = _configfile.GetSaveFormat();
            string expected = _configfile.WalletPath
                + Environment.NewLine
                + _configfile.ColorScheme
                + Environment.NewLine
                + _configfile.Language.ToString()
                + Environment.NewLine
                + _configfile.Currency.ToString();

            Assert.AreEqual(expected, result);


        }

        [Test]
        public void SetCultureCode()
        {
            // Note: Culture code is set only when language changes.

            _configfile.Language = ConfigLanguage.English;
            Assert.AreEqual("en-US", _configfile.CultCode);

            _configfile.Language = ConfigLanguage.French;
            Assert.AreEqual("fr-FR", _configfile.CultCode);

            _configfile.Language = ConfigLanguage.Spanish;
            Assert.AreEqual("es-ES", _configfile.CultCode);

            _configfile.Language = ConfigLanguage.None;
            Assert.AreEqual("en-US", _configfile.CultCode);


        }

        



    }
}
