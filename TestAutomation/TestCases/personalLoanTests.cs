using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;

namespace TestAutomation.LendingTree.tla
{
    [TestFixture]
    public class plaTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_PersonalLoan";
        private personalLoanPage personalLoan;
       
        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            personalLoan = new personalLoanPage(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
           driver.Quit();
           Common.ReportFinalResults();
        }

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(personalLoan.strQFormUID));
            personalLoan.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void personalLoan_01_Own()
        {
            personalLoan.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void personalLoan_02_Rent()
        {
            personalLoan.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void personalLoan_03_Other()
        {
            personalLoan.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void personalLoan_04_CreditScore()
        {
            personalLoan.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void personalLoan_05_NoMatch()
        {
            personalLoan.FillOutValidQF();
            FinishTest();
        }
    }
}


namespace TestAutomation.LendingTree.ProdTests_Forms_Other
{
    [TestFixture]
    public class personalLoanTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_PersonalLoan";
        private tla.personalLoanPage personalLoan;
        private mc.mcPersonalLoanPage mcPersonal;
        private tlm.p2Page p2;

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

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(personalLoan.strQFormUID));
            personalLoan.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_p2_desktop_01()
        {
            p2 = new tlm.p2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for p2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] = 
                    System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            p2.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(p2.strQFormUID));
            p2.VerifyRedirectToMyLendingTree(testData);
            //p2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_p2_desktop_02()
        {
            p2 = new tlm.p2Page(driver, testData);
            // Initialize test data -> If DOBMonth is numeric, need to convert it to the month name (string) for p2 form
            int number;
            bool isNumeric = int.TryParse(testData["DateOfBirthMonth"], out number);
            if (isNumeric && number < 13)
            {
                testData["DateOfBirthMonth"] =
                    System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(testData["DateOfBirthMonth"]));
            }

            p2.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(p2.strQFormUID));
            p2.VerifyRedirectToMyLendingTree(testData);
            //p2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_pla_01()
        {
            personalLoan = new tla.personalLoanPage(driver, testData);
            personalLoan.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void Prod_pla_02()
        {
            personalLoan = new tla.personalLoanPage(driver, testData);
            personalLoan.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void Prod_mcPersonal_01()
        {
            mcPersonal = new mc.mcPersonalLoanPage(driver, testData);
            mcPersonal.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(mcPersonal.strQFormUID));
            mcPersonal.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void Prod_mcPersonal_02()
        {
            mcPersonal = new mc.mcPersonalLoanPage(driver, testData);
            mcPersonal.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(mcPersonal.strQFormUID));
            mcPersonal.VerifyRedirectToMyLtExpress(testData);
        }
    }
}
