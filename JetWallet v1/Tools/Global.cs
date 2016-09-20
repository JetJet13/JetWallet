using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;
using JetWallet.Model;

namespace JetWallet.Tools
{
    // This Class holds all global variables
    public class Global
    {        
        public static ViewModelLocator VML = new ViewModelLocator();
          
        public static WalletModel ActiveWallet
        {
            get { return VML.Main.MainWallet; }
        }      
    }
}
