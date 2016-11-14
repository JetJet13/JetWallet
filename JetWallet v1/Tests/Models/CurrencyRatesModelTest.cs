using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet;
using JetWallet.Model;
using JetWallet.Tools;
using NBitcoin;
using NUnit.Framework;
using Newtonsoft.Json;

namespace JetWallet.Tests.Models
{
    [TestFixture(Author ="Johny Georges", Description ="Testing CurrencyRatesModel methods")]
    public class CurrencyRatesModelTest
    {
        string url = "http://api.fixer.io/latest?base=USD";
        CurrencyRatesModel _currModel;

        [SetUp]
        public void Init()
        {
            _currModel = new CurrencyRatesModel();
        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void Initialize()
        {
            _currModel.Initalize();

            Assert.GreaterOrEqual(_currModel.USD, 0);
            Assert.GreaterOrEqual(_currModel.EUR, 0);
            Assert.GreaterOrEqual(_currModel.CAD, 0);
        }

        [Test]
        public void FetchRatesURL()
        {
            Assert.DoesNotThrow(() => WebRequests.GET(url));
        }

        [Test]
        public void SetRates()
        {
            string result = WebRequests.GET(url);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<ExchangeRates>(result));
        }
    }
}
