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
    public class p2Tests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_PersonalLoan";
        private p2Page p2;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            p2 = new p2Page(driver, testData);

            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for p2 form
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
        public void p2_desktop_01_Own()
        {
            // Fill out and submit a QF
            p2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void p2_desktop_02_Rent()
        {
            // Fill out and submit a QF
            p2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void p2_desktop_03_Other()
        {
            // Fill out and submit a QF
            p2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void p2_desktop_04_CreditScore()
        {
            // Fill out and submit a QF
            p2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void p2_desktop_05_NoMatch()
        {
            // Fill out and submit a QF
            p2.FillOutValidQF();

            FinishTest();
        }

        private void FinishTest()
        {
            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(p2.strQFormUID));

            // Verify redirect to My LendingTree
            p2.VerifyRedirectToMyLtExpress(testData);
        }
    }
}





