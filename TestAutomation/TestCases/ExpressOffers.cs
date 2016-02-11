using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestAutomation.Lending;

namespace TestAutomation.LendingTree.ExpressOffers
{
     [TestFixture]
    class ExpressOffers: SeleniumTestBase
    {
        [SetUp]

        [TearDown]

        [Test]
        public void Express_PurchaseLocaLender()
        {
            BackDoorFormSubmit newQF = new BackDoorFormSubmit();
            newQF.FormSubmit();

        }
    }
}
