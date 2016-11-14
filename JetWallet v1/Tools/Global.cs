using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;
using JetWallet.Model;
using JetWallet.Controller;

namespace JetWallet.Tools
{
    public sealed class JetGlobal
    {
        private static readonly ConfigFileManager _cfm = new ConfigFileManager();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static JetGlobal()
        {
        }

        private JetGlobal()
        {
        }

        public static ConfigFileManager CFM
        {
            get
            {
                return _cfm;
            }
        }
    }
    // This Class holds all global variables
    public class Global
    {        
        public static ViewModelLocator VML = new ViewModelLocator();

        public static ViewController VC = new ViewController();

        public static ConfigFileManager CFM = new ConfigFileManager();
          
        public static WalletModel ActiveWallet
        {
            get { return VML.Main.MainWallet; }
        }      
    }
}
