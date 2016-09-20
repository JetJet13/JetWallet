using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JetWallet_v1.Tools;
using JetWallet_v1.View;
using System.Windows.Media;

namespace JetWallet_v1.ViewModel
{

    public class LicenseViewModel : ViewModelBase
    {
        private LicenseView _lview;

        public Brush ColorScheme
        {
            get { return new SolidColorBrush(Global.VML.ColorScheme.ColorPick); }
        }

        public string TextTitle
        {
            get { return TextTools.RetrieveStringFromResource("License_Title"); }
        }
        public string TextOk
        {
            get { return TextTools.RetrieveStringFromResource("Ok"); }
        }


        public LicenseViewModel()
        {
            CloseViewCmd = new RelayCommand(() => { this.CloseView(); });
            Messenger.Default.Register<string>(this, "OpenLicenseView", (string s) => this.OpenView(s));
        }


        public RelayCommand CloseViewCmd
        {
            get;
            private set;
        }

        private void OpenView(string s)
        {
            _lview = new LicenseView();
            _lview.ShowDialog();
        }

        private void CloseView()
        {
            _lview.Close();
        }

    }
}