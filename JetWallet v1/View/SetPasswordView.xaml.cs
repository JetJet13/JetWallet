using System.Windows;
using MahApps.Metro.Controls;
using JetWallet.Tools;

namespace JetWallet.View
{
    /// <summary>
    /// Description for SetPasswordView.
    /// </summary>
    public partial class SetPasswordView : MetroWindow, IView
    {
        
        /// <summary>
        /// Initializes a new instance of the SetPasswordView class.
        /// </summary>
        public SetPasswordView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "SetPasswordView";
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