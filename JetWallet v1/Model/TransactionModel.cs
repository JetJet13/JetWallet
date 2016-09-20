using NBitcoin;
using NBitcoin.SPV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet_v1.Tools;
using System.Diagnostics;
using System.ComponentModel;

namespace JetWallet_v1.Model
{
    public enum TxState
    {
        Unconfirmed,
        Awaiting,
        Confirmed
    }
    public enum TxAction
    {
        Sent,
        Received
    }
    /// <summary>
    /// TransactionModel is used to display/retrieve information regarding
    /// sent and received transactions
    /// </summary>
    public class TransactionModel : INotifyPropertyChanged
    {
        WalletTransaction _Transaction;

        public TransactionModel(WalletTransaction transaction)
        {
            _Transaction = transaction;
            BlockInfo = _Transaction.BlockInformation;
            BlockId = GetBlockId();
            Confirmations = GetConfs();
            State = GetTxState();
            Action = GetTxAction();


        }
        private BlockInformation _blockinfo;
        public BlockInformation BlockInfo
        {
            get { return _blockinfo; }
            set { _blockinfo = value; }
        }

        public string Id
        {
            get
            {
                return _Transaction.Transaction.GetHash().ToString();
            }
        }

        private TxState _state;
        public TxState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public string StateString
        {
            get { return TextTools.GetStateString(State); }
        }
        public decimal Balance
        {
            get { return _Transaction.Balance.ToDecimal(MoneyUnit.BTC); }
        }
        public string BalanceDisplay
        {
            get { return Balance.ToString() + " " + Global.VML.UiSettings.Symbol; }
        }
        public decimal AbsBalance
        {
            get
            {
                return _Transaction.Balance.Abs().ToDecimal(MoneyUnit.BTC);
            }
        }
        public decimal BalanceCurr
        {
            get { return Converters.Btc2Currency(_Transaction.Balance.Abs()); }
        }
        public string BalanceCurrDisplay
        {
            get
            {
                if (Global.VML.UiSettings.ShowCurrency)
                {
                    string value = Global.VML.Currency.ActiveSymbol + BalanceCurr.ToString("N") + " " + Global.VML.Currency.ActiveCurrency;
                    if (Balance < 0)
                    {
                        return "-" + value;
                    }
                    return " " + value;
                }
                return "";
            }
        }

        public Money Fee
        {
            get { return new Money(1000); }
        }

        private string _blockid;
        public string BlockId
        {
            get { return _blockid; }
            set { _blockid = value; }
        }

        
        private string _confirmations;
        public string Confirmations
        {
            get { return _confirmations; }
            set { _confirmations = value; }
        }
        public string ShortId
        {
            get
            {
                return Id.Substring(0, 16);
            }
        }

        public DateTimeOffset FirstSeen
        {
            get { return _Transaction.UnconfirmedSeen;}
        }

        public DateTimeOffset Date
        {
            get
            {
                TimeZoneInfo local = TimeZoneInfo.Local;
                return TimeZoneInfo.ConvertTime(_Transaction.AddedDate, local);
            }
        }

        public string DateDisplay
        {
            get { return TextTools.FormatFullDate(Date); }
        }

        private TxAction _action;
        public TxAction Action
        {
            get { return _action; }
            set { _action = value; }
        }
        public string ActionString
        {
            get { return TextTools.GetActionString(Action); }
        }
        public string Month
        {
            get { return TextTools.RetrieveStringFromResource(Date.ToString("MMMM")); }
        }
        public string Day
        {
            get { return Date.Day.ToString(); }
        }
        public string Year
        {
            get { return Date.Date.Year.ToString(); }
        }
        public string Message
        {
            get
            {

                if (Global.VML.UiSettings.ShowCurrency)
                {
                    return TextTools.RetrieveStringFromResource("Main_Tx_Message_Btc")
                 .Replace("*action*", ActionString)
                 .Replace("*amount*", AbsBalance.ToString())
                 .Replace("*symbol*", Global.VML.Currency.ActiveSymbol)
                 .Replace("*curramount*", BalanceCurr.ToString("N"))
                 .Replace("*curr*", Global.VML.Currency.ActiveCurrency);
                }
                else
                {
                    return TextTools.RetrieveStringFromResource("Main_Tx_Message_tBtc")
                 .Replace("*action*", ActionString)
                 .Replace("*amount*", AbsBalance.ToString());
                }
                               
            }
        }
        private TxAction GetTxAction()
        {
            if (_Transaction.Balance > 0)
            {
                return TxAction.Received;
            }
            return TxAction.Sent;
        }
        private string GetBlockId()
        {
            return _Transaction.BlockInformation == null ? null : _Transaction.BlockInformation.Header.GetHash().ToString();
        }

        private string GetConfs()
        {
            return (_Transaction.BlockInformation == null ? 0 : _Transaction.BlockInformation.Confirmations).ToString("N0");
        }

        private TxState GetTxState()
        {
            if (_Transaction.BlockInformation != null)
            {
                if (_Transaction.BlockInformation.Confirmations >= 6)
                {
                    return TxState.Confirmed;
                }
                else
                {
                    return TxState.Awaiting;
                }
            }
            return TxState.Unconfirmed;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
