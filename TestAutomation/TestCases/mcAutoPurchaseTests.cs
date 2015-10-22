using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestAutomation.LendingTree.mc
{
    [TestFixture]
    public class mcAutoPurchaseTest : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Auto";
        private mcAutoPurchasePage mcAutoPurchase;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            Console.WriteLine("Before Initialize");
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            mcAutoPurchase = new mcAutoPurchasePage(driver, testData);
        }

        [Test]
        public void mc_auto_purchase_01_New()
        {
            // Fill out and submit a QF
            mcAutoPurchase.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mc_auto_purchase_02_Used()
        {
            // Fill out and submit a QF
            mcAutoPurchase.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void Prod_mc_auto_purchase_New()
        {
            // Fill out and submit a QF
            mcAutoPurchase.FillOutValidQF();

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
            Assert.IsTrue(VerifytQFormRecord(mcAutoPurchase.strQFormUID));

            // Verify redirect to My LendingTree
            mcAutoPurchase.VerifyRedirectToMyLtExpress(testData);
        }

    }
}
