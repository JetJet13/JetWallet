using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Model;
using JetWallet.Tools;
using NBitcoin;
using NUnit.Framework;
using Newtonsoft.Json;

namespace JetWallet.Tests.Models
{
    [TestFixture(Author ="Johny Georges", Description ="Testing BitcoinRatesModel methods")]
    public class BitcoinRatesModelTest
    {
        string url = "https://www.bitstamp.net/api/v2/ticker/btcusd";
        string result;
        CurrencyRatesModel _currmodel;
        BitcoinRatesModel _btcrates;

        [SetUp]
        public void Init()
        {
            result = WebRequests.GET(url);

            _currmodel = new CurrencyRatesModel();
            _currmodel.Initalize();

            _btcrates = new BitcoinRatesModel();

        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void GetBtcRate()
        {
            Assert.DoesNotThrow(() => WebRequests.GET(url));
        }

        [Test]
        public void ParseBitstampAPI()
        {
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<BitStampAPI>(result));
        }

        [Test]
        public void Initialize()
        {
            _btcrates.Initialize();

            Assert.GreaterOrEqual(_btcrates.USD, 0);
            Assert.GreaterOrEqual(_btcrates.EUR, 0);
            Assert.GreaterOrEqual(_btcrates.CAD, 0);
            
        }
    }
}
