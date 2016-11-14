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
    [TestFixture(Author = "Johny Georges", Description = "Testing TransactionModel props")]
    public class TransactionModelTest
    {
        WalletTransaction _wtx;
        TransactionModel _txmodel;

        [SetUp]
        public void Init()
        {
            _wtx = new WalletTransaction();
            var _tx = Transaction.Parse("010000000200010000000000000000000000000000000000000000000000000000000000000000000049483045022100d180fd2eb9140aeb4210c9204d3f358766eb53842b2a9473db687fa24b12a3cc022079781799cd4f038b85135bbe49ec2b57f306b2bb17101b17f71f000fcab2b6fb01ffffffff0002000000000000000000000000000000000000000000000000000000000000000000004847304402205f7530653eea9b38699e476320ab135b74771e1c48b81a5d041e2ca84b9be7a802200ac8d1f40fb026674fe5a5edd3dea715c27baa9baca51ed45ea750ac9dc0a55e81ffffffff010100000000000000015100000000");
            _wtx.Transaction = _tx;
            _wtx.ReceivedCoins = _tx.Outputs.AsCoins().ToArray();
            _wtx.SpentCoins = new Coin[0];
            _txmodel = new TransactionModel(_wtx);
           
            
        }


        [Test]
        public void CheckId()
        {
            var expectedId = _wtx.Transaction.GetHash().ToString();
            var actualId = _txmodel.Id;
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void CheckTxState()
        {
            var expectedState = TxState.Unconfirmed;
            var actualState = _txmodel.State;
            Assert.AreEqual(expectedState, actualState);
        }

        [Test]
        public void CheckAction()
        {
            var expectedAction = TxAction.Received;
            var actualAction = _txmodel.Action;
            Assert.AreEqual(expectedAction, actualAction);
        }

        [Test]
        public void CheckNumConfs()
        {
            var expectedNumConfs = "0";
            var actualNumConfs = _txmodel.Confirmations;
            Assert.AreEqual(expectedNumConfs, actualNumConfs);
        }

        [Test]
        public void CheckBlockId()
        {
            string expectedBlockId = string.Empty;
            string actualBlockId = _txmodel.BlockId;
            Assert.AreEqual(expectedBlockId, actualBlockId);

        }
    }
}
