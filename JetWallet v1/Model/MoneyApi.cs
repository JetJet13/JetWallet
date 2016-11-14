using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Model
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
}
