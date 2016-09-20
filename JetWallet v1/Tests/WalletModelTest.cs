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
using FakeItEasy;

namespace JetWallet.Tests
{
    [TestFixture(Author ="Johny Georges",Description ="Testing WalletModel Methods")]
    public class WalletModelTest
    {
        public WalletModel _wallet;

        public WalletModelTest()
        {
            
            Network net = Network.TestNet;
            ExtKey masterKey = Generators.GenerateMasterKey();
            BitcoinExtPubKey rootKey = Generators.GenerateRootKey(masterKey, net);
            string id = Guid.NewGuid().ToString();
            _wallet = new WalletModel(id, "Johny", rootKey, DateTimeOffset.Now);
            
        }

        [Test]
        public void CheckDescriptionMethod()
        {
            string description = _wallet.Desc;
                        
            if (description == "")
            {
                Assert.Fail("Description is empty. Default value is 'No Description'");
            }
            Assert.True(true,"Working!");
            
        }

        [Test]
        public void CheckGenerateOptimalFee()
        {
            var fee = Money.Parse("0.00423");            
            var optimalfee = Generators.GenerateOptimalFee(fee);
            var result = Money.FromUnit(400000,MoneyUnit.Satoshi);
            Assert.True(optimalfee.Satoshi.Equals(result.Satoshi));            

        }
    }
}
