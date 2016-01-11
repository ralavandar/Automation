using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree
{
    [TestFixture]
    public class ProdTests_Forms_Auto : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Auto";

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        [Test]
        public void Prod_mc_auto_purchase_New()
        {
            mc.mcAutoPurchasePage mcAutoPurchase = new mc.mcAutoPurchasePage(driver, testData);

            // Fill out and submit a QF
            mcAutoPurchase.FillOutValidQF();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(mcAutoPurchase.strQFormUID));

            // Verify redirect to My LendingTree
            mcAutoPurchase.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_mc_auto_refi()
        {
            mc.mcAutoRefiPage mcAutoRefi = new mc.mcAutoRefiPage(driver, testData);

            // Fill out and submit a QF
            mcAutoRefi.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(mcAutoRefi.strQFormUID));

            // Verify redirect to My LendingTree
            mcAutoRefi.VerifyRedirectToMyLtExpress(testData);
        }
    }
}
