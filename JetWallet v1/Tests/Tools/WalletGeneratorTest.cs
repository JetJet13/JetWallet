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
using System.Collections;

namespace JetWallet.Tests.Tools
{
    [TestFixture(Author = "Johny Georges", Description = "Testing WalletGenerator Methods")]
    public class WalletGeneratorTest
    {
        IWallet _wallet;

        [SetUp]
        public void Init()
        {
            _wallet = WalletGenerator.GenerateMockWallet();
            _wallet.Initialize();
        }

        [Test]
        public void GenerateWallet()
        {
            Hashtable walletData = new Hashtable();
            walletData.Add("id", _wallet.Id);
            walletData.Add("name", _wallet.Name);
            walletData.Add("description", _wallet.Description);
            walletData.Add("masterkey", _wallet.MasterKeyWIF);
            walletData.Add("network", _wallet.NetworkChoice.ToString());
            IWallet result = WalletGenerator.GenerateWalletFromFile(walletData);

            Assert.AreEqual(result.Id, _wallet.Id);
            Assert.AreEqual(result.Name, _wallet.Name);
            Assert.AreEqual(result.Description, _wallet.Description);
            Assert.AreEqual(result.MasterKeyWIF, _wallet.MasterKeyWIF);
            Assert.AreEqual(result.NetworkChoice, _wallet.NetworkChoice);

        }

        [Test]
        public void GenerateWalletFail()
        {
            string badMasterKeyWIF = "5KRYM46bcstckRxtdDdBZTshjxtpWtTNBDXKcNWC7yjiGhnuBzV";
            Hashtable walletData = new Hashtable();
            walletData.Add("id", _wallet.Id);
            walletData.Add("name", _wallet.Name);
            walletData.Add("description", _wallet.Description);
            walletData.Add("masterkey", badMasterKeyWIF);
            walletData.Add("network", _wallet.NetworkChoice.ToString());

            Assert.Throws<FormatException>(() =>  WalletGenerator.GenerateWalletFromFile(walletData));
      
        }

        [Test]
        public void GenerateNewWallet()
        {
            IWallet result = WalletGenerator.GenerateNewWallet(_wallet.Id, _wallet.Name, _wallet.MasterKeyWIF, _wallet.NetworkChoice, _wallet.Description);
            Assert.AreEqual(_wallet.Id, result.Id);
            Assert.AreEqual(_wallet.Name, result.Name);
            Assert.AreEqual(_wallet.Description, result.Description);
            Assert.AreEqual(_wallet.MasterKeyWIF, result.MasterKeyWIF);
            Assert.AreEqual(_wallet.NetworkChoice, result.NetworkChoice);
        }

        [Test]
        public void GenerateNewWalletFail()
        {
            string badMasterKeyWIF = "5KRYM46bcstckRxtdDdBZTshjxtpWtTNBDXKcNWC7yjiGhnuBzV";
            Assert.Throws<FormatException>(() => WalletGenerator.GenerateNewWallet(_wallet.Id, _wallet.Name, badMasterKeyWIF, _wallet.NetworkChoice, _wallet.Description));
        }




    }
}
