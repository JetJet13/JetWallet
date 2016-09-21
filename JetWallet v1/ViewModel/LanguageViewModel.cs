 using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;

namespace JetWallet.ViewModel
{

    public class LanguageViewModel : ViewModelBase
    {


        public const string ActiveLanguagePropertyName = "ActiveLanguage";
        private string _activelanguage = string.Empty;
        public string ActiveLanguage
        {
            get
            {
                return _activelanguage;
            }

            set
            {
                if (_activelanguage == value)
                {
                    return;
                }

                _activelanguage = value;
                RaisePropertyChanged(ActiveLanguagePropertyName);
                Messenger.Default.Send<string>(_activelanguage, "NewLanguage");
            }
        }

        private List<string> _languagelist = new List<string>();
        public List<string> LanguageList
        {
            get { return _languagelist; }
            set { _languagelist = value; }
        }


        public LanguageViewModel()
        {
            LanguageList = new List<string>() { "English" };
        }

        public void SetLanguage(string lang)
        {
            ActiveLanguage = lang;
        }
    }
}