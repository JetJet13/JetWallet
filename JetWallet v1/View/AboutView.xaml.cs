using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Navigation;

namespace JetWallet_v1.View
{
    /// <summary>
    /// Description for AboutView.
    /// </summary>
    public partial class AboutView : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the AboutView class.
        /// </summary>
        public AboutView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void Hyperlink_RequestNavigate_1(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }
    }
}