﻿using System.Windows;
using MahApps.Metro.Controls;
using JetWallet.Tools;

namespace JetWallet.View
{
    /// <summary>
    /// Description for SendView.
    /// </summary>
    public partial class SendView : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the SendView class.
        /// </summary>
        public SendView()
        {
            InitializeComponent();
        }

        private void passChanged(object sender, RoutedEventArgs e)
        {
            Global.VML.Send.PassAttempt = passbox.SecurePassword;
        }
    }
}