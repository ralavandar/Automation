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
    public class FreeCreditSignup : SeleniumTestBase
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

            //Initialize MC Sign Up Birth Date Data
            testData = mcSignUp.InitializeBirthDateData(testData);

            Common.ReportEvent(Common.INFO, String.Format("Date of Birth Override for Free Credit Sign up for this test (MM/DD/YYYY) is: {0}/{1}/{2}", testData["DateOfBirthMonth"],
                    testData["DateOfBirthDay"], testData["DateOfBirthYear"]));

        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        [Test]
        public void FCU_01_SignUpSuccess()
        {
            mcSignUp.NavigateToMCSignUp(testData["TestEnvironment"], testData["QueryString"]);
            System.Threading.Thread.Sleep(2000);

            //Complete Free Credit Sign up PII Questions
            mcSignUp.CompleteFreeCreditSignupPII(testData);
            mcSignUp.ClickButton();
            string cssS = "h3[ng-show='outOfWallet.questions.length > 1']";
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

            //Out of Wallet Pre Step Check
            bool stepFound = mcSignUp.VerifyMCSignUpStep("step/10/");
            if (stepFound == true)
            {
                mcSignUp.VerifyOOWPrestep(testData["FCSignUpResult"]);
                System.Threading.Thread.Sleep(2000);
                mcSignUp.ClickButton();
                System.Threading.Thread.Sleep(2000);
                stepFound = false;
            }

            //Out of Wallet Questions
            stepFound = mcSignUp.VerifyMCSignUpStep("step/11");
            if (stepFound == true)
            {
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
                //System.Threading.Thread.Sleep(5000);
                stepFound = false;

            }

            //Email Address & Password Step
            try
            {
                mcSignUp.WaitForElement(By.Id("emailLogin"), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: emailLogin"));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }

            stepFound = mcSignUp.VerifyMCSignUpStep("step/13");
            if (stepFound == true)
            {
                mcSignUp.PopulateEmailAddress(testData["EmailAddress"]);
                System.Threading.Thread.Sleep(2000);
                mcSignUp.PopulatePassword(testData["Password"]);
                mcSignUp.ClickButton();
                
            }

            //Validate successful Sign Up
            mcSignUp.VerifyFreeCreditSignUpSuccess(testData);
        }

        [Test]
        public void FCU_02_OOWOopsSuccess()
        {
            mcSignUp.NavigateToMCSignUp(testData["TestEnvironment"], testData["QueryString"]);
            System.Threading.Thread.Sleep(2000);

            //Complete Free Credit Sign up PII Questions
            mcSignUp.CompleteFreeCreditSignupPII(testData);
            mcSignUp.ClickButton();
            string cssS = "h3[ng-show='outOfWallet.questions.length > 1']";
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

            //Out of Wallet Pre Step Check
            bool stepFound = mcSignUp.VerifyMCSignUpStep("step/10/");
            if (stepFound == true)
            {
                mcSignUp.VerifyOOWPrestep(testData["FCSignUpResult"]);
                System.Threading.Thread.Sleep(2000);
                mcSignUp.ClickButton();
                System.Threading.Thread.Sleep(2000);
                stepFound = false;
            }

            //Out of Wallet Questions
            stepFound = mcSignUp.VerifyMCSignUpStep("step/11");
            if (stepFound == true)
            {
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

                //Third OOW Question  ANSWering This one Correctly to trigger oops
                questionFound = mcSignUp.FindOOWQuestion();
                Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
                mcSignUp.Find_SelectDisplayedAnswers("Success");
                mcSignUp.ClickButton();
                stepFound = false;
                
            }

            //Try again Step
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

            stepFound = mcSignUp.VerifyMCSignUpStep("step/12");
            if (stepFound == true)
            {
                mcSignUp.VerifyTryAgainStep();
                mcSignUp.ClickButton();
                System.Threading.Thread.Sleep(5000);
            }


            //Fourth OOO Question  Answer this one Correctly...
            stepFound = mcSignUp.VerifyMCSignUpStep("step/11");
            if (stepFound == true)
            {

                string questionFound = mcSignUp.FindOOWQuestion();
                Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
                mcSignUp.Find_SelectDisplayedAnswers("Success");
                mcSignUp.ClickButton();
                
                stepFound = false;
            }

            //Email Address & Password Step
            try
            {
                mcSignUp.WaitForElement(By.Id("emailLogin"), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: emailLogin"));
                string strFilename = "ElementNotFound";
                mcSignUp.RecordScreenshot(strFilename);
                Assert.Fail();
            }
            stepFound = mcSignUp.VerifyMCSignUpStep("step/13");
            if (stepFound == true)
            {
                mcSignUp.PopulateEmailAddress(testData["EmailAddress"]);
                mcSignUp.PopulatePassword(testData["Password"]);
                System.Threading.Thread.Sleep(2000);
                mcSignUp.ClickButton();
                
            }

            //Validate successful Sign Up
            mcSignUp.VerifyFreeCreditSignUpSuccess(testData);
        }

        [Test]
        public void FCU_04_CreditCards()
        {
            mcSignUp.NavigateToMCSignUp(testData["TestEnvironment"], testData["QueryString"]);
            System.Threading.Thread.Sleep(2000);

            //Complete Free Credit Sign up PII Questions
            mcSignUp.CompleteFreeCreditSignupPII(testData);
            mcSignUp.ClickButton();
            string cssS = "h3[class='ng-scope']";
            mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            IWebElement tagLine = driver.FindElement(By.CssSelector(cssS));
            int n = 1;
            do
            {
                if (tagLine.Text.Contains("social security number"))
                {
                    Common.ReportEvent(Common.INFO, String.Format("App is still on last step of the PII Form.  Loop Count={0}", n));
                    System.Threading.Thread.Sleep(2000);
                    tagLine = driver.FindElement(By.CssSelector(cssS));
                    n = n + 1;
                }
                else
                {
                    Common.ReportEvent(Common.INFO, String.Format("App has moved to another step.  Loop Count={0}", n));
                    n = 61;
                }
            } while (n < 61);
            //Verify OOPS Page is displayed
            bool stepFound = mcSignUp.VerifyMCSignUpStep("step/7/");
            if (stepFound == true)
            {
                Common.ReportEvent(Common.INFO, String.Format("Tag line text:  {0}", tagLine.Text));
                mcSignUp.ClickButton();          
            }


            //Credit Card Bummer Page Check
            cssS = "h3";
            mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            tagLine = driver.FindElement(By.CssSelector(cssS));
            n = 1;
            do
            {
                if (tagLine.Text.Contains("Oops"))
                {
                    Common.ReportEvent(Common.INFO, String.Format("App is still on PII Oops Page.  Loop Count={0}", n));
                    System.Threading.Thread.Sleep(2000);
                    tagLine = driver.FindElement(By.CssSelector(cssS));
                    n = n + 1;
                }
                else
                {
                    Common.ReportEvent(Common.INFO, String.Format("App has moved to another step.  Loop Count={0}", n));
                    n = 61;
                }
            } while (n < 61);

            stepFound = mcSignUp.VerifyMCSignUpStep("step/9/");
            if (stepFound == true)
            {
                mcSignUp.VerifyOOWPrestep(testData["FCSignUpResult"]);
                mcSignUp.ClickButton();
                stepFound = false;
            }

            //Verify CreditCard Page is displayed
            cssS = "div[ng-bind-html='heading | superReg']";
            mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            stepFound = mcSignUp.VerifyMCSignUpStep("credit-cards");
            if (stepFound == true)
            {
                Common.ReportEvent(Common.PASS, String.Format("{0} successfully passed", testData["TestCaseName"]));
            }
        }

        [Test]
        public void FCU_03_OOWOopsLocked()
        {
            mcSignUp.NavigateToMCSignUp(testData["TestEnvironment"], testData["QueryString"]);
            System.Threading.Thread.Sleep(2000);

            //Complete Free Credit Sign up PII Questions
            mcSignUp.CompleteFreeCreditSignupPII(testData);
            mcSignUp.ClickButton();
            string cssS = "h3[ng-show='outOfWallet.questions.length > 1']";
            mcSignUp.WaitForElement(By.CssSelector(cssS), 30);


            //Out of Wallet Pre Step Check
            bool stepFound = mcSignUp.VerifyMCSignUpStep("step/10/");
            if (stepFound == true)
            {
                mcSignUp.VerifyOOWPrestep(testData["FCSignUpResult"]);
                System.Threading.Thread.Sleep(2000);
                mcSignUp.ClickButton();
                System.Threading.Thread.Sleep(2000);
                stepFound = false;
            }
            //Out of Wallet Questions
            stepFound = mcSignUp.VerifyMCSignUpStep("step/11");
            if (stepFound == true)
            {
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


                //Fourth OOO Question  Answer this one incorrectly...
                questionFound = mcSignUp.FindOOWQuestion();
                Common.ReportEvent(Common.INFO, String.Format("Question Found:  {0}", questionFound));
                mcSignUp.Find_SelectDisplayedAnswers(testData["FCSignUpResult"]);
                mcSignUp.ClickButton();
                System.Threading.Thread.Sleep(5000);
                stepFound = false;
            }

            //Account Locked result
            stepFound = mcSignUp.VerifyMCSignUpStep("step/14/");
            if (stepFound == true)
            {
                mcSignUp.VerifyOOWPrestep("locked");
                System.Threading.Thread.Sleep(2000);
                Common.ReportEvent(Common.PASS, String.Format("{0} successfully passed", testData["TestCaseName"]));
            }

        }

        [Test]
        public void FCU_05_Locked()
        {
            mcSignUp.NavigateToMCSignUp(testData["TestEnvironment"], testData["QueryString"]);
            System.Threading.Thread.Sleep(2000);

            //Complete Free Credit Sign up PII Questions
            mcSignUp.CompleteFreeCreditSignupPII(testData);
            mcSignUp.ClickButton();
            //Account Locked result
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

            bool stepFound = mcSignUp.VerifyMCSignUpStep("step/14/");
            if (stepFound == true)
            {
                mcSignUp.VerifyOOWPrestep(testData["FCSignUpResult"]);
                Common.ReportEvent(Common.PASS, String.Format("{0} successfully passed", testData["TestCaseName"]));
            }

        }

        [Test]
        public void FCU_06_PIIOopsSuccess()
        {
            mcSignUp.NavigateToMCSignUp(testData["TestEnvironment"], testData["QueryString"]);
            System.Threading.Thread.Sleep(2000);

            //Complete Free Credit Sign up PII Questions
            mcSignUp.CompleteFreeCreditSignupPII(testData);
            mcSignUp.ClickButton();
            string cssS = "h3[class='ng-scope']";
            mcSignUp.WaitForElement(By.CssSelector(cssS), 30);
            IWebElement tagLine = driver.FindElement(By.CssSelector(cssS));
            int n = 1;
            do
            {
                if (tagLine.Text.Contains("social security number"))
                {
                    Common.ReportEvent(Common.INFO, String.Format("App is still on last step of the PII Form.  Loop Count={0}", n));
                    System.Threading.Thread.Sleep(2000);
                    tagLine = driver.FindElement(By.CssSelector(cssS));
                    n = n + 1;
                }
                else
                {
                    Common.ReportEvent(Common.INFO, String.Format("App has moved to another step.  Loop Count={0}", n));
                    n = 61;
                }
            } while (n < 61);
            //Verify OOPS Page is displayed
            bool stepFound = mcSignUp.VerifyMCSignUpStep("step/7/");
            if (stepFound == true)
            {
                cssS = "h3[class='ng-scope']";
                tagLine = driver.FindElement(By.CssSelector(cssS));
                Common.ReportEvent(Common.INFO, String.Format("Tag line text:  {0}", tagLine.Text));
                mcSignUp.PopulatePIIOopsPage(testData);
                mcSignUp.ClickButton();
                
            }

            string cssSel = "h3[ng-show='outOfWallet.questions.length > 1']";
            mcSignUp.WaitForElement(By.CssSelector(cssSel), 30);

            //Out of Wallet Pre Step Check
            stepFound = mcSignUp.VerifyMCSignUpStep("step/10/");
            if (stepFound == true)
            {
                mcSignUp.VerifyOOWPrestep(testData["FCSignUpResult"]);
                mcSignUp.ClickButton();
                System.Threading.Thread.Sleep(2000);
                stepFound = false;
            }

            //Out of Wallet Questions
            stepFound = mcSignUp.VerifyMCSignUpStep("step/11");
            if (stepFound == true)
            {
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
                
                stepFound = false;

            }

            //Email Address & Password Step
            mcSignUp.WaitForElement(By.Id("emailLogin"), 30);
            stepFound = mcSignUp.VerifyMCSignUpStep("step/13");
            if (stepFound == true)
            {
                mcSignUp.PopulateEmailAddress(testData["EmailAddress"]);
                mcSignUp.PopulatePassword(testData["Password"]);
                System.Threading.Thread.Sleep(3000);
                mcSignUp.ClickButton();
            }

            //Validate successful Sign Up            
            mcSignUp.VerifyFreeCreditSignUpSuccess(testData);
        }
    }
}
