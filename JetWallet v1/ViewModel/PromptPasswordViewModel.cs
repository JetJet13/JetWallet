using System;
using System.Security;
using System.Windows.Media;
using System.Collections;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using JetWallet_v1.Tools;
using JetWallet_v1.Model;
using JetWallet_v1.View;
using MahApps.Metro.Controls.Dialogs;


namespace JetWallet_v1.ViewModel
{

    public class PromptPasswordViewModel : ViewModelBase
    {

        private string _file;
        private string _name; // wallet name
        private PromptPasswordView _ppview;

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("PromptPassword_Title"); }
        }
        public string TextUnlock
        {
            get { return TextTools.RetrieveStringFromResource("PromptPassword_UnlockWallet"); }
        }
        public string TextCancel
        {
            get { return TextTools.RetrieveStringFromResource("Cancel"); }
        }


        public const string PromptMessagePropertyName = "PromptMessage";
        private string _promptmessage;
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


        public const string PassAttemptPropertyName = "PassAttempt";
        private SecureString _passattempt; 
        public SecureString PassAttempt
        {
            get
            {
                return _passattempt;
            }

            set
            {
                if (_passattempt == value)
                {
                    return;
                }

                _passattempt = value;
                RaisePropertyChanged(PassAttemptPropertyName);
            }
        }


        public RelayCommand UnlockWalletCmd
        {
            get;
            private set;
        }

        public RelayCommand ClosePromptCmd
        {
            get;
            private set;
        }

        public PromptPasswordViewModel()
        {
            
            UnlockWalletCmd = new RelayCommand(() => { this.ExecuteUnlockWallet(); });
            ClosePromptCmd = new RelayCommand(() => { this.ExecuteClosePrompt(); });
            Messenger.Default.Register<string>(this, "OpenPasswordPrompt", (string s) => { this.OpenPassPromptView(s); });
        }

        private void OpenPassPromptView(string file)
        {
            _file = file;
            _name = TextTools.DecodeWalletName(file);
            PassAttempt = new SecureString();
            string message = TextTools.RetrieveStringFromResource("PromptPassword_Message").Replace("*name*",_name);
            this.SetPromptMessage(message);
            this.ShowView();
        }

        private void ShowView()
        {
            this.ResetPassAttempt();
            _ppview = new PromptPasswordView();
            _ppview.ShowDialog();
        }

        private void CloseView()
        {            
            _ppview.Close();
        }

        private void ExecuteUnlockWallet()
        {
            // A wallet is already active, 
            // its just been locked by the user
            if (Global.ActiveWallet != null && Global.ActiveWallet.IsLocked())
            {
                this.CheckAttempt();               
            }
            // A wallet is being either imported,recovered 
            // or it was a previously active wallet (occurs on app startup)
            else
            {
                this.UnlockWalletAttempt();                
            }
            
                        
        }

        private void UnlockWalletAttempt()
        {
            if (PassAttempt.Length == 0)
            {
                this.IncorrectPasswordAttempt();
                return;
            }

            try
            {
                string passHash = Generators.GenerateHash(PassAttempt);
                WalletModel wallet = FileTools.DecryptWallet(_file, passHash);

                // sets wallet file property to file path in which it was imported from.
                wallet.FileLocation = _file;
                Messenger.Default.Send<WalletModel>(wallet, "ChangeActiveWallet");
                _ppview.Close();
            }
            catch
            {
                this.IncorrectPasswordAttempt();
            }
        }
        private void IncorrectPasswordAttempt()
        {
            this.CloseView();
            this.SetPromptMessageIncorrect();
            this.ShowView();            
        }
        private void CheckAttempt()
        {
            if (PassAttempt.Length == 0)
            {
                this.IncorrectPasswordAttempt();
                return;
            }

            string passHash = Generators.GenerateHash(PassAttempt);
            bool correct = FileTools.CheckPasswordAttempt(_file, passHash);
            if (correct)
            {
                Messenger.Default.Send<string>("", "UnlockWallet");
                this.CloseView();
            }
            else
            {
                this.IncorrectPasswordAttempt();
            }
        }
        private void ResetPassAttempt()
        {
            PassAttempt.Dispose();
            PassAttempt = new SecureString();
        }
        private void SetPromptMessage(string message)
        {
            PromptMessage = message;
        }

        private void SetPromptMessageIncorrect()
        {
            string message = TextTools.RetrieveStringFromResource("PromptPassword_IncorrectMessage").Replace("*name*", _name);
            this.SetPromptMessage(message);
        }

        private async void ExecuteClosePrompt()
        {

            string title = TextTools.RetrieveStringFromResource("PromptPassword_DialogTitle");
            string message = TextTools.RetrieveStringFromResource("PromptPassword_DialogMessage").Replace("*name*",_name);
            var result = await _ppview.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                // check if wallet is null or locked
                if (Global.ActiveWallet == null)
                {
                    Messenger.Default.Send<string>("", "CloseWallet");
                }
                else if (Global.ActiveWallet.IsLocked())
                {
                    Messenger.Default.Send<string>("", "CloseWallet");
                    Messenger.Default.Send<string>("", "ShowMainWindow");
                }
                this.CloseView();
                
            }
            return;
        }
    }
}