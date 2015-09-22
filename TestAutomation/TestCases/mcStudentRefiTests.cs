using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestAutomation.LendingTree.mc
{
    [TestFixture]
    public class mcStudentRefiTest : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_StudentLoan";
        private mcStudentRefiPage mcStudentRefi;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            //Console.WriteLine("Before Initialize");     
            InitializeTestData();
            //Code to remove text from last intitute attended
            string strLastIns = testData["LastInstitutionAttended"];
            int lenth = strLastIns.Length;
            testData["LastInstitutionAttended"] = testData["LastInstitutionAttended"].Substring(0, lenth - 2);

            driver = StartBrowser(testData["BrowserType"]);
            mcStudentRefi = new mcStudentRefiPage(driver, testData);
        }

        [Test]
        public void mcStudentRefi_01_FullTime()
        {
            // Fill out and submit a QF
            mcStudentRefi.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcStudentRefi_02_PartTime()
        {
            // Fill out and submit a QF
            mcStudentRefi.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcStudentRefi_03_Self()
        {
            // Fill out and submit a QF
            mcStudentRefi.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcStudentRefi_04_MastersPartTime()
        {
            // Fill out and submit a QF
            mcStudentRefi.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void mcStudentRefi_05_DocUnemployed()
        {
            // Fill out and submit a QF
            mcStudentRefi.FillOutValidQF();

            FinishTest();
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
            Assert.IsTrue(VerifytQFormRecord(mcStudentRefi.strQFormUID));

            // Verify redirect to My LendingTree
            mcStudentRefi.VerifyRedirectToMyLendingTree(testData);
        }

    }
}
