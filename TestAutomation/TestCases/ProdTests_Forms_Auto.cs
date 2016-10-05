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
    public class ProdTests_Forms_Auto : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Auto";

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
        public void Prod_mc_auto_purchase_New()
        {
            mc.mcAutoPurchasePage mcAutoPurchase = new mc.mcAutoPurchasePage(driver, testData);

            // Fill out and submit a QF
            mcAutoPurchase.FillOutValidQF();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(mcAutoPurchase.strQFormUID));

            // Verify redirect to My LendingTree
            mcAutoPurchase.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_mc_auto_refi()
        {
            mc.mcAutoRefiPage mcAutoRefi = new mc.mcAutoRefiPage(driver, testData);

            // Fill out and submit a QF
            mcAutoRefi.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(mcAutoRefi.strQFormUID));

            // Verify redirect to My LendingTree
            mcAutoRefi.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_a2_NewCarPurchase()
        {
            tlm.a2Page a2 = new tlm.a2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for m2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            // Fill out and submit a QF
            a2.FillOutValidQF();
            
            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(a2.strQFormUID));

            // Verify redirect to My LendingTree
            a2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_a2_Refinance()
        {
            tlm.a2Page a2 = new tlm.a2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for m2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            // Fill out and submit a QF
            a2.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(a2.strQFormUID));

            // Verify redirect to My LendingTree
            a2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_rv2_NewPurchase()
        {
            tlm.rv2Page rv2 = new tlm.rv2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for m2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            //Fill out and submit a QF
            rv2.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(rv2.strQFormUID));

            // Verify redirect to My LendingTree
            rv2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_rv2_Refinance()
        {
            tlm.rv2Page rv2 = new tlm.rv2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for m2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            //Fill out and submit a QF
            rv2.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(rv2.strQFormUID));

            // Verify redirect to My LendingTree
            rv2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_boat2_NewPurchase()
        {
            tlm.boat2Page boat2 = new tlm.boat2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for m2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            // Fill out and submit a QF
            boat2.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(boat2.strQFormUID));

            // Verify redirect to My LendingTree
            boat2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_boat2_Refinance()
        {
            tlm.boat2Page boat2 = new tlm.boat2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for m2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            // Fill out and submit a QF
            boat2.FillOutValidQF();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(boat2.strQFormUID));

            // Verify redirect to My LendingTree
            boat2.VerifyRedirectToMyLtExpress(testData);
        }
    }
}
