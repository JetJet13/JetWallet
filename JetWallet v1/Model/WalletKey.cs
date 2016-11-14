using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using System.Diagnostics;

namespace JetWallet.Model
{
    /// <summary>
    /// Wallet Key contains a private key, a public key, and its corresponding key path 
    /// relative to its master key.
    /// </summary>
    public class WalletKey
    {
        private Network _net;

        private ExtKey _privkey;
        public ExtKey PrivKey
        {
            get { return _privkey; }
            private set { _privkey = value; }
        }

        private PubKey _publickey;
        public PubKey PublicKey
        {
            get { return _publickey; }
            private set { _publickey = value; }
        }

        private KeyPath _keypath;
        public KeyPath KeyPath
        {
            get { return _keypath; }
            private set { _keypath = value; }
        }

        private Script _scriptkey;
        public Script ScriptKey
        {
            get { return _scriptkey; }
            private set { _scriptkey = value; }
        }

        private BitcoinAddress _address;
        public BitcoinAddress Address
        {
            get { return _address; }
            private set { _address = value; }
        }

        private bool _consumed;
        public bool Consumed
        {
            get { return _consumed; }
            set { _consumed = value; }
        }

        public WalletKey(ExtKey rootKey, KeyPath keyPath, Network net)
        {
            _net = net;
            PrivKey = rootKey.Derive(keyPath);
            PublicKey = PrivKey.PrivateKey.PubKey;
            KeyPath = keyPath;
            ScriptKey = PublicKey.ScriptPubKey.Hash.ScriptPubKey;
            Address = PublicKey.GetAddress(net);
            Consumed = false;
            
        }
       
        public bool MatchPublicKey(Script script)
        {
            var scriptHex = script.Hash.ScriptPubKey.ToHex();
            var keyScriptHex = ScriptKey.ToHex();
            if (keyScriptHex.Equals(scriptHex))
            {
                return true;
            }
            return false;
        }
        public bool MatchKeyPath(uint i)
        {
            // each KeyPath Index looks like [0,N], so N is what we are using to match key paths
            return KeyPath.Indexes.Contains(i);
            
        }
        /// <summary>
        /// the key has been used at least once
        /// </summary>
        public void ConsumeKey()
        {
            if (!Consumed)
            {
                Consumed = true;
            }
            return;
            
        }

    }
}
