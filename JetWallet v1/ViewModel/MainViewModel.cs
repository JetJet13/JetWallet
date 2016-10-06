using System;
using System.Windows.Media;
using System.Collections;
using System.Security;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using JetWallet.Model;
using JetWallet.Tools;
using JetWallet.View;
using JetWallet.Components;
using MahApps.Metro.Controls.Dialogs;
using NBitcoin;
using System.Threading.Tasks;
using System.Windows;
using NBitcoin.SPV;

namespace JetWallet.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private MainWindow _mview;
        public MaterialColorThemes MaterialColor { get; }
        public Components.Menu.FileTab FileTab { get; set; }
        public Components.Menu.SettingsTab SettingsTab { get; set; }
        public Components.Menu.ResourceTab ResourceTab { get; set; }
        public StatusBarComponent StatusBarComp { get; set; }


        public const string ColorSchemePropertyName = "ColorScheme";
        private Brush _colorscheme = new SolidColorBrush(Global.VML.ColorScheme.ColorPick);
        public Brush ColorScheme
        {
            get
            {
                return _colorscheme;
            }

            set
            {
                if (_colorscheme == value)
                {
                    return;
                }

                _colorscheme = value;
                RaisePropertyChanged(ColorSchemePropertyName);
            }
        }

        public const string LangPropertyName = "Lang";
        private string _lang = Global.VML.Language.ActiveLanguage;
        public string Lang
        {
            get
            {
                return _lang;
            }

            set
            {
                if (_lang == value)
                {
                    return;
                }

                _lang = value;
                RaisePropertyChanged(LangPropertyName);
            }
        }

        public UiSettings UiSettings
        {
            get { return Global.VML.UiSettings; }
        }

        public const string TextTitlePropertyName = "TextTitle";
        private string _textitle = string.Empty;
        public string TextTitle
        {
            get
            {
                return _textitle;
            }

            set
            {
                if (_textitle == value)
                {
                    return;
                }

                _textitle = value;
                RaisePropertyChanged(TextTitlePropertyName);
            }
        }


        public const string InfoTagPropertyName = "InfoTag";
        private string _infotag;
        public string InfoTag
        {
            get
            {
                return _infotag;
            }

            set
            {
                if (_infotag == value)
                {
                    return;
                }

                _infotag = value;
                RaisePropertyChanged(InfoTagPropertyName);
            }
        }

        private string _textnowallet;
        public string TextNoWallet
        {
            get { return _textnowallet; }
            set { _textnowallet = value; RaisePropertyChanged("TextNoWallet"); }
        }

        private string _textsend;
        public string TextSend
        {
            get { return _textsend; }
            set { _textsend = value; RaisePropertyChanged("TextSend"); }
        }

        private string _textreceive;
        public string TextReceive
        {
            get { return _textreceive; }
            set { _textreceive = value; RaisePropertyChanged("TextReceive"); }
        }

        public const string WalletNotNullPropertyName = "WalletNotNull";
        private bool _walletnotnull = false;
        public bool WalletNotNull
        {
            get
            {
                return _walletnotnull;
            }

            private set
            {
                if (_walletnotnull == value)
                {
                    return;
                }

                _walletnotnull = value;
                RaisePropertyChanged(WalletNotNullPropertyName);
            }
        }
                       

        public const string MainWalletPropertyName = "MainWallet";
        private WalletModel _mainwallet;
        public WalletModel MainWallet
        {
            get
            {
                return _mainwallet;
            }

            private set
            {
                if (_mainwallet == value)
                {
                    return;
                }

                _mainwallet = value;
                RaisePropertyChanged(MainWalletPropertyName);
            }
        }
  

        public const string StrokeStateLightPropertyName = "StrokeStateLight";
        private Color _strokestatelight = Colors.WhiteSmoke;
        public Color StrokeStateLight
        {
            get
            {
                return _strokestatelight;
            }

            set
            {
                if (_strokestatelight == value)
                {
                    return;
                }

                _strokestatelight = value;
                RaisePropertyChanged(StrokeStateLightPropertyName);
            }
        }


        public const string FillStateLightPropertyName = "FillStateLight";
        private Color _fillstatelight = Colors.White;
        public Color FillStateLight
        {
            get
            {
                return _fillstatelight;
            }

            set
            {
                if (_fillstatelight == value)
                {
                    return;
                }

                _fillstatelight = value;
                RaisePropertyChanged(FillStateLightPropertyName);
            }
        }
        
        public MainViewModel()
        {
            MaterialColor = new MaterialColorThemes();
            FileTab = new Components.Menu.FileTab();
            SettingsTab = new Components.Menu.SettingsTab();
            ResourceTab = new Components.Menu.ResourceTab();

            StatusBarComp = new StatusBarComponent();

            ReceiveBitcoinCmd = new RelayCommand(() => { this.ExecuteReceiveBitcoin(); });
            SendBitcoinCmd = new RelayCommand(() => { this.ExecuteSendBitcoin(); });
            ShowTxCmd = new RelayCommand<string>((string s) => { this.ExecuteShowTx(s); });
            OpenWalletInfoCmd = new RelayCommand(() => { this.ExecuteOpenWalletInfo(); });

            Messenger.Default.Register<string>(this, "CloseWallet", (string s) => { this.CloseWallet(); });
            Messenger.Default.Register<string>(this, "LockWallet", (string s) => { this.LockWallet(); });
            Messenger.Default.Register<string>(this, "UnlockWallet", (string s) => { this.UnlockWallet(); });
            Messenger.Default.Register<Hashtable>(this, "FetchNewWallet", (Hashtable s) => { this.FetchNewWallet(s); });
            Messenger.Default.Register<WalletModel>(this, "ChangeActiveWallet", (WalletModel w) => { this.ChangeActiveWallet(w); });
            
            Messenger.Default.Register<string>(this, "SetColorScheme", (string c) => { this.SetColorScheme(c); });
            Messenger.Default.Register<string>(this, "UpdateColorScheme", (string c) => { this.UpdateColorScheme(c); });
            Messenger.Default.Register<WalletModelState>(this, "WalletStateChanged", (WalletModelState c) => { this.SetComponents(c); });
            Messenger.Default.Register<string>(this, "WalletConnStateChanged", (string s) => { this.SetStateLight(); });
            Messenger.Default.Register<string>(this, "NewLanguage", (string s) => { this.UpdateLanguage(s); });
            Messenger.Default.Register<string>(this, "NewCurrency", (string s) => { this.UpdateCurrency(s); });
            Messenger.Default.Register<string>(this, "ShowMainWindow", (string s) => { this.ShowMainView(); });
        }


        public RelayCommand ReceiveBitcoinCmd
        {
            get;
            private set;
        }
        public RelayCommand SendBitcoinCmd
        {
            get;
            private set;
        }
        public RelayCommand<string> ShowTxCmd
        {
            get;
            private set;
        }
        public RelayCommand OpenWalletInfoCmd
        {
            get;
            private set;
        }


        // Only invoked when user creates a new wallet and
        // sets it to the active wallet
        private void FetchNewWallet(Hashtable walletDetails)
        {
            string file = walletDetails["File"].ToString();
            string passhash = walletDetails["PassHash"].ToString();
            this.CloseWallet();
            this.SetupWallet(FileTools.DecryptWallet(file, passhash));
            

        }

        private async void ChangeActiveWallet(WalletModel wallet)
        {
            if (MainWallet == null)
            {
                this.SetupWallet(wallet);
                return;
            }
            else if (MainWallet.Equals(wallet))
            {                
                string titleSame = TextTools.RetrieveStringFromResource("Main_Dialog_SameWallet_Title");
                string messageSame = TextTools.RetrieveStringFromResource("Main_Dialog_SameWallet_Message");
                MessageDialogStyle styleSame = MessageDialogStyle.Affirmative;
                await _mview.ShowMessageAsync(titleSame, messageSame, styleSame);
                return;
            }

            string title = TextTools.RetrieveStringFromResource("Main_Dialog_ActiveWallet_Title");
            string message =  TextTools.RetrieveStringFromResource("Main_Dialog_ActiveWallet_Message");
            MessageDialogStyle style = MessageDialogStyle.AffirmativeAndNegative;

            var result = await _mview.ShowMessageAsync(title, message, style);
            if (result == MessageDialogResult.Affirmative)
            {
                this.CloseWallet();
                this.SetupWallet(wallet);
            }            

        }

        

        public bool WalletFileExists()
        {
            if (File.Exists(MainWallet.FileLocation))
            {
                return true;
            }
            return false;
        }
        private void CloseWallet()
        {
            if (MainWallet == null)
            {
                return;
            }

            MainWallet.Close();
            MainWallet = null;
            WalletNotNull = false;
            InfoTag = TextTools.RetrieveStringFromResource("Main_Info_Inactive");
            this.SetStateLight();
            Global.VML.UiSettings.ClearNetwork();
            // Closing wallet results in 
            // clearing the wallet path
            // from the conf file
            FileTools.ClearConfWalletSettings();
            Logger.StopLogging();
        }

        public WalletModel GetWallet()
        {
            return MainWallet;
        }

        private void SetupWallet(WalletModel newWallet)
        {
            Logger.StartLogging(newWallet.GetWalletFolderPath());
                        
            MainWallet = newWallet;
            MainWallet.Initialize();                     
            WalletNotNull = true;
            InfoTag = TextTools.RetrieveStringFromResource("Main_Info_Active");
            FileTools.SetWalletPath(newWallet.FileLocation);
            this.SetStateLight();
            Global.VML.UiSettings.SetNetwork(newWallet.NetworkChoice);            
        }

       
        private void SetComponents(WalletModelState newWalletState)
        {
            if (newWalletState.Equals(WalletModelState.Ready))
            {
                this.EnableWalletCommands();
                this.StatusBarComp.SetProperties(MainWallet);
            }
            else if (newWalletState.Equals(WalletModelState.Closing))
            {
                this.DisableWalletCommands();
                this.StatusBarComp.ClearProps();
            }
                        
        }
        private void LockWallet()
        {
            MainWallet.SetWalletSecurity(WalletSecurity.Lock);
            this.HideMainView();
            CheckPassword.InvokePasswordPrompt(MainWallet.FileLocation);
            
        }

        private void UnlockWallet()
        {
            MainWallet.SetWalletSecurity(WalletSecurity.Unlock);
            this.ShowMainView();
            
        }

        private void ExecuteReceiveBitcoin()
        {
            if (WalletNotNull && MainWallet.GetWalletState().Equals(WalletModelState.Ready))
            {
                PubKey newScript = MainWallet.GetNewPubKey();
                string address = newScript.GetAddress(MainWallet.NetworkChoice).ToString();
                Messenger.Default.Send<string>(address, "OpenReceiveView");            
                
            }
            else if (WalletNotNull && MainWallet.GetWalletState().Equals(WalletModelState.Preparing))
            {
                this.ShowWalletLoadingDialog();
            }
            else
            {
                this.ShowNoActiveWalletDialog();
            }
        }

        private void ExecuteSendBitcoin()
        {
            if (WalletNotNull && MainWallet.GetWalletState().Equals(WalletModelState.Ready))
            {
                Messenger.Default.Send<String>("", "OpenSendView");
            }
            else if (WalletNotNull && MainWallet.GetWalletState().Equals(WalletModelState.Preparing))
            {
                this.ShowWalletLoadingDialog();
            }
            else
            {
                this.ShowNoActiveWalletDialog();
            }
        }

        private void ExecuteShowTx(string txid)
        {
            TransactionModel tx = MainWallet.TxList.Find(x => x.Id == txid);
            Messenger.Default.Send<TransactionModel>(tx,"OpenTxInfoView");
        }
        
        private void ExecuteOpenWalletInfo()
        {
            if (WalletNotNull && MainWallet.GetWalletState().Equals(WalletModelState.Ready))
            {
                Messenger.Default.Send<string>("", "OpenWalletInfo");
            }
            else if (WalletNotNull && MainWallet.GetWalletState().Equals(WalletModelState.Preparing))
            {
                this.ShowWalletLoadingDialog();
            }
            else
            {
                this.ShowNoActiveWalletDialog();
            }
        }

        private async void ShowNoActiveWalletDialog()
        {
            string title = TextTools.RetrieveStringFromResource("Main_Dialog_NoActiveWallet_Title");
            string message = TextTools.RetrieveStringFromResource("Main_Dialog_NoActiveWallet_Message");
            await _mview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
        }

        private async void ShowWalletLoadingDialog()
        {
            string title = TextTools.RetrieveStringFromResource("Main_Dialog_WalletLoading_Title");
            string message = TextTools.RetrieveStringFromResource("Main_Dialog_WalletLoading_Message");
            await _mview.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative);
        }

        private void UpdateColorScheme(string color)
        {
            Color pick = (Color)ColorConverter.ConvertFromString(color);
            ColorScheme = new SolidColorBrush(pick);
        }

        private void SetColorScheme(string color)
        {
            this.UpdateColorScheme(color);
            Global.VML.ColorScheme.SetColorPick(color);
            FileTools.SetConfColorScheme(color);
            
        }

        private void UpdateCurrency(string s)
        {
            this.StatusBarComp.UpdateText();
        }

        private void UpdateLanguage(string s)
        {
            Lang = s;
            TextTitle = "JetWallet - " + Lang;

            this.TextNoWallet = TextTools.RetrieveStringFromResource("Main_NoWallet");
            this.TextSend = TextTools.RetrieveStringFromResource("Main_Send");
            this.TextReceive = TextTools.RetrieveStringFromResource("Main_Receive");

            if (WalletNotNull)
            {
                MainWallet.UpdateTxListLanguage();
                InfoTag = TextTools.RetrieveStringFromResource("Main_Info_Active");
            }
            else
            {
                InfoTag = TextTools.RetrieveStringFromResource("Main_Info_Inactive");
            }
                        
            this.FileTab.UpdateText();
            this.SettingsTab.UpdateText();
            this.ResourceTab.UpdateText();

            this.StatusBarComp.UpdateText();
        }
        private void SetStateLight()
        {
            if (MainWallet == null)
            {
                StrokeStateLight = Colors.White;
                FillStateLight = Colors.WhiteSmoke;
                return;
            }

            if (MainWallet.GetWalletConnState().Equals(WalletConnectionState.NotConnected))
            {
                
                StrokeStateLight = Colors.Firebrick;
                FillStateLight = Colors.Crimson;
                
            }
            else if (MainWallet.GetWalletConnState().Equals(WalletConnectionState.Connecting))
            {                
                StrokeStateLight = Colors.DarkOrange;
                FillStateLight = Colors.Orange;
            }
            else if (MainWallet.GetWalletConnState().Equals(WalletConnectionState.Connected))
            {
                StrokeStateLight = Colors.LimeGreen;
                FillStateLight = Colors.Chartreuse;
                
            }
            
        }

        private void DisableWalletCommands()
        {
            FileTab.DisableManageWallet();
        }
        private void EnableWalletCommands()
        {
            FileTab.EnableManageWallet();
        }

        // We get an instance of the main view for the
        // only purpose of showing a MahApp.Metro Dialog Box
        public void setMainView(MainWindow mw)
        {
            _mview = mw;
        }

        private void HideMainView()
        {
            _mview.Hide();
        }
        private void ShowMainView()
        {
            _mview.Show();
        }
    }
}