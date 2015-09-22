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
    public class mcPurchaseTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private mcPurchasePage mcPurchase;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            mcPurchase = new mcPurchasePage(driver, testData);
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
            Validation.IsTrue(VerifytQFormRecord(mcPurchase.strQFormUID));

            // Verify redirect to My LendingTree
            mcPurchase.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void mcPurchase_01_PurchasePrimary()
        {
            // Fill out and submit a QF
            mcPurchase.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcPurchase_02_PurchaseSecondary()
        {
            // Fill out and submit a QF
            mcPurchase.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcPurchase_03_PurchaseInvestmentProperty()
        {
            // Fill out and submit a QF
            mcPurchase.FillOutValidQF();

            FinishTest();
        }
    }
}




