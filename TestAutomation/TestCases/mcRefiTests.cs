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
    public class mcRefiTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private mcRefiPage mcRefin;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            mcRefin = new mcRefiPage(driver, testData);
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
            Validation.IsTrue(VerifytQFormRecord(mcRefin.strQFormUID));

            // Verify redirect to My LendingTree
            mcRefin.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void mcRefi_01_RefiPrimaryFirstAndSecond()
        {
            // Fill out and submit a QF
            mcRefin.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcRefi_02_RefiSecondaryFirstPlusCash()
        {
            // Fill out and submit a QF
            mcRefin.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcRefi_03_RefiInvestmentFirstOnlyNoCash()
        {
            // Fill out and submit a QF
            mcRefin.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcRefi_04_RefiPhoneValidationTest()
        {
            IFormField[][] steps = mcRefin.ValidQFSteps;

            driver.Manage().Window.Maximize();
            mcRefin.StartForm();
            mcRefin.PerformSteps(steps, 1, 13);

            // Verify error message on Phone field
            System.Threading.Thread.Sleep(5000);
            Assert.IsTrue(mcRefin.DoesPageContainText("Please enter a valid phone number."));

            // Populate the Dictionary with valid phone and re-fill step 13
            testData["BorrowerHomePhone1"] = "407";
            testData["BorrowerHomePhone2"] = "939";
            testData["BorrowerHomePhone3"] = "3463";
            //Refill Step 13 and auto advance
            mcRefin.PerformSteps(steps, 13, 13);
            FinishTest();
        }
    }
}
