using System;

using System.Diagnostics;
using QBitNinja.Client;
using GalaSoft.MvvmLight;
using JetWallet.Tools;
using JetWallet.View;
using JetWallet.Model;
using GalaSoft.MvvmLight.Messaging;
using NBitcoin;
using System.Collections;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using QBitNinja.Client.Models;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace JetWallet.ViewModel
{

    public class TxInfoViewModel : ViewModelBase
    {
        const int MIN_NUM_CONF = 6;
        const int ONE_MORE_CONF = MIN_NUM_CONF - 1; // 5
        private TxInfoView _tiview;
        private BlockchainApi _blockapi;
        private QBitNinjaClient _qbit;
        private TransactionModel _tx;
        private Network _net;      

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public UiSettings UiSettings
        {
            get { return Global.VML.UiSettings; }
        }

        public CurrencyViewModel Currency
        {
            get { return Global.VML.Currency; }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("TxInfo_Title"); }
        }

        public string TextCopy
        {
            get { return TextTools.RetrieveStringFromResource("Copy"); }
        }

        public string TextBlockHeight
        {
            get { return TextTools.RetrieveStringFromResource("TxInfo_BlockHeight"); }
        }

        public string TextBlockHash
        {
            get { return TextTools.RetrieveStringFromResource("TxInfo_BlockHash"); }
        }

        public string TextTxHash
        {
            get { return TextTools.RetrieveStringFromResource("TxInfo_TxHash"); }
        }

        public string TextFee
        {
            get { return TextTools.RetrieveStringFromResource("TxInfo_Fee"); }
        }

        public string TextConf
        {
            get { return TextTools.RetrieveStringFromResource("TxInfo_Conf"); }
        }

        public string TextTimestamp
        {
            get { return TextTools.RetrieveStringFromResource("TxInfo_Timestamp"); }
        }

        public string TextStatus
        {
            get { return TextTools.RetrieveStringFromResource("TxInfo_Status"); }
        }

        public string TextOk
        {
            get { return TextTools.RetrieveStringFromResource("Ok"); }
        }

        
        public const string BlockHeightPropertyName = "BlockHeight";
        private string _blockheight;        
        public string BlockHeight
        {
            get
            {
                return _blockheight;
            }

            set
            {
                if (_blockheight == value)
                {
                    return;
                }

                _blockheight = value;
                RaisePropertyChanged(BlockHeightPropertyName);
            }
        }

        
        public const string IsBlockPresentPropertyName = "IsBlockPresent";
        private bool _isblockpresent = false;        
        public bool IsBlockPresent
        {
            get
            {
                return _isblockpresent;
            }

            set
            {
                if (_isblockpresent == value)
                {
                    return;
                }

                _isblockpresent = value;
                RaisePropertyChanged(IsBlockPresentPropertyName);
            }
        }


        
        public const string BlockHashPropertyName = "BlockHash";
        private string _blockhash;        
        public string BlockHash
        {
            get
            {
                return _blockhash;
            }

            set
            {
                if (_blockhash == value)
                {
                    return;
                }

                _blockhash = value;
                RaisePropertyChanged(BlockHashPropertyName);
            }
        }

        
        public const string TxHashPropertyName = "TxHash";
        private string _txhash;        
        public string TxHash
        {
            get
            {
                return _txhash;
            }

            set
            {
                if (_txhash == value)
                {
                    return;
                }

                _txhash = value;
                RaisePropertyChanged(TxHashPropertyName);
            }
        }

        
        public const string TxActionPropertyName = "TxAction";
        private string _txaction;        
        public string TxAction
        {
            get
            {
                return _txaction;
            }

            set
            {
                if (_txaction == value)
                {
                    return;
                }

                _txaction = value;
                RaisePropertyChanged(TxActionPropertyName);
            }
        }

        
        public const string ActionAmountPropertyName = "ActionAmount";
        private decimal _actionamount = 0;        
        public decimal ActionAmount
        {
            get
            {
                return _actionamount;
            }

            set
            {
                if (_actionamount == value)
                {
                    return;
                }

                _actionamount = value;
                RaisePropertyChanged(ActionAmountPropertyName);
            }
        }

        
        public const string ActionAmountCurrPropertyName = "ActionAmountCurr";
        private decimal _actionamountcurr = 0;        
        public decimal ActionAmountCurr
        {
            get
            {
                return _actionamountcurr;
            }

            set
            {
                if (_actionamountcurr == value)
                {
                    return;
                }

                _actionamountcurr = value;
                RaisePropertyChanged(ActionAmountCurrPropertyName);
            }
        }

        
        public const string FeePropertyName = "Fee";
        private Money _fee;        
        public Money Fee
        {
            get
            {
                return _fee;
            }

            set
            {
                if (_fee == value)
                {
                    return;
                }

                _fee = value;
                RaisePropertyChanged(FeePropertyName);
            }
        }

        
        public const string FeeCurPropertyName = "FeeCurr";
        private decimal _feecurr = 0;        
        public decimal FeeCurr
        {
            get
            {
                return _feecurr;
            }

            set
            {
                if (_feecurr == value)
                {
                    return;
                }

                _feecurr = value;
                RaisePropertyChanged(FeeCurPropertyName);
            }
        }

        
        public const string ConfirmationsPropertyName = "Confirmations";
        private string _confirmation;        
        public string Confirmations
        {
            get
            {
                return _confirmation;
            }

            set
            {
                if (_confirmation == value)
                {
                    return;
                }

                _confirmation = value;
                RaisePropertyChanged(ConfirmationsPropertyName);
            }
        }


        public const string DatePropertyName = "Date";
        private string _date;        
        public string Date
        {
            get
            {
                return _date;
            }

            set
            {
                if (_date == value)
                {
                    return;
                }

                _date = value;
                RaisePropertyChanged(DatePropertyName);
            }
        }

        
        public const string TxStatusPropertyName = "TxStatus";
        private string _txstatus;        
        public string TxStatus
        {
            get
            {
                return _txstatus;
            }

            set
            {
                if (_txstatus == value)
                {
                    return;
                }

                _txstatus = value;
                RaisePropertyChanged(TxStatusPropertyName);
            }
        }

        
        public const string LoadingDataPropertyName = "LoadingData";
        private bool _loadingdata = false;        
        public bool LoadingData
        {
            get
            {
                return _loadingdata;
            }

            set
            {
                if (_loadingdata == value)
                {
                    return;
                }

                _loadingdata = value;
                RaisePropertyChanged(LoadingDataPropertyName);
            }
        }

        
        public TxInfoViewModel()
        {
            CopyCmd = new RelayCommand<string>((string s) => { this.ExecuteCopy(s); });
            CloseViewCmd = new RelayCommand(() => { this.ExecuteCloseView(); });
            Messenger.Default.Register<TransactionModel>(this, "OpenTxInfoView", (TransactionModel s) => { this.OpenView(s); });

            

        }

        public RelayCommand<string> CopyCmd
        {
            get;
            private set;
        }
        public RelayCommand CloseViewCmd
        {
            get;
            private set;
        }


        private void OpenView(TransactionModel tx)
        {
            this.ClearProps();
            LoadingData = true;
            _tiview = new TxInfoView();
            _net = Global.ActiveWallet.NetworkChoice;
            _blockapi = new BlockchainApi(_net);
            _qbit = new QBitNinjaClient(_net);
            _tx = tx;
            ConfigureTxInfo();          
            _tiview.ShowDialog();
        }
        
        private async void ConfigureTxInfo()
        {
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    this.UseSoChainValues();
                    LoadingData = false;
                    return;
                }
                catch
                {

                }
                
                try
                {
                    var qbitResult = await _qbit.GetTransaction((uint256.Parse(_tx.Id)));
                    this.UseQbitValues(qbitResult);
                }
                catch
                {
                    // neither SoChainApi and QbitClient are available
                    App.Current.Dispatcher.Invoke(() => ShowErrorDialog());
                }
                finally
                {
                    LoadingData = false;
                }            

            });

        }

        private async void ShowErrorDialog()
        {
            //TODO insert into english string resource
            string title = TextTools.RetrieveStringFromResource("TxInfo_Dialog_Fail_Title");
            string message = TextTools.RetrieveStringFromResource("TxInfo_Dialog_Fail_Message");
            await _tiview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
            ExecuteCloseView();
        }

        private void UseSoChainValues()
        {
            var result = _blockapi.FetchTxData(_tx.Id);
            // transaction is unconfirmed
            if (result.data["block_no"] == null)
            {
                BlockHeight = "NA";
                BlockHash = "NA";
                IsBlockPresent = false;
            }
            // transaction has at least 1 confirmation
            else
            {
                var height = int.Parse(result.data["block_no"].ToString());
                BlockHeight = height.ToString("N0");
                BlockHash = result.data["blockhash"].ToString();
                IsBlockPresent = true;
            }

            TxHash = result.data["txid"].ToString();
            TxAction = _tx.ActionString;
            ActionAmount = _tx.AbsBalance;
            ActionAmountCurr = _tx.BalanceCurr;
            Fee = Money.Parse(result.data["fee"].ToString());
            FeeCurr = Converters.Btc2Currency(Fee);

            var conf = int.Parse(result.data["confirmations"].ToString());
            Confirmations = conf.ToString("N0");

            Date = _tx.DateDisplay;
            TxStatus = this.DetermineTxStatus(conf);
            
            LoadingData = false;
        }
        
        private void UseQbitValues(GetTransactionResponse result)
        {
            // transaction is unconfirmed
            if (result.Block == null)
            {
                BlockHeight = "NA";
                BlockHash = "NA";
                IsBlockPresent = false;
            }
            // transaction has at least 1 confirmation
            else
            {
                BlockHeight = result.Block.Height.ToString("N0");
                BlockHash = result.Block.BlockId.ToString();
                IsBlockPresent = true;
            }

            TxHash = result.TransactionId.ToString();
            TxAction = _tx.ActionString;
            ActionAmount = _tx.AbsBalance;
            ActionAmountCurr = _tx.BalanceCurr;
            Fee = result.Fees;
            FeeCurr = Converters.Btc2Currency(Fee);            
            Confirmations = result.Block.Confirmations.ToString("N0");
            Date = _tx.DateDisplay;
            TxStatus = this.DetermineTxStatus(result.Block.Confirmations);

            
            LoadingData = false;

        }

        private bool QbitQueryOK(GetTransactionResponse query)
        {
            if (query == null)
            {
                return false;
            }
            return true;
            
        }

        private bool BlockIsNull(BlockInformation blockInfo)
        {
            if (blockInfo == null)
            {
                return true;
            }
            return false;
        }

        private string DetermineTxStatus(int conf)
        {
            if (conf.Equals(0))
            {
                return TextTools.RetrieveStringFromResource("TxInfo_Unconfirmed");
            }
            else if (conf.Equals(ONE_MORE_CONF))
            {
                int remaining = 1;
                return TextTools.RetrieveStringFromResource("TxInfo_InProgress_Singular")
                    .Replace("*num*", remaining.ToString());
            }
            else if (conf < MIN_NUM_CONF)
            {
                int remaining = 6 - conf;
                return TextTools.RetrieveStringFromResource("TxInfo_InProgress_Plural")
                    .Replace("*num*", remaining.ToString());
            }
            else
            {
                return TextTools.RetrieveStringFromResource("TxInfo_Confirmed");
            }
        }

        private void ExecuteCopy(string s)
        {
            Clipboard.SetText(s);
        }

        private void ClearProps()
        {
            this.ActionAmount = 0;
            this.ActionAmountCurr = 0;
            this.BlockHash = string.Empty;
            this.BlockHeight = string.Empty;
            this.IsBlockPresent = false;
            this.Confirmations = string.Empty;
            this.Date = string.Empty;
            this.Fee = Money.Zero;
            this.FeeCurr = 0;
            this.TxHash = string.Empty;
            this.TxStatus = string.Empty;
            _tx = null;
            _blockapi = null;
            _qbit = null;
        }

        private void ExecuteCloseView()
        {
            _tiview.Close();
        }

    }
}