using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.SPV;
using JetWallet.Tools;
using NBitcoin.Protocol;

namespace JetWallet.Model
{
   public interface IWallet
    {
        string Id { get; }

        string Name { get; }

        string Description { get; }

        string MasterKeyWIF { get; }

        Network NetworkChoice { get; }

        void Initialize();

        void Stop();

        bool PubKeyBelongs(Script script);

        PubKey GetNextPubKey();

        void Configure(NodesGroup group);

        bool IsConnected();
    }


    public class JetWalletModel: Wallet, IWallet
    {
        private JetWalletConnectModel _wconnect;
        private string _id;
        private string _name;
        private string _description;
        private ExtKey _masterkey;
        private List<WalletKey> _walletkeys;
        private Network _net;
        private WalletTransactionsCollection _txs;
        
        public string Id
        {
            get { return _id; }
        }

        public new string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string MasterKeyWIF
        {
            get { return _masterkey.GetWif(_net).ToString(); }
        }

        public Network NetworkChoice
        {
            get { return _net; }
        }

        public JetWalletModel(string id, string name, ExtKey masterKey, Network net, WalletCreation walletSetup, string description="No Description Provided")
            :base(walletSetup)
        {
            _wconnect = new JetWalletConnectModel(this);
            _id = id;
            _name = name;
            _description = description;
            _masterkey = masterKey;
            _walletkeys = new List<WalletKey>();
            _net = net;

            WalletFileTools.CreateWalletFolder(Id);
            GenerateWalletKeys();

        }

        public void Initialize()
        {            
            _wconnect.Start();
            base.Connect();            
        }

        public void Stop()
        {            
            _wconnect.Stop();
            base.Disconnect();
        }

        private void GenerateWalletKeys()
        {
            uint numKeys = 100;
            for (uint i = 0; i < numKeys; i++)
            {
                KeyPath path = new KeyPath(0, i);
                WalletKey newKey = new WalletKey(_masterkey, path, _net);
                _walletkeys.Add(newKey);
            }
        }

        public PubKey GetNextPubKey()
        {
            return _walletkeys.Where((k) => !k.Consumed).First().PublicKey;
        }

        public bool PubKeyBelongs(Script script)
        {
            return _walletkeys.Any((k) => k.MatchPublicKey(script));
        }

        public bool IsConnected()
        {
            if (this.State.Equals(WalletState.Created))
            {
                return false;
            }
            return true;
        }

    }
}