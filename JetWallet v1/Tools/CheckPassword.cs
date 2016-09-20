using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;

namespace JetWallet_v1.Tools
{
    class CheckPassword
    {         
        public static void InvokePasswordPrompt(string file)
        {
            if (File.Exists(file))
            {                     
                Messenger.Default.Send<string>(file, "OpenPasswordPrompt");
            }
            else
            {               
                string message = TextTools.RetrieveStringFromResource("Error_A400");
                Messenger.Default.Send<string>("", "CloseWallet");
                Messenger.Default.Send<string>(message, "OpenSimpleDialogView");              

            }
        }


        
        public static void WalletLockPrompt(string file)
        {
            if (File.Exists(file))
            {
                Messenger.Default.Send<string>(file, "OpenPasswordPrompt");
            }
            else
            {
                string message = TextTools.RetrieveStringFromResource("Error_A400");
                Messenger.Default.Send<string>("", "CloseWallet");
                Messenger.Default.Send<string>(message, "OpenSimpleDialogView");

            }
        }

    }
}
