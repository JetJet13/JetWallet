using System.Windows;
using MahApps.Metro.Controls;
using JetWallet.Tools;

namespace JetWallet.View
{
    /// <summary>
    /// Description for PromptPasswordView.
    /// </summary>
    public partial class PromptPasswordView : MetroWindow, IView
    {
       
        /// <summary>
        /// Initializes a new instance of the PromptPasswordView class.
        /// </summary>
        public PromptPasswordView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "PromptPasswordView";
        }

        private void passChanged(object sender, RoutedEventArgs e)
        {
            Global.VML.PromptPassword.PassAttempt = passbox.SecurePassword;
        }
    }
}