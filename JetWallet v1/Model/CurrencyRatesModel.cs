using JetWallet.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Model
{
    class CurrencyRatesModel
    {
        ExchangeRates _exchangerates;

        public decimal USD
        {
           get { return 1; } // the base rate is in USD.
        }

        public decimal EUR
        {
            get { return _exchangerates.Rates["EUR"]; }
        }

        public decimal CAD
        {
            get { return _exchangerates.Rates["CAD"]; }
        }
        

        public CurrencyRatesModel()
        {
                      
        }

        public void Initalize()
        {
            this.SetRates();
        }

        private string FetchRates()
        {
            string url = "http://api.fixer.io/latest?base=USD";
            return WebRequests.GET(url);  
        }

        private void SetRates()
        {
            try
            {
                string data = FetchRates();
                _exchangerates = JsonConvert.DeserializeObject<ExchangeRates>(data);
            }
            catch
            {
                _exchangerates = new ExchangeRates();
                _exchangerates.Rates = new Dictionary<string, decimal>();
                _exchangerates.Rates.Add("EUR", 0);
                _exchangerates.Rates.Add("CAD", 0);              
            }
}
    }
}
