using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.SPV;

namespace JetWallet.Model
{

    public class BaseWallet
    {
        private string _id = string.Empty;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private ExtKey _masterkey;
        private Network _network;
        private List<WalletKey> _keys = new List<WalletKey>();

        public BaseWallet(string newId, string newName, ExtKey newMasterKey, Network newNetwork, string newDescription="No Description")
        {
            _id = newId;
            _name = newName;
            _description = newDescription;
            _masterkey = newMasterKey;
            _network = newNetwork;
            this.Initialize();
        }

        // Methods
        private void Initialize()
        {
            this.GenerateWalletKeys();
        }
        private void GenerateWalletKeys()
        {
            int numKeys = 100;
            for (uint index = 0; index < numKeys; index++)
            {
                KeyPath keyPath = new KeyPath(0, index);
                WalletKey newKey = new WalletKey(_masterkey, keyPath, _network);
                _keys.Add(newKey);
            }
        }

        public PubKey GetNewPublicKey()
        {
            return _keys.Where(key => !key.Consumed).First().PublicKey;
        }

        public bool PublicKeyBelongs(Script script)
        {
            return _keys.Any(key => key.MatchPublicKey(script.Hash.ScriptPubKey));
            
        }


    }
}
