using System.Windows;
using MahApps.Metro.Controls;

namespace JetWallet.View
{
    /// <summary>
    /// Description for ReceiveView.
    /// </summary>
    public partial class ReceiveView : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the ReceiveView class.
        /// </summary>
        public ReceiveView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "ReceiveView";
        }
    }
}