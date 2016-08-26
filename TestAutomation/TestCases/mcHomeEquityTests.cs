using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.mc
{
    [TestFixture]
    class mcHomeEquityTests :SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_HomeEquity";
        private mcHomeEquityPage mcHomeEquity;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            mcHomeEquity = new mcHomeEquityPage(driver, testData);
        }

        [Test]
        public void mcHomeEquity_01_Primary()
        {
            // Fill out and submit a QF
            mcHomeEquity.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcHomeEquity_02_Secondary()
        {
            // Fill out and submit a QF
            mcHomeEquity.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcHomeEquity_03_Investment()
        {
            // Fill out and submit a QF
            mcHomeEquity.FillOutValidQF();

            FinishTest();
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        private void FinishTest()
        {
            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(mcHomeEquity.strQFormUID));

            // Verify redirect to My LendingTree
            mcHomeEquity.VerifyRedirectToMyLtExpress(testData);
        }
    }
}
