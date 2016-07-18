using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;

namespace TestAutomation.LendingTree.tla
{
    [TestFixture]
    public class prequalTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private prequalPage prequal;
        //private PageBase Qfromuid;

        [SetUp]
        public void SetupTest()
        {     
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            prequal = new prequalPage(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
            //int varTestCaseIdTest = int.Parse(testData["TestCaseID"]);
            //string varTestCaseNameTest = testData["TestCaseName"];
            //string varTestEnvironmentTest = testData["TestEnvironment"];
            //string varEmailAddressTest = testData["EmailAddress"];
            //string varPasswordTest = testData["Password"];
            //Common.ReportFinalResultsAuditLog(varTestCaseIdTest, varTestCaseNameTest, varTestEnvironmentTest, varQFormUIDTest, varEmailAddressTest, varPasswordTest);
            //varQFormUIDTest = null;
        }

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(prequal.strQFormUID));

            prequal.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void prequal_01_Primary()
        {
            prequal.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void prequal_02_Secondary()
        {
            prequal.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void prequal_03_Investment()
        {
            prequal.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void prequal_04_SSNValidationTest()
        {
            IFormField[][] steps = prequal.ValidQFSteps;

            prequal.StartForm();
            prequal.PerformSteps(steps, 1, 16);

            prequal.PrepareStep(17, true);
            prequal.FillOutStep(steps[17]);
            prequal.ConcludeStep();

            // Verify error message on Phone field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(prequal.DoesPageContainText("Please enter a valid social security number."));

            // Populate the Dictionary with valid ssn and re-fill step 17
            testData["BorrowerSsn1"] = "980";
            testData["BorrowerSsn2"] = strRandom.Substring(0, 2);
            testData["BorrowerSsn3"] = strRandom.Substring(2, 4);
            prequal.FillOutStep(steps[17]);
            prequal.ConcludeStep();

            // Targus oops step
            //prequal.PerformStep(18, steps[18]);

            FinishTest();
        }
    }
}
