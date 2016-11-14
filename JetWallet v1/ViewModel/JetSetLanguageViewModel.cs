using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Model;
using JetWallet.View;
using JetWallet.Tools;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace JetWallet.ViewModel
{
    public class JetSetLanguageViewModel: ViewModelBase
    {

        public List<ConfigLanguage> LanguageList
        {
            get
            {
                var none = ConfigLanguage.None;
                return Enum.GetValues(typeof(ConfigLanguage))
                            .Cast<ConfigLanguage>()
                            .Where((x) => x != none)
                            .ToList();
            }
        }
         
        public const string SelectedLanguagePropertyName = "SelectedLanguage";
        private ConfigLanguage _selectedlanguage = ConfigLanguage.English;
        public ConfigLanguage SelectedLanguage
        {
            get
            {
                return _selectedlanguage;
            }

            set
            {
                if (_selectedlanguage == value)
                {
                    return;
                }

                _selectedlanguage = value;
                RaisePropertyChanged(SelectedLanguagePropertyName);
            }
        }

        public JetSetLanguageViewModel()
        {
            SetConfigLanguage = new RelayCommand(() => ExecuteSetConfigLanguage());
        }

        public RelayCommand SetConfigLanguage
        {
            get;
            private set;
        }

        private void ExecuteSetConfigLanguage()
        {
            Messenger.Default.Send<ConfigLanguage>(SelectedLanguage,"SetConfigLanguage");                       
        }

    }
}
