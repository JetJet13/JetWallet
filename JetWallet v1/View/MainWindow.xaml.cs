using System.Windows;
using JetWallet.ViewModel;
using MahApps.Metro.Controls;
using MaterialIcons;
using JetWallet.Tools;
using JetWallet.Controller;

namespace JetWallet.View
{
    public interface IView
    {
        void Show();
        bool? ShowDialog();
        void Close();

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {            
            InitializeComponent();            
            Closing += (s, e) => ViewModelLocator.Cleanup();
            // will fetch the conf file and the default
            // wallet path and prompt user for password
            // to unlock wallet
            //FileTools.FetchConf();
            CheckNetwork.Status();
            
        }

    }
}