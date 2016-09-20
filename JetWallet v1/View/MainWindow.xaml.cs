using System.Windows;
using JetWallet.ViewModel;
using MahApps.Metro.Controls;
using MaterialIcons;
using JetWallet.Tools;

namespace JetWallet.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            App.ExistingInstance();
            InitializeComponent();
            Global.VML.Main.setMainView(this);
            Closing += (s, e) => ViewModelLocator.Cleanup();
            // will fetch the conf file and the default
            // wallet path and prompt user for password
            // to unlock wallet
            FileTools.FetchConf();
            CheckNetwork.Status();

        }
    }
}