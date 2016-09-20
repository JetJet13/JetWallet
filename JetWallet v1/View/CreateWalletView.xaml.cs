﻿using System.Windows;
using MahApps.Metro.Controls;
using JetWallet_v1.ViewModel;
using JetWallet_v1.Tools;
namespace JetWallet_v1.View
{
    /// <summary>
    /// Description for CreateWalletView.
    /// </summary>
    public partial class CreateWalletView : MetroWindow
    {

        /// <summary>
        /// Initializes a new instance of the CreateWalletView class.
        /// </summary>
        public CreateWalletView()
        {
            InitializeComponent();
        }

        private void PassChanged(object sender, RoutedEventArgs e)
        {
            Global.VML.CreateWallet.Pass  = txtPass.SecurePassword;

        }
    }
}