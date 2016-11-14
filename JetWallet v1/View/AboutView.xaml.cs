using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;

namespace JetWallet.View
{
    /// <summary>
    /// Description for AboutView.
    /// </summary>
    public partial class AboutView : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the AboutView class.
        /// </summary>
        public AboutView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "AboutView";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

    }
}