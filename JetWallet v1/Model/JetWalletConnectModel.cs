using JetWallet.Tools;
using NBitcoin;
using NBitcoin.Protocol;
using NBitcoin.Protocol.Behaviors;
using NBitcoin.SPV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Model
{
    public class JetWalletConnectModel
    {
        private IWallet _wallet;

        public JetWalletConnectModel(IWallet wallet)
        {
            _wallet = wallet;
        }

        public void Start()
        {
            var connectOK = Task.Run(async () => await StartConnecting()).GetAwaiter().GetResult();
            if (connectOK)
            {
                Configure();
            }
        }

        private const int MAX_NUM_CONNECTIONS = 8;
        private NodesGroup _group;
        private NodeConnectionParameters _connectionparameters;
        private async Task<bool> StartConnecting() {

            return await Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    NodeConnectionParameters parameters = new NodeConnectionParameters();
                    //So we find nodes faster
                    parameters.TemplateBehaviors.Add(new AddressManagerBehavior(GetAddressManager()));
                    //So we don't have to load the chain each time we start
                    parameters.TemplateBehaviors.Add(new ChainBehavior(GetChain()));
                    //Tracker knows which scriptPubKey and outpoints to track
                    parameters.TemplateBehaviors.Add(new TrackerBehavior(GetTracker()));

                    var nodeReq = new NodeRequirement() { RequiredServices = NodeServices.Network };
                    _group = new NodesGroup(_wallet.NetworkChoice, parameters, nodeReq);
                    _group.MaximumNodeConnection = MAX_NUM_CONNECTIONS;
                    _group.Connect();
                    _connectionparameters = parameters;
                    return true;

                }
                catch(Exception e)
                {
                    throw e;
                }
                

            });
            
        }

        private void Configure()
        {
            _wallet.Configure(_group);
        }

        public int GetConnNodes()
        {
            return GetAddressManager().Count;
        }

        public int GetCurrentHeight()
        {
            return GetChain().Height;
        }

        public void Stop()
        {
            SaveResources();
            _connectionparameters = null;
            StopNodeGroup();
        }

        private void SaveResources()
        {
            lock (App.Saving)
            {
                GetAddressManager().SavePeerFile(GetAddrmanFile(), _wallet.NetworkChoice);
                using (var fs = File.Open(GetChainFile(), FileMode.Create))
                {
                    GetChain().WriteTo(fs);
                }
                using (var fs = File.Open(GetTrackerFile(), FileMode.Create))
                {
                    GetTracker().Save(fs);
                }

            }

        }

        private void StopNodeGroup()
        {
            if (_group != null)
            {
                _group.Dispose();
            }
            _group = null;
        }

        private Tracker GetTracker()
        {
            if (_connectionparameters != null)
            {
                return _connectionparameters.TemplateBehaviors.Find<TrackerBehavior>().Tracker;
            }
            string trackerFile = this.GetTrackerFile();

            if (File.Exists(trackerFile))
            {
                try
                {
                    lock (App.Saving)
                    {
                        using (var file = File.OpenRead(trackerFile))
                        {
                            return Tracker.Load(file);
                        }

                    }

                }
                catch (Exception e)
                {
                    throw e;
                }


            }
            else
            {
                return new Tracker();
            }
        }

        private string GetTrackerFile()
        {
            string walletFolderPath = WalletFileTools.GetWalletFolder(_wallet.Id);
            var trackerFilePath = Path.Combine(walletFolderPath, "tracker.dat");
            return trackerFilePath;
        }

        private ConcurrentChain GetChain()
        {
            if (_connectionparameters != null)
            {
                return _connectionparameters.TemplateBehaviors.Find<ChainBehavior>().Chain;
            }
            var chain = new ConcurrentChain(_wallet.NetworkChoice);
            if (File.Exists(GetChainFile()))
            {
                try
                {
                    lock (App.Saving)
                    {                        
                        chain.Load(File.ReadAllBytes(GetChainFile()));
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return chain;
        }

        private string GetChainFile()
        {
            string walletFolderPath = WalletFileTools.GetWalletFolder(_wallet.Id);
            var chainFilePath = Path.Combine(walletFolderPath, "chain.dat");
            return chainFilePath;
        }

        private AddressManager GetAddressManager()
        {
            if (_connectionparameters != null)
            {
                return _connectionparameters.TemplateBehaviors.Find<AddressManagerBehavior>().AddressManager;
            }
            if (File.Exists(GetAddrmanFile()))
            {
                try
                {
                    lock (App.Saving)
                    {
                        return AddressManager.LoadPeerFile(GetAddrmanFile());
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return new AddressManager();
        }

        private string GetAddrmanFile()
        {
            string walletFolderPath = WalletFileTools.GetWalletFolder(_wallet.Id);
            var addrFilePath = Path.Combine(walletFolderPath, "addrman.dat");
            return addrFilePath;
        }

        

       

        private void PurgeNodes()
        {
            if (_wallet.IsConnected())
            {
                _group.Purge("Group Purge::Replenishing Bloom Filters for Privacy reasons");
                return;              
            }
            throw new Exception("Cannot purge nodes because wallet is not connected.");

        }

    }

}
