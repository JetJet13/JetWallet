using System.Windows;
using MahApps.Metro.Controls;

namespace JetWallet.View
{
    /// <summary>
    /// Description for RecoveryPhraseView.
    /// </summary>
    public partial class RecoveryPhraseView : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the RecoveryPhraseView class.
        /// </summary>
        public RecoveryPhraseView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "RecoveryPhraseView";
        }
    }
}