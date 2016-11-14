using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.Protocol;
using JetWallet.Tools;
using JetWallet.Model;
using JetWallet.ViewModel;
using NUnit.Framework;
using GalaSoft.MvvmLight.Threading;


namespace JetWallet.Tests.Models
{
    [TestFixture(Author = "Johny Georges", Description = "Testing WalletKey props and methods.")]
    public class WalletKeyTest
    {
        private ExtKey _masterkey;
        private KeyPath _keypath;
        private Network _net;
        private WalletKey _walletkey;

        [SetUp]
        public void Init()
        {
            _masterkey = new ExtKey();
            _keypath = new KeyPath(0, 2);
            _net = Network.Main;

            _walletkey = new WalletKey(_masterkey, _keypath, _net);

        }


        [Test]
        public void PassMatchPublicKey()
        {
            Script expectedPublicKey = _masterkey.Derive(_keypath).PrivateKey.PubKey.ScriptPubKey;                   
            Assert.True(_walletkey.MatchPublicKey(expectedPublicKey));
        }

        [Test]
        public void FailMatchPublicKey()
        {
            Script expectedPublicKey = _masterkey.Derive(_keypath).PrivateKey.ScriptPubKey.Hash.ScriptPubKey;
            Assert.False(_walletkey.MatchPublicKey(expectedPublicKey));
        }

        [Test]
        public void PassMatchKeyPath()
        {
            KeyPath kp = new KeyPath(0, 2);
            var index = kp.Indexes.Last(); // == 2
            Assert.True(_walletkey.MatchKeyPath(index));

        }

        [Test]
        public void FailMatchKeyPath()
        {
            KeyPath kp = new KeyPath(0, 4);
            var index = kp.Indexes.Last(); // == 4
            Assert.False(_walletkey.MatchKeyPath(index));
        }

        [Test]
        public void ConsumeKey()
        {
            _walletkey.ConsumeKey();
            Assert.True(_walletkey.Consumed);
        }
    }              
}
