using JetWallet.Tools;
using NBitcoin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Model
{

    public class SoChainTxAPI
    {
        public string status { get; set; }
        public Dictionary<string,object> data { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }

    public class BlockchainApi
    {
                
        private Network _net;
        private string _network;

        public BlockchainApi(Network net)
        {
            _net = net;
            this.SetNetworkString();
        }

        private void SetNetworkString()
        {
            if (_net.Equals(Network.Main))
            {
                _network = "BTC";
            }
            else if (_net.Equals(Network.TestNet))
            {
                _network = "BTCTEST";
            }
        }

        public bool ApiStatusOK()
        {
            
            string url = String.Format("https://chain.so/api/v2/get_info/{0}",_network);            
            try
            {
                WebRequests.GET(url);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public SoChainTxAPI FetchTxData(string txid)
        {            
            string url = String.Format("https://chain.so/api/v2/tx/{0}/{1}", _network, txid);
            string result = WebRequests.GET(url);
            var txdata = JsonConvert.DeserializeObject<SoChainTxAPI>(result);
            return txdata;            
        }

    }
}
