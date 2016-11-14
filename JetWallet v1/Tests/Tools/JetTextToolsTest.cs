using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet;
using JetWallet.Model;
using JetWallet.Tools;
using JetWallet.Resources;
using NUnit.Framework;
using NBitcoin;
using NBitcoin.SPV;
using System.Globalization;

namespace JetWallet.Tests.Tools
{
    [TestFixture(Author ="Johny Georges", Description ="Testing JetTextTools")]
    public class JetTextToolsTest
    {
        [SetUp]
        public void Init()
        {

        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void RetrieveStringFromResource()
        {
            string id = "About_Thank_You";
            //App.ConfigSettings.Language = ConfigLanguage.English;
            
            string result = JetTextTools.RetrieveStringFromResource(id);
            string expected = "Thank you for downloading JetWallet";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RetrieveStringFromResourceNotFound()
        {
            string id = "Bad_Text_Id";
            //App.ConfigSettings.Language = ConfigLanguage.English;
            string result = JetTextTools.RetrieveStringFromResource(id);

            string expected = "NA";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RetrieveStringFromResourceFail()
        {
            string id = "About_Thank_You";
            //App.ConfigSettings.Language = ConfigLanguage.None;

            string result = JetTextTools.RetrieveStringFromResource(id);
            string expected = "Language Not Set";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Base58Encode()
        {
            string plainText = "Johny's Wallet";
            string result = JetTextTools.Base58Encode(plainText);

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            string expected = Base58Check.Base58CheckEncoding.Encode(plainTextBytes);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Base58Decode()
        {

            string plainText = "Johny's Wallet";
            string base58EncodedPlainText = JetTextTools.Base58Encode(plainText);
            string result = JetTextTools.Base58Decode(base58EncodedPlainText);

            var base64EncodedBytes = Base58Check.Base58CheckEncoding.Decode(base58EncodedPlainText);
            string expected = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            Assert.AreEqual(expected, result);

        }

        [Test]
        public void FormatFullDate()
        {
            DateTime date = DateTime.Today;
            string cultCode = "en-US";
            string result = JetTextTools.FormatFullDate(date, cultCode);

            TimeZoneInfo local = TimeZoneInfo.Local;
            CultureInfo cult = new CultureInfo(cultCode);            
            string expected = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local).ToString("dd MMMM yyyy hh:mm tt", cult);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FormatFullDateFail()
        {
            DateTime date = DateTime.Today;
            string cultCode = "bad cult code";

            Assert.Throws<CultureNotFoundException>(() => JetTextTools.FormatFullDate(date, cultCode));
        }



    }
}
