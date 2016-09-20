using GalaSoft.MvvmLight;
using NBitcoin;
using System.Diagnostics;

public enum WalletNetwork
{
    Network, // Null Placeholder
    Mainnet,
    Testnet
}

namespace JetWallet.ViewModel
{
    public class UiSettings : ViewModelBase
    {
       
        public const string NetPropertyName = "Net";
        private WalletNetwork _net = WalletNetwork.Network;
        public WalletNetwork Net
        {
            get
            {
                return _net;
            }

            private set
            {
                if (_net == value)
                {
                    return;
                }

                _net = value;
                RaisePropertyChanged(NetPropertyName);
                this.UpdateShowCurrency();
                this.UpdateSymbol();
            }
        }

        
        public const string ShowCurrencyPropertyName = "ShowCurrency";
        private bool _showcurrency = false;        
        public bool ShowCurrency
        {
            get
            {
                return _showcurrency;
            }

            set
            {
                if (_showcurrency == value)
                {
                    return;
                }

                _showcurrency = value;
                RaisePropertyChanged(ShowCurrencyPropertyName);
            }
        }

        public const string SymbolPropertyName = "Symbol";
        private string _symbol = string.Empty;
        public string Symbol
        {
            get
            {
                return _symbol;
            }

            set
            {
                if (_symbol == value)
                {
                    return;
                }

                _symbol = value;
                RaisePropertyChanged(SymbolPropertyName);
            }
        }
        

        public UiSettings()
        {
        }

        public void SetNetwork(Network newNetwork)
        {
            Net = ConvertNetwork(newNetwork);
        }

        public void ClearNetwork()
        {
            Net = WalletNetwork.Network;
        }

        private WalletNetwork ConvertNetwork(Network net)
        {            
            if (net.Equals(Network.Main))
            {
                return WalletNetwork.Mainnet;
            }
            else
            {
                return WalletNetwork.Testnet;
            }
        }

        // used to determine if views should
        // display currency values.
        public void UpdateShowCurrency()
        {
            if (Net.Equals(WalletNetwork.Mainnet))
            {
                ShowCurrency = true;
            }
            else
            {
                ShowCurrency = false;
            }

        }

        public void UpdateSymbol()
        {
            if (Net.Equals(WalletNetwork.Mainnet))
            {
                Symbol = "BTC";
            }
            else if (Net.Equals(WalletNetwork.Testnet))
            {
                Symbol = "tBTC";
            }
            else
            {
                Symbol = string.Empty;
            }
        }
    }
}