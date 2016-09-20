using System;
using System.Windows.Media;
using System.Diagnostics;
using System.Security;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JetWallet.View;
using JetWallet.Tools;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using JetWallet.Model;
using System.ComponentModel;
using System.Threading.Tasks;

namespace JetWallet.ViewModel
{

    public class ManageWalletViewModel : ViewModelBase
    {
        private const int MIN_PASS_LENGTH = 4;
        private ManageWalletView _mwview;
        private WalletModel _wallet;

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_Title"); }
        }
        public string TextLockMessage
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_LockMessage"); }
        }
        public string TextLockWallet
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_LockWallet"); }
        }
        public string TextChangeMessage
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_ChangeMessage"); }
        }
        public string TextCurrPass
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_CurrPass"); }
        }
        public string TextNewPass
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_NewPass"); }
        }
        public string TextConfPass
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_ConfPass"); }
        }
        public string TextChangePass
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_ChangePass"); }
        }

        public string TextRepair
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_Repair"); }
        }

        public string TextExport
        {
            get { return TextTools.RetrieveStringFromResource("ManageWallet_Export"); }
        }

        public const string CurrPassPropertyName = "CurrPass";
        private SecureString _currpass = new SecureString();
        public SecureString CurrPass
        {
            get
            {
                return _currpass;
            }

            set
            {
                if (_currpass == value)
                {
                    return;
                }

                _currpass = value;
                RaisePropertyChanged(CurrPassPropertyName);
            }
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


        public const string RepairingWalletPropertyName = "RepairingWallet";
        private bool _repairingwallet = false;
        public bool RepairingWallet
        {
            get
            {
                return _repairingwallet;
            }

            set
            {
                if (_repairingwallet == value)
                {
                    return;
                }

                _repairingwallet = value;
                RaisePropertyChanged(RepairingWalletPropertyName);
            }
        }
        
        public RelayCommand ChangePassCmd
        {
            get;
            private set;
        }

        public RelayCommand LockWalletCmd
        {
            get;
            private set;
        }

        public RelayCommand RepairWalletCmd
        {
            get;
            private set;
        }

        public RelayCommand PrintLogsCmd
        {
            get;
            private set;
        }

        public ManageWalletViewModel()
        {
            ChangePassCmd = new RelayCommand(() => { this.ExecuteChangePass(); });
            LockWalletCmd = new RelayCommand(() => { this.ExecuteLockWallet(); });
            RepairWalletCmd = new RelayCommand(() => { this.ExecuteRepairWallet(); });
            PrintLogsCmd = new RelayCommand(() => { this.ExecutePrintWalletSummary(); });
            Messenger.Default.Register<WalletModel>(this, "OpenManageWallet", (WalletModel w) => { this.OpenManageWalletView(w); });
        }

        private void OpenManageWalletView(WalletModel w)
        {
            _wallet = w;
            _mwview = new ManageWalletView();
            _mwview.ShowDialog();
        }

        private void ExecuteLockWallet()
        {
            if (Global.VML.Main.WalletFileExists() == false)
            {
                string message = TextTools.RetrieveStringFromResource("Error_A100_Lock") 
                    + TextTools.RetrieveStringFromResource("Error_A100").Replace("*path*", _wallet.FileLocation);
                Messenger.Default.Send<string>(message, "OpenSimpleDialogView");
                Messenger.Default.Send<string>("", "CloseWallet");
                CloseView();
                return;
            }
            CloseView();
            Messenger.Default.Send<string>("", "LockWallet");
            
        }

        private async void ExecuteChangePass()
        {
            // Let's make sure that the active wallet file 
            // exists before we make changes to it
            if (File.Exists(_wallet.FileLocation) == false)
            {
                string message = TextTools.RetrieveStringFromResource("Error_A100_Change") 
                    + TextTools.RetrieveStringFromResource("Error_A100").Replace("*path*", _wallet.FileLocation);
                Messenger.Default.Send<string>(message, "OpenSimpleDialogView");

                Messenger.Default.Send<string>("", "CloseWallet");
                CloseView();
                return;
            }


            if (CurrPass.Length == 0 || NewPass.Length == 0 || ConfPass.Length == 0)
            {
                string title = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Incomplete_Title");
                string message = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Incomplete_Message");
                await _mwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return;
            }
            else if (NewPass.Length < MIN_PASS_LENGTH)
            {
                string title = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Insufficient_Title");
                string message = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Insufficient_Message")
                    .Replace("*NUM*",MIN_PASS_LENGTH.ToString());
                await _mwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return;
            }

            string currPassHash = Generators.GenerateHash(CurrPass);
            string newPassHash = Generators.GenerateHash(NewPass);
            string confPassHash = Generators.GenerateHash(ConfPass);
            
            bool isCurrPassMatch = _wallet.PassHash.Equals(currPassHash);
            bool isNewConfPassMatch = newPassHash.Equals(confPassHash);
            bool isCurrNewPassMatch = currPassHash.Equals(newPassHash);

            if (isCurrNewPassMatch == true)
            {
                string title = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Match_Title");
                string message = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Match_Message");
                await _mwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
            }
            else if (isCurrPassMatch == false && isNewConfPassMatch == false)
            {
                string title = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Invalid_Title");
                string message = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Invalid_Message");
                await _mwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
            }
            else if (isCurrPassMatch == false)
            {
                string title = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Incorrect_Title");
                string message = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Incorrect_Message");
                await _mwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
            }
            else if (isNewConfPassMatch == false)
            {
                string title = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Unequal_Title");
                string message = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Unequal_Message");
                await _mwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
            }
            else
            {
                _wallet.SetWalletPassword(NewPass);
                FileTools.UpdateWalletFile(_wallet);

                string title = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Success_Title");
                string message = TextTools.RetrieveStringFromResource("ManageWallet_Dialog_Success_Message");
                var result = await _mwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                if (result == MessageDialogResult.Affirmative)
                {
                    CloseView();
                }
            }
        }
        
        private async void ExecuteRepairWallet()
        {
            RepairingWallet = true;
            await Task.Factory.StartNew(() => {

                _wallet.RepairWallet();
                RepairingWallet = false;

            });
             string title = "Repair Complete";
             string message = "Wallet repair complete. Wallet has been reinitialized and validated.";
             await _mwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);

        }
        private async void ExecutePrintWalletSummary()
        {
            _wallet.LogWallet();
            string path = Path.Combine(_wallet.GetWalletFolderPath(), "wallet.log");
            string title = "Print Complete";
            string message = "A summary of your wallet has been printed to " + path;
            await _mwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
        }
        private void CloseView()
        {
            ClearProps();
            _mwview.Close();
        }
        private void ClearProps()
        {
            _wallet = null;
            CurrPass.Clear();
            NewPass.Clear();
            ConfPass.Clear();
        }
    }
}