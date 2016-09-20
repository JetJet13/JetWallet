using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using JetWallet.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace JetWallet.ViewModel
{
    class ExchangeRates
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }

    }

    class BitStampAPI
    {
        public string high { get; set; }
        public decimal last { get; set; }
        public string timestamp { get; set; }
        public string bid { get; set; }
        public string vwap { get; set; }
        public string volume { get; set; }
        public string low { get; set; }
        public string ask { get; set; }
        public string open { get; set; }
    }
   
    public class CurrencyViewModel : ViewModelBase
    {                       
        private ExchangeRates rates { get; set; }
        private BitStampAPI bitstamp { get; set; }

        
        public const string ActivePricePropertyName = "ActivePrice";
        private int _activePrice = 0;
        public int ActivePrice
        {
            get
            {
                return _activePrice;
            }

            set
            {
                if (_activePrice == value)
                {
                    return;
                }

                _activePrice = value;
                RaisePropertyChanged(ActivePricePropertyName);
            }
        }

        public const string ActiveSymbolPropertyName = "ActiveSymbol";
        private string _activesymbol;
        public string ActiveSymbol
        {
            get
            {
                return _activesymbol;
            }

            set
            {
                if (_activesymbol == value)
                {
                    return;
                }

                _activesymbol = value;
                RaisePropertyChanged(ActiveSymbolPropertyName);
            }
        }

        public const string ActiveCurrencyPropertyName = "ActiveCurrency";
        private string _activecurrency = "USD";
        public string ActiveCurrency
        {
            get
            {
                return _activecurrency;
            }

            set
            {
                if (_activecurrency == value)
                {
                    return;
                }

                _activecurrency = value;
                RaisePropertyChanged(ActiveCurrencyPropertyName);
                
            }
        }


        public const string UsdBtcPricePropertyName = "UsdBtcPrice";
        private int _usdbtcprice = 0;
        public int UsdBtcPrice
        {
            get
            {
                return _usdbtcprice;
            }

            set
            {
                if (_usdbtcprice == value)
                {
                    return;
                }

                _usdbtcprice = value;
                RaisePropertyChanged(UsdBtcPricePropertyName);
            }
        }

        public const string EurBtcPricePropertyName = "EurBtcPrice";
        private int _eurBtcPrice = 0;
        public int EurBtcPrice
        {
            get
            {
                return _eurBtcPrice;
            }

            set
            {
                if (_eurBtcPrice == value)
                {
                    return;
                }

                _eurBtcPrice = value;
                RaisePropertyChanged(EurBtcPricePropertyName);
            }
        }

        public const string CadBtcPricePropertyName = "CadBtcPrice";
        private int _cadbtcprice = 0;
        public int CadBtcPrice
        {
            get
            {
                return _cadbtcprice;
            }

            set
            {
                if (_cadbtcprice == value)
                {
                    return;
                }

                _cadbtcprice = value;
                RaisePropertyChanged(CadBtcPricePropertyName);
            }
        }

        public CurrencyViewModel()
        {
            this.FetchRates();
            this.FetchBtcPrice();
            this.SetPrices();
        }

        private void FetchRates()
        {
            string url = "http://api.fixer.io/latest?base=USD";
            try
            {
                string result = WebRequests.GET(url);
                rates = JsonConvert.DeserializeObject<ExchangeRates>(result);
            }
            catch
            {
                rates = new ExchangeRates();
                rates.Rates = new Dictionary<string, decimal>();
                rates.Rates.Add("EUR", 0);
                rates.Rates.Add("CAD", 0);              
            }            

        }

        private void FetchBtcPrice()
        {
            string url = "https://www.bitstamp.net/api/v2/ticker/btcusd";
            try
            {
                string result = WebRequests.GET(url);
                bitstamp = JsonConvert.DeserializeObject<BitStampAPI>(result);
            }
            catch
            {
                bitstamp = new BitStampAPI();
                bitstamp.last = 0;
            }
          
        }

        private void SetPrices()
        {
            UsdBtcPrice = (int)bitstamp.last;
            EurBtcPrice = this.ConvertRateTo("EUR");
            CadBtcPrice = this.ConvertRateTo("CAD");            
        }

        private int ConvertRateTo(string s)
        {
            decimal usdRate = bitstamp.last;
            switch (s)
            {
                case "EUR":
                    int eur = (int)(usdRate * rates.Rates["EUR"]);
                    return eur;
                case "CAD":
                    int cad = (int)(usdRate * rates.Rates["CAD"]);
                    return cad;
                default:
                    return 0;
            }
        }

        public void UpdateActiveProps(string curr)
        {
            switch (curr)
            {
                case "USD":
                    ActiveCurrency = "USD";
                    ActiveSymbol = "$";
                    ActivePrice = UsdBtcPrice;
                    break;
                case "EUR":
                    ActiveCurrency = "EUR";
                    ActiveSymbol = "€";
                    ActivePrice = EurBtcPrice;
                    break;
                case "CAD":
                    ActiveCurrency = "CAD";
                    ActiveSymbol = "$";
                    ActivePrice = CadBtcPrice;
                    break;
                default:
                    ActiveCurrency = "USD";
                    ActiveSymbol = "$";
                    ActivePrice = UsdBtcPrice;
                    break;            
            }
            Messenger.Default.Send<string>("", "NewCurrency");
        }
                
    }
}