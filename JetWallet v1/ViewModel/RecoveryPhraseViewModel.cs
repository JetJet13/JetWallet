using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Media;
using JetWallet.View;
using JetWallet.Tools;

namespace JetWallet.ViewModel
{
    public class RecoveryPhraseViewModel : ViewModelBase
    {

        private RecoveryPhraseView _rpview;

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("RecoveryPhrase_Title"); }
        }
        public string TextHeader
        {
            get { return TextTools.RetrieveStringFromResource("RecoveryPhrase_Header"); }
        }
        public string TextIntro
        {
            get { return TextTools.RetrieveStringFromResource("RecoveryPhrase_Intro"); }
        }
        public string TextOutro
        {
            get { return TextTools.RetrieveStringFromResource("RecoveryPhrase_Outro"); }
        }
        public string TextOk
        {
            get { return TextTools.RetrieveStringFromResource("Ok"); }
        }


        public const string RecoveryPhrasePropertyName = "RecoveryPhrase";
        private string _recoveryphrase = string.Empty;
        public string RecoveryPhrase
        {
            get
            {
                return _recoveryphrase;
            }

            set
            {
                if (_recoveryphrase == value)
                {
                    return;
                }

                _recoveryphrase = value;
                RaisePropertyChanged(RecoveryPhrasePropertyName);
            }
        }

        public RecoveryPhraseViewModel()
        {
            Messenger.Default.Register<string>(this, "OpenRecoveryPhraseView", (string s) => { this.OpenView(s); });
            CloseViewCmd = new RelayCommand(() => { this.ExecuteCloseView(); });
        }

        public RelayCommand CloseViewCmd
        {
            get;
            private set;
        }

         private void OpenView(string s)
        {
            RecoveryPhrase = s;

            _rpview = new RecoveryPhraseView();
            _rpview.ShowDialog();
        }

        private void ExecuteCloseView()
        {
            _rpview.Close();
        }
    }
}