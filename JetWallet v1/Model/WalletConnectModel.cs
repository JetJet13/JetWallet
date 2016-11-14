using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using NBitcoin.Protocol;
using NBitcoin.Protocol.Behaviors;
using System.IO;
using NBitcoin;
using NBitcoin.SPV;
using JetWallet.Tools;

namespace JetWallet.Model
{
    public class WalletConnectModel : INotifyPropertyChanged
    {
        const int MAX_NUM_CONNECTIONS = 8;
        private WalletModel _wallet;
        private bool _update = true;

        private int _connodes;
        public int ConNodes
        {
            get { return _connodes; }
            set
            {
                if (_connodes == value)
                {
                    return;
                }
                _connodes = value;
                OnPropertyChanged("ConNodes");

            }
        }

        private int _currheight;
        public int CurrHeight
        {
            get { return _currheight; }
            set
            {
                if (_currheight == value)
                {
                    return;
                }
                _currheight = value;
                OnPropertyChanged("CurrHeight");
            }
        }
        

        public WalletConnectModel(WalletModel w)
        {            
            _wallet = w;
            StartConnecting();
        }
       
        private NodesGroup _group;
        private NodeConnectionParameters _connectionparameters;
        private async void StartConnecting()
        {                        
            await Task.Factory.StartNew(() =>
            {

                    NodeConnectionParameters parameters = new NodeConnectionParameters();
                    //So we find nodes faster
                    parameters.TemplateBehaviors.Add(new AddressManagerBehavior(this.GetAddressManager()));
                    //So we don't have to load the chain each time we start
                    parameters.TemplateBehaviors.Add(new ChainBehavior(this.GetChain()));
                    //Tracker knows which scriptPubKey and outpoints to track
                    parameters.TemplateBehaviors.Add(new TrackerBehavior(this.GetTracker()));
                    var nodeReq = new NodeRequirement() { RequiredServices = NodeServices.Network };
                    _group = new NodesGroup(_wallet.NetworkChoice, parameters, nodeReq);
                    _group.MaximumNodeConnection = MAX_NUM_CONNECTIONS;
                    _group.Connect();
                    _connectionparameters = parameters;             
                
            });

            _wallet.Configure(_group);
            _wallet.Connect();
            _wallet.Connected = true;

            PeriodicKick();
            PeriodicSave();
            
                      
            
        }

        private async void AsyncSaveResources()
        {
           await  Task.Factory.StartNew(() =>
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

            });
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

        private string GetTrackerFile()
        {            
            var trackerFilePath = Path.Combine(_wallet.GetWalletFolderPath(), "tracker.dat");
            return trackerFilePath;
        }

        private string GetChainFile()
        {
            var chainFilePath = Path.Combine(_wallet.GetWalletFolderPath(), "chain.dat");           
            return chainFilePath;
        }

        private string GetAddrmanFile()
        {
            var addrFilePath = Path.Combine(_wallet.GetWalletFolderPath(), "addrman.dat");
            return addrFilePath;
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
                    Logger.WriteLine("GetTracker::Exception => " + e.Message);
                    return new Tracker();
                }


            }
            else
            {
                return new Tracker();
            }
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
                    Logger.WriteLine("GetChain::Exception => " + e.Message);
                }
            }
            return chain;
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
                    Logger.WriteLine("GetAddressManager::Exception => " + e.Message);
                    return new AddressManager();
                }

            }
            return new AddressManager();
        }

        const double MINUTE_DELAY = 10;
        private async void PeriodicKick()
        {
            while (_update)
            {
                await Task.Delay(TimeSpan.FromMinutes(MINUTE_DELAY));
                if (!_update) break;
                //Renewing Bloom Filters on Fresh Nodes
                this.PurgeNodes();


            }
        }

        private async void PeriodicSave()
        {
            while (_update)
            {
                await Task.Delay(10000);
                if (!_update) break;
                this.AsyncSaveResources();                
            }
        }

        
        public void PurgeNodes()
        {
            if (_group.ConnectedNodes.Count > 0)
            {
                _group.Purge("Group Purge::Replenishing Bloom Filters for Privacy reasons");
            }
         
        }

        public void Stop()
        {
            _update = false;
            this.SaveResources();
            this.StopNodeGroup();
            _connectionparameters = null;
            _wallet = null;            
        }

        private void StopNodeGroup()
        {
            if (_group != null)
            {
                _group.Dispose();
            }
                _group = null;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
