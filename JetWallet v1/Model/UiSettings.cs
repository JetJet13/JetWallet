using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;

namespace JetWallet.Model
{
    public enum BitcoinSymbol
    {
        NA, // null placeholder
        BTC,
        tBTC,
        milliBTC,
        bit,
        satoshis
    }
    public class UiSettings
    {
        private bool _showcurrency;
        private BitcoinSymbol _walletunitsymbol;


        public bool ShowCurrency
        {
            get { return _showcurrency; }
        }

       public BitcoinSymbol WalletUnitSymbol
        {
            get { return _walletunitsymbol; }
        }


        public UiSettings()
        {
            _showcurrency = false;
        }


        public void UpdateShowCurrency(Network net)
        {

            if (net.Equals(Network.Main))
            {
                _showcurrency = true;
            }
            else
            {
                _showcurrency = false;
            }

        }

        public void SetWalletUnitSymbol(BitcoinSymbol newUnitSymbol)
        {
            _walletunitsymbol = newUnitSymbol;
            
        }

        public void UpdateAll(Network newNetwork, BitcoinSymbol newUnitSymbol)
        {
            this.UpdateShowCurrency(newNetwork);
            this.SetWalletUnitSymbol(newUnitSymbol);
        }
    }
}
