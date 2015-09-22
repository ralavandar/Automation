using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.zzArchive
{
    [TestFixture]
    public class ltreturnTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";

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
        public void SubmitRefinanceForm_PrimaryHome()
        {
            ltreturnPage lt_return = new ltreturnPage(driver);

            // Fill out and submit a QF
            lt_return.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_return.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_return.strQFormUID));

            // Verify redirect to My LendingTree
            lt_return.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void SubmitRefinanceForm_InvestmentProperty()
        {
            ltreturnPage lt_return = new ltreturnPage(driver);

            // Fill out and submit a QF
            lt_return.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_return.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_return.strQFormUID));

            // Verify redirect to My LendingTree
            lt_return.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void SubmitRefinanceForm_SecondaryHome()
        {
            ltreturnPage lt_return = new ltreturnPage(driver);

            // Fill out and submit a QF
            lt_return.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_return.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_return.strQFormUID));

            // Verify redirect to My LendingTree
            lt_return.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void SubmitPurchaseForm_PrimaryHome()
        {
            ltreturnPage lt_return = new ltreturnPage(driver);

            // Fill out and submit a QF
            lt_return.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_return.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_return.strQFormUID));

            // Verify redirect to My LendingTree
            lt_return.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void SubmitPurchaseForm_InvestmentProperty()
        {
            ltreturnPage lt_return = new ltreturnPage(driver);

            // Fill out and submit a QF
            lt_return.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_return.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_return.strQFormUID));

            // Verify redirect to My LendingTree
            lt_return.VerifyRedirectToMyLendingTree(testData);
        }
    }
}
