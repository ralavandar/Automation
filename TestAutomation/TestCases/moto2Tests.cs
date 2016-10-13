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
    class moto2Tests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Auto";
        private moto2Page moto2;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            moto2 = new moto2Page(driver, testData);

            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for h2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }
        }

        [Test]
        public void moto2_01_NewPurchase()
        {
             // Fill out and submit a QF
            moto2.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void moto2_02_UsedPurchase()
        {
            // Fill out and submit a QF
            moto2.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void moto2_03_Refinance()
        {
            // Fill out and submit a QF
            moto2.FillOutValidQF();
            FinishTest();
        }
        
        private void FinishTest()
        {
            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(moto2.strQFormUID));
            // Verify redirect to My LendingTree
            moto2.VerifyRedirectToMyLtExpress(testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

    }
}


