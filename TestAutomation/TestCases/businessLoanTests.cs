using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;

namespace TestAutomation.LendingTree.tlm
{
    [TestFixture]
    public class b2Tests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_BusinessLoan";
        private b2Page b2;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            b2 = new b2Page(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            string result = Common.CalculateFinalTestResult();

            try
            {
                // This is experimental to see if I can report a result back to our Sauce Labs dashboard
                bool passed = (result == Common.PASS);
                if (testData["BrowserType"] == "SAUCELABS")
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
                }
            }
            finally
            {
                driver.Quit();
                // If the test did not PASS, then force an Assert exception, so the test case status in NUnit GUI will turn red
                if (result != Common.PASS)
                    NUnit.Framework.Assert.Fail();       
            }
        }

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(b2.strQFormUID));
            b2.VerifyRedirectToMyLtExpressUnauthorized(testData);
        }

        [Test]
        public void b2_01_ExcellentCorp()
        {
            b2.FillOutValidQF();
            FinishTest();
            // TO DO - can't get this to work
            // Check for at least one visible offer via the Apply Now button.
            //if (Validation.IsTrue(b2.IsElementDisplayed(By.CssSelector("a[ng-click*=clickApply]"), 150)))
            //{
            //    Common.ReportEvent(Common.PASS, "At least one 'Apply Now' button/link was displayed.");
            //}
            //else
            //{
            //    Common.ReportEvent(Common.FAIL, "No 'Apply Now' buttons/links displayed within 30 seconds.  Expecting at least one offer.  Check the offer URL again.");
            //}
        }

        [Test]
        public void b2_02_PoorStartupLlc()
        {
            b2.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void b2_03_InvalidPhoneTest()
        {
            IFormField[][] steps = b2.ValidQFSteps;

            b2.StartForm();
            b2.PerformSteps(steps, 1, 9);

            b2.PrepareStep(10, true);
            b2.FillOutStep(steps[10]);
            b2.ConcludeStep();

            // Verify error message on Phone field
            System.Threading.Thread.Sleep(2000);
            Validation.IsTrue(b2.DoesPageContainText("Please enter a valid phone number."));

            // Populate the Dictionary with valid phone and re-fill step 10
            testData["BorrowerHomePhone1"] = "407";
            testData["BorrowerHomePhone2"] = "939";
            testData["BorrowerHomePhone3"] = "3463";
            b2.FillOutStep(steps[10]);
            b2.ConcludeStep();

            FinishTest();
        }

        [Test]
        public void b2_04_PersonalCrossSell()
        {
            IFormField[][] steps = b2.ValidQFStepsWithCrossSell;

            b2.StartForm();
            b2.PerformSteps(steps, 1, 4);

            // Verify landed on cross-sell step (5) 
            b2.PrepareStep(5, true);
            Validation.IsTrue(b2.DoesPageContainText("Hmmm… based on the info you've given, you may have better success at getting funded for your business in other ways."));
            b2.FillOutStep(steps[5]);
            b2.ConcludeStep();

            // Continue with steps 6 to end
            b2.PerformSteps(steps, 6, Convert.ToUInt32(steps.Length - 1));
            FinishTest();
        }
    }
}


namespace TestAutomation.LendingTree.tla
{
    [TestFixture]
    public class blTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_BusinessLoan";
        private blPage bl;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            bl = new blPage(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(bl.strQFormUID));
            bl.VerifyRedirectToMyLtExpressUnauthorized(testData);
        }

        [Test]
        public void bl_01_ExcellentCorp()
        {
            bl.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void bl_02_PoorStartupLlc()
        {
            bl.FillOutValidQF();
            FinishTest();
        }

        [Test]
        public void bl_03_InvalidPhoneTest()
        {
            IFormField[][] steps = bl.ValidQFSteps;

            bl.StartForm();
            bl.PerformSteps(steps, 1, 9);

            bl.PrepareStep(10, true);
            bl.FillOutStep(steps[10]);
            bl.ConcludeStep();

            // Verify error message on Phone field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(bl.DoesPageContainText("Please enter a valid phone number."));

            // Populate the Dictionary with valid phone and re-fill step 10
            testData["BorrowerHomePhone1"] = "407";
            testData["BorrowerHomePhone2"] = "939";
            testData["BorrowerHomePhone3"] = "3463";
            bl.FillOutStep(steps[10]);
            bl.ConcludeStep();

            FinishTest();
        }

        [Test]
        public void bl_04_PersonalCrossSell()
        {
            IFormField[][] steps = bl.ValidQFStepsWithCrossSell;

            bl.StartForm();
            bl.PerformSteps(steps, 1, 4);

            // Verify landed on cross-sell step (5) 
            bl.PrepareStep(5, true);
            Assert.IsTrue(bl.DoesPageContainText("Hmmm… based on the info you've given, you may have better success at getting funded for your business in other ways."));
            bl.FillOutStep(steps[5]);
            bl.ConcludeStep();

            // Continue with steps 6 to end
            bl.PerformSteps(steps, 6, Convert.ToUInt32(steps.Length - 1));
            FinishTest();
        }
    }
}


namespace TestAutomation.LendingTree.ProdTests_Forms_Other
{
    [TestFixture]
    public class businessLoanTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_BusinessLoan";
        private tla.blPage bl;
        private tlm.b2Page b2;

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
        public void Prod_b2_01()
        {
            b2 = new tlm.b2Page(driver, testData);
            b2.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(b2.strQFormUID));
            b2.VerifyRedirectToMyLtExpressUnauthorized(testData);
        }

        [Test]
        public void Prod_b2_02()
        {
            b2 = new tlm.b2Page(driver, testData);
            b2.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(b2.strQFormUID));
            b2.VerifyRedirectToMyLtExpressUnauthorized(testData);
        }

        [Test]
        public void Prod_bl_01()
        {
            bl = new tla.blPage(driver, testData);
            bl.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(bl.strQFormUID));
            bl.VerifyRedirectToMyLtExpressUnauthorized(testData);
        }

        [Test]
        public void Prod_bl_02()
        {
            bl = new tla.blPage(driver, testData);
            bl.FillOutValidQF();
            Validation.IsTrue(VerifytQFormRecord(bl.strQFormUID));
            bl.VerifyRedirectToMyLtExpressUnauthorized(testData);
        }
    }
}
