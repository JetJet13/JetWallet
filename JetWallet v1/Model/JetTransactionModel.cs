using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.SPV;
using JetWallet.Tools;


namespace JetWallet.Model
{
    public interface ITransaction
    {
        string Id { get; }
        int BlockHeight { get; }
        string BlockHash { get; }
        TxAction Action { get; }
        TxState State { get; }
        int Confirmations { get; }
        DateTime Date { get; }
        Money AmountBTC { get; }
        decimal AmountCurr { get; }
        
    }

    public class JetTransactionModel : ITransaction
    {
        private const int MIN_NUM_CONFIRMATIONS = 6;
        private WalletTransaction _tx;

        public JetTransactionModel(WalletTransaction wtx)
        {
            _tx = wtx;
        }


        public TxAction Action
        {
            get
            {
                if (_tx.Balance > 0)
                {
                    return TxAction.Received;
                }
                return TxAction.Sent;
            }
        }

        public Money AmountBTC
        {
            get
            {
                return _tx.Balance;
            }
        }

        public decimal AmountCurr
        {
            get
            {
                return JetMoneyTools.CalcBtc2Curr(_tx.Balance);
            }
        }

        public string BlockHash
        {
            get
            {
                if (InsideBlock)
                {
                    return _tx.BlockInformation.Header.GetHash().ToString();
                }
                return "NA";
            }
        }

        public int BlockHeight
        {
            get
            {
                if (InsideBlock)
                {
                    return _tx.BlockInformation.Height;
                }
                return -1;
            }
        }

        public int Confirmations
        {
            get
            {
                if (InsideBlock)
                {
                    return _tx.BlockInformation.Confirmations;
                }
                return 0;
            }
        }

        public DateTime Date
        {
            get
            {
                if (InsideBlock)
                {
                    return _tx.BlockInformation.Header.BlockTime.Date;
                }
                return _tx.AddedDate.Date;
            }
        }

        public string Id
        {
            get
            {
                return _tx.Transaction.GetHash().ToString();
            }
        }

        public TxState State
        {
            get
            {
                if (InsideBlock)
                {
                    if (_tx.BlockInformation.Confirmations >= MIN_NUM_CONFIRMATIONS)
                    {
                        return TxState.Confirmed;
                    }
                    return TxState.Awaiting;
                }
                return TxState.Unconfirmed;
            }
        }

        private bool InsideBlock
        {
            get
            {
                if (_tx.BlockInformation != null)
                {
                    return true;
                }
                return false;

            }  
        }
    }
}
