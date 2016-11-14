using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet;
using JetWallet.Tools;
using NUnit.Framework;
using System.Security;
using System.Security.Cryptography;

namespace JetWallet.Tests.Tools
{
    [TestFixture(Author ="Johny Georges", Description ="Testing HashGenerator Methods")]
    public class HashGeneratorTest
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
        public void GeneratePasswordHash()
        {
            SecureString testPass = new SecureString();
            testPass.AppendChar('H');
            testPass.AppendChar('e');
            testPass.AppendChar('l');
            testPass.AppendChar('l');
            testPass.AppendChar('o');
            string result = HashGenerator.GeneratePasswordHash(testPass);

            string expected = "185F8DB32271FE25F561A6FC938B2E26";
            Assert.AreEqual(expected, result);

        }

        [Test]
        public void GenerateRecoveryPhraseHash()
        {
            string testRecoveryPhrase = "moon jump cat bump horse flip rope";
            string result = HashGenerator.GenerateRecoveryPhraseHash(testRecoveryPhrase);

            string expected = "1F3F6F3AE09D58F17DD4B8F46AE8BF6B";
            Assert.AreEqual(expected, result);            
        }

    }
}
