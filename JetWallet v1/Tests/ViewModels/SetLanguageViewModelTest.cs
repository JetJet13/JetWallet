using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using JetWallet.Model;
using JetWallet.View;
using JetWallet.ViewModel;
using JetWallet.Tools;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace JetWallet.Tests.ViewModels
{
    [TestFixture(Author ="Johny Georges", Description ="Testing SetLanguageViewModel behaviour")]
    public class SetLanguageViewModelTest
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
        public void LanguageListIgnoresNone()
        {
            var slvm = new JetSetLanguageViewModel();
            var langList = slvm.LanguageList;

            Assert.False(langList.Any((x) => x == ConfigLanguage.None));
        }

        [Test]
        public void SetLanguageInConfig()
        {
            var slvm = new JetSetLanguageViewModel();
            var langList = slvm.LanguageList;
            var lang = langList.Find((x) => x == ConfigLanguage.French);
            slvm.SelectedLanguage = lang;
            slvm.SetConfigLanguage.Execute(null);

            var expected = lang;
            var actual = Global.CFM.GetConfigLanguage();
            Assert.AreEqual(expected, actual);
        }


    }
}
