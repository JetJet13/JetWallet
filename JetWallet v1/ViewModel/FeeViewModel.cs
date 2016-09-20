using GalaSoft.MvvmLight;
using JetWallet.Tools;
using NBitcoin;
using NBitcoin.SPV;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace JetWallet.ViewModel
{
    public class BitcoinFees
    {
        public int fastestFee { get; set; }
        public int halfHourFee { get; set; }
        public int hourFee { get; set; }
    }

    public class FeeViewModel : ViewModelBase
    {
        public FeeViewModel()
        {
            this.FetchFeeRates();
        }

        private static BitcoinFees rates { get; set; }

        private void FetchFeeRates()
        {
            string url = "https://bitcoinfees.21.co/api/v1/fees/recommended";
            try
            {
                string result = WebRequests.GET(url);
                rates = JsonConvert.DeserializeObject<BitcoinFees>(result);
            }
            catch
            {
                rates = new BitcoinFees();
                rates.halfHourFee = 30;
            }

        }

        public BitcoinFees GetFeeRates()
        {            
            return rates;
        }

    }
}