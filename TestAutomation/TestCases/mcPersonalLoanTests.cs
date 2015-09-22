using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.mc
{
    [TestFixture]
    class mcPersonalLoanTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_PersonalLoan";
        private mcPersonalLoanPage mcPersonal;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            mcPersonal = new mcPersonalLoanPage(driver, testData);
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
            Validation.IsTrue(VerifytQFormRecord(mcPersonal.strQFormUID));

            // Verify redirect to My LendingTree
            mcPersonal.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void mcPersonal_01_Own()
        {
            // Fill out and submit a QF
            mcPersonal.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void mcPersonal_02_Rent()
        {
            // Fill out and submit a QF
            mcPersonal.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void mcPersonal_03_Other()
        {
            // Fill out and submit a QF
            mcPersonal.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void mcPersonal_04_SSNvalidation()
        {
            IFormField[][] steps = mcPersonal.ValidQFSteps;
            //maximize the window
            driver.Manage().Window.Maximize();
            //Login to Money Center
            mcPersonal.StartForm();
            mcPersonal.PerformSteps(steps, 1, 6);

            // Verify error message on Phone field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(mcPersonal.DoesPageContainText("Please enter a valid social security number."));

            // Populate the Dictionary with vali d phone and re-fill step 16
            testData["BorrowerSsn1"] = "980";
            testData["BorrowerSsn2"] = "39";
            testData["BorrowerSsn3"] = "1437";
            mcPersonal.PrepareStep(6, true);
            mcPersonal.FillOutStep(steps[6]);
            mcPersonal.ConcludeStep();
            mcPersonal.PerformStep(7, steps[7]);
            FinishTest();
        }
    }
}
