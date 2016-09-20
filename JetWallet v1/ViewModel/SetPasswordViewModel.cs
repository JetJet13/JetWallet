using System;
using System.Security;
using System.Windows.Media;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using JetWallet.View;
using JetWallet.Tools;
using JetWallet.Model;

namespace JetWallet.ViewModel
{

    public class SetPasswordViewModel : ViewModelBase
    {
        private const int MIN_PASS_LENGTH = 4;
        private SetPasswordView _spview;
        private WalletModel _wallet;

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("SetPassword_Title"); }
        }
        public string TextHeader
        {
            get { return TextTools.RetrieveStringFromResource("SetPassword_Header"); }
        }
        public string TextMessage
        {
            get { return TextTools.RetrieveStringFromResource("SetPassword_Message"); }
        }
        public string TextNewPass
        {
            get { return TextTools.RetrieveStringFromResource("SetPassword_NewPass"); }
        }
        public string TextConfPass
        {
            get { return TextTools.RetrieveStringFromResource("SetPassword_ConfPass"); }
        }
        public string TextSetNewPass
        {
            get { return TextTools.RetrieveStringFromResource("SetPassword_SetNewPass"); }
        }

        
        public const string NewPassPropertyName = "NewPass";
        private SecureString _newpass = new SecureString();        
        public SecureString NewPass
        {
            get
            {
                return _newpass;
            }

            set
            {
                if (_newpass == value)
                {
                    return;
                }

                _newpass = value;
                RaisePropertyChanged(NewPassPropertyName);
            }
        }

        
        public const string ConfPassPropertyName = "ConfPass";
        private SecureString _confpass = new SecureString();         
        public SecureString ConfPass
        {
            get
            {
                return _confpass;
            }

            set
            {
                if (_confpass == value)
                {
                    return;
                }

                _confpass = value;
                RaisePropertyChanged(ConfPassPropertyName);
            }
        }
                
        public SetPasswordViewModel()
        {
            Messenger.Default.Register<WalletModel>(this, "OpenSetPasswordView", (WalletModel w) => { this.OpenView(w); });
            SetPassCmd = new RelayCommand(() => { this.ExecuteSetPass(); });
        }


        public RelayCommand SetPassCmd
        {
            get;
            private set;
        }

        private void OpenView(WalletModel w)
        {
            _spview = new SetPasswordView();
            _wallet = w;
            _spview.ShowDialog();
        }

        private void CloseView()
        {
            ClearProps();
            _spview.Close();
        }

        private void ClearProps()
        {
            _wallet = null;
            ConfPass.Clear();
            NewPass.Clear();
            
        }

        private async void ExecuteSetPass()
        {
            
            if (NewPass.Length == 0 || ConfPass.Length == 0)
            {
                string title = TextTools.RetrieveStringFromResource("SetPassword_Dialog_Incomplete_Title");
                string message = TextTools.RetrieveStringFromResource("SetPassword_Dialog_Incomplete_Message");
                await _spview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return;
            }
            else if (NewPass.Length < MIN_PASS_LENGTH)
            {
                string title = TextTools.RetrieveStringFromResource("SetPassword_Dialog_Insufficient_Title");
                string message = TextTools.RetrieveStringFromResource("SetPassword_Dialog_Insufficient_Message").Replace("*NUM*", MIN_PASS_LENGTH.ToString());
                await _spview.ShowMessageAsync(title, message,
                    MessageDialogStyle.Affirmative);
                return;
            }

            string newPassHash = Generators.GenerateHash(NewPass);
            string confPassHash = Generators.GenerateHash(ConfPass);

            bool isNewConfPassMatch = newPassHash.Equals(confPassHash);
            

            
            if (isNewConfPassMatch == false)
            {
                string title = TextTools.RetrieveStringFromResource("SetPassword_Dialog_Match_Title");
                string message = TextTools.RetrieveStringFromResource("SetPassword_Dialog_Match_Message");
                await _spview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
            }
            else
            {
                _wallet.SetWalletPassword(NewPass);
                FileTools.UpdateWalletFile(_wallet);
                string title = TextTools.RetrieveStringFromResource("SetPassword_Dialog_Success_Title");
                string message = TextTools.RetrieveStringFromResource("SetPassword_Dialog_Success_Message");
                var result = await _spview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                if (result == MessageDialogResult.Affirmative)
                {
                    _spview.Close();
                }
            }
        }
    }
}