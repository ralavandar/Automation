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
    public class mcAutoRefiTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Auto";
        private mcAutoRefiPage mcAutoRefi;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            Console.WriteLine("Before Initialize");
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            mcAutoRefi = new mcAutoRefiPage(driver, testData);
        }

        [Test]
        public void mc_auto_refi_01_Refinance()
        {
            // Fill out and submit a QF
            mcAutoRefi.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void Prod_mc_auto_refi()
        {
            // Fill out and submit a QF
            mcAutoRefi.FillOutValidQF();

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
            Assert.IsTrue(VerifytQFormRecord(mcAutoRefi.strQFormUID));

            // Verify redirect to My LendingTree
            mcAutoRefi.VerifyRedirectToMyLtExpress(testData);
        }

    }
}
