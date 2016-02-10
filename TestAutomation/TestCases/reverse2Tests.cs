using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.tla
{
    [TestFixture]
    public class reverse2Tests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private reverse2Page reverse2;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            reverse2 = new reverse2Page(driver, testData);
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
            Validation.IsTrue(VerifytQFormRecord(reverse2.strQFormUID));

            // Verify redirect to My LendingTree
            reverse2.VerifyRedirectToMyLtExpress(testData);
        }

        [Test]
        public void reverse2_01_ValidAndEligible()
        {
            // Fill out and submit a QF
            reverse2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void reverse2_02_InvalidTooYoung()
        {
            reverse2.FillOutValidQF();

            VerifyInvalid();
            VerifyReasons("all individuals listed on the title must be at least 62 years old");
        }

        [Test]
        public void reverse2_03_InvalidNotPrimary()
        {
            reverse2.FillOutValidQF();

            VerifyInvalid();
            VerifyReasons("a home must be a borrower's primary residence");
        }

        [Test]
        public void reverse2_04_InvalidBalanceTooHigh()
        {
            reverse2.FillOutValidQF();

            VerifyInvalid();
            VerifyReasons("a borrower must have no or low mortgage balance");
        }

        [Test]
        public void reverse2_05_LoanTooHigh()
        {
            reverse2.FillOutValidQF();

            VerifyInvalid();
            VerifyReasons("the maximum current mortgage balance cannot exceed $375,500");
        }

        [Test]
        public void reverse2_06_InvalidEverythingWrong()
        {
            reverse2.FillOutValidQF();

            VerifyInvalid();
            VerifyReasons("all individuals listed on the title must be at least 62 years old",
                    "a home must be a borrower's primary residence",
                    "a borrower must have no or low mortgage balance",
                    "the maximum current mortgage balance cannot exceed $375,500");
        }

        [Test]
        public void reverse2_07_InvalidNotPrimaryBalanceTooHighLoanTooHigh()
        {
            reverse2.FillOutValidQF();

            VerifyInvalid();
            VerifyReasons("a home must be a borrower's primary residence",
                    "a borrower must have no or low mortgage balance",
                    "the maximum current mortgage balance cannot exceed $375,500");
        }

        [Test]
        public void reverse2_08_TestMarketplace()
        {
            // Fill out and submit a QF
            reverse2.FillOutValidQF();

            FinishTest();
        }

        private void VerifyInvalid()
        {
            reverse2.CheckForProcessing();
            if (reverse2.IsElementDisplayed(By.Id("xsell"), 30))
            {
                var xselltext = driver.FindElement(By.Id("xsell")).FindElement(By.TagName("p")).Text;
                Validation.StringContains("Unfortunately, you don't meet the eligibility requirements for a reverse mortgage. To qualify for a reverse mortgage,", xselltext);
            }
        }

        private void VerifyReasons(params string[] reasons)
        {
            var reasonsText = driver.FindElement(By.Id("reasons")).Text;
            foreach (string reason in reasons)
            {
                Validation.StringContains(reason, reasonsText);
            }

            if (reasons.Length > 1) //Check for the right grammatical format of the reasons.
            {
                var clauseRegex = @"[\w ,\$']+";
                var regex = @"\A" + clauseRegex + @"\. Also, " + clauseRegex;

                if (reasons.Length >= 4)
                {
                    //We only expect 4 reasons maximum, but this can handle 5 or more.
                    for (var i = 2; i < reasons.Length - 1; i++)
                        regex += @", " + clauseRegex;
                }

                if (reasons.Length > 2)
                    regex += " and " + clauseRegex;

                regex += @"\.";

                Common.ReportEvent("DEBUG", "Checking reasons with regex " + regex);

                Validation.StringCompareRegEx(regex, reasonsText);
            }
        }
    }
}

namespace TestAutomation.LendingTree.ProdTests_Forms_Other
{

    [TestFixture]
    public class reverseMortgageTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private tla.reverse2Page reverse2;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            reverse2 = new tla.reverse2Page(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        [Test]
        public void Prod_reverse2_01()
        {
            reverse2.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void Prod_reverse2_02()
        {
            reverse2.FillOutValidQF();

            FinishTest();
        }

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(reverse2.strQFormUID));

            reverse2.VerifyRedirectToMyLtExpress(testData);
        }
    }
}




