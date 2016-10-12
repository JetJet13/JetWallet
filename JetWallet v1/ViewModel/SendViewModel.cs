using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using JetWallet.Tools;
using JetWallet.View;
using JetWallet.Model;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System;
using System.Windows;
using NBitcoin;
using System.Security;
using System.Collections.Generic;
using System.ComponentModel;
using NBitcoin.SPV;
using NBitcoin.Protocol;
using NBitcoin.Protocol.Behaviors;

namespace JetWallet.ViewModel
{
    
    public class SendViewModel : ViewModelBase
    {
        private SendView _sview;
        private WalletModel _wallet;
        private bool _isbtcfocused = false;
        private bool _iscurrfocused = false;

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
            get { return TextTools.RetrieveStringFromResource("Send_Title"); }
        }

        public string TextReceipientAddress
        {
            get { return TextTools.RetrieveStringFromResource("Send_ReceipientAddress"); }
        }

        public string TextExample
        {
            get { return TextTools.RetrieveStringFromResource("Send_Example"); }
        }

        public string TextAmount
        {
            get { return TextTools.RetrieveStringFromResource("Send_Amount"); }
        }

        public string TextAvailAmount
        {
            get { return TextTools.RetrieveStringFromResource("Send_AvailAmount"); }
        }

        public string TextMax
        {
            get { return TextTools.RetrieveStringFromResource("Send_Max"); }
        }

        public string TextBtc
        {
            get { return TextTools.RetrieveStringFromResource("BTC"); }
        }

        public string TextFee
        {
            get { return TextTools.RetrieveStringFromResource("Send_Fee"); }
        }

        public string TextPass
        {
            get { return TextTools.RetrieveStringFromResource("Send_Pass"); }
        }

        public string TextSendBtc
        {
            get { return TextTools.RetrieveStringFromResource("Send_SendBtc"); }
        }

        public CurrencyViewModel Currency
        {
            get { return Global.VML.Currency; }
        }

        
        public const string PassAttemptPropertyName = "PassAttempt";
        private SecureString _passattempt = new SecureString();
        public SecureString PassAttempt
        {
            get
            {
                return _passattempt;
            }

            set
            {
                if (_passattempt == value)
                {
                    return;
                }

                _passattempt = value;
                RaisePropertyChanged(PassAttemptPropertyName);
            }
        }


        public const string ReceipientAddressPropertyName = "ReceipientAddress";
        private string _receipientaddress = string.Empty;
        public string ReceipientAddress
        {
            get
            {
                return _receipientaddress;
            }

            set
            {
                if (_receipientaddress == value)
                {
                    return;
                }

                _receipientaddress = value;
                RaisePropertyChanged(ReceipientAddressPropertyName);
            }
        }


        public const string AmountBtcPropertyName = "AmountBtc";
        private string _amountbtc = string.Empty;
        public string AmountBtc
        {
            get
            {
                return _amountbtc;
            }

            set
            {
                if (_amountbtc == value)
                {
                    return;
                }
                _amountbtc = value;
                if (_isbtcfocused) { this.UpdateCurr(); }
                RaisePropertyChanged(AmountBtcPropertyName);
                
            }
        }


        public const string AmountCurrPropertyName = "AmountCurr";
        private string _amountcurr = string.Empty;
        public string AmountCurr
        {
            get
            {
                return _amountcurr;
            }

            set
            {
                
                if (_amountcurr == value)
                {
                    return;
                }
                _amountcurr = value;
                if (_iscurrfocused) { this.UpdateBtc(); }
                RaisePropertyChanged(AmountCurrPropertyName);
                
            }
        }


        public const string TempBalancePropertyName = "TempBalance";
        private Money _tempbalance;
        public Money TempBalance
        {
            get
            {
                return _tempbalance;
            }

            set
            {
                if (_tempbalance == value)
                {
                    return;
                }

                _tempbalance = value;
                RaisePropertyChanged(TempBalancePropertyName);
            }
        }


        public const string TempBalanceCurrPropertyName = "TempBalanceCurr";
        private decimal _tempbalancecurr;
        public decimal TempBalanceCurr
        {
            get
            {
                return _tempbalancecurr;
            }

            set
            {
                if (_tempbalancecurr == value)
                {
                    return;
                }

                _tempbalancecurr = value;
                RaisePropertyChanged(TempBalanceCurrPropertyName);
            }
        }


        public const string AmountBtcErrorPropertyName = "AmountBtcError";
        private bool _amountbtcerror = false;
        public bool AmountBtcError
        {
            get
            {
                return _amountbtcerror;
            }

            set
            {
                if (_amountbtcerror == value)
                {
                    return;
                }

                _amountbtcerror = value;
                RaisePropertyChanged(AmountBtcErrorPropertyName);
            }
        }

 
        public const string AmountCurrErrorPropertyName = "AmountCurrError";
        private bool _amountcurrerror = false;
        public bool AmountCurrError
        {
            get
            {
                return _amountcurrerror;
            }

            set
            {
                if (_amountcurrerror == value)
                {
                    return;
                }

                _amountcurrerror = value;
                RaisePropertyChanged(AmountCurrErrorPropertyName);
            }
        }
        

        public const string AmountFeePropertyName = "AmountFee";
        private Money _amountfee = Money.Zero;
        public Money AmountFee
        {
            get
            {
                return _amountfee;
            }

            set
            {
                if (_amountfee == value)
                {
                    return;
                }

                _amountfee = value;
                RaisePropertyChanged(AmountFeePropertyName);
            }
        }


        public const string AmountFeeCurrPropertyName = "AmountFeeCurr";
        private decimal _amountfeecurr = 0;
        public decimal AmountFeeCurr
        {
            get
            {
                return _amountfeecurr;
            }

            set
            {
                if (_amountfeecurr == value)
                {
                    return;
                }

                _amountfeecurr = value;
                RaisePropertyChanged(AmountFeeCurrPropertyName);
            }
        }

        private Money Balance
        {
            get;
            set;
        }

        public decimal BalanceCurr
        {
            get { return (Balance.ToDecimal(MoneyUnit.BTC) * Global.VML.Currency.ActivePrice); }
        }
  
        public SendViewModel()
        {            
            SendBtcCmd = new RelayCommand(() => { this.ExecuteSendBtc(); });
            BtcFocusedCmd = new RelayCommand(() => { this.ExecuteBtcFocused(); });
            CurrFocusedCmd = new RelayCommand(() => { this.ExecuteCurrFocused(); });
            MaxAmountCmd = new RelayCommand(() => { this.ExecuteMaxAmount(); });
            CopyExampleAddressCmd = new RelayCommand(() => { this.ExecuteCopyExampleAddress(); });
            CopyCurrBalanceCmd = new RelayCommand(() => { this.ExecuteCopyCurrBalance(); });
            Messenger.Default.Register<string>(this, "OpenSendView", (string s) => { this.OpenView(); });
        }


        public RelayCommand SendBtcCmd
        {
            get;
            private set;
        }
        public RelayCommand BtcFocusedCmd
        {
            get;
            private set;
        }
        public RelayCommand CurrFocusedCmd
        {
            get;
            private set;
        }
        public RelayCommand MaxAmountCmd
        {
            get;
            private set;
        }
        public RelayCommand CopyExampleAddressCmd
        {
            get;
            private set;
        }
        public RelayCommand CopyCurrBalanceCmd
        {
            get;
            private set;
        }

        
        private void OpenView()
        {
            this.ClearProps();
            _sview = new SendView();
            _wallet = Global.ActiveWallet;            
            Balance = _wallet.AvailableBalance;
            TempBalance = _wallet.AvailableBalance;
            TempBalanceCurr = Math.Round(BalanceCurr,2);

            _sview.ShowDialog();
        }

        private void ExecuteMaxAmount()
        {
            if (Balance.Satoshi == 0)
            {
                return;
            }
           
            var fee = _wallet.CalculateFee(_wallet.AvailableBalance);
            AmountFee = fee;
            AmountFeeCurr = Converters.Btc2Currency(fee);
            // subtract fee amount from available balance
            // to get amount of BTC to send
            Money btc = _wallet.AvailableBalance - AmountFee;
            AmountBtc = (btc).ToString();
            decimal newAmountCurr = Converters.Btc2Currency(btc);
            AmountCurr = newAmountCurr.ToString();
            this.UpdateTemps();
        }

        private void UpdateFee()
        {
            if (EnoughFunds(false) && !AmountBtc.Equals("0"))
            {
                var txBuilder = new TransactionBuilder();
                Money amountToSend = Money.Parse(AmountBtc);
                var fee = _wallet.CalculateFee(amountToSend);
                AmountFee = fee;
                AmountFeeCurr = Converters.Btc2Currency(fee);
            }
            else
            {
                AmountFee = Money.Zero;
            }

        }

        /// <summary>
        /// Sends BTC to the receipient address, as long as the following conditions are met:
        /// 1. Entered amount is Valid
        /// 2. There are sufficient funds
        /// 3. Receipient address is valid
        /// 4. Password is correct
        /// </summary>
        private async void ExecuteSendBtc()
        {
            
            if (AddressValid() && AmountBtcOK() && EnoughFunds() && PasswordCorrect())
            {
                Money AmountToSend = Money.Parse(AmountBtc);
                                
                var readyTx = _wallet.BuildTransaction(ReceipientAddress, AmountToSend, AmountFee);
                var networkChoice = Network.GetNetworkFromBase58Data(ReceipientAddress);
                string titleConfirm;
                string messageConfirm;
                //PROMPT USER FOR CONFIRMATION
                if (networkChoice.Equals(Network.TestNet))
                {
                    
                    titleConfirm = TextTools.RetrieveStringFromResource("Send_Dialog_Confirm_Title");
                    messageConfirm = TextTools.RetrieveStringFromResource("Send_Dialog_Confirm_Message_Testnet")
                        .Replace("*btc_amount*", AmountBtc)
                        .Replace("*btc_symbol*", Global.VML.UiSettings.Symbol)
                        .Replace("*address*", ReceipientAddress);

                }
                else
                {

                    titleConfirm = TextTools.RetrieveStringFromResource("Send_Dialog_Confirm_Title");
                    messageConfirm = TextTools.RetrieveStringFromResource("Send_Dialog_Confirm_Message_Mainnet")
                        .Replace("*btc_amount*", AmountBtc)
                        .Replace("*btc_symbol*", Global.VML.UiSettings.Symbol)
                        .Replace("*symbol*", Global.VML.Currency.ActiveSymbol)
                        .Replace("*curr_amount*", AmountCurr)
                        .Replace("*curr*", Global.VML.Currency.ActiveCurrency)
                        .Replace("*address*", ReceipientAddress);
                }
                
                var result = await _sview.ShowMessageAsync(titleConfirm, messageConfirm, MessageDialogStyle.AffirmativeAndNegative);
                if (result.Equals(MessageDialogResult.Affirmative))
                {
                    var sendAttempt = await _wallet.SendBitcoin(readyTx);
                    if (sendAttempt.Equals(true))
                    {
                        string titleSuccess = TextTools.RetrieveStringFromResource("Send_Dialog_Success_Title");
                        string messageSuccess = TextTools.RetrieveStringFromResource("Send_Dialog_Success_Message");
                        await _sview.ShowMessageAsync(titleSuccess, messageSuccess, MessageDialogStyle.Affirmative);
                        _sview.Close();
                    }
                    else
                    {
                        string titleFailed = TextTools.RetrieveStringFromResource("Send_Dialog_Unsuccess_Title");
                        string messageFailed = TextTools.RetrieveStringFromResource("Send_Dialog_Unsuccess_Message");
                        await _sview.ShowMessageAsync(titleFailed, messageFailed, MessageDialogStyle.Affirmative);
                    }
                        
                }
            }

        }

        private bool EnoughFunds(bool promptMessage = true)
        {
            Money amount = Money.Parse(AmountBtc);
            Money funds = Money.Parse(Balance.ToString());
            if (amount <= funds)
            {
                return true;
            }
            else if (promptMessage)
            {
                string title = TextTools.RetrieveStringFromResource("Send_Dialog_NotEnoughFunds_Title");
                string message = TextTools.RetrieveStringFromResource("Send_Dialog_NotEnoughFunds_Message");
                _sview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return false;
            }
            else
            {
                return false;
            }
            
        }

        private bool PasswordCorrect()
        {
            if (PassAttempt.Length == 0)
            {
                string title = TextTools.RetrieveStringFromResource("Send_Dialog_EmptyPass_Title");
                string message = TextTools.RetrieveStringFromResource("Send_Dialog_EmptyPass_Message");
                _sview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return false;
            }
            
                string _walletfile = Global.ActiveWallet.FileLocation;

                string passHash = Generators.GenerateHash(PassAttempt);

                var correct = FileTools.CheckPasswordAttempt(_walletfile, passHash); 
                if (correct)
                {
                    return true;
                }
                else
                {
                    string title = TextTools.RetrieveStringFromResource("Send_Dialog_IncorrectPass_Title");
                    string message = TextTools.RetrieveStringFromResource("Send_Dialog_IncorrectPass_Message");
                    _sview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                    return false;
                }                
           
            
        }

        private bool AddressValid()
        {
            if (String.IsNullOrWhiteSpace(ReceipientAddress))
            {
                string title = TextTools.RetrieveStringFromResource("Send_Dialog_EmptyAddr_Title");
                string message = TextTools.RetrieveStringFromResource("Send_Dialog_EmptyAddr_Message");
                _sview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return false;
            }
            else if (ReceipientAddress.Length < 33)
            {
                string title = TextTools.RetrieveStringFromResource("Send_Dialog_InvalidAddr_Title");
                string message = TextTools.RetrieveStringFromResource("Send_Dialog_InvalidAddr_Message");
                _sview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return false;
            }
            return true;
        }

        private void ExecuteBtcFocused()
        {
            _isbtcfocused = true;
            _iscurrfocused = false;
        }

        private void ExecuteCurrFocused()
        {
            _isbtcfocused = false;
            _iscurrfocused = true;
        }

        private void ExecuteCopyExampleAddress()
        {
            Clipboard.SetText("178adALA4BWNsyyZuNWutDMvQJYRmuLwrs");
        }

        private void ExecuteCopyCurrBalance()
        {
            Clipboard.SetText(TempBalanceCurr.ToString());
        }

        // Invoked when AmountCurr changes and
        // _iscurrfocused is true
        const int FEE_SIZE_KB = 1;
        private void UpdateBtc()
        {

            if (AmountCurr.Length == 0)
            {               
                AmountCurrError = false; // remove red border if there is one
                AmountBtc = 0.ToString();
                AmountFee = Money.Zero;
                this.UpdateTemps();
                return;
            }

            decimal val;            
            if (AmountCurrOK() && decimal.TryParse(AmountCurr, out val))
            {
                Money newAmountBtc = Converters.Currency2Btc(val);
                UpdateFee();
                AmountBtc = newAmountBtc.ToString();
                this.UpdateTemps();

            }
            else
            {
                AmountFee = Money.Zero;
                AmountBtc = 0.ToString();
                
            }
            
        }

        // Invoked when AmountBtc changes and
        // _isbtcfocused is true
        private void UpdateCurr()
        {

            if (AmountBtc.Length == 0)
            {
                AmountBtcError = false; // remove red border if there is one
                AmountCurr = "0";
                AmountFee = Money.Zero;
                return;
            }

            Money val;
            if (AmountBtcOK() && Money.TryParse(AmountBtc, out val))
            {

                
                decimal newAmountCurr = Converters.Btc2Currency(val);
                AmountCurr = newAmountCurr.ToString();
                UpdateFee();
                this.UpdateTemps();
            }
            else
            {
                AmountFee = Money.Zero;
                AmountCurr = "0";
            }
            
        }

        private void UpdateTemps()
        {
            if (AmountBtc.Equals("0") || AmountCurr.Equals("0") || String.IsNullOrEmpty(AmountBtc) || String.IsNullOrEmpty(AmountCurr))
            {
                TempBalance = Balance;
                TempBalanceCurr = Math.Round(BalanceCurr,2);
                return;
            }

            decimal remainBtc = Balance.ToDecimal(MoneyUnit.BTC) - decimal.Parse(AmountBtc) - AmountFee.ToDecimal(MoneyUnit.BTC);
            if (remainBtc >= 0)
            {
                TempBalance = Money.FromUnit(remainBtc, MoneyUnit.BTC);
            }
            else
            {                
                TempBalance = 0;
            }

            decimal remainCurr = BalanceCurr - decimal.Parse(AmountCurr) - AmountFeeCurr;
            if (remainCurr >= 0)
            {
                TempBalanceCurr = Math.Round(remainCurr,2);
            }
            else
            {
                
                TempBalanceCurr = 0;
            }
        }

        /// <summary>
        /// checks to see if the user entered a valid bitcoin amount
        /// ie) only 8 decimal places
        /// </summary>
        /// <returns>return true if the bitcoin amount is valid</returns>
        private bool AmountBtcOK()
        {
            // disallow negative values
            if (AmountBtc.Contains("-"))
            {
                AmountBtcError = true;
                return false;
            }
            // ie) "2.21" => ["2","21"]
            string[] seperate = AmountBtc.Split('.');
            
            if (seperate.Length == 2 && IsNumeric(seperate[0]) && IsNumeric(seperate[1]))
            {
                string postDecimal = seperate[1];
                if (postDecimal.Length <= 8)
                {
                    AmountBtcError = false;
                    return true;
                }
                else
                {
                    AmountBtcError = true;
                    return false;
                }
            }
            // no decimal entered, just integer
            else if (seperate.Length == 1 && IsNumeric(seperate[0]))
            {
                AmountBtcError = false;
                return true;
            }
            else
            {
                AmountBtcError = true;
                return false;
            }
        }

        /// <summary>
        /// checks to see if the user entered a valid currency amount
        /// ie) only 2 decimal places
        /// </summary>
        /// <returns>return true if the currency amount is valid</returns>
        private bool AmountCurrOK()
        {
            // ie) "2.21" => ["2","21"]
            string[] seperate = AmountCurr.Split('.');
            if (seperate.Length == 2 && IsNumeric(seperate[0]) && IsNumeric(seperate[1]))
            {
                string postDecimal = seperate[1];
                if (postDecimal.Length <= 2)
                {
                    AmountCurrError = false;
                    return true;
                }
                else
                {
                    AmountCurrError = true;
                    return false;
                }
            }
            // no decimal entered, just integer
            else if (seperate.Length == 1 && IsNumeric(seperate[0]))
            {
                AmountCurrError = false;
                return true;
            }
            else
            {
                AmountCurrError = true;
                return false;
            }
        }

        private bool IsNumeric(string arg)
        {
            uint _;
            if(uint.TryParse(arg,out _))
            {
                return true;
            }
            return false;
        }

        private void ClearProps()
        {
            this.AmountBtc = string.Empty;
            this.AmountBtcError = false;

            this.AmountCurr = string.Empty;
            this.AmountCurrError = false;

            this.AmountFee = Money.Zero;
            this.Balance = Money.Zero;
           
            this.ReceipientAddress = string.Empty;

            this.TempBalance = Money.Zero;
            this.TempBalanceCurr = 0;

            this.PassAttempt.Clear();

            _wallet = null;
            
        }

        private void CloseView()
        {                        
            _sview.Close();
        }

    }
}