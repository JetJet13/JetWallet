using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NBitcoin;
using NBitcoin.SPV;
using JetWallet.Model;
using Moq;
using JetWallet.Tools;

namespace JetWallet.Tests.Models
{
    [TestFixture(Author ="Johny Georges", Description ="Testing JetTransactionModel properties")]
    public class JetTransactionModelTest
    {
       
        private const string RAW_BLOCK = "01000000741b3828fe3eb36e21ceaba8e8f8475f93db6e578703d0823312000000000000d3e0023c2f9aab07699b6dabd6f036723e1f756fbf98abcf3e94307613d219df3db6dc4df2b9441a487fdf100201000000010000000000000000000000000000000000000000000000000000000000000000ffffffff0704f2b9441a014effffffff0100f2052a010000004341048cc66115faeb38982322cbcbc46448b08c277a61d2141431c1a4cfbdaf717b9cc913c165baa33ec091432d9373d025e6ee34895956c9ff8a2d383196c9abaaefac000000000100000001c5d0712e5359de348cb386cee6d3745a6633652e55b28e57b8e3bf5b963c2238000000008c493046022100f2bd8d19981449e46e11322d52a4ae467c70b6342e20960781dbd6330fd90ab7022100bd260f83ed0cff14734c382c0bf7cbfa04d89f9b5208aabb48010c5259b414940141044073aff779907c50e37029bf635319097991040d6b253cf9b368d054f1259f5293e4cb883449e62fa9ea2620f2636499c66a5c9447c1640efacbd037b0b63308ffffffff02c0fd320f000000001976a914957eb91528cc377c3597e8390e6a0fb7b7eff1aa88ac40ec0834000000001976a9142b2155084264078ad6ecc30b217ee1062bb93cb688ac00000000";
        private const string BLOCK_HASH = "00000000000003b780d187c25ff97b564dc57f0c62e57df3bf53d9be53e0182d";
        private const int BLOCK_HEIGHT = 126554;

        // tx from block 126,554 (above)
        private const string RAW_TX = "0100000001c5d0712e5359de348cb386cee6d3745a6633652e55b28e57b8e3bf5b963c2238000000008c493046022100f2bd8d19981449e46e11322d52a4ae467c70b6342e20960781dbd6330fd90ab7022100bd260f83ed0cff14734c382c0bf7cbfa04d89f9b5208aabb48010c5259b414940141044073aff779907c50e37029bf635319097991040d6b253cf9b368d054f1259f5293e4cb883449e62fa9ea2620f2636499c66a5c9447c1640efacbd037b0b63308ffffffff02c0fd320f000000001976a914957eb91528cc377c3597e8390e6a0fb7b7eff1aa88ac40ec0834000000001976a9142b2155084264078ad6ecc30b217ee1062bb93cb688ac00000000";
        private const string TX_HASH = "d82c6f5cc0abc08f00bb4360c4c0d1943fe11e2ed66f86a2265aeed98045c447";

        Block _block;
        Transaction _tx;

        [SetUp]
        public void Init()
        {
            _block = Block.Parse(RAW_BLOCK);
            _tx = _block.Transactions.Find((x) => x.GetHash().ToString().Equals(TX_HASH));
            
            
        }

        private WalletTransaction GenerateWalletTransaction(int sent = 0, int recvd = 0)
        {
            Coin[] received = new Coin[1] { GenerateCoin(recvd) };
            Coin[] spent = new Coin[1] { GenerateCoin(sent) };

            WalletTransaction wtx = new WalletTransaction();
            wtx.ReceivedCoins = received;
            wtx.SpentCoins = spent;
            
            return wtx;
        }

        private Coin GenerateCoin(int amount)
        {
            var c = new Coin();
            c.Amount = new Money(amount);
            return c;
        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void ReceivedAction()
        {
            var sent = 0;
            var received = 1200;
            var wtx = GenerateWalletTransaction(sent, received);
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = TxAction.Received;
            Assert.AreEqual(expected, tx.Action);
        }

        [Test]
        public void SentAction()
        {
            var sent = 2000;
            var received = 1200;
            var wtx = GenerateWalletTransaction(sent, received);
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = TxAction.Sent;
            Assert.AreEqual(expected, tx.Action);
        }

        [Test]
        public void GetId()
        {
            var wtx = GenerateWalletTransaction();
            wtx.Transaction = _tx;
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = TX_HASH;
            Assert.AreEqual(expected, tx.Id);
        
        }

        [Test]
        public void GetBlockHash()
        {
            var wtx = GenerateWalletTransaction();
            var blockInfo = new BlockInformation();
            blockInfo.Header = _block.Header;
            wtx.BlockInformation = blockInfo;
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = BLOCK_HASH;
            Assert.AreEqual(expected, tx.BlockHash);

        }

        [Test]
        public void GetBlockHashEmpty()
        {
            var wtx = GenerateWalletTransaction();
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = "NA";
            Assert.AreEqual(expected, tx.BlockHash);
        }

        [Test]
        public void GetBlockHeight()
        {
            var wtx = GenerateWalletTransaction();
            var blockInfo = new BlockInformation();
            blockInfo.Header = _block.Header;
            blockInfo.Height = BLOCK_HEIGHT;
            wtx.BlockInformation = blockInfo;
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = BLOCK_HEIGHT;
            Assert.AreEqual(expected, tx.BlockHeight);
        }

        [Test]
        public void NoBlockHeight()
        {
            var wtx = GenerateWalletTransaction();
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = -1;
            Assert.AreEqual(expected, tx.BlockHeight);
        }

        [Test]
        public void ConfirmationsOK()
        {
            var wtx = GenerateWalletTransaction();
            var blockInfo = new BlockInformation();
            blockInfo.Header = _block.Header;
            blockInfo.Confirmations = 1000;
            wtx.BlockInformation = blockInfo;
            ITransaction tx = new JetTransactionModel(wtx);

            var greaterThan = 0;
            Assert.Greater(tx.Confirmations, greaterThan);
        }

        [Test]
        public void ConfirmationsNone()
        {
            var wtx = GenerateWalletTransaction();
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = 0;
            Assert.AreEqual(expected, tx.Confirmations);
        }

        [Test]
        public void DateFromBlock()
        {
            var wtx = GenerateWalletTransaction();
            var blockInfo = new BlockInformation();
            blockInfo.Header = _block.Header;
            wtx.BlockInformation = blockInfo;
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = blockInfo.Header.BlockTime.Date;
            Assert.AreEqual(expected, tx.Date);
        }

        [Test]
        public void DateFromTx()
        {
            var wtx = GenerateWalletTransaction();
            wtx.Transaction = _tx;
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = wtx.AddedDate.Date;
            Assert.AreEqual(expected, tx.Date);
        }

        [Test]
        public void UnconfirmedState()
        {
            var wtx = GenerateWalletTransaction();
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = TxState.Unconfirmed;
            Assert.AreEqual(expected, tx.State);
        }

        [Test]
        public void AwaitingState()
        {
            var wtx = GenerateWalletTransaction();
            var blockInfo = new BlockInformation();
            var notEnoughConfirmations = 1;

            blockInfo.Confirmations = notEnoughConfirmations;
            wtx.BlockInformation = blockInfo;
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = TxState.Awaiting;
            Assert.AreEqual(expected, tx.State);
        }

        [Test]
        public void ConfirmedState()
        {
            var wtx = GenerateWalletTransaction();
            var blockInfo = new BlockInformation();
            var enoughConfirmations = 8; // >= 6 is enough

            blockInfo.Confirmations = enoughConfirmations;
            wtx.BlockInformation = blockInfo;
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = TxState.Confirmed;
            Assert.AreEqual(expected, tx.State);
        }

        [Test]
        public void AmountBTC()
        {
            var sent = 1200;
            var received = 200;
            var wtx = GenerateWalletTransaction(sent, received);
            ITransaction tx = new JetTransactionModel(wtx);

            var expected = new Money(received - sent);
            Assert.AreEqual(expected, tx.AmountBTC);
        }

        [Test]
        public void AmountCurr()
        {
            var sent = 1200;
            var received = 200;
            var wtx = GenerateWalletTransaction(sent, received);
            ITransaction tx = new JetTransactionModel(wtx);

            var balance = new Money(received - sent);
            var expected = JetMoneyTools.CalcBtc2Curr(balance);
            Assert.AreEqual(expected, tx.AmountCurr);
        }


    }
}
