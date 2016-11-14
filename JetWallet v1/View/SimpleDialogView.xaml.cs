using System.Windows;
using MahApps.Metro.Controls;

namespace JetWallet.View
{
    /// <summary>
    /// Description for FileMissingView.
    /// </summary>
    public partial class SimpleDialogView : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the FileMissingView class.
        /// </summary>
        public SimpleDialogView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "SimpleDialogView";
        }

    }
}