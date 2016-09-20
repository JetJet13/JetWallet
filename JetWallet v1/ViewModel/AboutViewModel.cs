using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JetWallet.Tools;
using JetWallet.View;
using System.Windows;
using System.Windows.Media;

namespace JetWallet.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        private AboutView _aview;

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("About_Title"); }
        }

        public string TextAboutHeader
        {
            get { return TextTools.RetrieveStringFromResource("About_Header_About"); }
        }

        public string TextResHeader
        {
            get { return TextTools.RetrieveStringFromResource("About_Header_Resources"); }
        }

        public string TextContactHeader
        {
            get { return TextTools.RetrieveStringFromResource("About_Header_Contact"); }
        }

        public string TextContHeader
        {
            get { return TextTools.RetrieveStringFromResource("About_Header_Contribute"); }
        }

        public string TextDesc
        {
            get { return TextTools.RetrieveStringFromResource("About_Description"); }
        }

        public string TextLicense
        {
            get { return TextTools.RetrieveStringFromResource("About_License"); }
        }

        public string TextShowLicense
        {
            get { return TextTools.RetrieveStringFromResource("About_Show_License"); }
        }

        public string TextShowDoc
        {
            get { return TextTools.RetrieveStringFromResource("About_Show_Doc"); }
        }

        public string TextLearnBitcoin
        {
            get { return TextTools.RetrieveStringFromResource("About_Learn_Bitcoin"); }
        }

        public string TextReportBug
        {
            get { return TextTools.RetrieveStringFromResource("About_Report_Bug"); }
        }

        public string TextFeedback
        {
            get { return TextTools.RetrieveStringFromResource("About_Feedback"); }
        }

        public string TextEmail
        {
            get { return TextTools.RetrieveStringFromResource("About_Email"); }
        }

        public string TextContribute
        {
            get { return TextTools.RetrieveStringFromResource("About_Contribute"); }
        }

        public string TextTranslate
        {
            get { return TextTools.RetrieveStringFromResource("About_Translate"); }
        }

        public string TextThankYou
        {
            get { return TextTools.RetrieveStringFromResource("About_Thank_You"); }
        }

        public string TextOK
        {
            get { return TextTools.RetrieveStringFromResource("Ok"); }
        }

        public AboutViewModel()
        {
            CopyCmd = new RelayCommand(() => { this.ExecuteCopy(); });
            CloseViewCmd = new RelayCommand(() => { this.ExecuteCloseView(); });
            OpenLicenseViewCmd = new RelayCommand(() => { this.ExecuteOpenLicenseView(); });
            Messenger.Default.Register<string>(this, "OpenAboutView", (string s) => { this.OpenView(s); });
        }

        public RelayCommand OpenLicenseViewCmd
        {
            get;
            private set;
        }
        public RelayCommand CopyCmd
        {
            get;
            private set;
        }
        public RelayCommand CloseViewCmd
        {
            get;
            private set;
        }

        private void OpenView(string s)
        {
            _aview = new AboutView();
            _aview.ShowDialog();
        }
        private void ExecuteOpenLicenseView()
        {
            Messenger.Default.Send<string>("", "OpenLicenseView");
        }
        private void ExecuteCopy()
        {
            Clipboard.SetText("jgeorges371@gmail.com");
        }
        private void ExecuteCloseView()
        {
            _aview.Close();
        }
    }
}