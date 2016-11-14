using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetWallet.Tools;
using NUnit.Framework;
using System.Net;

namespace JetWallet.Tests.Tools
{
    [TestFixture(Author ="Johny Georges", Description ="Testing the WebRequest method GET")]
    public class WebRequestTest
    {
        [SetUp]
        public void Init()
        {

        }

        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void GETpass()
        {
            string url = "https://google.ca";
            Assert.DoesNotThrow(() => WebRequests.GET(url));
        }

        [Test]
        public void GETfail()
        {
            string url = "https://gomonomodomoromo.bad";
            Assert.Throws<WebException>(() => WebRequests.GET(url));
        }
    }
}
