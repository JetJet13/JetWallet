using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NBitcoin;
using JetWallet;
using JetWallet.Model;
using JetWallet.Tools;

namespace JetWallet.Tests.Models
{
    [TestFixture(Author = "Johny Georges", Description = "Testing UiSettings Method")]
    public class UiSettingsTest
    {
        UiSettings _uisettings;

        [SetUp]
        public void Init()
        {
            _uisettings = new UiSettings();
        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void ShowCurrencyTrue()
        {
            Network net = Network.Main;

            _uisettings.UpdateShowCurrency(net);
            Assert.True(_uisettings.ShowCurrency);
        }

        [Test]
        public void ShowCurrencyFalse()
        {
            Network net = Network.TestNet;

            _uisettings.UpdateShowCurrency(net);
            Assert.False(_uisettings.ShowCurrency);
        }

        [Test]
        public void SetWalletUnitSymbol()
        {
            var newUnitSymbol = BitcoinSymbol.BTC;
            _uisettings.SetWalletUnitSymbol(newUnitSymbol);

            Assert.AreEqual(_uisettings.WalletUnitSymbol, newUnitSymbol);
        }

        [Test]
        public void UpdateAll()
        {
            var newNetwork = Network.TestNet;
            var newUnitSymbol = BitcoinSymbol.tBTC;

            _uisettings.UpdateAll(newNetwork, newUnitSymbol);
            Assert.AreEqual(false, _uisettings.ShowCurrency);
            Assert.AreEqual(newUnitSymbol, _uisettings.WalletUnitSymbol);

        }

       







    }
}
