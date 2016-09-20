using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Collections;
using System.Linq;
using System.Deployment.Application;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using JetWallet_v1;
using JetWallet_v1.Tools;
using JetWallet_v1.Model;

namespace JetWallet_v1.Components
{
    namespace Menu
    {
        public class FileTab : ViewModelBase
        {
            
            private string _textfile;
            public string TextFile
            {
                get { return _textfile; }
                set { _textfile = value; RaisePropertyChanged("TextFile"); }
            }

            private string _textnew;
            public string TextNew
            {
                get { return _textnew; }
                set { _textnew = value; RaisePropertyChanged("TextNew"); }
            }

            private string _textimport;
            public string TextImport
            {
                get { return _textimport; }
                set { _textimport = value; RaisePropertyChanged("TextImport"); }
            }

            private string _textclose;
            public string TextClose
            {
                get { return _textclose; }
                set { _textclose = value; RaisePropertyChanged("TextClose"); }
            }

            private string _textrecover;
            public string TextRecover
            {
                get { return _textrecover; }
                set { _textrecover = value; RaisePropertyChanged("TextRecover"); }
            }

            private string _textmanage;
            public string TextManage
            {
                get { return _textmanage; }
                set { _textmanage = value; RaisePropertyChanged("TextManage"); }
            }

            private string _textexit;
            public string TextExit
            {
                get { return _textexit; }
                set { _textexit = value; RaisePropertyChanged("TextExit"); }
            }


            public const string ManageWalletMenuPropertyName = "ManageWalletMenu";

            private bool _managewalletmenu = false;


            public bool ManageWalletMenu
            {
                get
                {
                    return _managewalletmenu;
                }

                set
                {
                    if (_managewalletmenu == value)
                    {
                        return;
                    }

                    _managewalletmenu = value;
                    RaisePropertyChanged(ManageWalletMenuPropertyName);
                }
            }


            public FileTab()
            {
                OpenCreateViewCmd = new RelayCommand(ExecuteOpenCreateView);
                CloseAppCmd = new RelayCommand(ExecuteCloseApp);
                ManageWalletCmd = new RelayCommand(ExecuteManageWallet);
                ImportWalletCmd = new RelayCommand(() => { this.ImportWallet(); });
                CloseWalletCmd = new RelayCommand(() => { this.CloseWallet(); });
                RecoverWalletCmd = new RelayCommand(() => { this.ExecuteRecoverWallet(); });
                Messenger.Default.Register<string>(this, "ImportWallet", (string s) => { this.ImportWallet(); });
            }


            public RelayCommand OpenCreateViewCmd
            {
                get;
                private set;
            }
            public RelayCommand ImportWalletCmd
            {
                get;
                private set;
            }
            public RelayCommand CloseWalletCmd
            {
                get;
                private set;
            }
            public RelayCommand RecoverWalletCmd
            {
                get;
                private set;
            }
            public RelayCommand CloseAppCmd
            {
                get;
                private set;
            }

            public RelayCommand ManageWalletCmd
            {
                get;
                private set;
            }



            Action ExecuteOpenCreateView = () =>
            {               
                Messenger.Default.Send("Open UP!", "OpenCreateWalletView");
            };
            Action ExecuteCloseApp = () =>
            {                
                App.Current.Shutdown();
            };
            Action ExecuteManageWallet = () =>
            {               
                Messenger.Default.Send<WalletModel>(Global.ActiveWallet, "OpenManageWallet");
            };


            private void ImportWallet()
            {
                
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = App.WalletsDir;
                ofd.Filter = "Wallet files (*.jet)|*.jet|All files (*.*)|*.*";
                Nullable<bool> result = ofd.ShowDialog();
                if (result.Value)
                {
                    if (ofd.FileName.EndsWith(".recover.jet"))
                    {                        
                        // User attempting to import a .recover file
                        string message = TextTools.RetrieveStringFromResource("Error_A500");
                        Messenger.Default.Send<string>(message, "OpenSimpleDialogView");
                    }
                    else if (ofd.FileName.EndsWith(".jet"))
                    {
                        CheckPassword.InvokePasswordPrompt(ofd.FileName);
                    }
                    else
                    {
                        string message = TextTools.RetrieveStringFromResource("Error_A501");
                        Messenger.Default.Send<string>(message, "OpenSimpleDialogView");
                    }
                }
            }

            private void ExecuteRecoverWallet()
            {                
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = App.WalletsDir;
                ofd.Filter = "Wallet files (*.jet)|*.jet|All files (*.*)|*.*";
                Nullable<bool> result = ofd.ShowDialog();
                if (result.Value)
                {
                    if (ofd.FileName.EndsWith(".recover.jet"))
                    {
                        Messenger.Default.Send<string>(ofd.FileName, "OpenRecoverWalletView");
                    }
                    else if (ofd.FileName.EndsWith(".jet"))
                    {
                        //User attempted to recover a regular .jet file
                        string message = TextTools.RetrieveStringFromResource("Error_A502");
                        Messenger.Default.Send<string>(message, "OpenSimpleDialogView");
                    }
                    else
                    {
                        string message = TextTools.RetrieveStringFromResource("Error_A503");
                        Messenger.Default.Send<string>(message, "OpenSimpleDialogView");

                    }
                }
            }

            public void UpdateText()
            {
                TextFile = TextTools.RetrieveStringFromResource("Main_Menu_File");
                TextNew = TextTools.RetrieveStringFromResource("Main_Menu_New");
                TextImport = TextTools.RetrieveStringFromResource("Main_Menu_Import");
                TextClose = TextTools.RetrieveStringFromResource("Main_Menu_Close");
                TextRecover = TextTools.RetrieveStringFromResource("Main_Menu_Recover");
                TextManage = TextTools.RetrieveStringFromResource("Main_Menu_Manage");
                TextExit = TextTools.RetrieveStringFromResource("Main_Menu_Exit");

            }

            public void EnableManageWallet()
            {
                ManageWalletMenu = true;
            }

            public void DisableManageWallet()
            {
                ManageWalletMenu = false;
            }

            private void CloseWallet()
            {
                Messenger.Default.Send<string>("", "CloseWallet");
            }
        }

        public class SettingsTab : ViewModelBase
        {
            private string _textsettings;
            public string TextSettings
            {
                get { return _textsettings; }
                set { _textsettings = value; RaisePropertyChanged("TextSettings"); }
            }

            private string _textlanguage;
            public string TextLanguage
            {
                get { return _textlanguage; }
                set { _textlanguage = value; RaisePropertyChanged("TextLanguage"); }
            }

            private string _textcurrency;
            public string TextCurrency
            {
                get { return _textcurrency; }
                set { _textcurrency = value; RaisePropertyChanged("TextCurrency"); }
            }

            private string _textcolorscheme;
            public string TextColorScheme
            {
                get { return _textcolorscheme; }
                set { _textcolorscheme = value; RaisePropertyChanged("TextColorScheme"); }
            }

            private string _textred;
            public string TextRed
            {
                get { return _textred; }
                set { _textred = value; RaisePropertyChanged("TextRed"); }
            }

            private string _textpink;
            public string TextPink
            {
                get { return _textpink; }
                set { _textpink = value; RaisePropertyChanged("TextPink"); }
            }

            private string _textpurple;
            public string TextPurple
            {
                get { return _textpurple; }
                set { _textpurple = value; RaisePropertyChanged("TextPurple"); }
            }

            private string _textdeeppurple;
            public string TextDeepPurple
            {
                get { return _textdeeppurple; }
                set { _textdeeppurple = value; RaisePropertyChanged("TextDeepPurple"); }
            }

            private string _textindigo;
            public string TextIndigo
            {
                get { return _textindigo; }
                set { _textindigo = value; RaisePropertyChanged("TextIndigo"); }
            }

            private string _textblue;
            public string TextBlue
            {
                get { return _textblue; }
                set { _textblue = value; RaisePropertyChanged("TextBlue"); }
            }

            private string _textlightblue;
            public string TextLightBlue
            {
                get { return _textlightblue; }
                set { _textlightblue = value; RaisePropertyChanged("TextLightBlue"); }
            }

            private string _textcyan;
            public string TextCyan
            {
                get { return _textcyan; }
                set { _textcyan = value; RaisePropertyChanged("TextCyan"); }
            }

            private string _textteal;
            public string TextTeal
            {
                get { return _textteal; }
                set { _textteal = value; RaisePropertyChanged("TextTeal"); }
            }

            private string _textgreen;
            public string TextGreen
            {
                get { return _textgreen; }
                set { _textgreen = value; RaisePropertyChanged("TextGreen"); }
            }

            private string _textlightgreen;
            public string TextLightGreen
            {
                get { return _textlightgreen; }
                set { _textlightgreen = value; RaisePropertyChanged("TextLightGreen"); }
            }

            private string _textlimegreen;
            public string TextLimeGreen
            {
                get { return _textlimegreen; }
                set { _textlimegreen = value; RaisePropertyChanged("TextLimeGreen"); }
            }

            private string _textyellow;
            public string TextYellow
            {
                get { return _textyellow; }
                set { _textyellow = value; RaisePropertyChanged("TextYellow"); }
            }

            private string _textamber;
            public string TextAmber
            {
                get { return _textamber; }
                set { _textamber = value; RaisePropertyChanged("TextAmber"); }
            }

            private string _textorange;
            public string TextOrange
            {
                get { return _textorange; }
                set { _textorange = value; RaisePropertyChanged("TextOrange"); }
            }

            private string _textdeeporange;
            public string TextDeepOrange
            {
                get { return _textdeeporange; }
                set { _textdeeporange = value; RaisePropertyChanged("TextDeepOrange"); }
            }

            private string _textbrown;
            public string TextBrown
            {
                get { return _textbrown; }
                set { _textbrown = value; RaisePropertyChanged("TextBrown"); }
            }

            private string _textbluegrey;
            public string TextBlueGrey
            {
                get { return _textbluegrey; }
                set { _textbluegrey = value; RaisePropertyChanged("TextBlueGrey"); }
            }

            public SettingsTab()
            {
                SetLanguageCmd = new RelayCommand<string>((string s) => { this.ExecuteSetLanguage(s); });
                SetCurrencyCmd = new RelayCommand<string>((string s) => { this.ExecuteSetCurrency(s); });
                SetColorSchemeCmd = new RelayCommand<Color>((Color c) => { this.ExecuteSetColorScheme(c); });

            }

            public RelayCommand<string> SetLanguageCmd
            {
                get;
                private set;
            }

            public RelayCommand<Color> SetColorSchemeCmd
            {
                get;
                private set;
            }

            public RelayCommand<string> SetCurrencyCmd
            {
                get;
                private set;
            }

            private void ExecuteSetLanguage(string lang)
            {                
                FileTools.SetConfLanguage(lang);
            }

            private void ExecuteSetCurrency(string curr)
            {                
                FileTools.SetConfCurrency(curr);
            }

            private void ExecuteSetColorScheme(Color c)
            {                
                Messenger.Default.Send<string>(c.ToString(), "SetColorScheme");
            }

            public void UpdateText()
            {
                TextSettings = TextTools.RetrieveStringFromResource("Main_Menu_Settings");
                TextLanguage = TextTools.RetrieveStringFromResource("Main_Menu_Language");
                TextCurrency = TextTools.RetrieveStringFromResource("Main_Menu_Currency");
                TextColorScheme = TextTools.RetrieveStringFromResource("Main_Menu_ColorScheme");
                // COLORS
                TextRed = TextTools.RetrieveStringFromResource("Main_Menu_Red");
                TextPink = TextTools.RetrieveStringFromResource("Main_Menu_Pink");
                TextPurple = TextTools.RetrieveStringFromResource("Main_Menu_Purple");
                TextDeepPurple = TextTools.RetrieveStringFromResource("Main_Menu_DeepPurple");
                TextIndigo = TextTools.RetrieveStringFromResource("Main_Menu_Indigo");
                TextBlue = TextTools.RetrieveStringFromResource("Main_Menu_Blue");
                TextLightBlue = TextTools.RetrieveStringFromResource("Main_Menu_LightBlue");
                TextCyan = TextTools.RetrieveStringFromResource("Main_Menu_Cyan");
                TextTeal = TextTools.RetrieveStringFromResource("Main_Menu_Teal");
                TextGreen = TextTools.RetrieveStringFromResource("Main_Menu_Green");
                TextLightGreen = TextTools.RetrieveStringFromResource("Main_Menu_LightGreen");
                TextLimeGreen = TextTools.RetrieveStringFromResource("Main_Menu_LimeGreen");
                TextYellow = TextTools.RetrieveStringFromResource("Main_Menu_Yellow");
                TextAmber = TextTools.RetrieveStringFromResource("Main_Menu_Amber");
                TextOrange = TextTools.RetrieveStringFromResource("Main_Menu_Orange");
                TextDeepOrange = TextTools.RetrieveStringFromResource("Main_Menu_DeepOrange");
                TextBrown = TextTools.RetrieveStringFromResource("Main_Menu_Brown");
                TextBlueGrey = TextTools.RetrieveStringFromResource("Main_Menu_BlueGrey");

            }


        }

        public class ResourceTab : ViewModelBase
        {

            private string _textResources;
            public string TextResources
            {
                get { return _textResources; }
                set { _textResources = value; RaisePropertyChanged("TextResources"); }
            }

            private string _textabout;
            public string TextAbout
            {
                get { return _textabout; }
                set { _textabout = value; RaisePropertyChanged("TextAbout"); }
            }

            private string _textlicense;
            public string TextLicense
            {
                get { return _textlicense; }
                set { _textlicense = value; RaisePropertyChanged("TextLicense"); }
            }


            public ResourceTab()
            {
                
                OpenAboutViewCmd = new RelayCommand(() => { this.ExecuteOpenAboutView(); });
                OpenLicenseViewCmd = new RelayCommand(() => { this.ExecuteOpenLicenseView(); });
            }

           

            public RelayCommand CheckForUpdatesCmd
            {
                get;
                private set;
            }

            public RelayCommand OpenAboutViewCmd
            {
                get;
                private set;
            }

            public RelayCommand OpenLicenseViewCmd
            {
                get;
                private set;
            }


            private void ExecuteOpenLicenseView()
            {
                Messenger.Default.Send<string>("", "OpenLicenseView");
            }

            private void ExecuteOpenAboutView()
            {
                Messenger.Default.Send<string>("", "OpenAboutView");
            }

            public void UpdateText()
            {
                TextResources = TextTools.RetrieveStringFromResource("Main_Menu_Resources");
                TextAbout = TextTools.RetrieveStringFromResource("Main_Menu_About");
                TextLicense = TextTools.RetrieveStringFromResource("Main_Menu_License");
            }

        }
    }


}
