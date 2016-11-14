using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Model;
using JetWallet.Tools;
using NUnit.Framework;
using NBitcoin;
using NBitcoin.SPV;
using Moq;
using System.IO;
using Newtonsoft.Json;
using NBitcoin.Protocol;

namespace JetWallet.Tests.Models
{
    [TestFixture(Author ="Johny Georges", Description ="Testing JetWalletConnectModel methods")]
    public class JetWalletConnectModelTest
    {
        Mock<IWallet> _mock;
        IWallet _wallet;
        JetWalletConnectModel _wconnect;

        [SetUp]
        public void Init()
        {
            _mock = new Mock<IWallet>();
            _mock.SetupGet<string>((x) => x.Id).Returns("my-wallet-id");
            _mock.SetupGet<Network>((x) => x.NetworkChoice).Returns(Network.TestNet);

            _wallet = _mock.Object;
            _wconnect = new JetWalletConnectModel(_mock.Object);                        
            
            CleanWalletFolder();

        }
        
        [TearDown]
        public void Clear()
        {
            CleanWalletFolder();
        }

        public void CleanWalletFolder()
        {
            string walletFolder = WalletFileTools.GetWalletFolder(_wallet.Id);
            if (Directory.Exists(walletFolder))
            {
                Directory.Delete(walletFolder, true);
            }
            WalletFileTools.CreateWalletFolder(_mock.Object.Id);
        }

        [Test]
        public void Start()
        {
            _wconnect.Start();
                        
        }

        [Test]
        public void StartFail()
        {
            Mock<IWallet> mock = new Mock<IWallet>();
            mock.SetupGet<string>((x) => x.Id).Returns("my-wallet-id");
            mock.Setup((x) => x.Configure(It.IsAny<NodesGroup>()))
                .Throws<InvalidOperationException>();
            JetWalletConnectModel wcon = new JetWalletConnectModel(mock.Object); 

            CreateWrongChain();
            Assert.Throws<InvalidOperationException>(()=> wcon.Start());
            
        }

        private void CreateWrongChain()
        {
            string walletFolderPath = WalletFileTools.GetWalletFolder(_wallet.Id);
            var chainFilePath = Path.Combine(walletFolderPath, "chain.dat");
            var wrongNetwork = Network.Main; // _wallet.NetworkChoice = Network.Testnet
            ConcurrentChain mainChain = new ConcurrentChain(wrongNetwork);
            using (var fs = File.Open(chainFilePath, FileMode.Create))
            {
                mainChain.WriteTo(fs);
            }
        }

        [Test]
        public void Stop()
        {
            _wconnect.Start();

            _wconnect.Stop();
            Assert.False(_wallet.IsConnected());
        }

        [Test]
        public void GetCurrentHeight()
        {
            _wconnect.Start();
            var result = _wconnect.GetCurrentHeight();
            Assert.NotNull(result);
        }

        [Test]
        public void GetConnectedNodes()
        {
            _wconnect.Start();
            var result = _wconnect.GetConnNodes();
            Assert.NotNull(result);
        }


        [Test]
        public void CreateAndUseWalletFiles()
        {
            _wconnect.Start();
            _wconnect.Stop();

            _wconnect.Start();
            _wconnect.Stop();

        }

        [Test]
        public void BadTrackerFile()
        {
            _wconnect.Start();
            _wconnect.Stop();

            string walletFolderPath = WalletFileTools.GetWalletFolder(_wallet.Id);
            var trackerFilePath = Path.Combine(walletFolderPath, "tracker.dat");
            using (var fs = File.Open(trackerFilePath, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(fs);
                writer.WriteLine("BAD_TRACKER");
                writer.Dispose();
            }

            Assert.Throws<JsonReaderException>(() => _wconnect.Start());
        }

        [Test]
        public void BadAddressManager()
        {
            _wconnect.Start();
            _wconnect.Stop();

            string walletFolderPath = WalletFileTools.GetWalletFolder(_wallet.Id);
            var addrmanFilePath = Path.Combine(walletFolderPath, "addrman.dat");
            using (var fs = File.Open(addrmanFilePath, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(fs);
                writer.WriteLine("BAD_ADDRESS_MANAGER");
                writer.Dispose();
            }

            Assert.Throws<OverflowException>(() => _wconnect.Start());
        }

    }
}
