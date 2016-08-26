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
    class h2Tests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_HomeEquity";
        private h2Page h2;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            h2 = new h2Page(driver, testData);

            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for h2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }
        }

        [Test]
        public void h2_01_PrimaryFirstAndSecond()
        {
            // Fill out and submit a QF
            h2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void h2_02_SecondaryFirstOnly()
        {
            // Fill out and submit a QF
            h2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void h2_03_InvestmentPropNoCurrentMortgage()
        {
            // Fill out and submit a QF
            h2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void h2_04_LeadingZeros()
        {
            // Fill out and submit a QF
            h2.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void h2_05_CreditPullSuccess()
        {
            // Fill out and submit a QF
            h2.FillOutValidQF();
            FinishTest();
        }

        private void FinishTest()
        {
            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(h2.strQFormUID));
            // Verify redirect to My LendingTree
            h2.VerifyRedirectToMyLtExpress(testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

    }
}

namespace TestAutomation.LendingTree.ProdTests_Forms_Other
{

    [TestFixture]
    public class homeequityTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_HomeEquity";
        private tlm.h2Page h2;
        private tla.heaPage hea;

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
        public void Prod_h2_01()
        {
            // Fill out and submit a QF
            h2 = new tlm.h2Page(driver, testData);
            ConvertDOBMonth();
            h2.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(h2.strQFormUID));
            h2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_h2_02()
        {
            // Fill out and submit a QF
            h2 = new tlm.h2Page(driver, testData);
            ConvertDOBMonth();
            h2.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(h2.strQFormUID));
            h2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_hea_01()
        {
            // Fill out and submit a QF
            hea = new tla.heaPage(driver, testData);
            hea.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(hea.strQFormUID));
            hea.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_hea_02()
        {
            // Fill out and submit a QF
            hea = new tla.heaPage(driver, testData);
            hea.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(hea.strQFormUID));
            hea.VerifyRedirectToMyLtExpress(testData);
        }

        private void ConvertDOBMonth()
        {
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for h2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }
        }
    }
}

