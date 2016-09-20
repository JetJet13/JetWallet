using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JetWallet_v1.View;
using JetWallet_v1.Tools;
using JetWallet_v1.Model;


namespace JetWallet_v1.ViewModel
{
    public class SetLanguageViewModel : ViewModelBase
    {
        private SetLanguageView _slview;

        public List<string> LangList
        {
            get { return Global.VML.Language.LanguageList; }
        }


        public const string SelectedLangPropertyName = "SelectedLang";
        private string _selectedlang = "English";
        public string SelectedLang
        {
            get
            {
                return _selectedlang;
            }

            set
            {
                if (_selectedlang == value)
                {
                    return;
                }

                _selectedlang = value;
                RaisePropertyChanged(SelectedLangPropertyName);
            }
        }
 

        public SetLanguageViewModel()
        {
            
            SetLanguageCmd = new RelayCommand(() => { this.ExecuteSetLanguage(); });
            Messenger.Default.Register<string>(this, "OpenSetLanguageView", (string s) => { this.OpenView(); });
        }


        public RelayCommand SetLanguageCmd
        {
            get;
            private set;
        }

        private void OpenView()
        {
            _slview = new SetLanguageView();
            _slview.ShowDialog();
        }

        private void ExecuteSetLanguage()
        {
            FileTools.SetConfLanguage(SelectedLang);
            this.CloseView();
            Messenger.Default.Send<string>("", "OpenWelcomeView");
        }
        
        private void CloseView()
        {
            _slview.Close();
        }
    }
}