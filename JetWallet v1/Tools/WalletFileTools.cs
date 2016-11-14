using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JetWallet.Model;
using NBitcoin;

namespace JetWallet.Tools
{
    class WalletFileTools
    {
       
        public static string GetWalletFilePath(string id)
        {
            CheckWalletFilePath(id);
            string walletFilePath = Path.Combine(App.WalletsDir, id, "wallet.jet");
            return walletFilePath;
        }

        public static string GetWalletRecoveryFilePath(string id)
        {
            CheckWalletFilePath(id);
            string walletRecoveryFilePath = Path.Combine(App.WalletsDir, id, "wallet.recover.jet");
            return walletRecoveryFilePath;
        }

        public static void CheckWalletFilePath(string id)
        {
            if (!Directory.Exists(GetWalletFolder(id)))
            {
                CreateWalletFolder(id);
            }
        }


        public static void CreateWalletFolder(string id)
        {
            string path = Path.Combine(App.WalletsDir, id);
            Directory.CreateDirectory(path);
        }

        public static string GetWalletFolder(string id)
        {
            string path = Path.Combine(App.WalletsDir, id);
            return path;
        }
        
    }
}
