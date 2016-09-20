using System;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JetWallet.View;
using JetWallet.Tools;
using JetWallet.Resources;

namespace JetWallet.ViewModel
{

    public class SimpleDialogViewModel : ViewModelBase
    {
        private SimpleDialogView _fmview;
        

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("SimpleDialog_Title"); }
        }
        public string TextOk
        {
            get { return TextTools.RetrieveStringFromResource("Ok"); }
        }        

        public const string PromptMessagePropertyName = "PromptMessage";
        private string _promptmessage = "Default Prompt Message";
        public string PromptMessage
        {
            get
            {
                return _promptmessage;
            }

            set
            {
                if (_promptmessage == value)
                {
                    return;
                }

                _promptmessage = value;
                RaisePropertyChanged(PromptMessagePropertyName);
            }
        }

        public SimpleDialogViewModel()
        {
            CloseViewCmd = new RelayCommand(() => { this.ExecuteCloseView(); });
            Messenger.Default.Register<string>(this, "OpenSimpleDialogView", (string s) => { this.OpenView(s); });

        }

        public RelayCommand CloseViewCmd
        {
            get;
            private set;

        }

        private void OpenView(string s)
        {
            PromptMessage = s;
            _fmview = new SimpleDialogView();
            _fmview.ShowDialog();
        }

        private void ExecuteCloseView()
        {
            _fmview.Close();
        }
    }
}