using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.zzArchive
{
    public class wizardPage : PageBase
    {
        private readonly IWebDriver wizardDriver;
        private String strStepNum;

        private const int WAIT_TIME_ANIMATE = 2; //s
        private const int WAIT_TIME_AJAX = 10; //s
        private const int WAIT_TIME_STEP_ADVANCE = 5; //s

        //TODO: This delay before ending or beginning a step prevents certain mysterious issues like IE not hitting radio buttons. Increasing it might help if those issues resurface.
        private const int WAIT_TIME_INPUT_DELAY = 1000; //ms

        private bool isLoggedIn;

        // Constructor
        public wizardPage(IWebDriver driver)
            : base(driver)
        {
            wizardDriver = driver;
        }

        public void NavigateToWizard(Dictionary<string, string> testData)
        {
            String strUrl;

            switch (testData["TestEnvironment"].ToUpper())
            {
                case "PROD":
                    strUrl = "https://www.lendingtree.com/tools/wizard/start";
                    break;
                case "QA":
                case "STAGING":
                case "STAGE":
                    strUrl = "https://staging.lendingtree.com/tools/wizard/start";
                    break;
                case "DEV":
                    strUrl = "http://dev.lendingtree.com/tools/wizard/start";
                    break;
                default:    //QA
                    strUrl = "http://staging.lendingtree.com/tools/wizard/start";
                    break;
            }

            Common.ReportEvent(Common.INFO, String.Format("Navigating to Wizard URL: {0}", strUrl));
            wizardDriver.Navigate().GoToUrl(strUrl);

            //Check that we have landed on expected wizard start page
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            WaitForElementDisplayed(By.Id("loan-options"), WAIT_TIME_ANIMATE);

            isLoggedIn = driver.FindElement(By.Id("nav-main")).FindElement(By.TagName("a")).Text.Contains("Hello, " + testData["BorrowerFirstName"] + " " + testData["BorrowerLastName"]);

            // Select starting option - Buy or Refi
            if (testData["LoanType"].Equals("PURCHASE", StringComparison.OrdinalIgnoreCase))
                SelectByValue("loan-options", "/tools/wizard/homenew");
            else if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
                SelectByValue("loan-options", "/tools/wizard/homerefi");
            else
                Common.ReportEvent(Common.ERROR, "The 'LoanType' value is not valid for Wizard. "
                    + " Correct the LoanType value in the test data and try again.");

            ClickButton("learn-submit");
        }

        public void FillOutValidWizardQF(Dictionary<string, string> testData)
        {
            NavigateToWizard(testData);
            CompleteStep1(testData);
            CompleteStep2(testData);
            CompleteStep3(testData);
            CompleteStep4(testData);
            CompleteStep5(testData);
            CompleteStep6(testData);
            CompleteStep7(testData);
            if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
            {
                CompleteStep8(testData);
            }

            System.Threading.Thread.Sleep(WAIT_TIME_AJAX);
            CheckForProcessingDialog();
        }

        public void CompleteStep1(Dictionary<string, string> testData)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 1 *****");
            // Checks to ensure we are on the expected step
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            StringAssert.Contains("tools/wizard/home", wizardDriver.Url);
            WaitForElementDisplayed(By.Id("step-1"), WAIT_TIME_ANIMATE);
            WaitForElementDisplayed(By.Id("PropertyUse"), WAIT_TIME_ANIMATE);
            // Having all kinds of timing issues despite the above Wait statements.
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

            // Fill out the Purchase or Refi fields and click Continue
            if (testData["LoanType"].Equals("PURCHASE", StringComparison.OrdinalIgnoreCase))
            {
                if (testData["FoundNewHomeYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("FoundHomeY");
                }
                else
                {
                    ClickRadio("FoundHomeN");
                }
                SelectByText("PropertyType", testData["HomeDescription"]);
                SelectByText("PropertyUse", testData["PropertyUseType"]);
            }
            else if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("PropertyType", testData["HomeDescription"]);
                SelectByText("PropertyUse", testData["PropertyUseType"]);
            }
            else
            {
                Common.ReportEvent(Common.ERROR, "The 'LoanType' value is not valid for Wizard. "
                    + " Correct the LoanType value in the test data and try again.");
            }

            ContinueToNextStep();
        }

        public void CompleteStep2(Dictionary<string, string> testData)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 2 *****");
            StringAssert.Contains("#_wizardForm=step-2", wizardDriver.Url);
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            WaitForElementDisplayed(By.Id("step-2"), WAIT_TIME_ANIMATE);
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("PURCHASE", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("PropertyState", testData["PropertyState"]);
                WaitForAjaxToComplete(WAIT_TIME_AJAX);
                System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
                SelectByText("PropertyCity", testData["PropertyCity"]);
                System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

                if (testData["CurrentREAgentYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("ConsiderAgentY");
                }
                else
                {
                    ClickRadio("ConsiderAgentN");
                }
            }

            // Fill out Refi-specific fields
            if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
            {
                Fill("PropertyStreet", testData["PropertyStreet"]);
                Fill("PropertyZip", testData["PropertyZipCode"]);
            }

            // Capture/Report the GUID
            strQFormUID = this.GetFormLeadId("#wizardForm");
            Common.ReportEvent(Common.INFO, String.Format("The QForm GUID = {0}", strQFormUID));

            ContinueToNextStep();
        }
        
        public void CompleteStep3(Dictionary<string, string> testData)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 3 *****");
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            StringAssert.Contains("#_wizardForm=step-3", wizardDriver.Url);
            WaitForElementDisplayed(By.Id("step-3"), WAIT_TIME_ANIMATE);
            // Having all kinds of timing issues despite the above Wait statements.
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("PURCHASE", StringComparison.OrdinalIgnoreCase))
            {
                Fill("PurchasePrice", testData["PurchasePrice"]);
                SelectByText("DownPaymentAmt", testData["PurchaseDownPayment"]);
            }

            // Fill out Refi-specific fields
            if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
            {
                Fill("EstPropertyValue", testData["RefiPropertyValue"]);
            }

            ContinueToNextStep();
        }

        public void CompleteStep4(Dictionary<string, string> testData)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 4 *****");
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            StringAssert.Contains("#_wizardForm=step-4", wizardDriver.Url);
            WaitForElementDisplayed(By.Id("step-4"), WAIT_TIME_ANIMATE);
            // Having all kinds of timing issues despite the above Wait statements.
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

            // Fill out Refi-specific fields
            if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("EstMortgageBalance", testData["FirstMortgageBalance"]);
                SelectByText("CurrentMonthlyPayment", testData["FirstMortgagePayment"]);
                SelectByText("MortgageInterestRate", testData["FirstMortgageRate"]);

                if (testData["SecondMortgageYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
                    ClickRadio("HasMultipleMortgagesY");
                    // Wait to ensure the Browser displays the 2nd mortgage questions
                    WaitForElementDisplayed(By.Id("EstSecondMortgageBalance"), 5);
                    SelectByText("EstSecondMortgageBalance", testData["SecondMortgageBalance"]);
                    SelectByText("SecondCurrentMonthlyPayment", testData["SecondMortgagePayment"]);
                    System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
                    SelectByText("SecondMortgageInterestRate", testData["SecondMortgageRate"]);
                }
                else
                {
                    ClickRadio("HasMultipleMortgagesN");
                }
            }

            ContinueToNextStep();
        }

        public void CompleteStep5(Dictionary<string, string> testData)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 5 *****");
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            StringAssert.Contains("#_wizardForm=step-5", wizardDriver.Url);
            WaitForElementDisplayed(By.Id("step-5"), WAIT_TIME_ANIMATE);
            // Having all kinds of timing issues despite the above Wait statements.
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("PURCHASE", StringComparison.OrdinalIgnoreCase))
            {
                if (testData["FirstTimeBuyerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("IsFirstTimeY");
                }
                else
                {
                    ClickRadio("IsFirstTimeN");
                }

                FillMilitaryService(testData["MilitaryServiceYesNo"]);
                FillFinancialInfo(testData);
                System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
            }

            // Fill out Refi-specific fields
            if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("CashOut", testData["RefiCashoutAmount"]);
            }

            ContinueToNextStep();
        }

        public void CompleteStep6(Dictionary<string, string> testData)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 6 *****");
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            StringAssert.Contains("#_wizardForm=step-6", wizardDriver.Url);
            WaitForElementDisplayed(By.Id("step-6"), WAIT_TIME_ANIMATE);
            // Having all kinds of timing issues despite the above Wait statements.
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("PURCHASE", StringComparison.OrdinalIgnoreCase))
            {
                FillBorrowerAddress(testData);
            }

            // Fill out Refi-specific fields
            if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
            {
                FillMilitaryService(testData["MilitaryServiceYesNo"]);
                FillFinancialInfo(testData);
            }

            ContinueToNextStep();
        }

        public void CompleteStep7(Dictionary<string, string> testData)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 7 *****");
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            StringAssert.Contains("#_wizardForm=step-7", wizardDriver.Url);
            WaitForElementDisplayed(By.Id("step-7"), WAIT_TIME_ANIMATE);
            // Having all kinds of timing issues despite the above Wait statements.
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("PURCHASE", StringComparison.OrdinalIgnoreCase))
            {
                FillBorrowerContactInfo(testData);
                if (!isLoggedIn)
                {
                    FillPassword(testData["Password"], testData["ConfirmPassword"]);
                }
                FillSSN(testData["BorrowerSsn1"], testData["BorrowerSsn2"], testData["BorrowerSsn3"]);
                SubmitQF();
            }

            // Fill out Refi-specific fields
            if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
            {
                FillBorrowerAddress(testData);
                ContinueToNextStep();
            }
        }

        public void CompleteStep8(Dictionary<string, string> testData)
        {
            // Fill out Refinance-specific fields
            if (testData["LoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
            {
                Common.ReportEvent(Common.INFO, "***** Starting Step 8 - Refi Only *****");
                WaitForAjaxToComplete(WAIT_TIME_AJAX);
                StringAssert.Contains("#_wizardForm=step-8", wizardDriver.Url);
                WaitForElementDisplayed(By.Id("step-8"), WAIT_TIME_ANIMATE);
                // Having all kinds of timing issues despite the above Wait statements.
                System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

                FillBorrowerContactInfo(testData);
                if (!isLoggedIn)
                {
                    FillPassword(testData["Password"], testData["ConfirmPassword"]);
                }
                FillSSN(testData["BorrowerSsn1"], testData["BorrowerSsn2"], testData["BorrowerSsn3"]);
                SubmitQF();
            }
        }

        public void ContinueToNextStep()
        {
            // Having all kinds of timing issues despite the above Wait statements.
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
            WaitForAjaxToComplete(WAIT_TIME_AJAX);

            // Find all elements with class name 'next-step'
            IList<IWebElement> allNextButtons = wizardDriver.FindElements(By.ClassName("next-step"));

            if (allNextButtons.Count == 0)
            {
                Common.ReportEvent(Common.WARNING, "The page did not contain any buttons with a ClassName = 'next-step'."
                    + " Double-check the actual class name for the button.");
            }
            
            // TODO: Change this to a 'do until'

            for (int i = 0; i < allNextButtons.Count; i++)
            {
                //Common.ReportEvent("DEBUG", String.Format("Displayed property for next button #{0} is {1}.",
                //    i, allNextButtons[i].Displayed));

                if (allNextButtons[i].Displayed)
                {
                    //Common.ReportEvent("DEBUG", String.Format("Clicking Next button, index #{0}.", i));
                    allNextButtons[i].Click();
                    break;
                }
                else
                {
                    // Move on to the next element, but first, check and see if we've checked the last element
                    if (i == (allNextButtons.Count - 1))
                    {
                        Common.ReportEvent(Common.WARNING, String.Format("{0} 'next-step' buttons were checked, but none of"
                            + " them had a Displayed value = True.  Double-check the actual class name for the button.",
                            allNextButtons.Count.ToString()));
                    }
                }
            }
        }

        public void SubmitQF()
        {
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
            WaitForAjaxToComplete(WAIT_TIME_AJAX);

            if (IsElementDisplayed(By.ClassName("submitfinalstep")))
            {
                Common.ReportEvent(Common.INFO, "***** Submitting Wizard QF *****");
                ClickButton(By.ClassName("submitfinalstep"));
            }
            else
            {
                Common.ReportEvent(Common.WARNING, "The 'submitfinalstep' button is NOT displayed on the page. "
                    + " Double-check the actual class name for the button.");
            }
        }

        public void ClickThroughSteps(Int32 intNumSteps)
        {
            // Loop through steps - Verify on expected step, delay for 1 sec, then click Continue/Submit
            for (int i = 1; i <= intNumSteps; i++)
            {
                strStepNum = "step-" + i.ToString();
                System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
                WaitForElementDisplayed(By.Id(strStepNum), WAIT_TIME_ANIMATE);                

                if (i.Equals(intNumSteps))
                {
                    // Last page, click the submit button
                    SubmitQF();
                }
                else
                {
                    // Click the continue button
                    ContinueToNextStep();
                }
            }
        }

        public void ClickButton(By objBy)
        {
            var objElement = wizardDriver.FindElement(objBy);
            objElement.Click();
        }

        public void FillMilitaryService(string strMilitaryYesNo)
        {
            if (strMilitaryYesNo.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("IsVeteranY");
            }
            else
            {
                ClickRadio("IsVeteranN");
            }
        }

        public void FillFinancialInfo(Dictionary<string, string> testData)
        {
            SelectByText("StatedCreditHistory", testData["CreditProfile"]);
            SelectByText("BankruptcyDischarged", testData["BankruptcyHistory"]);
            SelectByText("Foreclosure", testData["ForeclosureHistory"]);
        }

        public void FillBorrowerContactInfo(Dictionary<string, string> testData)
        {
            Fill("FirstName", testData["BorrowerFirstName"]);
            Fill("LastName", testData["BorrowerLastName"]);
            FillBirthDate(testData["DateOfBirthMonth"], testData["DateOfBirthDay"], testData["DateOfBirthYear"]);
            Fill("HomePhone", testData["BorrowerHomePhone1"] + testData["BorrowerHomePhone2"] +
                testData["BorrowerHomePhone3"]);
            Fill("WorkPhone", testData["BorrowerWorkPhone1"] + testData["BorrowerWorkPhone2"] +
                testData["BorrowerWorkPhone3"]);
            Fill("Email", testData["EmailAddress"]);
        }

        public void FillBorrowerAddress(Dictionary<string, string> testData)
        {
            Fill("Street1", testData["BorrowerStreetAddress"]);
            Fill("ZipCode", testData["BorrowerZipCode"]);
        }

        public void FillPassword(string strPassword, string strConfirmPassword)
        {
            Fill("Password", strPassword);
            Fill("ConfirmPassword", strConfirmPassword);
        }

        public void FillSSN(string strSsn1, string strSsn2, string strSsn3)
        {
            Fill("ssn-one", strSsn1);
            Fill("ssn-two", strSsn2);
            Fill("ssn-three", strSsn3);
        }

        public void FillBirthDate(string strMonth, string strDay, string strYear)
        {
            // Click on the BirthDate field to display the calandar select
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
            this.GetElement("BirthDate").Click();

            // Select on the birth year
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
            if (strYear.Length > 0)
            {
                SelectElement objSelect = new SelectElement(wizardDriver.FindElement(By.ClassName("ui-datepicker-year")));
                System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

                try
                {
                    objSelect.SelectByText(strYear);
                }
                catch
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByText failed to select the text '{0}' in "
                        + "dropdown '{1}'.", strYear, "ui-datepicker-year"));
                }
            }

            // Select on the birth month
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
            if (strMonth.Length > 0)
            {
                SelectElement objSelect = new SelectElement(wizardDriver.FindElement(By.ClassName("ui-datepicker-month")));
                System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
                                
                try
                {
                    int monthVal = Convert.ToInt32(strMonth) - 1;
                    objSelect.SelectByValue(monthVal.ToString());
                }
                catch
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByText failed to select the text '{0}' in "
                        + "dropdown '{1}'.", strMonth, "ui-datepicker-month"));
                }
            }

            // Click on the appropriate day
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
            if (strDay.Length > 0)
            {
                //strip leading 0 off 'of' strDay
                if (strDay[0].Equals('0'))
                    strDay = strDay.Remove(0, 1);
                
                IWebElement calendar = wizardDriver.FindElement(By.ClassName("ui-datepicker-calendar"));
                IWebElement link = calendar.FindElement(By.LinkText(strDay));
                System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
                link.Click();
                System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
            }
        }

        public void CheckForProcessingDialog()
        {
            
            // TODO: Re-write this so it does not fail if the dialog is not found
            WaitForElementDisplayed(By.ClassName("progress-dialog"), WAIT_TIME_AJAX);
        }

        public int GetErrorLabelCount()
        {
            int errorLabelCount = 0;
            var elements = wizardDriver.FindElements(By.ClassName("error"));

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].GetAttribute("tagName").ToUpper().Equals("LABEL"))
                {
                    errorLabelCount++;
                }
            }

            return errorLabelCount;
        }
    }
}
