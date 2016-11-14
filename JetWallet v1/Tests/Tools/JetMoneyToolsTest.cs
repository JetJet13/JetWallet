using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet;
using JetWallet.Tools;
using NBitcoin;
using NBitcoin.SPV;
using NUnit.Framework;


namespace JetWallet.Tests.Tools
{
    [TestFixture(Author = "Johny Georges", Description ="Testing JetMoneyTools methods")]
    public class JetMoneyToolsTest
    {

        [SetUp]
        public void Init()
        {

        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void CalcBtc2Curr()
        {
            int btcAmount = 450000000;
            Money btc = new Money(btcAmount);
            decimal currAmount = JetMoneyTools.CalcBtc2Curr(btc);

            decimal expected = 4.5m; // $4.50
            Assert.AreEqual(expected, currAmount);
        }

    }
}
