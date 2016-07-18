using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.tla
{
    [TestFixture]
    public class mortgage2Tests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private mortgage2Page mortgage2;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            mortgage2 = new mortgage2Page(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        [Test]
        public void mortgage2_01_RefiPrimaryFirstAndSecond()
        {
            // Fill out and submit a QF
            mortgage2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mortgage2_02_RefiSecondaryFirstPlusCash()
        {
            // Fill out and submit a QF
            mortgage2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mortgage2_03_RefiInvestmentFirstOnlyNoCash()
        {
            // Fill out and submit a QF
            mortgage2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mortgage2_04_RefiPhoneValidationTest()
        {
            IFormField[][] steps = mortgage2.ValidQFSteps;

            mortgage2.StartForm();
            mortgage2.PerformSteps(steps, 1, 15);

            mortgage2.PrepareStep(16, true);
            mortgage2.FillOutStep(steps[16]);

            // Verify error message on Phone field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(mortgage2.DoesPageContainText("Please enter a valid phone number."));

            // Populate the Dictionary with valid phone and re-fill step 16
            testData["BorrowerHomePhone1"] = "407";
            testData["BorrowerHomePhone2"] = "939";
            testData["BorrowerHomePhone3"] = "3463";
            mortgage2.FillOutStep(steps[16]);
            mortgage2.ConcludeStep();

            // SSN step
            mortgage2.PerformStep(17, steps[17]);

            FinishTest();
        }

        [Test]
        public void mortgage2_05_PurchasePrimary()
        {
            // Fill out and submit a QF
            mortgage2.FillOutValidQF();

            FinishTest();
            
        }

        [Test]
        public void mortgage2_06_PurchaseSecondary()
        {
            // Fill out and submit a QF
            mortgage2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mortgage2_07_PurchaseInvestmentProperty()
        {
            // Fill out and submit a QF
            mortgage2.FillOutValidQF();

            FinishTest();
        }

        private void FinishTest()
        {
            mortgage2.BypassCrossSells();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(mortgage2.strQFormUID));

            // Verify redirect to My LendingTree
            mortgage2.VerifyRedirectToMyLtExpress(testData);
        }
    }
}




