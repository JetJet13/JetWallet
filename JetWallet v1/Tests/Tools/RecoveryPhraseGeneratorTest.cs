using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet;
using JetWallet.Tools;
using JetWallet.Resources;
using NUnit.Framework;

namespace JetWallet.Tests.Tools
{
    [TestFixture(Author ="Johny Georges", Description ="Testing RecoveryPhraseGenerator Methods")]
    public class RecoveryPhraseGeneratorTest
    {
        RecoveryPhraseGenerator _recoveryphrase;
        [SetUp]
        public void Init()
        {
            _recoveryphrase = new RecoveryPhraseGenerator();
        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void GenerateRecoveryPhrase()
        {
            string first = _recoveryphrase.Generate();
            string second = _recoveryphrase.Generate();

            Assert.AreNotEqual(first, second);
        }
    }
}
