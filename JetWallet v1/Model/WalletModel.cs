using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Windows.Media;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using JetWallet.Tools;
using NBitcoin;
using NBitcoin.Protocol;
using NBitcoin.SPV;
using NBitcoin.Protocol.Behaviors;
using QBitNinja.Client;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using Base58Check;
using JetWallet.ViewModel;
using System.Collections.ObjectModel;

namespace JetWallet.Model
{

    public enum WalletModelState
    {
        Closing,
        NotReady,
        Preparing,
        Ready
    }
    public enum WalletConnectionState
    {
        NotConnected,
        Connecting,
        Connected
    }
    public enum WalletSecurity
    {
        Lock,
        Unlock
    }


    public abstract class WalletBase : Wallet
    {
        /// <summary>Wallet Base contains properties and methods that deal with the wallet
        /// basics.</summary>
        public uint _private_key_pool_size = 1000;
        // Constructor
        public WalletBase(BitcoinExtPubKey rootKey) : base(rootKey, 1000) { }

        // Properties
        private string _displayName = string.Empty;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        private SecureString _pass;
        private string _passhash;
        public string PassHash
        {
            get { return _passhash; }
            set { _passhash = value; }
        }

        private string _recoveryphrase;
        private string _recoveryphrasehash;
        public string RecoveryPhraseHash
        {
            get { return _recoveryphrasehash; }
            set { _recoveryphrasehash = value; }
        }
        
        private string _desc = string.Empty;
        public string Desc
        {
            get { return _desc; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _desc = "No Wallet Description";
                    return;
                }
                _desc = value;
            }
        }


        private WalletModelState _currentstate;
        public WalletModelState CurrentState
        {
            get { return _currentstate; }
            private set
            {
                if (_currentstate == value)
                {
                    return;
                }
                _currentstate = value;
                Messenger.Default.Send<WalletModelState>(_currentstate, "WalletStateChanged");           

            }
        }

        private WalletConnectionState _connState;
        private WalletConnectionState ConnState
        {
            get { return _connState; }
            set
            {
                if (_connState == value)
                {
                    return;
                }
                _connState = value;
                Messenger.Default.Send<string>("", "WalletConnStateChanged");
            }
        }


        private WalletSecurity _security;
        private WalletSecurity Security
        {
            get { return _security; }
            set { _security = value; }
        }

        private Network _network;
        public Network NetworkChoice
        {
            get { return _network; }
            set { _network = value; }
        }

        protected string _folder;
        private string _filelocation;
        public string FileLocation
        {
            get { return _filelocation; }
            set { _filelocation = value; }
        }

        private string _filerecover;
        public string FileRecover
        {
            get { return _filerecover; }
            set { _filerecover = value; }
        }

        
        //methods
        public void SetRecoveryPhrase(string s)
        {
            _recoveryphrase = s;
            RecoveryPhraseHash = Generators.GenerateRecPhraseHash(s);
        }
        public void SetWalletPassword(SecureString s)
        {
            _pass = s;
            _passhash = Generators.GenerateHash(s);
            
        }

        public string GetWalletPassHash()
        {
            return _passhash;
        }

        public WalletConnectionState GetWalletConnState()
        {
            return ConnState;
        }

        public void SetWalletConnState(WalletConnectionState newState)
        {
            ConnState = newState;
        }

        public WalletModelState GetWalletState()
        {
            return CurrentState;
        }

        public void SetWalletState(WalletModelState newState)
        {
            CurrentState = newState;
        }

        public WalletSecurity GetWalletSecurity()
        {
            return Security;
        }

        public void SetWalletSecurity(WalletSecurity val)
        {
            Security = val;
        }

        public bool IsLocked()
        {
            if (Security == WalletSecurity.Lock)
            {
                return true;
            }
            return false;
        }

        
       
    }

    public class WalletModel : WalletBase, INotifyPropertyChanged
    {
        private WalletConnectModel _wconnect;
        const int MIN_NUM_CONFS = 6;
        const int NUM_RECENT_TX_SHOW = 4;
        const int MAX_NUM_CONNECTIONS = 24;
        
        // Wallet Master Key
        private ExtKey _masterkey;

        private readonly ObservableCollection<WalletKey> _keys = new ObservableCollection<WalletKey>();
        public ObservableCollection<WalletKey> Keys
        {
            get { return _keys; }
        }

        public List<Coin> _spendableOutputs = new List<Coin>();

        
        //List of Transactions
        private WalletTransactionsCollection _txs;
        public WalletTransactionsCollection Txs
        {
            get { return _txs; }
            set
            {
                if (_txs == value)
                {
                    return;
                }
                
                _txs = value;
                OnPropertyChanged("Txs");
                

                
                
            }
        }

        private List<TransactionModel> _txcollection = new List<TransactionModel>();
        public List<TransactionModel> TxCollection
        {
            get { return _txcollection; }
            set
            {
                if (_txcollection == value)
                {
                    return;
                }
                
                _txcollection = value;
                OnPropertyChanged("TxCollection");
                
            }
        }

        // Used to databind to MainWindow ItemControl
        private List<TransactionModel> _txlist;
        public List<TransactionModel> TxList
        {
            get { return _txlist; }
            set
            {
                _txlist = value;
                OnPropertyChanged("TxList");
            }
        }

        private int _numtxs = 0;
        public int NumTxs
        {
            get { return _numtxs; }
            set
            {
                if (_numtxs == value)
                {
                    return;
                }
                _numtxs = value;
                OnPropertyChanged("NumTxs");                
            }
        }

        public int NumSentTxs
        {
            get { return this.GetAllSentTransactions().Length; }
        }

        public int NumReceivedTxs
        {
            get { return this.GetAllReceiveTransactions().Length; }
        }

        /// <summary>
        /// stores the true balance of the wallet
        /// Balance = sum of outputs - sum of inputs
        /// </summary>
        private Money _balance = Money.Zero;
        private Money Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        /// <summary>
        ///stores the sum of unspent outputs; Used for sending bitcoin 
        /// </summary>
        private Money _availablebalance = Money.Zero;
        public Money AvailableBalance
        {
            get { return _availablebalance; }
            set
            {
                _availablebalance = value;
                OnPropertyChanged("AvailableBalance");
            }
        }

        private decimal _availablebalancedisplay = 0;
        public decimal AvailableBalanceDisplay
        {
            get { return _availablebalancedisplay; }
            set
            {
                _availablebalancedisplay = value;
                OnPropertyChanged("AvailableBalanceDisplay");
            }
        }

        public string AvailableBalanceCurrDisplay
        {
            get;
            set;
        }

        /// <summary>
        ///stores the sum of outputs from unconfirmed txs. 
        /// </summary>
        private Money _unconfirmedbalance = Money.Zero;
        public Money UnconfirmedBalance
        {
            get { return _unconfirmedbalance; }
            private set { _unconfirmedbalance = value; OnPropertyChanged("UnconfirmedBalance"); }
        }

        private decimal _unconfirmedbalancedisplay = 0;
        public decimal UnconfirmedBalanceDisplay
        {
            get { return _unconfirmedbalancedisplay; }
            set
            {
                _unconfirmedbalancedisplay = value;
                OnPropertyChanged("UnconfirmedBalanceDisplay");
            }
        }

        public string UnconfirmedBalanceCurrDisplay
        {
            get;
            set;
        }

        
        private decimal _balancedisplay = 0;
        public decimal BalanceDisplay
        {
            get { return _balancedisplay; }
            set
            {
                _balancedisplay = value;
                OnPropertyChanged("BalanceDisplay");
            }
        }

        private string _balancecurrdisplay = "0";
        public string BalanceCurrDisplay
        {
            get { return _balancecurrdisplay; }
            set
            {
                _balancecurrdisplay = value;
                OnPropertyChanged("BalanceCurrDisplay");
            }
        }   

        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set
            {
                if (_connected == value)
                {
                    return;
                }
                _connected = value;
                OnPropertyChanged("Connected");
                this.ConfigureConnect();
                //Messenger.Default.Send<bool>(_connected, "WalletConnectionChanged");
            }
        }
        
        private int _currentheight = 0;
        public int CurrentHeight
        {
            get { return _currentheight; }
            set { _currentheight = value; OnPropertyChanged("CurrentHeight"); }
        }

        private int _connectednodes = 0;
        public int ConnNodes
        {
            get { return _connectednodes; }
            set { _connectednodes = value; OnPropertyChanged("ConnNodes"); }
        }

        /// <summary>
        /// The <see cref="IsLoading" /> property's name.
        /// </summary>
        public const string IsLoadingPropertyName = "IsLoading";

        private bool _isloading = true;

        /// <summary>
        /// Sets and gets the IsLoading property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return _isloading;
            }

            set
            {
                if (_isloading == value)
                {
                    return;
                }

                _isloading = value;
                OnPropertyChanged(IsLoadingPropertyName);
            }
        }
        // Constructor - NBitcoin.SPV Wallet Class requires a WalletCreation Class as parameter
        public WalletModel(string id, string newName, BitcoinExtPubKey rootKey, DateTimeOffset newCreated, string description="No Wallet Description"):base(rootKey)
        {
            
            this.CreationSettings.Name = id;
            this.CreationSettings.DerivationPath = new KeyPath();            
            this.CreationSettings.PurgeConnectionOnFilterChange = false;
            
            DisplayName = newName;
            _folder = Path.Combine(App.WalletsDir, this.Name);
            FileLocation = Path.Combine(_folder, "wallet.jet");
            FileRecover = Path.Combine(_folder, "wallet.recover.jet");
            Desc = description;
            this.Created = newCreated;
            this.SetWalletSecurity(WalletSecurity.Unlock);
            this.SetWalletState(WalletModelState.NotReady);
            this.SetWalletConnState(WalletConnectionState.NotConnected);
            NetworkChoice = rootKey.Network;            
            this.NewWalletTransaction += new NewWalletTransactionDelegate(CheckNewTransaction);
            
        }

        public void Initialize()
        {
            this.SetWalletState(WalletModelState.Preparing);
            _wconnect = new WalletConnectModel(this);
            Logger.WriteLine("WalletModel::Initialize => " + this.DisplayName);
 
        }
        
        private async void ConfigureConnect()
        {
            if (Connected)
            {
               await Task.Factory.StartNew(() => {

                    InitializeWalletKeys();
                    UpdateWalletData();
                    FetchTxs();
                    this.SetWalletState(WalletModelState.Ready);
                              
                });   
                
            }
        }

        public WalletConnectModel GetWalletConnect()
        {
            return _wconnect;
        }
        public ExtKey GetMasterKey()
        {
            return _masterkey;
        }

        public bool SetMasterKey(ExtKey key)
        {
            if (_masterkey == null)
            {
                _masterkey = key;
                return true;

            }
            Logger.WriteLine("WalletModel::SetMasterKey::Error => Attempting to set master key after it has already been set.");
            return false;
        }

        private void InitializeWalletKeys()
        {

                this.GetNextScriptPubKey();

                uint numKnownScripts = (uint)this.GetKnownScripts().Length;
                for (uint index = 0; index < numKnownScripts; index++)
                {
                    var key = _masterkey.Derive(0).Derive(index);

                    KeyPath keypath = new KeyPath(0, index);
                    WalletKey newKey = new WalletKey(_masterkey, keypath, NetworkChoice);
                    Keys.Add(newKey);

                }
            
        }
        public PubKey GetNewPubKey()
        {

            Logger.WriteLine("WalletModel::GetNewPubKey");

            
            var newPubKeyIndex = (uint)Keys.Count((key) => key.Consumed.Equals(true));
            Logger.WriteLine("newPubKeyIndex => " + newPubKeyIndex);
            if (newPubKeyIndex >= _private_key_pool_size)
            {
                Logger.WriteLine("newPubKeyIndex is greater than private_key_pool_size...");
                Logger.WriteLine("generating new walletkey");
                this.GenerateNextWalletKey();
            }
            
            var newPubKey = Keys.First((key) => key.MatchKeyPath(newPubKeyIndex)).PublicKey;
            return newPubKey;

            
        }
        
        private void GenerateNextWalletKey()
        {
            this.GetNextScriptPubKey();
            var newIndex = _private_key_pool_size;
            KeyPath keypath = new KeyPath(0, newIndex);
            WalletKey newKey = new WalletKey(_masterkey, keypath, NetworkChoice);
            Keys.Add(newKey);
            
            _private_key_pool_size++;
        }

        private void GeneratePubKeys()
        {
            
            foreach (var tx in this.GetTransactions())
            {
                
                foreach (var input in tx.SpentCoins)
                {
                    var script = input.ScriptPubKey;
                    var scriptAddress = script.GetDestinationAddress(this.NetworkChoice);
                    
                    this.CheckScriptExists(script);
                    Keys.First((key) => key.MatchPublicKey(script)).ConsumeKey();
                   
                }
                foreach (var output in tx.ReceivedCoins)
                {
                    var script = output.ScriptPubKey;
                    var scriptAddress = script.GetDestinationAddress(this.NetworkChoice);
                    
                    this.CheckScriptExists(script);
                    Keys.First((key) => key.MatchPublicKey(script)).ConsumeKey();
                    
                }
            }
           
           
        }

        private void CheckScriptExists(Script script)
        {
           
            var exists = Keys.Any((k) => k.MatchPublicKey(script));
            
            if (exists)
            {
                return;
            }
           
            while (!exists)
            {
                this.GenerateNextWalletKey();
                exists = Keys.Any((k) => k.MatchPublicKey(script));
            }

        }

        private ExtKey GetPrivateKey(Script scriptpubkey)
        {
            var scriptAddress = scriptpubkey.GetDestinationAddress(this.NetworkChoice);
            var privKey = Keys.First((key) => key.MatchPublicKey(scriptpubkey)).PrivKey;
            if (privKey == null)
            {

                Logger.WriteLine("Private Key could not be located in wallet");
                throw new Exception("Private Key could not be located in Wallet");

            }
            return privKey;
            
        }
               
        

        public List<PubKey> GetAllPubKeys()
        {
            Logger.WriteLine("WalletModel::GetAllPubKeys");
            return Keys.Where((k) => k.Consumed == true).Select((k) => k.PublicKey).ToList();
        }

        public Money GetBalance()
        {
            Logger.WriteLine("WalletModel::GetBalance");
            return Balance;
        }

        // reinstantiate TxList to invoke OnPropertyChanged so the language will update
        public void UpdateTxListLanguage()
        {
            Logger.WriteLine("WalletModel::UpdateTxListLanguage");
            this.PurgeTxList();
            this.UpdateTxList();
        }

        private void UpdateTxList()
        {
            
            this.TxCollection = this.Txs.OrderByDescending(x => x.AddedDate.LocalDateTime).Select(x => new TransactionModel(x)).ToList();
            this.TxList = this.TxCollection.Take(NUM_RECENT_TX_SHOW).ToList();
        }

        private void PurgeTxList()
        {
            Logger.WriteLine("WalletModel::PurgeTxList");
            this.TxList = new List<TransactionModel>();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            WalletModel compareWallet = obj as WalletModel;
            if ((System.Object)compareWallet == null)
            {
                return false;
            }

            return (compareWallet.FileLocation == FileLocation) && (compareWallet.Name == Name) && (compareWallet.PassHash == PassHash);
        }

        const int SECOND_DELAY = 2500;
        private async void FetchTxs()
        {
            while (Connected)
            {
               await Task.Delay(SECOND_DELAY);
               if (!Connected) break;
                
                this.UpdateWalletData();
            }

        }

        public string GetWalletFolderPath()
        {
            return _folder;
        }

        public string LogFile()
        {
            return Path.Combine(_folder, "debug.log");
        }

        private void GenerateSpendableOutputs()
        {
            _spendableOutputs.Clear();        
            foreach (var output in this.Txs.GetSpendableCoins())
            {
                var address = output.ScriptPubKey.GetDestinationAddress(this.NetworkChoice);                
                var tx = this.Txs.First((x) => x.Transaction.GetHash().Equals(output.Outpoint.Hash));
                if (tx.BlockInformation != null && tx.BlockInformation.Confirmations >= MIN_NUM_CONFS)
                {                    
                    _spendableOutputs.Add(output);
                }
            }
        }     

        
        private void GenerateWalletBalances()
        {
            Balance = Money.Zero;
            AvailableBalance = Money.Zero;
            UnconfirmedBalance = Money.Zero;
            var confirmedTxs = this.GetConfirmedTransactions();
            var unconfirmedTxs = this.GetUnconfirmedTransactions();
            var spendableOutputs = _spendableOutputs.ToArray();


            UnconfirmedBalance = unconfirmedTxs.Select((x) => x.ReceivedCoins.Select((r) => r.Amount).Sum()).Sum();

            AvailableBalance = spendableOutputs.Select((x) => x.Amount).Sum();

            Balance = this.Txs.Select((x) => x.Balance).Sum();

            BalanceDisplay = Balance.ToDecimal(MoneyUnit.BTC);
            AvailableBalanceDisplay = AvailableBalance.ToDecimal(MoneyUnit.BTC);
            UnconfirmedBalanceDisplay = UnconfirmedBalance.ToDecimal(MoneyUnit.BTC);

            this.UpdateBalanceCurrDisplay();
        }

        public void UpdateBalanceCurrDisplay()
        {
            decimal currValue = BalanceDisplay * Global.VML.Currency.ActivePrice;
            BalanceCurrDisplay = String.Format("{0}{1} {2}", Global.VML.Currency.ActiveSymbol, currValue.ToString("N"), Global.VML.Currency.ActiveCurrency);

            decimal currValueAvailable = AvailableBalanceDisplay * Global.VML.Currency.ActivePrice;
            AvailableBalanceCurrDisplay = String.Format("{0}{1} {2}", Global.VML.Currency.ActiveSymbol, currValueAvailable.ToString("N"), Global.VML.Currency.ActiveCurrency);

            decimal currValueUnconfirmed = UnconfirmedBalanceDisplay * Global.VML.Currency.ActivePrice;
            UnconfirmedBalanceCurrDisplay = String.Format("{0}{1} {2}", Global.VML.Currency.ActiveSymbol, currValueUnconfirmed.ToString("N"), Global.VML.Currency.ActiveCurrency);
        }

        public WalletTransaction[] GetConfirmedTransactions()
        {            
            List<WalletTransaction> confirmedTxs = new List<WalletTransaction>();
            
            foreach (WalletTransaction tx in Txs)
            {
                
                if (tx.BlockInformation != null && tx.BlockInformation.Confirmations >= MIN_NUM_CONFS)
                {
                    confirmedTxs.Add(tx);
                }
            }
            return confirmedTxs.ToArray();

        }

        public WalletTransaction[] GetUnconfirmedTransactions()
        {            
            List<WalletTransaction> unconfirmedTxs = new List<WalletTransaction>();

            foreach (WalletTransaction tx in Txs)
            {

                if (tx.BlockInformation == null)
                {
                    unconfirmedTxs.Add(tx);
                }
                else if (tx.BlockInformation != null && tx.BlockInformation.Confirmations < MIN_NUM_CONFS)
                {
                    unconfirmedTxs.Add(tx);
                }
            }
            return unconfirmedTxs.ToArray();

        }

        public WalletTransaction[] GetAllReceiveTransactions()
        {            
            List<WalletTransaction> receiveTxs = new List<WalletTransaction>();
           
            foreach (WalletTransaction tx in Txs)
            {

                if (tx.Balance > 0)
                {
                    receiveTxs.Add(tx);
                }
            }
            return receiveTxs.ToArray();
        }

        public WalletTransaction[] GetConfirmedReceiveTransactions()
        {           
            List<WalletTransaction> receiveTxs = new List<WalletTransaction>();
            var confirmedTxs = this.GetConfirmedTransactions();
            foreach (WalletTransaction tx in confirmedTxs)
            {
                
                if (tx.Balance > 0)
                {
                    receiveTxs.Add(tx);
                }
            }
            return receiveTxs.ToArray();
        }

        public WalletTransaction[] GetUnconfirmedReceiveTransactions()
        {           
            List<WalletTransaction> receiveTxs = new List<WalletTransaction>();
            var unconfirmedTxs = this.GetUnconfirmedTransactions();
            foreach (WalletTransaction tx in unconfirmedTxs)
            {

                if (tx.Balance > 0)
                {
                    receiveTxs.Add(tx);
                }
            }
            return receiveTxs.ToArray();
        }


        public WalletTransaction[] GetAllSentTransactions()
        {            
            List<WalletTransaction> sendTxs = new List<WalletTransaction>();
            
            foreach (WalletTransaction tx in Txs)
            {

                if (tx.Balance < 0)
                {
                    sendTxs.Add(tx);
                }
            }
            return sendTxs.ToArray();
        }

        public WalletTransaction[] GetConfirmedSentTransactions()
        {            
            List<WalletTransaction> sendTxs = new List<WalletTransaction>();
            var confirmedTxs = this.GetConfirmedTransactions();
            foreach (WalletTransaction tx in confirmedTxs)
            {
                
                if ( tx.Balance < 0)
                {
                    sendTxs.Add(tx);
                }
            }
            return sendTxs.ToArray();
        }

        public WalletTransaction[] GetUnconfirmedSentTransactions()
        {            
            List<WalletTransaction> sendTxs = new List<WalletTransaction>();
            var unconfirmedTxs = this.GetUnconfirmedTransactions();
            foreach (WalletTransaction tx in unconfirmedTxs)
            {
                if (tx.Balance < 0)
                {
                    sendTxs.Add(tx);
                }
            }
            return sendTxs.ToArray();
        }

        public Transaction BuildTransaction(string address, Money amount, Money feeAmount)
        {            
            Logger.WriteLine("WalletModel::BuildTransaction");
            var receipient = new BitcoinPubKeyAddress(address);            
            Logger.WriteLine("receipient => " + receipient.ToString());

            var feeRate = Generators.GetTxFeeRate();
            Transaction newTx = new Transaction();
            List<ISecret> keys = new List<ISecret>();
            List<Coin> coins = new List<Coin>();
            var unspentOutputs = _spendableOutputs.ToArray();
            var amountToSend = amount + feeAmount;
            foreach (Coin output in unspentOutputs)
            {                               
                if (amountToSend <= output.Amount)
                {
                    coins.Add(output);
                    ExtKey privateKey = this.GetPrivateKey(output.TxOut.ScriptPubKey);
                    keys.Add(privateKey);
                    break;

                }
                else
                {
                    coins.Add(output);
                    ExtKey privateKey = this.GetPrivateKey(output.TxOut.ScriptPubKey);
                    keys.Add(privateKey);
                    amountToSend -= output.Amount;
                    
                }


            }
            
            ISecret[] readyKeys = keys.ToArray();
            Coin[] readyCoins = coins.ToArray();

            var newPubKey = this.GetNewPubKey();
            var txBuilder = new TransactionBuilder();
            
            var readyTx = txBuilder
                    .AddCoins(readyCoins)
                    .AddKeys(readyKeys)
                    .Send(receipient, amount)
                    .SetChange(newPubKey.Hash.ScriptPubKey)
                    .SendFees(feeAmount)
                    .BuildTransaction(true);            

            Logger.WriteLine("tx raw hex => " + readyTx.ToHex());
            if (txBuilder.Verify(readyTx))
            {
                Logger.WriteLine("returning tx => " + readyTx.GetHash());
                return readyTx;
            }
            Logger.WriteLine("Transaction is not valid");
            throw new Exception("Transaction is not valid.");
            
        }
                
        public async Task<bool> SendBitcoin(Transaction tx)
        {
            Logger.WriteLine("WalletModel::SendBitcoin");
            var result = await this.BroadcastTransactionAsync(tx);
            if (result != null)
            {
                Logger.WriteLine("Transaction did not broadcast..." + result.Reason);
                Logger.WriteLine("result => " + result.Code.ToString());
                return false;
            }     
                    
            Logger.WriteLine("SUCCESSFULY BROADCASTED TRANSACTION => " + tx.GetHash());
            _wconnect.PurgeNodes();
            return true;
            
        }

        public Money CalculateFee(Money amount)
        {            
            Logger.WriteLine("WalletModel::CalculateFee");
            var feeRate = Generators.GetTxFeeRate();
            Logger.WriteLine("fee rate (satoshi/KB) => " + feeRate.ToString());
            Transaction newTx = new Transaction();
            List<ISecret> keys = new List<ISecret>();
            List<Coin> coins = new List<Coin>();
            var unspentOutputs = _spendableOutputs.ToArray();
            var amountToSend = amount;
            
            foreach (Coin output in unspentOutputs)
            {               
                if (amountToSend <= output.Amount)
                {
                    coins.Add(output);
                    ExtKey privateKey = this.GetPrivateKey(output.TxOut.ScriptPubKey);
                    keys.Add(privateKey);
                    break;

                }
                else
                {
                    coins.Add(output);
                    ExtKey privateKey = this.GetPrivateKey(output.TxOut.ScriptPubKey);
                    keys.Add(privateKey);
                    amountToSend -= output.Amount;

                }


            }
            
            ISecret[] readyKeys = keys.ToArray();
            Coin[] readyCoins = coins.ToArray();

                        
            var txBuilder = new TransactionBuilder();
            // mock recipeint address
            var receipient = new BitcoinPubKeyAddress("mwCwTceJvYV27KXBc3NJZys6CjsgsoeHmf");
            var changeAddress = Keys.Last().PublicKey.Hash.ScriptPubKey;
            var prepTx = txBuilder
                    .AddCoins(readyCoins)
                    .AddKeys(readyKeys)
                    .Send(receipient,amount)
                    .SetChange(changeAddress)
                    .BuildTransaction(true);

            var estFee = txBuilder.EstimateFees(prepTx, feeRate);            
            var txSize = txBuilder.EstimateSize(prepTx);
            Logger.WriteLine("estimated tx size => " + txSize);
            decimal decFee = txSize * feeRate.FeePerK.Satoshi;
            var txFee = Money.FromUnit(decFee, MoneyUnit.Satoshi);
            Logger.WriteLine("estimated tx fee => " + txSize);
            return Generators.GenerateOptimalFee(txFee);
        }

        private void UpdateWalletData()
        {
            this.Txs = this.GetTransactions();
            this.NumTxs = this.Txs.Count;
            this.UpdateTxList();
            this.GenerateSpendableOutputs();
            this.GeneratePubKeys();
            this.GenerateWalletBalances();
            this.EvaluateWalletConnState();
            this.UpdateStatusProps();
        }

        private void CheckNewTransaction(Wallet w, WalletTransaction newTx)
        {
           
            if (this.Txs == null)
            {                
                return;
            }
            
            this.UpdateWalletData();
            Money val = newTx.Balance;          

            if (val < 0)
            {
                // SENT BITCOIN
                string amount = val.Abs().ToUnit(MoneyUnit.BTC).ToString();               
            }
            else
            {
                // RECEIVED BITCOIN
                string amount = val.ToUnit(MoneyUnit.BTC).ToString();
                TimeSpan timespan = DateTimeOffset.UtcNow - newTx.AddedDate;
                const int TIMESPAN_TOLERANCE = 5; // in seconds
                if (timespan.Seconds <= TIMESPAN_TOLERANCE)
                {                  
                    App.Current.Dispatcher.Invoke(() => Messenger.Default.Send<string>(amount, "NewReceivedTransactionFound"));                 
                }
                
            }
            this.GeneratePubKeys();

        }

        private void UpdateStatusProps()
        {
            CurrentHeight = this.Chain.Height;
            ConnNodes = this.ConnectedNodes;
        }

        private void EvaluateWalletConnState()
        {
            if (CurrentHeight == this.Chain.Height && this.ConnectedNodes > 1)
            {
                this.SetWalletConnState(WalletConnectionState.Connected);
                this.IsLoading = false;
            }
            else if (CurrentHeight < this.Chain.Height && this.ConnectedNodes >= 1)
            {
                this.SetWalletConnState(WalletConnectionState.Connecting);
                this.IsLoading = false;
            }
            else
            {
                this.SetWalletConnState(WalletConnectionState.NotConnected);
                this.IsLoading = true;
            }
        }

        private void SaveChanges()
        {
            FileTools.UpdateWalletFile(this);
        }

        // used when user closes the wallet
        // or changes the active wallet
        public void Close()
        {
            Task.Factory.StartNew(() => {

                this.SetWalletState(WalletModelState.Closing);
                this.SaveChanges();
                _wconnect.Stop();
                this.Disconnect();
                this.Connected = false;

            });

        }
        // only used when app is closing
        public void Stop()
        {

            this.SetWalletState(WalletModelState.Closing);
            this.Disconnect();
            this.Connected = false;
            _wconnect.Stop();
        }

        public void RepairWallet()
        {
            _wconnect.PurgeNodes();
            this.InitializeWalletKeys();
            this.UpdateWalletData();
        }

        public void LogWallet()
        {
            string path = Path.Combine(_folder, "wallet.log");
            using (var fs = File.Create(path))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    var numKeys = Keys.Count((k) => k.Consumed.Equals(true));
                    writer.WriteLine("Wallet Id: " + this.Name);
                    writer.WriteLine("Wallet Name: " + this.DisplayName);
                    writer.WriteLine("Wallet Balance: " + this.BalanceDisplay + " BTC");
                    writer.WriteLine("Wallet Network: " + this.NetworkChoice);
                    writer.WriteLine("Wallet Folder: " + _folder);
                    writer.WriteLine("Num. Private Keys: " + numKeys);
                    writer.WriteLine(Environment.NewLine);
                    writer.WriteLine("Public Keys");
                    writer.WriteLine("Num. Public Keys: " + numKeys);
                    writer.WriteLine("=========================");
                    writer.WriteLine(Environment.NewLine);
                    foreach (var key in Keys.Where((k)=>k.Consumed.Equals(true)))
                    {
                        writer.WriteLine("Pub. Key: " + key.PublicKey.ToHex());
                        writer.WriteLine("Address: " + key.Address);
                        writer.WriteLine(Environment.NewLine);
                    }
                    writer.WriteLine("=========================");
                    writer.WriteLine(Environment.NewLine);
                    writer.WriteLine("Spendable Outputs");
                    writer.WriteLine("=========================");
                    writer.WriteLine(Environment.NewLine);
                    foreach (var output in _spendableOutputs)
                    {
                        writer.WriteLine("Amount: " + output.Amount.ToString() + " || " + "Address: " + output.ScriptPubKey.GetDestinationAddress(this.NetworkChoice));
                    }
                    writer.WriteLine("=========================");
                    writer.WriteLine(Environment.NewLine);
                    writer.WriteLine("Transactions");
                    writer.WriteLine("Num. Transactions: " + this.Txs.Count);
                    writer.WriteLine("Num. Sent Transactions: " + this.GetAllSentTransactions().Length);
                    writer.WriteLine("Num. Received Transactions: " + this.GetAllReceiveTransactions().Length);
                    writer.WriteLine("=========================");
                    writer.WriteLine(Environment.NewLine);
                    foreach (var tx in this.Txs)
                    {
                        writer.WriteLine("Tx. Hash: " + tx.Transaction.GetHash());
                        writer.WriteLine("Timestamp: " + TextTools.FormatFullDate(tx.UnconfirmedSeen));
                        writer.WriteLine("Value: " + tx.Balance.ToString() + " BTC");
                        writer.WriteLine(Environment.NewLine);
                    }
                    writer.WriteLine("=========================");
                    writer.WriteLine("END OF WALLET LOG");
                    writer.Close();
                }
            }
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
