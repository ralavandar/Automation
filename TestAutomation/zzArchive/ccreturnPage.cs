using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.zzArchive
{
    public class ccreturnPage : PageBase
    {
        private readonly IWebDriver ccreturn;
        private const String strTid = "cc-return";
        private String strStepNum;

        // Constructor
        public ccreturnPage(IWebDriver driver) : base(driver)
        {
            ccreturn = driver;
        }


        public void FillOutCcQF(Dictionary<string, string> testData)
        {
            NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", "cc-qf", "");
            CompleteCcQfStep1();
            CompleteCcQfStep2(testData);

            // Need to wait for the "SUCCESS!  Please Close this window now" msg...
            WaitForElement(By.ClassName("errorNotification"), 10);
        }

        
        public void CompleteCcQfStep1()
        {
            Common.ReportEvent(Common.INFO, "***** Starting cc-qf Step 1 *****");
            //WaitForElement(By.ClassName("step-1"), 10);
            //Assert.IsTrue(IsElementPresent(By.ClassName("step-1")), "Unable to verify the script is on cc-qf Step 1. "
            //    + "Cannot locate element of class 'step-1'.");
            WaitForAjaxToComplete(10);
            WaitForElementDisplayed(By.Id("step-1"), 5);

            System.Threading.Thread.Sleep(2000);
            //RecordScreenshot("cc-qf_Step1_Submit");
            ClickButton("next");
        }


        public void CompleteCcQfStep2(Dictionary<string, string> testData)
        {
            Common.ReportEvent(Common.INFO, "***** Starting cc-qf Step 2 *****");
            WaitForAjaxToComplete(10);
            WaitForElementDisplayed(By.Id("step-2"), 5);
            WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            //Assert.IsTrue(IsElementDisplayed(By.Id("homeloan-product-type")), "Unable to verify the script is on cc-qf Step 2. "
            //    + "Cannot locate element of id 'homeloan-product-type'.");

            // TODO: Need a better solution here...the following javascript is failing to execute without the wait, so it must be a timing issue
            System.Threading.Thread.Sleep(5000);

            // Capture/Report the GUID
            IJavaScriptExecutor js = ccreturn as IJavaScriptExecutor;
            String strScript = "return $($(\"#aspnetForm\").get(0)).data(\"fossa\").lead.id";
            strQFormUID = (String)js.ExecuteScript(strScript);
            Common.ReportEvent(Common.INFO, String.Format("The cc-qf QForm GUID = {0}", strQFormUID));

            SelectByValue("homeloan-product-type", testData["LoanType"]);
            Fill("first-name", testData["BorrowerFirstName"]);
            Fill("middle-name", testData["BorrowerMiddleName"]);
            Fill("last-name", testData["BorrowerLastName"]);
            SelectByText("name-suffix", testData["BorrowerSuffix"]);
            SelectByText("birth-month", testData["DateOfBirthMonth"]);
            SelectByText("birth-day", testData["DateOfBirthDay"]);
            SelectByText("birth-year", testData["DateOfBirthYear"]);
            Fill("street-address", testData["BorrowerStreetAddress"]);
            Fill("zip-code", testData["BorrowerZipCode"]);
            Fill("home-phone-one", testData["BorrowerHomePhone1"]);
            Fill("home-phone-two", testData["BorrowerHomePhone2"]);
            Fill("home-phone-three", testData["BorrowerHomePhone3"]);
            Fill("work-phone-one", testData["BorrowerWorkPhone1"]);
            Fill("work-phone-two", testData["BorrowerWorkPhone2"]);
            Fill("work-phone-three", testData["BorrowerWorkPhone3"]);
            Fill("email", testData["EmailAddress"]);
            
            // 06/27/2012 - Question removed from ccreturn
            //SelectByText("debt-amount", testData["CreditCardDebtAmount"]);

            // 06/27/2012 - Question removed from ccreturn
            //if (testData["DebtConsultYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            //{
            //    ClickRadio("debt-consultation-yes");
            //}
            //else
            //{
            //    ClickRadio("debt-consultation-no");
            //}

            if (testData["CreditScoreProductYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("credit-score-product-yes");
            }
            else
            {
                ClickRadio("credit-score-product-no");
            }

            Fill("email-message-addon", testData["CcMessageToBorrower"]);

            //RecordScreenshot("cc-qf_Step2_Submit");
            ClickButton("next");
        }


        public void NavigateToCcReturn(String environment, String signature, String vid)
        {
            String strBaseUrl;
            String strUrl;

            switch (environment.ToUpper())
            {
                case "PROD":
                    strBaseUrl = "http://offers.lendingtree.com";
                    break;
                case "QA":
                case "STAGE":
                case "STAGING":
                    strBaseUrl = "http://offers.staging.lendingtree.com";
                    break;
                case "DEV":
                    strBaseUrl = "http://offers.staging.lendingtree.com";
                    break;
                default:    //QA
                    strBaseUrl = "http://offers.staging.lendingtree.com";
                    break;
            }

            // If a vid was provided, then use it.  Else, do not include it on the end of the URL
            if (vid.Length > 0)
            {
                strUrl = strBaseUrl + "/tl.aspx?tid=cc-return&" + signature + "&vid=" + vid;
            }
            else
            {
                strUrl = strBaseUrl + "/tl.aspx?tid=cc-return&" + signature;
            }

            Common.ReportEvent(Common.INFO, String.Format("The cc-return URL for this test is: {0}", strUrl));
            ccreturn.Navigate().GoToUrl(strUrl);
        }


        public void FillOutValidQF(Dictionary<string, string> testData)
        {
            CompleteStep1(testData, true);
            CompleteStep2(testData, true);
            CompleteStep3(testData, true);
        }


        public void FillOutValidationStep(Dictionary<string, string> testData)
        {
            CompleteStep1(testData, false);
            CompleteStep2(testData, false);
            System.Threading.Thread.Sleep(2000);
            CompleteStep3(testData, false);
        }

        public void CompleteStep1(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 1 *****");
            if (blnValidateHeader)
            {
                WaitForElementDisplayed(By.Id("property-type"), 10);
                Assert.IsTrue(IsElementPresent(By.Id("property-type")), "Unable to verify the script is on Step 1. "
                    + "Cannot locate element of id 'property-type'.");
            }

            // Capture/Report the GUID
            IJavaScriptExecutor js = ccreturn as IJavaScriptExecutor;
            String strScript = "return $($(\"#aspnetForm\").get(0)).data(\"fossa\").lead.id";
            strQFormUID = (String)js.ExecuteScript(strScript);
            Common.ReportEvent(Common.INFO, String.Format("The cc-return QForm GUID = {0}", strQFormUID));

            // Fill out the fields & click Continue

            ////  Begin Refinance specific fields  ////
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("property-type", testData["HomeDescription"]);
                SelectByText("property-use", testData["PropertyUseType"]);
                Fill("property-zip", testData["PropertyZipCode"]);
                SelectByText("estproperty-value", testData["RefiPropertyValue"]);
                System.Threading.Thread.Sleep(500);
                SelectByText("current-monthly-payment", testData["FirstMortgagePayment"]);
                SelectByText("est-mortgage-balance", testData["FirstMortgageBalance"]);
                SelectByText("1st-mortgage-interest-rate", testData["FirstMortgageRate"]);

                if (testData["SecondMortgageYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("second-mortgage-yes");
                    // Explicit wait to ensure the Browser displays the 2nd mortgage questions
                    WaitForElementDisplayed(By.Id("2nd-mortgage-monthly-payment"), 5);
                    SelectByText("2nd-mortgage-monthly-payment", testData["SecondMortgagePayment"]);
                    SelectByText("second-mortgage-balance", testData["SecondMortgageBalance"]);
                    SelectByText("2nd-mortgage-interest-rate", testData["SecondMortgageRate"]);
                }
                else
                {
                    ClickRadio("second-mortgage-no");
                }

                SelectByText("cash-out", testData["RefiCashoutAmount"]);
            }
            ////  Begin Purchase specific fields  ////
            else if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                WaitForElementDisplayed(By.Id("new-home-yes"), 5);

                if (testData["FoundNewHomeYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("new-home-yes");
                }
                else
                {
                    ClickRadio("new-home-no");
                }

                SelectByText("property-state", testData["PropertyState"]);
                WaitForAjaxToComplete(5);
                SelectByText("property-city", testData["PropertyCity"]);
                SelectByText("purchase-price", testData["PurchasePrice"]);
                SelectByText("down-payment-amt", testData["PurchaseDownPayment"]);

                if (testData["CurrentREAgentYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("current-realestate-agent_yes");
                }
                else
                {
                    ClickRadio("current-realestate-agent_no");
                }

                SelectByText("property-type", testData["HomeDescription"]);
                SelectByText("property-use", testData["PropertyUseType"]);
            }
            else
            {
                // Report invalid Loan_Type
                Common.ReportEvent(Common.ERROR, String.Format("Loan_Type '{0}' is not valid. " +
                    "Please check the test data database and try again.", testData["LoanType"]));
            }
            
            ////  Begin shared Purchase/Refi fields  ////
            if (IsElementDisplayed(By.Id("ltl-optin-yes")))
            {
                if (testData["LTLOptInYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("ltl-optin-yes");
                }
                else
                {
                    ClickRadio("ltl-optin-no");
                }
            }

            System.Threading.Thread.Sleep(1000);
            //RecordScreenshot("Step1_Submit");
            ClickButton("next");
        }


        public void CompleteStep2(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 2 *****");
            if (blnValidateHeader)
            {
                WaitForElementDisplayed(By.Id("birth-month"), 10);
                Assert.IsTrue(IsElementPresent(By.Id("birth-month")), "Unable to verify the script is on Step 2. "
                    + "Cannot locate element of id 'birth-month'.");
            }

            SelectByText("birth-month", testData["DateOfBirthMonth"]);
            SelectByText("birth-day", testData["DateOfBirthDay"]);
            SelectByText("birth-year", testData["DateOfBirthYear"]);
            SelectByText("stated-credit-history", testData["CreditProfile"]);

            if (testData["MilitaryServiceYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("is-veteran-yes");
            }
            else
            {
                ClickRadio("is-veteran-no");
            }

            SelectByText("foreclosure-text", testData["ForeclosureHistory"]);
            SelectByText("bankruptcy-discharged", testData["BankruptcyHistory"]);

            System.Threading.Thread.Sleep(1000);
            //RecordScreenshot("Step2_Submit");
            ClickButton("next");
        }


        public void CompleteStep3(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 3 *****");
            if (blnValidateHeader)
            {
                WaitForElementDisplayed(By.Id("social-security-one"), 10);
                Assert.IsTrue(IsElementPresent(By.Id("social-security-one")), "Unable to verify the script is on Step 3. "
                    + "Cannot locate element of id 'social-security-one'.");
            }

            Fill("social-security-one", testData["BorrowerSsn1"]);
            Fill("social-security-two", testData["BorrowerSsn2"]);
            Fill("social-security-three", testData["BorrowerSsn3"]);
            Fill("password", testData["Password"]);

            System.Threading.Thread.Sleep(1000);
            //RecordScreenshot("Step3_Submit");
            Common.ReportEvent(Common.INFO, "***** Submitting cc-return QF *****");
            ClickButton("next");
        }


        public void ClickThroughSteps(Int32 intNumSteps)
        {
            // Loop through steps - Verify on expected step, delay for 1 sec, then click Continue/Submit
            for (int i = 1; i <= intNumSteps; i++)
            {
                strStepNum = "step-" + i.ToString();
                WaitForElementDisplayed(By.Id(strStepNum), 10);
                System.Threading.Thread.Sleep(1000);
                ClickButton("next");

                //if (i.Equals(intNumSteps))
                //{
                //    // Last page, click the submit button
                //    SubmitQF();
                //}
                //else
                //{
                //    // Click the continue button
                //    ContinueToNextStep();
                //}
            }
        }


        public string GenerateSignature(Dictionary<string, string> testData, String strGuid)
        {
            string sig;
            Guid guid = new Guid(strGuid);

            sig = UrlCreator.CreateSignedParameters("dev-key", guid, "1");
            Common.ReportEvent(Common.INFO, String.Format("The generated Signature is: {0}", sig));
            return sig;
        }


        public void ValidateContactForm(Dictionary<string, string> testData)
        {
            // Validate class=product-type value 
            this.WaitForElementDisplayed(By.Id("property-type"), 10);
            switch (testData["LoanType"].ToUpper())
            {
                case "REFINANCE":
                    Validation.StringCompare("Refinance Request", ccreturn.FindElement(By.ClassName("product-type")).Text);
                    break;
                case "PURCHASE":
                    Validation.StringCompare("Home Purchase Request", ccreturn.FindElement(By.ClassName("product-type")).Text);
                    break;
                default:    //Unknown LoanType
                    Validation.StringCompare("Unknown LoanType", ccreturn.FindElement(By.ClassName("product-type")).Text);
                    break;
            }
            
            // Validate that contact data is pre-popped correctly
            String strContactFormText = ccreturn.FindElement(By.ClassName("contact-form-inner")).Text;
            String strHomePhone = "(" + testData["BorrowerHomePhone1"] + ") " + testData["BorrowerHomePhone2"] + "-"
                + testData["BorrowerHomePhone3"];
            Validation.StringContains(testData["BorrowerFirstName"] + " " + testData["BorrowerLastName"], strContactFormText);
            Validation.StringContains(testData["BorrowerStreetAddress"], strContactFormText);
            Validation.StringContains(testData["BorrowerZipCode"], strContactFormText);
            Validation.StringContains(strHomePhone, strContactFormText);
            Validation.StringContains(testData["EmailAddress"], strContactFormText);
        }


        public void ValidateDateOfBirth(Dictionary<string, string> testData)
        {
            this.WaitForElementDisplayed(By.Id("birth-month"), 10);
            Assert.IsTrue(this.IsElementPresent(By.Id("birth-month")), "Unable to verify the script is on Step 2. "
                + "Cannot locate element of id 'birth-month'.");
            Validation.VerifySelectedOption(ccreturn, "birth-month", testData["DateOfBirthMonth"]);
            Validation.VerifySelectedOption(ccreturn, "birth-day", testData["DateOfBirthDay"]);
            Validation.VerifySelectedOption(ccreturn, "birth-year", testData["DateOfBirthYear"]);
        }
    }
}
