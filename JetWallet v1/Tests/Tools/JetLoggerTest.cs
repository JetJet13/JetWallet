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
    [TestFixture(Author="Johny Georges", Description ="Testing JetLogger Methods")]
    public class JetLoggerTest
    {
        JetLogger logger;

        [SetUp]
        public void Init()
        {
            logger = new JetLogger();
        }

        [TearDown]
        public void Clear()
        {
            //LoggerTools.ClearFile();
        }

        [Test]
        public void LogText()
        {
            string text = "JetWallet is a Bitcoin Wallet for Windows Desktop.";
            JetLogger.Log(text);

            string path = LoggerTools.GetFilePath();
            string result = File.ReadAllText(path);
            Assert.That(result.Contains(text));
        }

        [Test]
        public void LogException()
        {
            Exception e = new Exception("Uh-Oh, something broke!");
            JetLogger.LogException(e);

            string path = LoggerTools.GetFilePath();
            string result = File.ReadAllText(path);
            Assert.That(result.Contains(e.Message));
        }
    }
}
