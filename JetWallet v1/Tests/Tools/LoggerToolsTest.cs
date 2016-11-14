using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JetWallet;
using JetWallet.Model;
using JetWallet.ViewModel;
using JetWallet.Tools;
using NUnit.Framework;
using NBitcoin;
using NBitcoin.SPV;


namespace JetWallet.Tests.Tools
{
    [TestFixture(Author ="Johny Georges",Description ="Testing LoggerTools Methods")]
    public class LoggerToolsTest
    {
        [SetUp]
        public void Init()
        {

        }

        [TearDown]
        public void Clear()
        {
            string path = LoggerTools.GetFilePath();
            File.Delete(path);
        }

        [Test]
        public void GetLoggerFilePath()
        {
            string path = LoggerTools.GetFilePath();
            string relativePath = Path.Combine(App.AppDir, @"../data.log");
            string expectedPath = Path.GetFullPath(relativePath);

            Assert.AreEqual(expectedPath, path);
        }

        [Test]
        public void CreateLoggerFile()
        {
            LoggerTools.CreateFile();
            
            string path = LoggerTools.GetFilePath();
            Assert.True(File.Exists(path));
        }

        [Test]
        public void ClearLoggerFile()
        {
            LoggerTools.ClearFile();

            string path = LoggerTools.GetFilePath();
            string result = File.ReadAllText(path);
            Assert.IsEmpty(result);
        }

    }
}
