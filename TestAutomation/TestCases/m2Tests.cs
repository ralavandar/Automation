using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;

namespace TestAutomation.LendingTree.tlm
{
    [TestFixture]
    public class m2Tests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private m2Page m2;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            m2 = new m2Page(driver, testData);

            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for m2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13) 
            {
                testData["DateOfBirthMonth"] = DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        [Test]
        public void m2_01_RefiPrimaryFirstAndSecond()
        {
            // Fill out and submit a QF
            m2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void m2_02_RefiSecondaryFirstPlusCash()
        {
            // Fill out and submit a QF
            m2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void m2_03_RefiInvestmentFirstOnlyNoCash()
        {
            // Fill out and submit a QF
            m2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void m2_04_RefiSsnValidationTest()
        {
            IFormField[][] steps = m2.ValidQFSteps;

            m2.StartForm();
            m2.PerformSteps(steps, 1, 21);

            m2.PrepareStep(22, true);
            m2.FillOutStep(steps[22]);
            m2.ConcludeStep();

            // Verify error message on Phone field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(m2.DoesPageContainText("Please enter a valid social security number."));

            // Populate the Dictionary with valid ssn and re-fill step 17
            testData["BorrowerSsn1"] = "980";
            testData["BorrowerSsn2"] = strRandom.Substring(0, 2);
            testData["BorrowerSsn3"] = strRandom.Substring(2, 4);
            m2.FillOutStep(steps[22]);
            m2.ConcludeStep();

            // Targus/Credit oops steps and 'What's Next' step
            m2.PerformSteps(steps, 23, 25);

            FinishTest();
        }

        [Test]
        public void m2_05_PurchasePrimary()
        {
            // Fill out and submit a QF
            m2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void m2_06_PurchaseSecondary()
        {
            // Fill out and submit a QF
            m2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void m2_07_PurchaseInvestmentProperty()
        {
            // Fill out and submit a QF
            m2.FillOutValidQF();

            FinishTest();
        }

        private void FinishTest()
        {
            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(m2.strQFormUID));

            // Verify redirect to My LendingTree
            m2.VerifyRedirectToMyLtExpress(testData);
        }
    }
}





