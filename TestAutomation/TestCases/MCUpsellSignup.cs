using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;


namespace TestAutomation.LendingTree.FreeCreditUsers
{
    [TestFixture]
    class MCFreeCreditUpsell : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_FreeCreditUserPII";
        private mcSignUp mcSignUp;
        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            mcSignUp = new mcSignUp(driver);

            //Find MC Credentials to exercise the upsell
            testData = mcSignUp.SetMCUpsellCredentials(testData);
            //Initialize MC Sign Up Birth Date Data
            testData = mcSignUp.InitializeBirthDateData(testData);
            
            Common.ReportEvent(Common.INFO, String.Format("Date of Birth Override for Free Credit Sign up for this test (MM/DD/YYYY) is: {0}/{1}/{2}", testData["DateOfBirthMonth"],
                    testData["DateOfBirthDay"], testData["DateOfBirthYear"]));
            Common.ReportEvent(Common.INFO, String.Format("Email Address OVERRIDE for this test is {0}", testData["EmailAddress"]));
            Common.ReportEvent(Common.INFO, String.Format("Password OVERRIDE for this test is {0}", testData["Password"]));
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        [Test]
        public void FCU_01_MCUpsellUpSuccess()
        {
            mcSignUp.MCSignIn(testData);            
            mcSignUp.ClickUpsellStartButton();
            mcSignUp.PopulateUpsellPII(testData);
            mcSignUp.ClickButton();
            string cssS = "div[class='outOfWallet ng-scope']";
            try
            {
                mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }
                        
            //Out of Wallet Questions:
            string questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers(testData["FCSignUpResult"]);
            mcSignUp.ClickButton();
            System.Threading.Thread.Sleep(2000);
            //Second OOW Question
            questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers(testData["FCSignUpResult"]);
            mcSignUp.ClickButton();
            //Third OOW Question
            questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers(testData["FCSignUpResult"]);
            mcSignUp.ClickButton();
            
            mcSignUp.VerifyFreeCreditUpSellSuccess(testData);

        }

        [Test]
        public void FCU_02_MCUpsellUpOOWOopsSuccess()
        {
            mcSignUp.MCSignIn(testData);
            mcSignUp.ClickUpsellStartButton();
            mcSignUp.PopulateUpsellPII(testData);
            mcSignUp.ClickButton();
            string cssS = "div[class='outOfWallet ng-scope']";
            try
            {
                mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }
            
            //Out of Wallet Questions
            string questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers(testData["FCSignUpResult"]);
            mcSignUp.ClickButton();
            System.Threading.Thread.Sleep(2000);

            //Second OOW Question
            questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers(testData["FCSignUpResult"]);
            mcSignUp.ClickButton();

            //Third OOW Question  Answering This one Correctly to trigger oops
            questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers("Success");
            mcSignUp.ClickButton();
            
            //Try again step            
            cssS = "h3[id=tryAgainOowHeader]";
            try
            {
                mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }
            mcSignUp.VerifyTryAgainStep();
            mcSignUp.ClickButton();
            System.Threading.Thread.Sleep(2000);

            //Fourth OOW Question--Answering Correctly to successfully complete sign up
            questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers("Success");
            mcSignUp.ClickButton();
            
            mcSignUp.VerifyFreeCreditUpSellSuccess(testData);

        }

        [Test]
        public void FCU_03_MCUpsellUpOOWOopsLocked()
        {
            mcSignUp.MCSignIn(testData);
            mcSignUp.ClickUpsellStartButton();
            mcSignUp.PopulateUpsellPII(testData);
            mcSignUp.ClickButton();

            string cssS = "div[class='outOfWallet ng-scope']";
            try
            {
                mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }

            string questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers(testData["FCSignUpResult"]);
            mcSignUp.ClickButton();
            System.Threading.Thread.Sleep(2000);

            //Second OOW Question
            questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers(testData["FCSignUpResult"]);
            mcSignUp.ClickButton();

            //Third OOW Question  Answering This one Correctly to trigger oops
            questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers("Success");
            mcSignUp.ClickButton();

            //Try again step            
            cssS = "h3[id=tryAgainOowHeader]";
            try
            {
                mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }
            mcSignUp.VerifyTryAgainStep();
            mcSignUp.ClickButton();
            System.Threading.Thread.Sleep(2000);

            //Fourth OOO Question  Answer this one incorrectly...
            questionFound = mcSignUp.FindOOWQuestion();
            Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
            mcSignUp.Find_SelectDisplayedAnswers(testData["FCSignUpResult"]);
            mcSignUp.ClickButton();

            cssS = "div[class='warning-container']";
            try
            {
                mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }
            mcSignUp.VerifyOOWPrestep("locked");
            System.Threading.Thread.Sleep(2000);
            Common.ReportEvent(Common.PASS, String.Format("{0} successfully passed", testData["TestCaseName"]));
            
        }
        [Test]
        public void FCU_04_MCUpsellCreditCards()
        {

            mcSignUp.MCSignIn(testData);
            mcSignUp.ClickUpsellStartButton();
            mcSignUp.PopulateUpsellPII(testData);
            mcSignUp.ClickButton();
            string cssS = "div[class='warning-container']";
            try
            {
                mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }
            //Verify Bummer Page is displayed
            cssS = "div[class='bummer ng-scope']";
            IWebElement bummer = driver.FindElement(By.CssSelector(cssS));
            if (bummer.Text.Contains("access your credit file"))
            {
                Common.ReportEvent(Common.PASS, String.Format("Consumer Sign Up Successfully displayed the Bummer Step stating unable to access your credit file"));
                mcSignUp.ClickButton();
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("Consumer Sign Up FAILED to display the the Bummer Step stating unable to access your credit file"));
                string strFilename = "FCU_04_MCUpsellCreditCards_";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail("Out of Wallet PreStep for Credit Cards was not displayed.");
                
            }
            
            //Verify CreditCard Page is displayed
            cssS = "div[id='vertical-overview-header']";
            try
            {
                mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }
            IWebElement h1= driver.FindElement(By.CssSelector(cssS));
            if (h1.Text.Contains("Credit Card"))
            {
                Common.ReportEvent(Common.PASS, String.Format("{0} successfully passed", testData["TestCaseName"]));
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("{0} failed to navigate", testData["TestCaseName"]));
                string strFilename = testData["TestCaseName"]+"Page Not Found";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }

        }
        [Test]
        public void FCU_05_MCUpsellLocked()
        {
            mcSignUp.MCSignIn(testData);
            mcSignUp.ClickUpsellStartButton();
            mcSignUp.PopulateUpsellPII(testData);
            mcSignUp.ClickButton();
            string cssS = "div[class='warning-container']";
            try
            {
                mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }
            mcSignUp.VerifyOOWPrestep("locked");
            System.Threading.Thread.Sleep(2000);
            Common.ReportEvent(Common.PASS, String.Format("{0} successfully passed", testData["TestCaseName"]));

        }
       
    }
}
