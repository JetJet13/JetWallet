using System.Windows;
using MahApps.Metro.Controls;
using JetWallet_v1.Tools;

namespace JetWallet_v1.View
{
    /// <summary>
    /// Description for PromptPasswordView.
    /// </summary>
    public partial class PromptPasswordView : MetroWindow
    {
       
        /// <summary>
        /// Initializes a new instance of the PromptPasswordView class.
        /// </summary>
        public PromptPasswordView()
        {
            InitializeComponent();
        }

        private void passChanged(object sender, RoutedEventArgs e)
        {
            Global.VML.PromptPassword.PassAttempt = passbox.SecurePassword;
        }
    }
}