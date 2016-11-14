using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Model;

namespace JetWallet.Tools
{
    public class ConverterTools
    {
        public static Hashtable Wallet2Hashtable(IWallet _wallet)
        {
            Hashtable table = new Hashtable();
            table.Add("id", _wallet.Id);
            table.Add("name", _wallet.Name);
            table.Add("description", _wallet.Description);
            table.Add("masterkey", _wallet.MasterKeyWIF);
            table.Add("network", _wallet.NetworkChoice.ToString());

            return table;
        }
    }
}
