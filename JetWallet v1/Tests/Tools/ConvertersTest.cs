using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using JetWallet.Tools;
using NBitcoin.SPV;
using NBitcoin;


namespace JetWallet.Tests.Tools
{
    [TestFixture(Author = "Johny Georges", Description = "Testing Converter methods")]
    public class ConvertersTest
    {
        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void Btc2Currency()
        {
            Money testAmount = new Money(45000000);
            decimal expectedAmount = Global.VML.Currency.ActivePrice * testAmount.ToDecimal(MoneyUnit.BTC);
            decimal result = Converters.Btc2Currency(testAmount);
            Assert.AreEqual(expectedAmount, result);

        }

    }
}
