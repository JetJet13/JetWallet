using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.Protocol;
using JetWallet.Tools;
using JetWallet.Model;
using JetWallet.ViewModel;
using NUnit.Framework;
using System.Diagnostics;
using GalaSoft.MvvmLight.Threading;
using Moq;
using System.Security;
using System;
using System.IO;

namespace JetWallet.Tests.Tools
{
    [TestFixture(Author = "Johny Georges", Description = "Testing WalletFileTools")]
    public class WalletFileToolsTest
    {

        IWallet _wallet;

        [SetUp]
        public void Init()
        {
            _wallet = WalletGenerator.GenerateMockWallet();
            _wallet.Initialize();

        }

        [Test]
        public void CreateWalletFolder()
        {
            WalletFileTools.CreateWalletFolder(_wallet.Id);

            string folderPath = Path.Combine(App.WalletsDir, _wallet.Id);
            Assert.True(Directory.Exists(folderPath));
        }

        [Test]
        public void GetWalletFolder()
        {
            string actualFolderPath = WalletFileTools.GetWalletFolder(_wallet.Id);
            string expectedFolderPath = Path.Combine(App.WalletsDir, _wallet.Id);
            Assert.AreEqual(expectedFolderPath, actualFolderPath);
        }

        [Test]
        public void CheckWalletFilePath()
        {
            Assert.DoesNotThrow(()=>WalletFileTools.CheckWalletFilePath(_wallet.Id));
        }

        [Test]
        public void GetWalletFilePath()
        {
            string actualPath = WalletFileTools.GetWalletFilePath(_wallet.Id);
            string expectedPath = Path.Combine(App.WalletsDir, _wallet.Id, "wallet.jet");
            Assert.AreEqual(expectedPath, actualPath);
        }

        [Test]
        public void GetWalletRecoveryFilePath()
        {
            string actualPath = WalletFileTools.GetWalletRecoveryFilePath(_wallet.Id);
            string expectedPath = Path.Combine(App.WalletsDir, _wallet.Id, "wallet.recover.jet");
            Assert.AreEqual(expectedPath, actualPath);
        }

    }
}
