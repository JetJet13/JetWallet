using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Navigation;

namespace JetWallet.View
{
    /// <summary>
    /// Description for LicenseView.
    /// </summary>
    public partial class LicenseView : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the LicenseView class.
        /// </summary>
        public LicenseView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}