using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Model;
using NBitcoin;
using NBitcoin.SPV;
using Moq;

namespace JetWallet.Tools
{
    public class WalletGenerator
    {
        public static IWallet GenerateWalletFromFile(Hashtable walletData)
        {
            var id = walletData["id"].ToString();
            var name = walletData["name"].ToString();
            var description = walletData["description"].ToString();
            var masterKeyWIF = walletData["masterkey"].ToString();
            var network = Network.GetNetwork(walletData["network"].ToString());

            var masterKey = ExtKey.Parse(masterKeyWIF);
            var walletSetup = GenerateWalletCreation(name, network);
            return new JetWalletModel(id, name, masterKey, network, walletSetup, description);
        }

        public static IWallet GenerateMockWallet()
        {
            var net = Network.TestNet;
            var masterKeyWIF = "tprv8ZgxMBicQKsPeimWMhvvBBgKpdmuhRUaj5EKPvhhaXm16PZCotCxdEd9oJv3ZGG9L6bb4yEA7ZibttyR6117A2Kkh3yQC9EAagKxJWirmuh";

            Mock<IWallet> mock = new Mock<IWallet>();
            mock.SetupGet<string>((x) => x.Id).Returns("wallet-id");
            mock.SetupGet<string>((x) => x.Name).Returns("MyWallet");
            mock.SetupGet<string>((x) => x.Description).Returns("My Wallet Description");
            mock.SetupGet<string>((x) => x.MasterKeyWIF).Returns(masterKeyWIF);
            mock.SetupGet<Network>((x) => x.NetworkChoice).Returns(net);

            return mock.Object;
        }

        public static IWallet GenerateNewWallet(string id, string name, string masterKeyWIF, Network networkChoice, string description)
        {
            var masterKey = ExtKey.Parse(masterKeyWIF);
            var walletSetup = GenerateWalletCreation(name, networkChoice);
            
            return new JetWalletModel(id, name, masterKey, networkChoice, walletSetup, description);
        }

        private static WalletCreation GenerateWalletCreation(string name, Network networkChoice)
        {
            var walletSetup = new WalletCreation();
            walletSetup.Name = name;
            walletSetup.Network = networkChoice;
            walletSetup.SignatureRequired = 1;
            walletSetup.UseP2SH = false;
            walletSetup.DerivationPath = new KeyPath();
            walletSetup.PurgeConnectionOnFilterChange = false;
            return walletSetup;
        }
    }
}
