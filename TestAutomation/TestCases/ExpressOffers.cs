using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestAutomation.Lending;

namespace TestAutomation.LendingTree.ExpressOffers
{
     [TestFixture]
    class ExpressOffers_Mortgage: SeleniumTestBase
    {
        
        private const String strTableName = "tTestData_FormPostMortgage";

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
        }
        [TearDown]
        public void TeardownTest()
        {
            //driver.Quit();
            Common.ReportFinalResults();
        }
        [Test]
        public void Express_PurchaseLocalLender()
        {
            //Create new QF
            Common.ReportEvent(Common.INFO, String.Format("Creating New FormSubmit QF {0}", Guid.NewGuid()));
            BackDoorFormSubmit.FormSubmit(testData);

        }
    }
}
