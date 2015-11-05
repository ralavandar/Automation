using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree
{
    [TestFixture]
    public class ProdTests_Forms_Mortgage : SeleniumTestBase
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
        public void Prod_Mortgage2_Refinance()
        {
            tla.mortgage2Page mortgage2 = new tla.mortgage2Page(driver, testData);

            // Fill out and submit a QF
            mortgage2.FillOutValidQF();
            mortgage2.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(mortgage2.strQFormUID));

            // Verify redirect to My LendingTree
            mortgage2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_Mortgage2_Purchase()
        {
            tla.mortgage2Page mortgage2 = new tla.mortgage2Page(driver, testData);

            // Fill out and submit a QF
            mortgage2.FillOutValidQF();
            mortgage2.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(mortgage2.strQFormUID));

            // Verify redirect to My LendingTree
            mortgage2.VerifyRedirectToMyLtExpress(testData);
        }


        [Test]
        public void Prod_mcPurchase()
        {
            mc.mcPurchasePage mcPurchase = new mc.mcPurchasePage(driver, testData);

            // Fill out and submit a QF
            mcPurchase.FillOutValidQF();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(mcPurchase.strQFormUID));

            // Verify redirect to My LendingTree
            mcPurchase.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_mcRefi()
        {
            mc.mcRefiPage mcRefi = new mc.mcRefiPage(driver, testData);

            // Fill out and submit a QF
            mcRefi.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(mcRefi.strQFormUID));

            // Verify redirect to My LendingTree
            mcRefi.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_Prequal_01()
        {
            tla.prequalPage prequal = new tla.prequalPage(driver, testData);

            // Fill out and submit a QF
            prequal.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(prequal.strQFormUID));

            // Verify redirect to My LendingTree
            prequal.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_Prequal_02()
        {
            tla.prequalPage prequal = new tla.prequalPage(driver, testData);

            // Fill out and submit a QF
            prequal.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(prequal.strQFormUID));

            // Verify redirect to My LendingTree
            prequal.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_m2_Purchase()
        {
            tlm.m2Page m2 = new tlm.m2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for m2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            // Fill out and submit a QF
            m2.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(m2.strQFormUID));

            // Verify redirect to My LendingTree
            m2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_m2_Refinance()
        {
            tlm.m2Page m2 = new tlm.m2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for m2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            // Fill out and submit a QF
            m2.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(m2.strQFormUID));

            // Verify redirect to My LendingTree
            m2.VerifyRedirectToMyLtExpress(testData);
        }
    }
}
