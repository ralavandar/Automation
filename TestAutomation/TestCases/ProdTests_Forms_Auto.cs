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
        public void Prod_Auto_NewCarPurchase()
        {
            tl.autoPage auto = new tl.autoPage(driver, testData);

            // Fill out and submit a QF
            auto.FillOutValidQF();

            // Handle the cross-sells
            // auto.BypassAutoCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(auto.strQFormUID));

            // Verify redirect to My LendingTree
            auto.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void Prod_Auto_UsedCarPurchase()
        {
            tl.autoPage auto = new tl.autoPage(driver, testData);

            // Fill out and submit a QF
            auto.FillOutValidQF();

            // Handle the cross-sells
            // auto.BypassAutoCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(auto.strQFormUID));

            // Verify redirect to My LendingTree
            auto.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void Prod_Auto_Refinance()
        {
            tl.autoPage auto = new tl.autoPage(driver, testData);

            // Fill out and submit a QF
            auto.FillOutValidQF();

            // Verify validation check on Student Annual Income = 0.  Then submit again.
            //System.Threading.Thread.Sleep(2000);
            //Validation.IsTrue(auto.DoesPageContainText("Please correct any errors in entered information"));
            //Validation.IsTrue(auto.DoesPageContainText("You entered a value less than $12,000 a year. Please verify and click \"Show Me My Results\" if this is correct."));
            //System.Threading.Thread.Sleep(2000);
            //auto.SubmitQF();

            // Handle the cross-sells
            // auto.BypassAutoCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(auto.strQFormUID));

            // Verify redirect to My LendingTree
            auto.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void Prod_Auto_LeaseBuyOut()
        {
            tl.autoPage auto = new tl.autoPage(driver, testData);

            // Fill out and submit a QF
            auto.FillOutValidQF();

            // Verify validation check on Student Annual Income = 0.  Then submit again.
            //System.Threading.Thread.Sleep(2000);
            //Validation.IsTrue(auto.DoesPageContainText("Please correct any errors in entered information"));
            //Validation.IsTrue(auto.DoesPageContainText("You entered a value less than $12,000 a year. Please verify and click \"Show Me My Results\" if this is correct."));
            //System.Threading.Thread.Sleep(2000);
            //auto.SubmitQF();

            // Handle the cross-sells
            // auto.BypassAutoCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(auto.strQFormUID));

            // Verify redirect to My LendingTree
            auto.VerifyRedirectToMyLendingTree(testData);
        }
    }
}
