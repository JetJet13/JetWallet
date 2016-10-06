using System;
using System.Windows.Media;
using System.Diagnostics;
using MahApps.Metro.Controls.Dialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JetWallet.View;
using JetWallet.Tools;
using JetWallet.Model;

namespace JetWallet.ViewModel
{
    public class RecoverWalletViewModel : ViewModelBase
    {
        private RecoverWalletView _rwview;
        private string _path;
        private string _walletname;
        public string Header { get; set; }

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("RecoverWallet_Title"); }
        }
        public string TextHeader
        {
            get { return TextTools.RetrieveStringFromResource("RecoverWallet_Header").Replace("*name*", _walletname); }
        }
        public string TextMessage
        {
            get { return TextTools.RetrieveStringFromResource("RecoverWallet_Message"); }
        }
        public string TextNote
        {
            get { return TextTools.RetrieveStringFromResource("RecoverWallet_Note"); }
        }
        public string TextRecover
        {
            get { return TextTools.RetrieveStringFromResource("RecoverWallet_Recover"); }
        }
        public string TextCancel
        {
            get { return TextTools.RetrieveStringFromResource("Cancel"); }
        }

        public const string RecoverPhrasePropertyName = "RecoverPhrase";
        private string _recoverphrase;
        public string RecoverPhrase
        {
            get
            {
                return _recoverphrase;
            }

            set
            {
                if (_recoverphrase == value)
                {
                    return;
                }

                _recoverphrase = value;
                RaisePropertyChanged(RecoverPhrasePropertyName);
            }
        }


        public RecoverWalletViewModel()
        {
            Messenger.Default.Register<string>(this, "OpenRecoverWalletView", (string s) => { this.OpenView(s); });
            RecoverWalletCmd = new RelayCommand(() => { this.ExecuteRecoverWallet(); });
            CloseViewCmd = new RelayCommand(() => { this.ExecuteCloseView(); });
        }

        public RelayCommand RecoverWalletCmd
        {
            get;
            private set;
        }
        public RelayCommand CloseViewCmd
        {
            get;
            private set;
        }

        private void OpenView(string path)
        {
            // set recovery wallet path
            _path = path;
            _walletname = TextTools.DecodeWalletName(path);
            Header = "You are attempting to recover the wallet '" + _walletname +"'";
            RecoverPhrase = string.Empty;
            _rwview = new RecoverWalletView();
            _rwview.ShowDialog();
        }
        private async void ExecuteRecoverWallet()
        {
            try
            {
               string recoverPhraseHash = Generators.GenerateRecPhraseHash(RecoverPhrase);
               WalletModel recoveredWallet = FileTools.DecryptWallet(_path, recoverPhraseHash);

                // Decrypting the File was a success
                // We set the recovered wallet as the active wallet and then
                // prompt the user to set a new password for the recovered wallet
                string title = TextTools.RetrieveStringFromResource("RecoverWallet_Dialog_Success_Title");
                string message = TextTools.RetrieveStringFromResource("RecoverWallet_Dialog_Success_Message").Replace("*name*", _walletname);
                await _rwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                Messenger.Default.Send<WalletModel>(recoveredWallet, "OpenSetPasswordView");
                this.ExecuteCloseView();
                Messenger.Default.Send<WalletModel>(recoveredWallet, "ChangeActiveWallet");

            }
            catch
            {
                string title = TextTools.RetrieveStringFromResource("RecoverWallet_Dialog_Incorrect_Title");
                string message = TextTools.RetrieveStringFromResource("RecoverWallet_Dialog_Incorrect_Message");
                await _rwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
            }
        }
        private void ExecuteCloseView()
        {
            _rwview.Close();
        }

    }
}