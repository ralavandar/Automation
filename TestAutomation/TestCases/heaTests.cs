using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;

namespace TestAutomation.LendingTree.tla
{
    [TestFixture]
    public class heaTests : SeleniumTestBase
    {
        public string varEmailAddressTest;
        public IWebDriver driver;

        //private const String strTableName = "tTestData_NewHomeEquity";
        private const String strTableName = "tTestData_HomeEquity";
        private heaPage hea;
        
        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            hea = new heaPage(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
           driver.Quit();
           Common.ReportFinalResults();
           /* Commenting this out until I can figure out SP permissions issue
           int varTestCaseIdTest = int.Parse(testData["TestCaseID"]);
           string varTestCaseNameTest = testData["TestCaseName"];
           string varTestEnvironmentTest = testData["TestEnvironment"];
           string varEmailAddressTest = testData["EmailAddress"];
           string varPasswordTest = testData["Password"];
           Common.ReportFinalResultsAuditLog(varTestCaseIdTest, varTestCaseNameTest, varTestEnvironmentTest, varQFormUIDTest, varEmailAddressTest, varPasswordTest);
           */
        }

        [Test]
        public void hea_01_PrimaryFirstAndSecond()
        {
            hea.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void hea_02_SecondaryFirstOnly()
        {
            hea.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void hea_03_InvestmentPropNoCurrentMortgage()
        {
            hea.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void hea_04_LeadingZeros()
        {
            hea.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void hea_05_LtvTest()
        {
            hea.FillOutValidQF();

            FinishTest();
        }

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(hea.strQFormUID));

           hea.VerifyRedirectToMyLendingTree(testData);
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
        private tla.heaPage homeequityLoan;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            homeequityLoan = new tla.heaPage(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
            /* Commenting this out until I can figure out SP permissions issue
            int varTestCaseIdTest = int.Parse(testData["TestCaseID"]);
            string varTestCaseNameTest = testData["TestCaseName"];
            string varTestEnvironmentTest = testData["TestEnvironment"];
            string varEmailAddressTest = testData["EmailAddress"];
            string varPasswordTest = testData["Password"];
            Common.ReportFinalResultsAuditLog(varTestCaseIdTest, varTestCaseNameTest, varTestEnvironmentTest, varQFormUIDTest, varEmailAddressTest, varPasswordTest);
            */
        }

        [Test]
        public void Prod_hea_01()
        {
            homeequityLoan.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void Prod_hea_02()
        {
            homeequityLoan.FillOutValidQF();

            FinishTest();
        }

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(homeequityLoan.strQFormUID));

            homeequityLoan.VerifyRedirectToMyLendingTree(testData);
        }
    }
}
