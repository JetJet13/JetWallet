using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Tools;
using Newtonsoft.Json;

namespace JetWallet.Model
{
    public class BitcoinRatesModel
    {
        private CurrencyRatesModel _curr;

        private int _btcusd;
        private int _btceur;
        private int _btccad;

        public int USD
        {
            get { return _btcusd; }
        }

        public int EUR
        {
            get { return _btceur; }
        }

        public int CAD
        {
            get { return _btccad; }
        }

        public BitcoinRatesModel()
        {

        }

        public void Initialize()
        {
            _curr = new CurrencyRatesModel();
            _curr.Initalize();
            SetupRates();
        }

        private void SetupRates()
        {
            BitStampAPI bitstamp = ParseRates();
            int baseRate = (int)bitstamp.last;

            _btcusd = baseRate;
            _btceur = ConvertRate(baseRate, _curr.EUR);
            _btccad = ConvertRate(baseRate, _curr.CAD);
            
            
        }

        private int ConvertRate(int baseRate, decimal currRate)
        {
            return (int)(baseRate * currRate);
        }

        private BitStampAPI ParseRates()
        {
            string result = FetchRates();
            return JsonConvert.DeserializeObject<BitStampAPI>(result);
        }

        private string FetchRates()
        {
            string url = "https://www.bitstamp.net/api/v2/ticker/btcusd";
            return WebRequests.GET(url);
        }

        

    }
}
