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
    [TestFixture(Author = "Johny Georges", Description = "Testing Converter Tools Methods")]
    public class ConverterToolsTest
    {
        IWallet _wallet;
        Mock<IWallet> mock;

        [SetUp]
        public void Init()
        {
            InitMock();
            _wallet = mock.Object;
        }

        private void InitMock()
        {
            var net = Network.TestNet;
            var masterKeyWIF = new ExtKey().GetWif(net).ToString();

            mock = new Mock<IWallet>();
            mock.SetupGet<string>((x) => x.Id).Returns("wallet-id");
            mock.SetupGet<string>((x) => x.Name).Returns("MyWallet");
            mock.SetupGet<string>((x) => x.Description).Returns("My Wallet Description");
            mock.SetupGet<string>((x) => x.MasterKeyWIF).Returns(masterKeyWIF);
            mock.SetupGet<Network>((x) => x.NetworkChoice).Returns(net);

        }

        [Test]
        public void ConvertWallet2HashTable()
        {
            
            Hashtable result = ConverterTools.Wallet2Hashtable(_wallet);
            
            Assert.AreEqual(result["id"], _wallet.Id);
            Assert.AreEqual(result["name"], _wallet.Name);
            Assert.AreEqual(result["description"], _wallet.Description);
            Assert.AreEqual(result["masterkey"], _wallet.MasterKeyWIF);
            Assert.AreEqual(result["network"], _wallet.NetworkChoice.ToString());
        }

    }
}
