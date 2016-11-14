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
using NBitcoin.SPV;

namespace JetWallet.Tests.Models
{
    [TestFixture(Author ="Johny Georges",Description ="Testing JetWalletModel Methods")]
    public class JetWalletModelTest
    {
        string _id = Guid.NewGuid().ToString();
        string _name = "My Wallet";
        string _description = "This is my wallet.";
        ExtKey _masterKey = new ExtKey();
        Network _networkChoice = Network.TestNet;
        IWallet _wallet;

        [SetUp]
        public void Init()
        {
            var masterKeyWIF = _masterKey.GetWif(_networkChoice).ToString();
            _wallet = WalletGenerator.GenerateNewWallet(_id, _name, masterKeyWIF, _networkChoice, _description);
            
        }

        [TearDown]
        public void Clear()
        {
            
        }

        [Test]
        public void CheckPropertyValues()
        {
            Assert.AreEqual(_id, _wallet.Id);
            Assert.AreEqual(_name, _wallet.Name);
            Assert.AreEqual(_description, _wallet.Description);
            Assert.AreEqual(_masterKey.GetWif(_networkChoice).ToString(), _wallet.MasterKeyWIF);

        }

        [Test]
        public void InitializeWallet()
        {
            Assert.DoesNotThrow(() => _wallet.Initialize());
            
        }

        [Test]
        public void Stop()
        {

            _wallet.Initialize();
            _wallet.Stop();
        }
        
        [Test]
        public void IsConnected()
        {
            _wallet.Initialize();
            Assert.True(_wallet.IsConnected());
        }

        [Test]
        public void IsConnectedFalse()
        {
            _wallet.Initialize();
            _wallet.Stop();
            Assert.False(_wallet.IsConnected());
        }

        [Test]
        public void GetNextPubKey()
        {
            Assert.NotNull(_wallet.GetNextPubKey());
        }

        [Test]
        public void PubKeyBelongsSuccess()
        {
            Script walletScriptPubKey = _wallet.GetNextPubKey().ScriptPubKey;
            Assert.True(_wallet.PubKeyBelongs(walletScriptPubKey));
            
        }

        [Test]
        public void PubKeyBelongsFail()
        {
            // randomly generated private key
            Script walletScriptPubKey = new ExtKey().PrivateKey.PubKey.ScriptPubKey;
            Assert.False(_wallet.PubKeyBelongs(walletScriptPubKey));
        }

        



    }
}
