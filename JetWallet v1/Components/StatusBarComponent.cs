﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics;
using JetWallet.Tools;
using JetWallet.Model;
using NBitcoin;
using GalaSoft.MvvmLight.Messaging;

namespace JetWallet.Components
{
    class ExchangeRates
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string,decimal> Rates { get; set; }

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

    public class StatusBarComponent : ViewModelBase
    {

        private WalletModel _wallet;
        public WalletModel Wallet
        {
            get { return _wallet; }
            set { _wallet = value; RaisePropertyChanged("Wallet"); }
        }

        private string _textprice = string.Empty;
        public string TextPrice
        {
            get { return _textprice; }
            set
            {
                _textprice = value;
                RaisePropertyChanged("TextPrice");
            }
        }

        // Tooltip text for nodes icon
        private string _textnodes;
        public string TextNodes
        {
            get { return _textnodes; }
            set { _textnodes = value; RaisePropertyChanged("TextNodes"); }
        }

        // Tooltip text for height icon
        private string _textheight;
        public string TextHeight
        {
            get { return _textheight; }
            set { _textheight = value; RaisePropertyChanged("TextHeight"); }
        }

        // Tooltip text for num txs icon
        private string _textnumtxs;
        public string TextNumTxs
        {
            get { return _textnumtxs; }
            set { _textnumtxs = value; RaisePropertyChanged("TextNumTxs"); }
        }


        public const string NetPropertyName = "Net";

        private Network _net;


        public Network Net
        {
            get
            {
                return _net;
            }

            set
            {
                if (_net == value)
                {
                    return;
                }

                _net = value;
                RaisePropertyChanged(NetPropertyName);
            }
        }


        public const string NumTxsPropertyName = "NumTxs";

        private int _numtxs = 0;

 
        public int NumTxs
        {
            get
            {
                return _numtxs;
            }

            set
            {
                if (_numtxs == value)
                {
                    return;
                }

                _numtxs = value;
                RaisePropertyChanged(NumTxsPropertyName);
            }
        }


        public const string ConnectedNodesPropertyName = "ConnectedNodes";

        private int _connectedNodes = 0;


        public int ConnectedNodes
        {
            get
            {
                return _connectedNodes;
            }

            set
            {
                if (_connectedNodes == value)
                {
                    return;
                }

                _connectedNodes = value;
                RaisePropertyChanged(ConnectedNodesPropertyName);
            }
        }


        public const string CurrentHeightPropertyName = "CurrentHeight";

        private int _currentheight = 0;


        public int CurrentHeight
        {
            get
            {
                return _currentheight;
            }

            set
            {
                if (_currentheight == value)
                {
                    return;
                }

                _currentheight = value;
                RaisePropertyChanged(CurrentHeightPropertyName);
            }
        }

        public StatusBarComponent()
        {
            this.UpdateText();
        }

        public void SetProperties(WalletModel w)
        {
            Wallet = w;                 
            
        }
        
        public void ClearProps()
        {            
            Wallet = null;
            
        }

        public void UpdateText()
        {
            if (Wallet != null)
            {
                Wallet.UpdateBalanceCurrDisplay();
                
            }

            TextNodes = TextTools.RetrieveStringFromResource("Main_Tooltip_Nodes");
            TextHeight = TextTools.RetrieveStringFromResource("Main_Tooltip_Height");
            TextNumTxs = TextTools.RetrieveStringFromResource("Main_Tooltip_NumTxs");
            TextPrice = String.Format("1 BTC = {0}{1} {2}", Global.VML.Currency.ActiveSymbol, Global.VML.Currency.ActivePrice, Global.VML.Currency.ActiveCurrency);
                   
        }

    }
}
