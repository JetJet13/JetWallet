using System.Windows;
using MahApps.Metro.Controls;
using JetWallet_v1.Tools;

namespace JetWallet_v1.View
{
    /// <summary>
    /// Description for SetPasswordView.
    /// </summary>
    public partial class SetPasswordView : MetroWindow
    {
        
        /// <summary>
        /// Initializes a new instance of the SetPasswordView class.
        /// </summary>
        public SetPasswordView()
        {
            InitializeComponent();
        }

        private void NewPassChanged(object sender, RoutedEventArgs e)
        {
            Global.VML.SetPassword.NewPass = TxtNewPass.SecurePassword;

        }
        private void ConfPassChanged(object sender, RoutedEventArgs e)
        {
            Global.VML.SetPassword.ConfPass = TxtConfPass.SecurePassword;

        }
    }
}