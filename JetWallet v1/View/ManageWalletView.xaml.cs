using System.Windows;
using System.Diagnostics;
using MahApps.Metro.Controls;
using JetWallet.Tools;

namespace JetWallet.View
{
    /// <summary>
    /// Description for ManageWalletView.
    /// </summary>
    public partial class ManageWalletView : MetroWindow, IView
    {
        
        /// <summary>
        /// Initializes a new instance of the ManageWalletView class.
        /// </summary>
        public ManageWalletView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "ManageWalletView";
        }

        private void CurrPassChanged(object sender, RoutedEventArgs e)
        {
            Global.VML.ManageWallet.CurrPass = TxtCurrPass.SecurePassword;
            

        }
        private void NewPassChanged(object sender, RoutedEventArgs e)
        {
            Global.VML.ManageWallet.NewPass = TxtNewPass.SecurePassword;
            
        }
        private void ConfPassChanged(object sender, RoutedEventArgs e)
        {
            Global.VML.ManageWallet.ConfPass = TxtConfPass.SecurePassword;
            
        }
    }
}