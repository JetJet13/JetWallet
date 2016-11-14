using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Tools
{
    public class CheckNetwork
    {
        public static void Status()
        {
            string url = "http://google.com";
            try
            {
                WebRequests.GET(url);
            }
            catch
            {
                string message = TextTools.RetrieveStringFromResource("Error_N100");
                Messenger.Default.Send<string>(message, "OpenSimpleDialogView");
            }
            
        }
    }
}
