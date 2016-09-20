using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Media;
using System.Collections;
using System.Security;
using System.Diagnostics;
using JetWallet.Tools;
using JetWallet.Model;
using JetWallet.View;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using NBitcoin;
using NBitcoin.SPV;
using System.IO;
using NBitcoin.Protocol;
using NBitcoin.Protocol.Behaviors;
using System.Threading.Tasks;

namespace JetWallet.ViewModel
{
   
    public class CreateWalletViewModel : ViewModelBase
    {
        const int MIN_PASS_LENGTH = 4;
        private CreateWalletView _cwview;

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("CreateWallet_Title"); }
        }
        public string TextMessage
        {
            get { return TextTools.RetrieveStringFromResource("CreateWallet_Message"); }
        }
        public string TextName
        {
            get { return TextTools.RetrieveStringFromResource("CreateWallet_Name"); }
        }
        public string TextPass
        {
            get { return TextTools.RetrieveStringFromResource("CreateWallet_Pass"); }
        }
        public string TextDesc
        {
            get { return TextTools.RetrieveStringFromResource("CreateWallet_Desc"); }
        }
        public string TextTestnet
        {
            get { return TextTools.RetrieveStringFromResource("CreateWallet_Testnet"); }
        }
        public string TextNameExample
        {
            get { return TextTools.RetrieveStringFromResource("CreateWallet_NameExample"); }
        }
        public string TextTestnetMessage
        {
            get { return TextTools.RetrieveStringFromResource("CreateWallet_TestnetMessage"); }
        }
        public string TextCreateWallet
        {
            get { return TextTools.RetrieveStringFromResource("CreateWallet_CreateWallet"); }
        }
        public string TextCancel
        {
            get { return TextTools.RetrieveStringFromResource("Cancel"); }
        }

        

        
        public const string NewNamePropertyName = "NewName";

        private string _newname = string.Empty;

        public string NewName
        {
            get
            {
                return _newname;
            }

            set
            {
                if (_newname == value)
                {
                    return;
                }

                _newname = value;
                RaisePropertyChanged(NewNamePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Pass" /> property's name.
        /// </summary>
        public const string PassPropertyName = "Pass";

        private SecureString _pass = new SecureString();

        public SecureString Pass
        {
            get
            {
                return _pass;
            }

            set
            {
                if (_pass == value)
                {
                    return;
                }

                _pass = value;
                RaisePropertyChanged(PassPropertyName);
            }
        }

        public const string DescriptionPropertyName = "Description";

        private string _description;

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (_description == value)
                {
                    return;
                }

                _description = value;
                RaisePropertyChanged(DescriptionPropertyName);
            }
        }

        public const string NetworkPropertyName = "Testnet";

        private bool _testnet = false;

        public bool Testnet
        {
            get
            {
                return _testnet;
            }

            set
            {
                if (_testnet == value)
                {
                    return;
                }

                _testnet = value;
                RaisePropertyChanged(NetworkPropertyName);
            }
        }

        public CreateWalletViewModel()
        {
            
            CreateWalletCmd = new RelayCommand(() => { this.ExecuteCreateWallet(); });
            CloseViewCmd = new RelayCommand(() => { this.ExecuteCloseView(); });
            Messenger.Default.Register<string>(this, "OpenCreateWalletView", (string s)=> { this.ExecuteOpenCreateWalletView(); });
        }

        public RelayCommand CreateWalletCmd
        {
            get;
            private set;
        }

        public RelayCommand CloseViewCmd
        {
            get;
            private set;
        }


        private void ExecuteOpenCreateWalletView() 
        {            
            this.ClearProps();
            _cwview = new CreateWalletView();
            _cwview.ShowDialog();
            
        }

        
        private async void ExecuteCreateWallet()
        {
            if (string.IsNullOrWhiteSpace(NewName) && Pass.Length < MIN_PASS_LENGTH)
            {
                string title = TextTools.RetrieveStringFromResource("CreateWallet_Dialog_Invalid_Title");
                string message = TextTools.RetrieveStringFromResource("CreateWallet_Dialog_Invalid_Message")
                    .Replace("*NUM*",MIN_PASS_LENGTH.ToString());
                await _cwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return;
            }
            else if (string.IsNullOrWhiteSpace(NewName))
            {
                string title = TextTools.RetrieveStringFromResource("CreateWallet_Dialog_NameEmpty_Title");
                string message = TextTools.RetrieveStringFromResource("CreateWallet_Dialog_NameEmpty_Message");
                await _cwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return;
            }
            else if (Pass.Length < MIN_PASS_LENGTH)
            {
                string title = TextTools.RetrieveStringFromResource("CreateWallet_Dialog_Insufficient_Title");
                string message = TextTools.RetrieveStringFromResource("CreateWallet_Dialog_Insufficient_Message")
                    .Replace("*NUM*", MIN_PASS_LENGTH.ToString());
                await _cwview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
                return;
            }

            string recPhrase = Generators.GenerateRecPhrase();
            Network net = this.GetNetworkChoice();
            // WalletModel inherits from NBitcoin.SPV.Wallet which
            // requires a WalletCreation Class as a parameter
            WalletCreation create = Generators.GenerateWalletCreation(NewName, net);
            
            ExtKey masterKey = Generators.GenerateMasterKey();
            BitcoinExtPubKey rootKey = Generators.GenerateRootKey(masterKey, net);
            string id = TextTools.Base58Encode(NewName);

            string walletFolder = Path.Combine(App.WalletsDir, id);
            Directory.CreateDirectory(walletFolder);
            
            var newWallet = new WalletModel(id, NewName, rootKey, DateTimeOffset.Now, Description);
            newWallet.SetWalletPassword(Pass);
            newWallet.SetRecoveryPhrase(recPhrase);
            newWallet.SetMasterKey(masterKey);
            


            if (FileTools.SaveNewWallet(newWallet))
            {
                Messenger.Default.Send<string>(recPhrase, "OpenRecoveryPhraseView");
                // If a wallet is already active, lets ask the user to close
                // current active wallet and use the newly created one
                this.NewWalletPrompt(newWallet.FileLocation);
                
            }
        }

        private void ExecuteCloseView()
        {
            this.ClearProps();
            _cwview.Close();
        }

        private Network GetNetworkChoice()
        {            
            if (Testnet == true)
            {
                return Network.TestNet;
            }
            return Network.Main;
        }

        private async void NewWalletPrompt(string filepath)
        {
            if (Global.ActiveWallet == null)
            {
                this.NotifyMainWallet(filepath);
                this.ExecuteCloseView();
                return;
            }

            string title = TextTools.RetrieveStringFromResource("CreateWallet_Dialog_ActiveWallet_Title");
            string message = TextTools.RetrieveStringFromResource("CreateWallet_Dialog_ActiveWallet_Message");
            var result = await _cwview.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative);

            
            if (result == MessageDialogResult.Affirmative)
            {
                // Set active wallet to newly created wallet.
                this.NotifyMainWallet(filepath);

            }
            this.ExecuteCloseView();

        }

        private void NotifyMainWallet(string filepath)
        {
                        
            FileTools.SetWalletPath(filepath);
            Hashtable walletDetails = new Hashtable();
            walletDetails.Add("File", filepath);
            walletDetails.Add("Name", NewName);
            walletDetails.Add("PassHash", Generators.GenerateHash(Pass));
            Messenger.Default.Send<Hashtable>(walletDetails, "FetchNewWallet");
        }

        private void ClearProps()
        {
            NewName = string.Empty;
            Description = string.Empty;
            Pass.Clear();
            Testnet = false;
        }

        
    }
}