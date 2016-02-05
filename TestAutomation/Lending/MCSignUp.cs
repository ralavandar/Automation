using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System.Data;
using System.Data.SqlClient;

namespace TestAutomation.LendingTree
{
    public class mcSignUp : PageBase
    {
        private readonly IWebDriver mcSignUpDriver;

        // Constructor
        public mcSignUp(IWebDriver driver)
            : base(driver)
        {
            mcSignUpDriver = driver;
        }

        public void NavigateToMCSignUp(string strEnvironment, string queryString)
        {
            //Navigates to MyLT Sign up Outside Flow Form
            String strUrl;

            switch (strEnvironment.ToUpper())
            {
                case "PROD":
                    strUrl = "https://my.lendingtree.com/Signup";
                    break;
                case "QA":
                case "STAGING":
                case "STAGE":
                    strUrl = "http://mc.staging.lendingtree.com/Signup";
                    break;
                case "DEV":
                    strUrl = "http://mc.dev.lendingtree.com/Signup";
                    break;
                default:    //QA
                    strUrl = "http://mc.staging.lendingtree.com/Signup";
                    break;
            }
            if (queryString.Length > 0)
            {
                strUrl = strUrl + "?" + queryString;
            }
            Common.ReportEvent(Common.INFO, String.Format("Navigating to MyLendingTree URL: {0}", strUrl));
            mcSignUpDriver.Navigate().GoToUrl(strUrl);
            //Make the window maximized.
            mcSignUpDriver.Manage().Window.Maximize();

        }

        public void MCSignIn(Dictionary<string, string> testData)
        {
            //Successfully Navigates to MyLT and signs in with provided user credentials.
            string strUrl;

            switch (testData["TestEnvironment"].ToUpper())
            {
                case "PROD":
                    strUrl = "https://my.lendingtree.com/";
                    break;
                case "QA":
                case "STAGING":
                case "STAGE":
                    strUrl = "http://mc.staging.lendingtree.com/";
                    break;                
                case "DEV":
                    strUrl = "http://mc.dev.lendingtree.com/";
                    break;
                default:    //QA
                    strUrl = "http://mc.staging.lendingtree.com/";
                    break;
            }

            Common.ReportEvent(Common.INFO, String.Format("Navigating to MyLendingTree URL: {0}", strUrl));
            mcSignUpDriver.Navigate().GoToUrl(strUrl);
            //Make the window maximized.
            mcSignUpDriver.Manage().Window.Maximize();
            //Select Sign in button
            IWebElement signinBtn = mcSignUpDriver.FindElement(By.ClassName("blue-btn"));
            signinBtn.Click();
            IWebElement email = mcSignUpDriver.FindElement(By.Id("UserName"));
            email.SendKeys(testData["EmailAddress"]);
            IWebElement pass = mcSignUpDriver.FindElement(By.Id("Password"));
            pass.SendKeys(testData["Password"]);
            signinBtn = mcSignUpDriver.FindElement(By.ClassName("sign-in-button"));
            signinBtn.Click();
            System.Threading.Thread.Sleep(5000);
            if (testData["QueryString"].Length > 0)
            {
                strUrl = mcSignUpDriver.Url;
                strUrl = strUrl + "?" + testData["QueryString"];
                mcSignUpDriver.Navigate().GoToUrl(strUrl);
            }
            System.Threading.Thread.Sleep(2000);
            string cssS = "div[class='loader spinner-wrapper']";
            IWebElement loader = mcSignUpDriver.FindElement(By.CssSelector(cssS));

            int counter = 1;
            while (counter < 60)
            {
                string style = loader.GetAttribute("style");
                Common.ReportEvent(Common.INFO, String.Format("Loader Style: {0}", style));
                if (style.Contains("visible"))
                {
                    System.Threading.Thread.Sleep(1000);
                    Common.ReportEvent(Common.INFO, String.Format("Loader Still Visible", style));
                    counter = counter + 1;
                }
                else
                {
                    Common.ReportEvent(Common.INFO, String.Format("Loader Gone", style));
                    counter = 100;
                }
            }
        }

        public void     MCOfferPageNavigation(Dictionary<string, string> testData)
        {
            string strURL = "";
            if (testData["OfferExperience"].ToUpper() == "FOSSA")
            {
                switch (testData["TestEnvironment"].ToUpper())
                {
                    case "PROD":
                        strURL = "https://offers.lendingtree.com/mc.aspx";
                        break;
                    case "QA":
                    case "STAGING":
                    case "STAGE":
                        strURL = "https://offers.staging.lendingtree.com/mc.aspx";
                        break;
                    case "DEV":
                        strURL = "https://offers.dev.lendingtree.com/mc.aspx";
                        break;
                    default:    //QA
                        strURL = "https://offers.staging.lendingtree.com/mc.aspx";
                        break;
                }
                switch (Convert.ToInt32(testData["ProductType"]))
                {
                    case 1:  //Mortgage Products
                        if (testData["ProductSubtype"] == "1")
                        {
                            strURL = strURL + "?tid=mc-mortgage-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        }
                        else
                        {
                            strURL = strURL + "?tid=mc-mortgage-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        }
                        break;
                    case 2:  //Home equity products
                        strURL = strURL + "?tid=mc-hel-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        break;
                    case 3: //Personal Loan products
                        strURL = strURL + "?tid=mc-personal-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        break;
                    case 4:  //Auto Loan Products
                        if (testData["ProductSubtype"] == "1")  //CARS
                        {
                            strURL = strURL + "?tid=mc-auto-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        }
                        else if (testData["ProductSubtype"] == "2") // Boats
                        {
                            strURL = strURL + "?tid=mc-boat-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        }
                        else if (testData["ProductSubtype"] == "3") // Motorcycles
                        {
                            strURL = strURL + "?tid=mc-moto-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        }
                        else if (testData["ProductSubtype"] == "4") //Powersports
                        {
                            strURL = strURL + "?tid=mc-power-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        }
                        else  //RVs
                        {
                            strURL = strURL + "?tid=mc-rv-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        }
                        break;
                    case 6: // student loans
                        strURL = strURL + "?tid=mc-student-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        break;
                    case 11: //Reverse Mortgage
                        strURL = strURL + "?tid=mc-reverse-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        break;
                    case 12:  // Prequal
                        strURL = strURL + "?tid=mc-prequalification-offers&qformuid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        break;
                    default:
                        Common.ReportEvent(Common.ERROR, String.Format("FAILED to provide a known QF Product Type: {0}", testData["ProductType"]));
                        Assert.Fail("No recognized QF Product Type was provided");
                        break;
                }
            }
            else if (testData["OfferExperience"].ToUpper() == "EXPRESS")  //Framework for express offers experience
            {
                switch (testData["TestEnvironment"].ToUpper())
                {
                    case "PROD":
                        strURL = "https://my.lendingtree.com/";
                        break;
                    case "QA":
                    case "STAGING":
                    case "STAGE":
                        strURL = "http://mc.staging.lendingtree.com/";
                        break;
                    case "DEV":
                        strURL = "http://mc.dev.lendingtree.com/";
                        break;
                    default:    //QA
                        strURL = "http://mc.staging.lendingtree.com/";
                        break;
                }
                switch (Convert.ToInt32(testData["ProductType"]))
                {
                    case 1:  //Mortgage Products
                        if (testData["ProductSubtype"] == "1")
                        {
                            strURL = strURL + "/express-offers/purchase?SpID=purchase-offer-ux-split&sticky=true&guid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        }
                        else
                        {
                            strURL = strURL + "/express-offers/refinance?SpID=purchase-offer-ux-split&sticky=true&guid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        }
                        break;
                    case 3:  // Personal Loans
                        strURL = strURL + "/express-offers?voffers=B&tid=pla&guid=" + testData["QFormUID"] + "&" + testData["QueryString"];
                        break;
                    default:
                        Common.ReportEvent(Common.ERROR, String.Format("FAILED to provide a known QF Product Type: {0}", testData["ProductType"]));
                        Assert.Fail("No recognized QF Product Type was provided");
                        break;
                }
            }
            Common.ReportEvent(Common.INFO, String.Format("Navigating to MyLendingTree Offer URL: {0}", strURL));
            mcSignUpDriver.Navigate().GoToUrl(strURL);
            string cssS = "div[class='sign-up upsell upsell-inline upsellIntro']";
            try
            {
                WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                RecordScreenshot(strFilename);
                Assert.Fail();
            }
        }

        
    


        public bool VerifyMCSignUpStep(string step)
        {

            //Verify the correct step has been presented for the MyLT sign up outside flow form.            
            string strURL = mcSignUpDriver.Url;
            bool StepFound;
            StepFound = strURL.Contains(step);

            if (StepFound == true)
            {
                Common.ReportEvent(Common.PASS, String.Format("Successfully displayed Step: {0}", step));
                return StepFound;
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Step: {0}", step));
                string strFilename = "VerifyMCSignUpStep_" + step.Replace("/", "-");
                RecordScreenshot(strFilename);
                return StepFound;
            }


        }

        public bool CheckMCSignUpStep(string step)
        {

            //Verify the correct step has been presented for the MyLT sign up outside flow form.            
            string strURL = mcSignUpDriver.Url;
            bool StepFound;
            StepFound = strURL.Contains(step);

            if (StepFound == true)
            {
                Common.ReportEvent(Common.PASS, String.Format("Successfully displayed Step: {0}", step));
                return StepFound;
            }
            else
            {
                Common.ReportEvent(Common.INFO, String.Format("WAITING to display Step: {0}", step));
                return StepFound;
            }


        }

        public void PopulateFirstName(string firstName)
        {
            //used to populate the first name field on the MyLT outside sign up flow
            IWebElement fName = mcSignUpDriver.FindElement(By.Id("first-name"));
            fName.SendKeys(firstName);

        }

        public void PopulateLastName(string lastName)
        {
            //used to populate the last name field on the MyLT outside sign up flow
            IWebElement lname = mcSignUpDriver.FindElement(By.Id("last-name"));
            lname.SendKeys(lastName);
        }

        public void PopulateZipCode(string zipCode)
        {
            //used to populate the zip code field on the MyLT outside sign up flow
            IWebElement zcode = mcSignUpDriver.FindElement(By.Id("zip-code"));
            zcode.SendKeys(zipCode);
        }
        public void PopulateStreetAddress(string streetAddress)
        {
            //used to populate the Street Address field on MyLT outside sign up flow
            IWebElement strAddress = mcSignUpDriver.FindElement(By.Id("street_address"));
            strAddress.SendKeys(streetAddress);

        }

        public void PopulateDateofBirth(string dobMonth, string dobDay, string dobYear)
        {
            //used to populate date of birth on the MyLT outside sign up flow
            SelectElement Month = new SelectElement(mcSignUpDriver.FindElement(By.Id("dobMonthSignUp")));
            SelectElement Day = new SelectElement(mcSignUpDriver.FindElement(By.Id("dobDaySignUp")));
            SelectElement Year = new SelectElement(mcSignUpDriver.FindElement(By.Id("dobYearSignUp")));

            Month.SelectByText(dobMonth);
            Day.SelectByText(dobDay);
            Year.SelectByText(dobYear);

        }

        public void PopulateConsumerSSN(string SSN)
        {
            //used to populate Consumer SSN on the MyLT outside sign up flow
            IWebElement cSSN = mcSignUpDriver.FindElement(By.Id("ssn3"));
            cSSN.SendKeys(SSN);

        }

        public void PopulateEmailAddress(string emailAddress)
        {
            //Populating email address for consumer on the MyLT outside sign up flow
            IWebElement email = mcSignUpDriver.FindElement(By.Id("emailLogin"));
            email.SendKeys(emailAddress);
        }

        public void PopulatePassword(string password)
        {
            //Populating password for consumer on the MyLT outside sign up flow
            IWebElement pass = mcSignUpDriver.FindElement(By.Id("password"));
            pass.SendKeys(password);
        }

        public void ClickButton()
        {

            //Clicks the button on a sign up step.  Used for both inside and outside flow forms
            WaitForElement(By.ClassName("main_button"), 15);
            IWebElement button = mcSignUpDriver.FindElement(By.ClassName("main_button"));
            button.Click();
        }

        public void ClickUpsellStartButton()
        {
            //Clicks the Start button on the MyLT upsell.  
            string cssS = "span[class=introButton]";
            try
            {
                WaitForElement(By.CssSelector(cssS), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: {0}", cssS));
                string strFilename = "ElementNotFound";
                RecordScreenshot(strFilename);
                Assert.Fail();
            }
            IWebElement button = mcSignUpDriver.FindElement(By.CssSelector(cssS));
            button.Click();
        }
        public void VerifyTryAgainStep()
        {
            //Verifies the Out of Wallet Try again step is presented if the user gets 2 out of 3 Out of Wallet questions wrong
                //Used for both outside and inside flow try again steps.
            string cssS = "h3[id=tryAgainOowHeader]";
            IWebElement tagLine = mcSignUpDriver.FindElement(By.CssSelector(cssS));
            Common.ReportEvent(Common.INFO, String.Format("Tag line text:  {0}", tagLine.Text));
            bool blSuccess = tagLine.Text.Contains("try again");
            if (blSuccess == true)
            {
                Common.ReportEvent(Common.PASS, String.Format("Consumer Sign Up Successfully displayed the Out of Wallet Try again step stating try again"));

            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("Consumer Sign Up FAILED to display the Out of Wallet Try again step stating try again"));
                string strFilename = "VerifyTryAgainStep";
                RecordScreenshot(strFilename);
                Assert.Fail("Out of Wallet PreStep for Sign up Success was not displayed.");

            }
        }

        public void VerifyOOWPrestep(string OOWResult)
        {
            //Verifies the Out of Wallet Question Pre step presentation for the MyLT sign up outside flow form.
            switch (OOWResult.ToUpper())
            {
                case "SUCCESS":
                case "OOWOOPSSUCCESS":
                case "OOWOOPSLOCKED":
                    string cssS = "h3[class='ng-scope ng-binding']";
                    IWebElement tagLine = mcSignUpDriver.FindElement(By.CssSelector(cssS));
                    Common.ReportEvent(Common.INFO, String.Format("Tag line text:  {0}", tagLine.Text));
                    //06.05.15 -- SHOULD I MAKE THE H3 Tag line check Dynamic??
                    bool blSuccess = tagLine.Text.Contains("verify your identity");
                    if (blSuccess == true)
                    {
                        Common.ReportEvent(Common.PASS, String.Format("Consumer Sign Up Successfully displayed the Out of Wallet Pre Step stating verify your identity"));
                        break;
                    }
                    else
                    {
                        Common.ReportEvent(Common.FAIL, String.Format("Consumer Sign Up FAILED to display the Out of Wallet Pre Step stating verify your identity"));
                        string strFilename = "VerifyOOWPrestep_" + OOWResult;
                        RecordScreenshot(strFilename);
                        Assert.Fail("Out of Wallet PreStep for Sign up Success was not displayed.");
                        break;
                    }

                case "LOCKED":
                    cssS = "div[class='warning-template ng-scope']";
                    tagLine = mcSignUpDriver.FindElement(By.CssSelector(cssS));
                    Common.ReportEvent(Common.INFO, String.Format("Tag line text:  {0}", tagLine.Text));
                    blSuccess = tagLine.Text.Contains("Identity Validation Failed");
                    if (blSuccess == true)
                    {
                        Common.ReportEvent(Common.PASS, String.Format("Consumer Sign Up Successfully displayed the Account Locked stating Identity Validation Failed"));
                        break;
                    }
                    else if (tagLine.Text.Contains("Account Locked for 30 days"))
                    {
                        Common.ReportEvent(Common.PASS, String.Format("Consumer Sign Up Successfully displayed the Account Locked stating Identity Validation Failed"));
                        break;
                    }
                    else
                    {
                        Common.ReportEvent(Common.FAIL, String.Format("Consumer Sign Up FAILED to display the Account Locked stating Identity Validation Failed"));
                        string strFilename = "VerifyOOWPrestep_" + OOWResult;
                        RecordScreenshot(strFilename);
                        Assert.Fail("Out of Wallet PreStep for Sign up Success was not displayed.");
                        break;
                    }
                case "CREDITCARD":
                    cssS = "h3";
                    tagLine = mcSignUpDriver.FindElement(By.CssSelector(cssS));
                    Common.ReportEvent(Common.INFO, String.Format("Tag line text:  {0}", tagLine.Text));
                    blSuccess = tagLine.Text.Contains("access your credit file");
                    if (blSuccess == true)
                    {
                        Common.ReportEvent(Common.PASS, String.Format("Consumer Sign Up Successfully displayed the Bummer Step stating unable to access your credit file"));
                        break;
                    }
                    else
                    {
                        Common.ReportEvent(Common.FAIL, String.Format("Consumer Sign Up FAILED to display the the Bummer Step stating unable to access your credit file"));
                        string strFilename = "VerifyOOWPrestep_" + OOWResult;
                        RecordScreenshot(strFilename);
                        Assert.Fail("Out of Wallet PreStep for Sign up Success was not displayed.");
                        break;
                    }

                default: // Default check for OOWresult
                    Common.ReportEvent(Common.ERROR, String.Format("FAILED to provide a known Out of Wallet Result: {0}", OOWResult));
                    Assert.Fail("No recognized Out of Wallet Result value was provided");
                    break;
            }

        }

        public string FindOOWQuestion()
        {
            //Finds and reports the out of wallet question text presented to the consumer.
                //Used on both MyLT outside flow and upsell flow forms.
            string cssS = "h4[class='ng-binding']";
            IWebElement question = mcSignUpDriver.FindElement(By.CssSelector(cssS));
            Common.ReportEvent(Common.INFO, String.Format("Question text:  {0}", question.Text));
            return question.Text;
        }


        public void Find_SelectDisplayedAnswers(string OOWResult)
        {
            //Finds and reports the out of wallet answer values presented to the consumer and based on desired outcome selects an answer
            //Used on both MyLT outside flow and upsell flow forms.
            List<string> answers = InitializeAnswers();
            string cssS = "label[class='radio ng-scope ng-binding']";
            ReadOnlyCollection<IWebElement> optionRadios = mcSignUpDriver.FindElements(By.CssSelector(cssS));
            int radioCount = optionRadios.Count();
            Common.ReportEvent(Common.INFO, String.Format("RadioButtonCount  {0}", radioCount));
            for (int i = 0; i < radioCount; i++)
            {
                string appAnswer = optionRadios.ElementAt(i).Text;
                Common.ReportEvent(Common.INFO, String.Format("RadioButton Index {0}, Radio Button Value {1}", i, appAnswer));
            }

            if (OOWResult.ToUpper() != "SUCCESS")
            {
                //If the desired outcome is to fail out of wallet, this will select "None of the Above" which is always the last answer in the list.
                Common.ReportEvent(Common.INFO, String.Format("Free Credit result {0}, Selecting None of the Above", OOWResult));
                optionRadios.ElementAt(radioCount - 1).Click();

            }
            else 
                // Answer OOW Correctly based on the hard coded InitializeAnswers list.  If the question set changes, the available answers will be reported during execution and the InitializeAnswers list 
                    //will need to be updated
            {
                for (int i = 0; i < radioCount; i++)
                {
                    for (int j = 0; j < answers.Count(); j++)
                    {
                        string appAnswer = optionRadios.ElementAt(i).Text;
                        bool answerFound = appAnswer.Equals(answers.ElementAt(j));
                        if (answerFound == true)
                        {
                            Common.ReportEvent(Common.INFO, String.Format("RadioButton Index {0}, Found & Selecting Desired Answer {1}", i, answers.ElementAt(j)));
                            optionRadios.ElementAt(i).Click();
                        }

                    }
                }
            }
        }

        public void VerifyFreeCreditSignUpSuccess(Dictionary<string, string> testData)
        {
            //Verifies a successful Free Credit Sign up when initiated from the MyLT outside flow form.
            try
            {
                WaitForElement(By.ClassName("user-icon"), 30);
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format("FAILED to displayed Desired Element: user-icon"));
                string strFilename = "ElementNotFound";
                RecordScreenshot(strFilename);
                Assert.Fail();
                return;
            }
            IWebElement user = mcSignUpDriver.FindElement(By.ClassName("user-icon"));            
            int n = 1;
            do
            {
                if (user.Text == "")
                {
                    Common.ReportEvent(Common.INFO, String.Format("User-Icon found on page, waiting for User-Icon menu to populate with a value.  Loop Count={0}", n));
                    user = mcSignUpDriver.FindElement(By.ClassName("user-icon"));
                    System.Threading.Thread.Sleep(1000);
                    n = n + 1; 
                }
                else
                {
                    Common.ReportEvent(Common.INFO, String.Format("User-Icon found on page, User-Icon menu populated.  Loop Count={0}", n));
                    n = 61;
                }
            } while (n < 61);


            //Verify Destination URL
            string strURL = mcSignUpDriver.Url;
            bool success;
            success = strURL.Contains("credit-report");
            if (success == true)
            {
                Common.ReportEvent(Common.PASS, String.Format("Test Case Name: {0} -- SUCCESSFULLY logged into myLT for {1} {2}",
                    testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"]));
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("Test Case Name: {0} -- FAILED TO logged into myLT for for {1} {2}",
                    testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"]));
                string strFilename = "VerifyFreeCreditSignUpSuccess_URLnotFound";
                RecordScreenshot(strFilename);

            }
            //Verify Header Name displayed
            string fullName = testData["ConsumerFirstName"] + " " + testData["ConsumerLastName"];

            if (user.Text.Contains(fullName))
            {
                Common.ReportEvent(Common.PASS, String.Format("Test Case Name: {0} -- MyLendingTree Nav successfully contains expected Consumer Name: {1} {2}.  Actual User Icon Text = {3}.",
                    testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"], user.Text));
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("Test Case Name: {0} -- MyLendingTree Nav FAILED TO contain expected Consumer Name: {1} {2}.  Actual User Icon Text = {3}.",                
                    testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"], user.Text));
                string strFilename = "VerifyFreeCreditSignUpSuccess_UserNameNotFound";
                RecordScreenshot(strFilename);
            }

            //Verify Credit Score on Report displayed.
            string cssS = "h1[class='score ng-binding']";
            IWebElement creditScore = mcSignUpDriver.FindElement(By.CssSelector(cssS));
            if (creditScore.Text == testData["ExpectedCreditScore"])
            {
                Common.ReportEvent(Common.PASS, String.Format("Test Case Name: {0} -- SUCCESSFULLY displayed credit score for {1} {2} with expected credit score value of {3}.  Actual Credit Score Text = {4}",
                    testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"], testData["ExpectedCreditScore"],creditScore.Text));
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("Test Case Name: {0} -- FAILED TO display credit score for {1} {2} with expected credit score value of {3}.  Actual Credit Score Text = {4}",
                    testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"], testData["ExpectedCreditScore"], creditScore.Text));
                string strFilename = "VerifyFreeCreditSignUpSuccess_ScoreNotFound";
                RecordScreenshot(strFilename);
            }
        }

        public void CompleteFreeCreditSignupPII(Dictionary<string, string> testData)
        {
            //Completes the MyLT free Credit Sign up PII steps for the outside flow form
            //Sign Up Step 1
            bool stepFound = VerifyMCSignUpStep("step/1/");

            if (stepFound == true)
            {
                PopulateFirstName(testData["ConsumerFirstName"]);
                PopulateLastName(testData["ConsumerLastName"]);
                System.Threading.Thread.Sleep(2000);
                PopulateEmailAddress(testData["EmailAddress"]);
                System.Threading.Thread.Sleep(2000);
                PopulatePassword(testData["Password"]);
                System.Threading.Thread.Sleep(3000);
                //ClickButton();
                WaitForElement(By.CssSelector("button.main_button.submit"), 15);
                IWebElement CreateAccount = mcSignUpDriver.FindElement(By.CssSelector("button.main_button.submit"));
                CreateAccount.Click();
                Common.ReportEvent(Common.INFO, String.Format("Clicked Create Account Button"));
                stepFound = false;
            }


            //Sign Up Step 2
            bool waiting = true;

            while (waiting == true)
            {
                int counter = 1;
                //System.Threading.Thread.Sleep(1000);
                if (stepFound==true)
                {
                    waiting = false;
                }
                if (counter > 30 && waiting==true)
                {
                    Common.ReportEvent(Common.WARNING, String.Format("Searching for Step 2, waited for 30 seconds, no step 2 found, exiting wait loop"));
                    waiting = false;
                }
                else if (counter < 30 && waiting==true)
                {
                    Common.ReportEvent(Common.INFO, String.Format("Searching for Step 2, waiting 1 second"));
                    System.Threading.Thread.Sleep(1000);
                    stepFound = CheckMCSignUpStep("step/2/");
                    Common.ReportEvent(Common.INFO, String.Format("stepFound = {0}",stepFound));
                    Common.ReportEvent(Common.INFO, String.Format("waiting = {0}", waiting));
                    waiting = true;
                    counter = counter + 1;
                }
            }
            
            if (stepFound == true)
            {
                PopulateZipCode(testData["ConsumerZipCode"]);
                System.Threading.Thread.Sleep(4000);
                ClickButton();
                System.Threading.Thread.Sleep(2000);
                stepFound = false;
            }


            //Sign Up Step 3
            stepFound = VerifyMCSignUpStep("step/3/");
            if (stepFound == true)
            {
                PopulateStreetAddress(testData["ConsumerStreetAddress"]);
                ClickButton();
                System.Threading.Thread.Sleep(2000);
                stepFound = false;
            }

            //Sign Up Step 4
            stepFound = VerifyMCSignUpStep("step/4/");
            if (stepFound == true)
            {
                PopulateDateofBirth(testData["DateOfBirthMonth"], testData["DateOfBirthDay"], testData["DateOfBirthYear"]);
                System.Threading.Thread.Sleep(2000);
                ClickButton();
                System.Threading.Thread.Sleep(2000);
                stepFound = false;

            }

            //Sign up Step 5
            stepFound = VerifyMCSignUpStep("step/5/");
            if (stepFound == true)
            {
                PopulateConsumerSSN(testData["ConsumerSSN3"]);
                //Check Terms & Conditions check box
                IWebElement acceptChx = mcSignUpDriver.FindElement(By.Name("accepted"));
                acceptChx.Click();
                System.Threading.Thread.Sleep(2000);
                //ClickButton();
                //if (testData["FCSignUpResult"].ToLower() != "creditcard")
                //{
                //    // for OOW step to show
                //    System.Threading.Thread.Sleep(15000);
                //    stepFound = false;
                //}
                //else
                //{
                //    //Wait for step other than OOW to show
                //    System.Threading.Thread.Sleep(5000);
                //    stepFound = false;
                //}
            }
        }

        public void PopulatePIIOopsPage(Dictionary<string, string> testData)
        {
           
            //Populates the MyLT Outside flow form PII Oops Page
            IWebElement zcode = mcSignUpDriver.FindElement(By.Id("zipCode"));
            zcode.Clear();
            zcode.SendKeys(testData["ConsumerZipCode_Corrected"]);
            System.Threading.Thread.Sleep(5000);
            IWebElement sAddress = mcSignUpDriver.FindElement(By.Name("addressStreet"));
            sAddress.Clear();
            sAddress.SendKeys(testData["ConsumerStreetAddress_Corrected"]);
            IWebElement city = mcSignUpDriver.FindElement(By.Name("addressCity"));
            city.Clear();
            city.SendKeys(testData["ConsumerCity_Corrected"]);
            SelectElement state = new SelectElement(mcSignUpDriver.FindElement(By.Name("states")));
            state.SelectByText(testData["ConsumerState_Corrected"]);
        }

        public void PopulateUpsellPII(Dictionary<string, string> testData)
        {
            //Populates the MyLT upsell PII form for non - free credit users signed in to MyLT.

            //Define Upsell PII elements--Intial Set
            IWebElement fname = mcSignUpDriver.FindElement(By.Name("firstName"));
            IWebElement lname = mcSignUpDriver.FindElement(By.Name("lastName"));
            IWebElement ssn = mcSignUpDriver.FindElement(By.Name("ssn3"));
            IWebElement acceptChx = mcSignUpDriver.FindElement(By.Name("accepted"));
            IWebElement continueButton = mcSignUpDriver.FindElement(By.CssSelector("span[class=nonloading-label]"));
            //Set Initial values
            ssn.SendKeys(testData["ConsumerSSN3"]);
            acceptChx.Click();
            System.Threading.Thread.Sleep(2000);
            continueButton.Click();
            System.Threading.Thread.Sleep(5000);

            //Add Additional PII Elements
            IWebElement street = mcSignUpDriver.FindElement(By.Name("addressStreet"));
            IWebElement city = mcSignUpDriver.FindElement(By.Name("addressCity"));
            SelectElement state = new SelectElement(mcSignUpDriver.FindElement(By.Name("states")));
            IWebElement zcode = mcSignUpDriver.FindElement(By.Name("zipCode"));
            SelectElement birthMonth = new SelectElement(mcSignUpDriver.FindElement(By.Name("dobMonth")));
            SelectElement birthDay = new SelectElement(mcSignUpDriver.FindElement(By.Name("dobDay")));
            SelectElement birthYear = new SelectElement(mcSignUpDriver.FindElement(By.Name("dobYear")));

            //Clear PII Elements
            fname.Clear();
            lname.Clear();
            zcode.Clear();


            //Set PII Elements
            fname.SendKeys(testData["ConsumerFirstName"]);
            lname.SendKeys(testData["ConsumerLastName"]);
            zcode.SendKeys(testData["ConsumerZipCode"]);
            System.Threading.Thread.Sleep(5000);
            street.Clear();
            street.SendKeys(testData["ConsumerStreetAddress"]);
            city.Clear();
            city.SendKeys(testData["ConsumerCity"]);
            state.SelectByText(testData["ConsumerState"]);
            birthMonth.SelectByText(testData["DateOfBirthMonth"]);
            birthDay.SelectByText(testData["DateOfBirthDay"]);
            birthYear.SelectByText(testData["DateOfBirthYear"]);
            ssn.SendKeys(testData["ConsumerSSN3"]);
           
          
        }

        public void VerifyFreeCreditUpSellSuccess(Dictionary<string, string> testData)
        {
            //Verifies a successful Free Credit Sign up when initiated from the MyLT Upsell form.
            //Verify Header Name displayed
            string fullName = testData["ConsumerFirstName"] + " " + testData["ConsumerLastName"];
            WaitForElement(By.ClassName("user-icon"), 30);
            IWebElement user = mcSignUpDriver.FindElement(By.ClassName("user-icon"));
            int n = 1;
            do
            {
                user = mcSignUpDriver.FindElement(By.ClassName("user-icon"));
                if (user.Text.Contains(fullName))
                {
                    Common.ReportEvent(Common.PASS, String.Format("Test Case Name: {0} -- MyLendingTree Nav successfully contains expected Consumer Name: {1} {2}.  Loop Count = {3}",
                            testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"], n));
                    n = 100;
                }
                else
                {
                    Common.ReportEvent(Common.INFO, String.Format("User-Icon found on page, Waiting for Consumer Name to populate.  Loop Count={0}", n));                    
                    System.Threading.Thread.Sleep(1000);
                    n = n + 1;
                }
            } while (n < 61);
            
            if (n==61)
            {
                Common.ReportEvent(Common.INFO, String.Format("Wait for Consumer Name Loop Exited without detecting correct expected Consumer Name:  Loop Count={0}", n));
                Common.ReportEvent(Common.FAIL, String.Format("Test Case Name: {0} -- MyLendingTree Nav FAILED TO contain expected Consumer Name: {1} {2}",
                    testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"]));
                string strFilename = "VerifyFreeCreditSignUpSuccess_UserNameNotFound";
                RecordScreenshot(strFilename);
            }

            //Verify Credit Score on Report displayed.
            //string cssS = "a[nav-trigger class=ng-binding]";
            IWebElement creditScore = mcSignUpDriver.FindElement(By.ClassName("credit-link"));  //This is the Credit score link the the menu header
            if (creditScore.Text.Contains(testData["ExpectedCreditScore"]))
            {
                Common.ReportEvent(Common.PASS, String.Format("Test Case Name: {0} -- SUCCESSFULLY displayed credit score for {1} {2} with expected credit score value of {3}.  CSS Text ={4}",
                    testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"], testData["ExpectedCreditScore"], creditScore.Text));
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("Test Case Name: {0} -- FAILED TO display credit score for {1} {2} with expected credit score value of {3}.  CSS Text ={4}",
                    testData["TestCaseName"], testData["ConsumerFirstName"], testData["ConsumerLastName"], testData["ExpectedCreditScore"],creditScore.Text));
                string strFilename = "VerifyFreeCreditSignUpSuccess_ScoreNotFound";
                RecordScreenshot(strFilename);
            }
        }
        //Data Initialization Functions
        public string InitializeDateOfBirthMonth(string month)
        {
            //Initializes the Date of Birth Month into the proper format for MyLT sign up.
            int mm = Convert.ToInt32(month);
            month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(mm);
            return month;

        }

        public void AcceptEmailValidation(string email)
        {
            string strURL = "http://mailinator.com/inbox.jsp?to=" + email;
            Common.ReportEvent(Common.INFO, String.Format("Mailinator URL for this test is {0}", strURL));
            mcSignUpDriver.Navigate().GoToUrl(strURL);
            string cssS = "div[class='subject ng-binding']";
            ReadOnlyCollection<IWebElement> emailSubjects = mcSignUpDriver.FindElements(By.CssSelector(cssS));
            int subjectCount = emailSubjects.Count();
            Common.ReportEvent(Common.INFO, String.Format("Email Subject Count  {0}", subjectCount));
            for (int i = 0; i < subjectCount;i++)
            {
                string subject = emailSubjects.ElementAt(i).Text;
                Common.ReportEvent(Common.INFO, String.Format("Email Subject Index {0}, Email Subject Value {1}", i, subject));
                if (subject == "LendingTree Account Validation")
                {
                    Common.ReportEvent(Common.INFO, String.Format("Desired Email Subject Found"));
                    emailSubjects.ElementAt(i).Click();
                }
            }
            System.Threading.Thread.Sleep(2000);
            IWebElement frame = mcSignUpDriver.FindElement(By.Name("rendermail"));
            mcSignUpDriver.SwitchTo().Frame(frame);
            IWebElement mcURL = mcSignUpDriver.FindElement(By.PartialLinkText("verifyaccountaddress"));
            strURL = mcURL.Text;
            mcSignUpDriver.Navigate().GoToUrl(strURL);
            IWebElement signInButton = mcSignUpDriver.FindElement(By.CssSelector("button.main-button.sign-in-button"));
            IWebElement userName = mcSignUpDriver.FindElement(By.Id("UserName"));
            IWebElement pass = mcSignUpDriver.FindElement(By.Id("Password"));
            string password = email;
            email = email + "@mailinator.com";
            userName.SendKeys(email);
            pass.SendKeys(password);
            signInButton.Click();
            

        }
        public void UpdateLockedDateOfBirth(string month, string day, string year)
        {
            //Function to update the LOCKED scenarios with a known locked birth date.
            string strConnectionString = "Data Source=CCAVMBZTSB01;Initial Catalog=AutomatedTesting;Integrated Security=True";
            string strSQL = "update tTestData_FreeCreditUserPII";
            strSQL = strSQL + " set DateofBirthMonth ='" + month + "',";
            strSQL = strSQL + " DateofBirthDay='" + day + "',";
            strSQL = strSQL + " DateofBirthYear='" + year + "'";
            strSQL = strSQL + " where FCSignUpResult='LOCKED'";
            using (SqlConnection objConn = new SqlConnection(strConnectionString))
            {
                try
                {
                    objConn.Open();
                }
                catch (Exception e)
                {
                    Common.ReportEvent(Common.ERROR, String.Format("Unable to open a connection to the database "
                        + "with the provided connection string.  Check the connection string and try again.  "
                        + "Connection String: {0}", strConnectionString));
                    Common.ReportEvent(Common.ERROR, e.ToString());
                    return;
                }
                SqlCommand objSqlCommand = new SqlCommand(strSQL, objConn);
                objSqlCommand.ExecuteNonQuery();

            }
        }
        public Dictionary<string, string> InitializeBirthDateData(Dictionary<string, string> testData)
        {
            //Converts Randomly generated birth date value from InitializeTestData() function into the MyLT sign up format.

            switch (testData["FCSignUpResult"].ToUpper())
            {
                case "SUCCESS":
                    testData["DateOfBirthMonth"] = InitializeDateOfBirthMonth(testData["DateOfBirthMonth"]);
                    int bDay = Convert.ToInt32(testData["DateOfBirthDay"]);
                    testData["DateOfBirthDay"] = bDay.ToString();
                    testData["DateOfBirthYear"] = "1940";
                    break;
                case "OOWOOPSSUCCESS":
                    testData["DateOfBirthMonth"] = InitializeDateOfBirthMonth(testData["DateOfBirthMonth"]);
                    bDay = Convert.ToInt32(testData["DateOfBirthDay"]);
                    testData["DateOfBirthDay"] = bDay.ToString();
                    testData["DateOfBirthYear"] = "1930";
                    break;
                case "CREDITCARD":
                    testData["DateOfBirthMonth"] = InitializeDateOfBirthMonth(testData["DateOfBirthMonth"]);
                    bDay = Convert.ToInt32(testData["DateOfBirthDay"]);
                    testData["DateOfBirthDay"] = bDay.ToString();
                    testData["DateOfBirthYear"] = "1945";
                    break;
                case "OOWOOPSLOCKED":
                    testData["DateOfBirthYear"] = "1935";
                    UpdateLockedDateOfBirth(testData["DateOfBirthMonth"], testData["DateOfBirthDay"], testData["DateOfBirthYear"]);
                    testData["DateOfBirthMonth"] = InitializeDateOfBirthMonth(testData["DateOfBirthMonth"]);
                    bDay = Convert.ToInt32(testData["DateOfBirthDay"]);
                    testData["DateOfBirthDay"] = bDay.ToString();                    
                    break;
                case "LOCKED":
                    testData["DateOfBirthMonth"] = InitializeDateOfBirthMonth(testData["DateOfBirthMonth"]);
                    bDay = Convert.ToInt32(testData["DateOfBirthDay"]);
                    testData["DateOfBirthDay"] = bDay.ToString();
                    break;
                default: // Default check for OOWresult
                    Common.ReportEvent(Common.ERROR, String.Format("FAILED to provide a known Out of Wallet Result: {0}", testData["FCSignUpResult"]));
                    Assert.Fail("No recognized Out of Wallet Result value was provided");
                    break;
            }
            return testData;
        }

        public Dictionary<string, string> SetMCUpsellCredentials(Dictionary<string, string> testData)
        {
            //Pulls a set of existing QF consumers generated by the FOSSA automation from Lendx and compares each TreeAuthUID retrieved from Lendx to the MC_Correlation database to find one that hasn't been upsold yet.
            DataTable consumers = FindExistingQFConsumers();
            string strConnectionString = "Data Source=pkx1dbmcQA01V, 1499;Initial Catalog=MC_Correlation;Integrated Security=True";
           
            bool loginChanged = false;
            foreach (DataRow row in consumers.Rows)
            {
                Guid treeAuth = row.Field<Guid>("TreeAuthUserUID");
                Common.ReportEvent(Common.INFO, String.Format("User GUID:  {0}", treeAuth));
                string strSQL = "select PassedOutOfWalletIndicator from Credit.Consumer WHERE AuthUserUID=";
                strSQL = strSQL + "'" + treeAuth.ToString() + "'";
                Common.ReportEvent(Common.INFO, String.Format("MC Correlation SQl:  {0}", strSQL));
                using (SqlConnection objConn = new SqlConnection(strConnectionString))
                {
                    try
                    {
                        objConn.Open();
                    }

                    catch (Exception e)
                    {
                        Common.ReportEvent(Common.ERROR, String.Format("Unable to open a connection to the database "
                                + "with the provided connection string.  Check the connection string and try again.  "
                                + "Connection String: {0}", strConnectionString));
                        Common.ReportEvent(Common.ERROR, e.ToString());
                        return testData;
                    }
                    SqlCommand objSqlCommand = new SqlCommand(strSQL, objConn);
                    var result = objSqlCommand.ExecuteScalar();
                    if (result == null)
                    {
                        Common.ReportEvent(Common.INFO, String.Format("PassedOutofWalletIndicator:  not found for TreeAuthUID: {0}",treeAuth.ToString()));
                    }
                    else
                    {
                        string passedOOW = result.ToString();
                        Common.ReportEvent(Common.INFO, String.Format("PassedOutofWalletIndicator:  {0}", passedOOW));
                        if (passedOOW == "False")
                        {
                            loginChanged = true;
                            testData["EmailAddress"] = row.Field<string>("emailaddress");
                            int index = testData["EmailAddress"].IndexOf("@");
                            testData["Password"] = testData["EmailAddress"].Substring(0, index);
                            testData["Password"] = testData["Password"].Replace("qa+", "otto");
                            testData.Add("TreeAuthUID", treeAuth.ToString());
                            break;
                        }
                    }
                      
                    

                }

            }
            if (loginChanged == true)
            {
                return testData;
            }
            else
            {

                Common.ReportEvent(Common.FAIL, String.Format("Unable to find a existing user who has not signed up for Free Credit"));
                Assert.Fail();
                return testData;
            }
        }

        public DataTable FindExistingQFConsumers()
        {
            //Pulls a set of existing QF consumers generated by the FOSSA automation from Lendx who only have 1 qf associated with the email address.
            string strConnectionString = "Data Source=lendxqa\\lxprod01;Initial Catalog=Lendx;Integrated Security=True";
            string strSQL = "USE LENDX";
            //strSQL = strSQL + " GO";
            strSQL = strSQL + " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";
            strSQL = strSQL + " select distinct top 40 CN.Consumerid,C.emailaddress, ac.TreeAuthUserUID, Count(Q.QFormid) from tContact C";
            strSQL = strSQL + " join tborrowercontact BC on BC.Contactid = C.contactid";
            strSQL = strSQL + " join tQform Q on Q.qformid = BC.qformid";
            strSQL = strSQL + " join tconsumer Cn on Cn.currentcontactid=C.contactid";
            strSQL = strSQL + " JOIN lendx.Auth.tConsumerMapping ac with (nolock)  ON cn.ConsumerUID = ac.ConsumerUID";
            strSQL = strSQL + " where C.EmailAddress like 'qa+%' and C.EmailAddress like '%lendingtree.com'";
            //strSQL = strSQL + " and q.producttypelookup =4";// and Cn.insertdatetime < getdate()";
            strSQL = strSQL + " group by CN.Consumerid, C.emailaddress, ac.TreeAuthUserUID";
            strSQL = strSQL + " having Count(Q.QFormid) = 1";
            strSQL = strSQL + " order by CN.Consumerid desc";
            DataTable dt = new DataTable();

            using (SqlConnection objConn = new SqlConnection(strConnectionString))
            {
                try
                {
                    objConn.Open();
                }
                catch (Exception e)
                {
                    Common.ReportEvent(Common.ERROR, String.Format("Unable to open a connection to the database "
                        + "with the provided connection string.  Check the connection string and try again.  "
                        + "Connection String: {0}", strConnectionString));
                    Common.ReportEvent(Common.ERROR, e.ToString());
                    return dt;
                }
                SqlCommand objSqlCommand = new SqlCommand(strSQL, objConn);
                using (SqlDataReader dr = objSqlCommand.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            Common.ReportEvent(Common.INFO, String.Format("Returning 20 existing QF users for potential use in test"));
            return dt;
        }

        public Dictionary<string, string> SetQFParametersForOfferUpsellCredentials(Dictionary<string, string> testData)
        {
            //Pulls QFUID, ProductType and MortgageSubtype for FOSSA upsell Navigation.
            string strConnectionString = "Data Source=lendxqa\\lxprod01;Initial Catalog=Lendx;Integrated Security=True";
            string strSQL = "USE LENDX";
            strSQL = strSQL + " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";
            strSQL = strSQL + " select QFormUID,ProductTypeLookup,MortgageClassLookup, VehicleCategoryId  from tqform Q";
            strSQL = strSQL + " left join tqformmortgage M on M.qformid = Q.qformid";
            strSQL = strSQL + " left join tqformAuto At on At.qformid = Q.Qformid";
            strSQL = strSQL + " join tborrowercontact BC on BC.qformid = Q.qformid";
            strSQL = strSQL + " join tQformBorrower QB on QB.borrowerid=BC.borrowerid";
            strSQL = strSQL + " join tconsumer Cn on Cn.currentcontactid=BC.contactid";
            strSQL = strSQL + " join auth.tConsumerMapping A on A.Consumeruid = Cn.consumeruid";
            strSQL = strSQL + " where A.TreeAuthUserUID = '" + testData["TreeAuthUID"] + "'";
            DataTable qfParams = new DataTable();
            using (SqlConnection objConn = new SqlConnection(strConnectionString))
            {
                try
                {
                    objConn.Open();
                }
                catch (Exception e)
                {
                    Common.ReportEvent(Common.ERROR, String.Format("Unable to open a connection to the database "
                        + "with the provided connection string.  Check the connection string and try again.  "
                        + "Connection String: {0}", strConnectionString));
                    Common.ReportEvent(Common.ERROR, e.ToString());
                    
                }
                SqlCommand objSqlCommand = new SqlCommand(strSQL, objConn);
                using (SqlDataReader dr = objSqlCommand.ExecuteReader())
                {
                    qfParams.Load(dr);
                }
            }
            if (qfParams.Rows.Count > 0)
            {
                foreach (DataRow row in qfParams.Rows)
                {
                    testData.Add("QFormUID",row.Field<Guid>("QFormUID").ToString());
                    testData.Add("ProductType",row.Field<int>("ProductTypeLookup").ToString());
                    if (testData["ProductType"] == "1")  //Add in subtype for refinance vs purchase
                    {
                        testData.Add("ProductSubtype",row.Field<int>("MortgageClassLookup").ToString());
                    }
                    if (testData["ProductType"] == "4")  //Add in subtype for Car vs Boat VS Motorcycle VS PowerSports vs RV
                    {
                        testData.Add("ProductSubtype", row.Field<int>("VehicleCategoryId").ToString());
                    }

                    
                }
                return testData;
            }
            else
            {
                Common.ReportEvent(Common.ERROR, String.Format("QF Parameters not found in database, exiting test"));
                Assert.Fail();
                return testData;
            }

        }
        
        public List<string> InitializeAnswers()
        {
            //List of Correct Answers to the OOW questions            
            List<string> answers = new List<string>();
            answers.Add("1978");
            answers.Add("San Bernardino");
            answers.Add("Stanislaus");
            answers.Add("Los Angeles");
            answers.Add("Oakdale");
            answers.Add("Duck");
            answers.Add("Sand Dunes");
            answers.Add("25000");
            answers.Add("Pepsi Co");
            answers.Add("Pepsi Co.");
            answers.Add("44 River");
            answers.Add("Chino");
            answers.Add("2001");
            answers.Add("2006");
            answers.Add("Blue");
            answers.Add("Camry");
            answers.Add("Toyota");
            answers.Add("Richard");
            answers.Add("Hunting");
            answers.Add("Fishing");
            answers.Add("Florida");
            answers.Add("2008");
            answers.Add("Silver");
            answers.Add("Civic");
            answers.Add("Honda");
            answers.Add("Toyota Camry");
            answers.Add("Honda Civic");
            return answers;

        }
    }
}
