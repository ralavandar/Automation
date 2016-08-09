using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.zzArchive
{
    [TestFixture]
    public class studentRefiTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_StudentLoan";
        private studentRefiPage studentRefi;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            string strLastIns = testData["LastInstitutionAttended"];
            int lenth = strLastIns.Length;
            testData["LastInstitutionAttended"] = testData["LastInstitutionAttended"].Substring(0, lenth - 2);
            driver = StartBrowser(testData["BrowserType"]);//testData["BrowserType"]
            studentRefi = new studentRefiPage(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        [Test]
        public void student_refi_01_FullTime()
        {
            // Fill out and submit a QF
            studentRefi.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void student_refi_02_PartTime()
        {
            // Fill out and submit a QF
            studentRefi.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void student_refi_03_Self()
        {
            // Fill out and submit a QF
            studentRefi.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void student_refi_04_SSNValidation()
        {
            IFormField[][] steps = studentRefi.ValidQFSteps;
            //maximize the window
            driver.Manage().Window.Maximize();
            //Navigate to Student Refinance Loan Page
            studentRefi.StartForm();
            studentRefi.PerformSteps(steps, 1, 14);
            // Verify error message on Phone field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(studentRefi.DoesPageContainText("Please enter a valid social security number."));
            // Populate the Dictionary with vali d phone and re-fill step 14
            testData["BorrowerSsn1"] = "980";
            testData["BorrowerSsn2"] = "39";
            testData["BorrowerSsn3"] = "1437";
            studentRefi.PerformSteps(steps, 14, 15);
            FinishTest();
        }

        [Test]
        public void student_refi_05_UnemployedCreditPullSuccess()
        {
            // Fill out and submit a QF
            studentRefi.FillOutValidQF();

            FinishTest();
        }

        private void FinishTest()
        {
            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(studentRefi.strQFormUID));

            // Verify redirect to My LendingTree
            studentRefi.VerifyRedirectToMyLtExpress(testData);
        }
    }
}


namespace TestAutomation.LendingTree.ProdTests_Forms_Other
{
    [TestFixture]
    public class studentLoanTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_StudentLoan";
        private tla.studentRefiPage studentRefi;
        private mc.mcStudentRefiPage mcStudentRefi;

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
        public void Prod_StudentRefi_01()
        {
            studentRefi = new tla.studentRefiPage(driver, testData);
            studentRefi.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void Prod_StudentRefi_02()
        {
            studentRefi = new tla.studentRefiPage(driver, testData);
            studentRefi.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void Prod_mcStudentRefi_01()
        {
            mcStudentRefi = new mc.mcStudentRefiPage(driver, testData);
            mcStudentRefi.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(mcStudentRefi.strQFormUID));
            mcStudentRefi.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_mcStudentRefi_02()
        {
            mcStudentRefi = new mc.mcStudentRefiPage(driver, testData);
            mcStudentRefi.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(mcStudentRefi.strQFormUID));
            mcStudentRefi.VerifyRedirectToMyLtExpress(testData);
        }

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(studentRefi.strQFormUID));
            studentRefi.VerifyRedirectToMyLtExpress(testData);
        }
    }
}