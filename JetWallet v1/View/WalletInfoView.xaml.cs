﻿using System.Windows;
using MahApps.Metro.Controls;

namespace JetWallet.View
{
    /// <summary>
    /// Description for WalletInfoView.
    /// </summary>
    public partial class WalletInfoView : MetroWindow, IView
    {
        /// <summary>
        /// Initializes a new instance of the WalletInfoView class.
        /// </summary>
        public WalletInfoView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "WalletInfoView";
        }
    }
}