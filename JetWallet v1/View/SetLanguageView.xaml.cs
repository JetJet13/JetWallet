using System.Windows;
using MahApps.Metro.Controls;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace JetWallet.View
{
    /// <summary>
    /// Description for SetLanguageView.
    /// </summary>
    public partial class SetLanguageView : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the SetLanguageView class.
        /// </summary>
        public SetLanguageView()
        {
            InitializeComponent();
            Closed += SetLanguageConfig;
            Closed += OpenWelcomeView;
        }

        private void SetLanguageConfig(object sender, System.EventArgs e)
        {
            var param = this.BtnOK.CommandParameter;
            this.BtnOK.Command.Execute(param);
        }

        private void OpenWelcomeView(object sender, System.EventArgs e)
        {
            Messenger.Default.Send<string>("", "OpenWelcomeView");
        }

        public override string ToString()
        {
            return "SetLanguageView";
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();            
        }

    }
}