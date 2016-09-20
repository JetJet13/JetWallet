using System;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JetWallet.Resources;
using JetWallet.View;
using JetWallet.Tools;

namespace JetWallet.ViewModel
{

    public class WelcomeViewModel : ViewModelBase
    {
        private WelcomeView _wview;
        
        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextHeader
        {
            get { return TextTools.RetrieveStringFromResource("Welcome_Header"); }
            
        }
        public string TextIntroMessage
        {
            get { return TextTools.RetrieveStringFromResource("Welcome_IntroMessage"); }
        }
        public string TextOutroMessage
        {
            get { return TextTools.RetrieveStringFromResource("Welcome_OutroMessage"); }
        }
        public string TextQuoteMessage
        {
            get { return TextTools.RetrieveStringFromResource("Welcome_QuoteMessage"); }

        }        
        public string TextCreateWallet
        {
            get { return TextTools.RetrieveStringFromResource("Welcome_CreateWallet"); }

        }
        public string TextSkip
        {
            get { return TextTools.RetrieveStringFromResource("Welcome_Skip"); }

        }

        public WelcomeViewModel()
        {
            CreateWalletCmd = new RelayCommand(() => { this.ExecuteCloseView(); Messenger.Default.Send<string>("", "OpenCreateWalletView"); });
            CloseViewCmd = new RelayCommand(() => { this.ExecuteCloseView(); });
            Messenger.Default.Register<string>(this, "OpenWelcomeView", (string s) => { this.ExecuteOpenView(); });
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
                
        private void ExecuteOpenView()
        {
            _wview = new WelcomeView();
            _wview.ShowDialog();
        }
        private void ExecuteCloseView()
        {
            _wview.Close();
        }

    }
}