using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using JetWallet.Model;
using NBitcoin.SPV;
using NBitcoin;

namespace JetWallet.Tests.Models
{
    [TestFixture(Author = "Johny Georges", Description = "Testing BlockchainApi methods")]
    public class BlockchainApiTest
    {
        Network _net;
        BlockchainApi _blockapi;

        [SetUp]
        public void Init()
        {
            _net = Network.Main;
            _blockapi = new BlockchainApi(_net);
        }

        [Test]
        public void CheckApiStatusOk()
        {
            Assert.True(_blockapi.ApiStatusOK());            
        }

        [Test]
        public void FetchTxData()
        {
            // Block 50,000 coinbase transaction
            string txid = "27f1d66f8a1ee5280f4e92508dcb647e954d53004905d08a75574daee4988360";
            SoChainTxAPI result = _blockapi.FetchTxData(txid);

            // status code 200 represents a successful response.
            Assert.AreEqual(200, result.code);
        }



    }
}
