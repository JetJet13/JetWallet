using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using JetWallet.Model;

namespace JetWallet.Tools
{
    public class JetMoneyTools
    {
        private static CurrencyRatesModel CurrInfo = new CurrencyRatesModel();
        public static decimal CalcBtc2Curr(Money btcAmount)
        {
            decimal btcPrice = 1; // $1USD/BTC
            decimal calc = btcAmount.ToDecimal(MoneyUnit.BTC) * btcPrice;
            return calc;
        }
    }
}
