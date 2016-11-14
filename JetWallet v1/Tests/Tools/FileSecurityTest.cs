using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Security.Cryptography;
using System.IO;

namespace JetWallet.Tests.Tools
{
    [TestFixture(Author = "Johny Georges", Description = "Testing FileSecurity Methods")]
    public class FileSecurityTest
    {
        IWallet _wallet;
        string _key;
        string _path;
        string _recoverypath;

        [SetUp]
        public void Init()
        {


            _wallet = WalletGenerator.GenerateMockWallet();


            // for encryption and decryption tests
            _key = "238085A3C30982B6DBE1F6F5CEFA4584";
            _path = WalletFileTools.GetWalletFilePath(_wallet.Id);
            _recoverypath = WalletFileTools.GetWalletRecoveryFilePath(_wallet.Id);
        }

        [TearDown]
        public void Clear()
        {
            FileSecurity.LiftFileDefense(_path);
            FileSecurity.LiftFileDefense(_recoverypath);
            Directory.Delete(WalletFileTools.GetWalletFolder(_wallet.Id), true);
        }

        [Test]
        public void EncryptWallet()
        {

            Assert.DoesNotThrow(()=> FileSecurity.EncryptWalletFile(_wallet, _key, _path));
        }

        [Test]
        public void EncryptWalletFail()
        {
            string badKey = "238085A3C30982B6DBE1";
            Assert.Throws<CryptographicException>(() => FileSecurity.EncryptWalletFile(_wallet, badKey, _path));
        }

        [Test]
        public void DecryptWallet()
        {
            EncryptWallet();
            Assert.DoesNotThrow(() => FileSecurity.DecryptWalletFile(_path, _key));
        }

        [Test]
        public void DecryptWalletFail()
        {
            // equivalent to entering an incorrect password
            string badKey = "4854AFEC5F6F1EBD6B28903C3A580832";
            EncryptWallet();

            Assert.Throws<CryptographicException>(() => FileSecurity.DecryptWalletFile(_path, badKey));
        }

        [Test]
        public void ApplyFileDefense()
        {
            FileSecurity.EncryptWalletFile(_wallet, _key, _path);
            
            FileSecurity.ApplyFileDefense(_path);


            Assert.True(File.Exists(_path));

            FileAttributes attributes = File.GetAttributes(_path);
            Assert.AreEqual(FileAttributes.ReadOnly, attributes);          

        }

        [Test]
        public void ApplyFileDefenseRecovery()
        {           
            FileSecurity.EncryptWalletFile(_wallet, _key, _recoverypath);
            FileSecurity.ApplyFileDefense(_recoverypath);

            Assert.True(File.Exists(_recoverypath));

            FileAttributes attributes = File.GetAttributes(_recoverypath);
            Assert.AreEqual(FileAttributes.Hidden | FileAttributes.ReadOnly, attributes);
        }

        [Test]
        public void ApplyFileDefenseFail()
        {
            string path = "bad file path";
            Assert.Throws<Exception>(() => FileSecurity.ApplyFileDefense(path));
        }

        [Test]
        public void LiftFileDefense()
        {            
            FileSecurity.EncryptWalletFile(_wallet, _key, _path);
            FileSecurity.LiftFileDefense(_path);

            FileAttributes attributes = File.GetAttributes(_path);
            Assert.AreEqual(FileAttributes.Normal, attributes);
        }

        [Test]
        public void LiftFileDefenseRecovery()
        {
            FileSecurity.EncryptWalletFile(_wallet, _key, _recoverypath);
            FileSecurity.LiftFileDefense(_recoverypath);

            FileAttributes attributes = File.GetAttributes(_recoverypath);
            Assert.AreEqual(FileAttributes.Normal, attributes);
        }


    }
}
