using System.Windows;
using MahApps.Metro.Controls;

namespace JetWallet.View
{
    /// <summary>
    /// Description for RecoverWalletView.
    /// </summary>
    public partial class RecoverWalletView : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the RecoverWalletView class.
        /// </summary>
        public RecoverWalletView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "RecoverWalletView";
        }
    }
}