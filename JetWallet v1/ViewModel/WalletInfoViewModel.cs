using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JetWallet.Model;
using JetWallet.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using NBitcoin.SPV;
using NBitcoin;
using System.Collections;
using System.Windows;
using JetWallet.Tools;
using System.Windows.Media;
using System.Diagnostics;

namespace JetWallet.ViewModel
{

    public class WalletInfoViewModel : ViewModelBase
    {
        private WalletInfoView _wiview;

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public UiSettings UiSettings
        {
            get { return Global.VML.UiSettings; }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Title"); }
        }

        public string TextDetails
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Details"); }
        }

        public string TextAddresses
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Addresses"); }
        }

        public string TextTransactions
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Transactions"); }
        }

        public string TextName
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Name"); }
        }

        public string TextDesc
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Desc"); }
        }

        public string TextNetwork
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Network"); }
        }

        public string TextDate
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Date"); }
        }

        public string TextFile
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_File"); }
        }

        public string TextCopy
        {
            get { return TextTools.RetrieveStringFromResource("Copy"); }
        }

        public string TextBalance
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Balance"); }
        }

        public string TextAvailBalance
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_AvailBalance"); }
        }

        public string TextUnconfBalance
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_UnconfBalance"); }
        }

        public string TextNumTx
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_NumTx"); }
        }

        public string TextNumRecTx
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_NumRecTx"); }
        }

        public string TextNumSentTx
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_NumSentTx"); }
        }

        public string TextAddrDesc
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_AddrDesc"); }
        }

        public string TextSearch
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_Search"); }
        }

        public string TextTxDesc
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_TxDesc"); }
        }

        public string TextTxHash
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_TxHash"); }
        }

        public string TextTxDate
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_TxDate"); }
        }

        public string TextTxStatus
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_TxStatus"); }
        }

        public string TextTxAction
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_TxAction"); }
        }

        public string TextTxAmount
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_TxAmount"); }
        }

        public string TextTxValue
        {
            get { return TextTools.RetrieveStringFromResource("WalletInfo_TxValue"); }
        }

        private WalletModel _wallet;
        public WalletModel Wallet
        {
            get { return _wallet; }
            set { _wallet = value; }
        }

        
        public const string WalletAddressesPropertyName = "WalletAddresses";
        private ICollectionView _walletaddresses;
        public ICollectionView WalletAddresses
        {
            get
            {
                return _walletaddresses;
            }

            set
            {
                if (_walletaddresses == value)
                {
                    return;
                }

                _walletaddresses = value;
                RaisePropertyChanged(WalletAddressesPropertyName);
            }
        }
        

        public const string AddrSearchTextPropertyName = "AddrSearchText";
        private string _addrsearchtext = string.Empty;
        public string AddrSearchText
        {
            get
            {
                return _addrsearchtext;
            }

            set
            {
                if (_addrsearchtext == value)
                {
                    return;
                }

                _addrsearchtext = value;
                WalletAddresses.Refresh();
                RaisePropertyChanged(AddrSearchTextPropertyName);
            }
        }

        public const string WalletTxsPropertyName = "WalletTxs";
        private ICollectionView _wallettxs;
        public ICollectionView WalletTxs
        {
            get
            {
                return _wallettxs;
            }

            set
            {
                if (_wallettxs == value)
                {
                    return;
                }

                _wallettxs = value;
                RaisePropertyChanged(WalletTxsPropertyName);
            }
        }


        public const string TxSearchTextPropertyName = "TxSearchText";
        private string _txsearchtext = string.Empty;
        public string TxSearchText
        {
            get
            {
                return _txsearchtext;
            }

            set
            {
                if (_txsearchtext == value)
                {
                    return;
                }

                _txsearchtext = value;
                RaisePropertyChanged(TxSearchTextPropertyName);
                WalletTxs.Refresh();
                
            }
        }

        public WalletInfoViewModel()
        {
            OpenTxInfoCmd = new RelayCommand<string>((string s) => { this.ExecuteOpenTxInfo(s); });
            CopyStringCmd = new RelayCommand<string>((string s) => { this.ExecuteCopyString(s); });
            CopyDecimalCmd = new RelayCommand<decimal>((decimal d) => { this.ExecuteCopyDecimal(d); });
            CloseCmd = new RelayCommand(() => { this.CloseView(); });

            Messenger.Default.Register<string>(this, "CloseWallet", (string s) => { this.ClearValues(); });
            Messenger.Default.Register<string>(this, "OpenWalletInfo", (s) => { this.OpenView(); });
        }

               
        public RelayCommand<string> OpenTxInfoCmd
        {
            get;
            private set;
        }
        public RelayCommand<string> CopyStringCmd
        {
            get;
            private set;
        }
        public RelayCommand<decimal> CopyDecimalCmd
        {
            get;
            private set;
        }
        public RelayCommand CloseCmd
        {
            get;
            private set;
        }

        private void OpenView()
        {
            Initialize();
            _wiview = new WalletInfoView();
            _wiview.ShowDialog();
        }

        private void Initialize()
        {
            Wallet = Global.ActiveWallet;
            GenerateWalletAddresses();
            GenerateWalletTxs();          

        }

        private void GenerateWalletAddresses()
        {
            List<string> addresses = new List<string>();
            foreach (var pub in Wallet.GetAllPubKeys())
            {
                
                string address = pub.GetAddress(Wallet.NetworkChoice).ToString();
                addresses.Add(address);
            }

            WalletAddresses = CollectionViewSource.GetDefaultView(addresses);
            WalletAddresses.Filter = (o) => String.IsNullOrWhiteSpace(AddrSearchText) ? true : ((string)o).Contains(AddrSearchText.Trim());
        }

        private void GenerateWalletTxs()
        {
            WalletTxs = CollectionViewSource.GetDefaultView(Wallet.TxCollection);
            WalletTxs.Filter = (o) => String.IsNullOrWhiteSpace(TxSearchText) ? true : ((TransactionModel)o).Id.Contains(TxSearchText.Trim());
        }

        private void ExecuteOpenTxInfo(string txid)
        {
            TransactionModel tx = Wallet.TxCollection.Find(x => x.Id == txid);
            Messenger.Default.Send<TransactionModel>(tx, "OpenTxInfoView");
        }

        private void ExecuteCopyString(string s)
        {
            Clipboard.SetText(s);
        }
        private void ExecuteCopyDecimal(decimal d)
        {
            Clipboard.SetText(d.ToString());
        }

        private void ClearValues()
        {
            this.AddrSearchText = string.Empty;
            this.TxSearchText = string.Empty;
            Wallet = null;
            WalletAddresses = null;
            WalletTxs = null;
        }

        private void CloseView()
        {
            _wiview.Close();
        }
    }
}