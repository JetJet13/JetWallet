using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using JetWallet_v1.Resources;
using JetWallet_v1.Model;
using NBitcoin;
using NBitcoin.Protocol;
using NBitcoin.SPV;


namespace JetWallet_v1.Tools
{
    class Generators
    {
        private static string[] wordList = LowercaseWords.lowercase_words.Split('\n');
        private static Random rnd = new Random();


        public static string GenerateRecPhrase()
        {
            const int PHRASE_LENGTH = 7;

            string phrase = string.Empty;
            for (int i = 0; i < PHRASE_LENGTH; i++)
            {
                string rndWord = wordList[rnd.Next(0, wordList.Length)];
                phrase += rndWord.Trim();
                if (i < 6)
                {
                    phrase += " ";
                }
            }
            return phrase;
        }
        public static string GenerateRecPhraseHash(string s)
        {
            HashAlgorithm SHA = new SHA256CryptoServiceProvider();
            byte[] sbytes = Encoding.UTF8.GetBytes(s);
            byte[] hash = SHA.ComputeHash(sbytes);
            return BitConverter.ToString(hash).Replace("-", "").Substring(0, 32);
        }
        public static string GenerateHash(SecureString s)
        {
            HashAlgorithm SHA = new SHA256CryptoServiceProvider();
            IntPtr ss = Marshal.SecureStringToGlobalAllocUnicode(s);
            byte[] passBytes = Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(ss));

            // Clear IntPtr from Memory after converting to byte array
            Marshal.ZeroFreeGlobalAllocUnicode(ss);

            byte[] hash = SHA.ComputeHash(passBytes);
            return BitConverter.ToString(hash).Replace("-", "").Substring(0, 32);

        }

        public static Hashtable Wallet2HashTable(WalletModel wallet)
        {
            Hashtable wht = new Hashtable();
            wht.Add("File", wallet.FileLocation);
            wht.Add("FileRecover", wallet.FileRecover);
            wht.Add("Id", wallet.Name);
            wht.Add("Name", wallet.DisplayName);
            wht.Add("PassHash", wallet.GetWalletPassHash());
            wht.Add("RecPhraseHash", wallet.RecoveryPhraseHash);
            wht.Add("Description", wallet.Desc);
            wht.Add("Network", wallet.NetworkChoice.ToString());
            wht.Add("MasterKey", wallet.GetMasterKey().GetWif(wallet.NetworkChoice).ToString());
            wht.Add("Created", wallet.Created);

            return wht;
        }

        public static WalletModel HashTable2Wallet(Hashtable hashT)
        {
            var walletFile = hashT["File"].ToString();
            var walletFileRecover = hashT["FileRecover"].ToString();
            var walletId = hashT["Id"].ToString();
            var walletName = hashT["Name"].ToString();
            var walletPassHash = hashT["PassHash"].ToString();
            var walletRecPhraseHash = hashT["RecPhraseHash"].ToString();
            var walletDescription = hashT["Description"].ToString();
            var walletNetwork = Network.GetNetwork(hashT["Network"].ToString());
            var walletMasterKey = ExtKey.Parse(hashT["MasterKey"].ToString(), walletNetwork);
            var walletCreated = (DateTimeOffset)hashT["Created"];

            BitcoinExtPubKey rootKey = Generators.GenerateRootKey(walletMasterKey,walletNetwork);
            var wallet = new WalletModel(id:walletId,newName:walletName, rootKey: rootKey, newCreated:walletCreated, description: walletDescription);
                        
            wallet.FileRecover = walletFileRecover;
            wallet.PassHash = walletPassHash;
            wallet.RecoveryPhraseHash = walletRecPhraseHash;
            wallet.SetMasterKey(walletMasterKey);
            return wallet;

        }

        public static WalletCreation GenerateWalletCreation(string name, Network net)
        {
            WalletCreation create = new WalletCreation();
            create.Name = name;
            create.Network = net;
            return create;
        }

        public static ExtKey GenerateMasterKey()
        {
            return new ExtKey();          
        }

        public static BitcoinExtPubKey GenerateRootKey(ExtKey masterKey, Network net)
        {
            PubKey pubKey = masterKey.PrivateKey.PubKey;
            ExtPubKey pub = new ExtPubKey(pubKey, masterKey.ChainCode);
            return new BitcoinExtPubKey(pub, net);
        }

        public static Money GenerateOptimalFee(Money fee)
        {
            var satoshis = fee.Satoshi.ToString().ToArray();
            Money optimalFee = Money.Zero;
            char first = satoshis.First();
            string feeString = "";
            feeString += first;
            for (int i = 1; i < satoshis.Length; i++)
            {
                feeString += '0';
            }
        
            optimalFee = Money.FromUnit(decimal.Parse(feeString), MoneyUnit.Satoshi);
            return optimalFee;
        }

        public static FeeRate GetTxFeeRate()
        {
            var fees = Global.VML.Fee.GetFeeRates();
            return new FeeRate(fees.halfHourFee);            
        }

        public static TxAction GetTxAction(WalletTransaction tx)
        {
            Money amount = tx.Balance;
            if (amount < 0) { return TxAction.Sent; }
            else { return TxAction.Received; }
        }
                      
    }
}
