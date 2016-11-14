using System.Windows;
using MahApps.Metro.Controls;

namespace JetWallet.View
{
    /// <summary>
    /// Description for TxInfoView.
    /// </summary>
    public partial class TxInfoView : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the TxInfoView class.
        /// </summary>
        public TxInfoView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "TxInfoView";
        }
    }
}