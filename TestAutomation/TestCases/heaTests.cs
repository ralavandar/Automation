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

            hea.VerifyRedirectToMyLtExpress(testData);
        }
    }
}