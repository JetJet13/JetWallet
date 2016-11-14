using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using JetWallet.ViewModel;



namespace JetWallet.Tests.ViewModels
{
    [TestFixture(Author ="Johny Georges", Description ="Testing WelcomeViewModel")]
    public class WelcomeViewModelTest
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
        public void TextOK()
        {
            var wvm = new JetWelcomeViewModel();
            
            var badExpected = "Language Not Set";
            Assert.AreNotEqual(badExpected, wvm.TextTitle);
            Assert.AreNotEqual(badExpected, wvm.TextHeader);
            Assert.AreNotEqual(badExpected, wvm.TextIntroMessage);
            Assert.AreNotEqual(badExpected, wvm.TextOutroMessage);
            Assert.AreNotEqual(badExpected, wvm.TextQuoteMessage);
            Assert.AreNotEqual(badExpected, wvm.TextCreateWallet);
            Assert.AreNotEqual(badExpected, wvm.TextSkip);
        }



    }
}
