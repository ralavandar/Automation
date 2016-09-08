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
    class a2Tests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Auto";
        private a2Page a2;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            a2 = new a2Page(driver, testData);

            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for h2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }
        }

        [Test]
        public void a2_01_NewCarPurchase()
        {
            // Fill out and submit a QF
            a2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void a2_02_UsedCarPurchase()
        {
            // Fill out and submit a QF
            a2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void a2_03_Refinance()
        {
            // Fill out and submit a QF
            a2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void a2_04_LeaseBuyOut()
        {
            // Fill out and submit a QF
            a2.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void Prod_a2_NewCarPurchase()
        {
            // Fill out and submit a QF
            a2.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void Prod_a2_Refinance()
        {
            // Fill out and submit a QF
            a2.FillOutValidQF();
            FinishTest();
        }

        private void FinishTest()
        {
            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(a2.strQFormUID));
            // Verify redirect to My LendingTree
            a2.VerifyRedirectToMyLtExpress(testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

    }
}


