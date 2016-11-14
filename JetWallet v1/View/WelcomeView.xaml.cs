using System.Windows;
using MahApps.Metro.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace JetWallet.View
{
    /// <summary>
    /// Description for WelcomeView.
    /// </summary>
    public partial class WelcomeView : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the WelcomeView class.
        /// </summary>
        public WelcomeView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "WelcomeView";
        }

        private void OpenCreateWalletView(object sender, RoutedEventArgs e)
        {
            this.Close();
            Messenger.Default.Send<string>("", "OpenCreateWalletView");
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}