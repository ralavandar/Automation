using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.zzArchive
{
    public class homeequityPage : PageBase
    {
        private readonly IWebDriver hedriver;
        private const String strTid = "homeequity";
        private String strStepNum;

        // Constructor
        public homeequityPage(IWebDriver driver) : base(driver)
        {
            hedriver = driver;
        }

        public void FillOutValidQF(Dictionary<string, string> testData)
        {
            NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", strTid, testData["Variation"], testData["QueryString"]);
            CompleteStep1(testData, true);
            CompleteStep2(testData, true);
            CompleteStep3(testData, true);
            CompleteStep4(testData, true);
            CompleteStep5(testData, true);
            CompleteStep6(testData, true);
            CompleteStep7(testData, true);
            CompleteStep8(testData, true);
            CompleteStep9(testData, true);
            CompleteStep10(testData, true);
            CompleteStep11(testData, true);
            CompleteStep12(testData, true);
            CompleteStep13(testData, true);
        }

        public void FillOutValidationStep(Dictionary<string, string> testData)
        {
            CompleteStep1(testData, false);
            CompleteStep2(testData, false);
            CompleteStep3(testData, false);
            CompleteStep4(testData, false);
            CompleteStep5(testData, false);
            CompleteStep6(testData, false);
            CompleteStep7(testData, false);
            CompleteStep8(testData, false);
            CompleteStep9(testData, false);
            CompleteStep10(testData, false);
            CompleteStep11(testData, false);
            CompleteStep12(testData, false);
            WaitForAjaxToComplete(5);
            // System.Threading.Thread.Sleep(2000);
            CompleteStep13(testData, false);
            WaitForAjaxToComplete(10);
        }

        public void CompleteStep1(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 1 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-1"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-1")), "Unable to verify the script is on Step 1. "
                    + "Cannot locate element of class 'step-1'.");
            }

            // Fill out the fields & click Continue
            SelectByValue("homeloan-product-type", testData["LoanType"]);
            SelectByText("property-type", testData["PropertyType"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep2(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 2 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-2"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-2")), "Unable to verify the script is on Step 2. "
                    + "Cannot locate element of class 'step-2'.");
            }

            System.Threading.Thread.Sleep(5000);

            // Capture/Report the GUID and QFVersion
            //strQFormUID = this.GetFossaFormGuid();
            strQFormUID = this.GetFormLeadId("#aspnetForm");
            Common.ReportEvent(Common.INFO, String.Format("The QForm GUID = {0}", strQFormUID));
            //IJavaScriptExecutor js = hedriver as IJavaScriptExecutor;
            //String strScript = "return $($(\"#aspnetForm\").get(0)).data(\"fossa\").lead.id";
            //Common.ReportEvent("DEBUG", String.Format("The Javascript for QForm GUID is {0}", strScript));
            //strQFormUID = (String)js.ExecuteScript(strScript);
            //Common.ReportEvent(Common.INFO, String.Format("The cc-qf QForm GUID = {0}", strQFormUID));

            // Check desired options for Loan Purpose (Debt Consolidation, Purchase a Boat, etc)
            if (testData["LoanPurposeDebtYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                Check("loan-purpose-debt");
            else
                Uncheck("loan-purpose-debt");

            if (testData["LoanPurposeBoatYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                Check("loan-purpose-boat");
            else
                Uncheck("loan-purpose-boat");

            if (testData["LoanPurposeRvYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                Check("loan-purpose-rv");
            else
                Uncheck("loan-purpose-rv");

            if (testData["LoanPurposeMotorcycleYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                Check("loan-purpose-motorcycle");
            else
                Uncheck("loan-purpose-motorcycle");

            if (testData["LoanPurposeImprovementYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                Check("loan-purpose-improvement");
            else
                Uncheck("loan-purpose-improvement");

            if (testData["LoanPurposeAutoYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                Check("loan-purpose-auto");
            else
                Uncheck("loan-purpose-auto");

            if (testData["LoanPurposeOtherYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Check("loan-purpose-other");
                Fill("loan-purpose-reason", testData["LoanPurposeOtherReason"]);
            }
            else
                Uncheck("loan-purpose-other");

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep3(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 3 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-3"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-3")), "Unable to verify the script is on Step 3. "
                    + "Cannot locate element of class 'step-3'.");
            }

            SelectByText("desired-loan-type", testData["HELoanType"]);
            SelectByText("desired-loan-term", testData["HELoanTerm"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep4(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 4 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-4"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-4")), "Unable to verify the script is on Step 4. "
                    + "Cannot locate element of class 'step-4'.");
            }

            Fill("property-zip", testData["PropertyZipCode"]);
            SelectByText("property-use", testData["PropertyUse"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep5(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 5 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-5"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-5")), "Unable to verify the script is on Step 5. "
                    + "Cannot locate element of class 'step-5'.");
            }

            SelectByText("estproperty-value", testData["PropertyValue"]);
            SelectByText("purchase-price", testData["PurchasePrice"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep6(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 6 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-6"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-6")), "Unable to verify the script is on Step 6. "
                    + "Cannot locate element of class 'step-6'.");
            }

            SelectByText("purchase-year", testData["PurchaseYear"]);
            SelectByText("property-mortgage", testData["CurrentMortgages"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep7(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 7 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-7"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-7")), "Unable to verify the script is on Step 7. "
                    + "Cannot locate element of class 'step-7'.");
            }

            if (IsElementDisplayed(By.Id("est-mortgage-balance")))
            {
                SelectByText("est-mortgage-balance", testData["FirstMortgageBalance"]);
                SelectByText("current-monthly-payment", testData["FirstMortgagePayment"]);
            }

            if (IsElementDisplayed(By.Id("second-mortgage-balance")))
            {
                System.Threading.Thread.Sleep(500);
                SelectByText("second-mortgage-balance", testData["SecondMortgageBalance"]);
                SelectByText("2nd-mortgage-monthly-payment", testData["SecondMortgagePayment"]);
            }

            System.Threading.Thread.Sleep(500);
            SelectByText("desired-loan-amount", testData["RequestedLoanAmount"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep8(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 8 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-8"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-8")), "Unable to verify the script is on Step 8. "
                    + "Cannot locate element of class 'step-8'.");
            }

            SelectByText("stated-credit-history", testData["CreditProfile"]);
            SelectByText("birth-month", testData["DateOfBirthMonth"]);
            SelectByText("birth-day", testData["DateOfBirthDay"]);
            SelectByText("birth-year", testData["DateOfBirthYear"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep9(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 9 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-9"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-9")), "Unable to verify the script is on Step 9. "
                    + "Cannot locate element of class 'step-9'.");
            }

            SelectByText("foreclosure-text", testData["ForeclosureHistory"]);
            SelectByText("bankruptcy-discharged", testData["BankruptcyHistory"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep10(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 10 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-10"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-10")), "Unable to verify the script is on Step 10. "
                    + "Cannot locate element of class 'step-10'.");
            }

            Fill("first-name", testData["BorrowerFirstName"]);
            Fill("middle-name", testData["BorrowerMiddleName"]);
            Fill("last-name", testData["BorrowerLastName"]);
            SelectByText("name-suffix", testData["BorrowerSuffix"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep11(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 11 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-11"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-11")), "Unable to verify the script is on Step 11. "
                    + "Cannot locate element of class 'step-11'.");
            }

            Fill("street1", testData["BorrowerStreetAddress"]);
            Fill("zip-code", testData["BorrowerZipCode"]);
            Fill("time-at-address-years", testData["YearsAtAddress"]);
            SelectByText("time-at-address-months", testData["MonthsAtAddress"]);
            WaitForAjaxToComplete(5);

            if (IsElementDisplayed(By.Id("previous-street-address")))
            {
                Fill("previous-street-address", testData["PreviousStreetAddress"]);
                Fill("previous-zip-code", testData["PreviousZipCode"]);
            }

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep12(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 12 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-12"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-12")), "Unable to verify the script is on Step 12. "
                    + "Cannot locate element of class 'step-12'.");
            }

            Fill("home-phone-one", testData["BorrowerHomePhone1"]);
            Fill("home-phone-two", testData["BorrowerHomePhone2"]);
            Fill("home-phone-three", testData["BorrowerHomePhone3"]);
            Fill("work-phone-one", testData["BorrowerWorkPhone1"]);
            Fill("work-phone-two", testData["BorrowerWorkPhone2"]);
            Fill("work-phone-three", testData["BorrowerWorkPhone3"]);
            Fill("social-security-one", testData["BorrowerSsn1"]);
            Fill("social-security-two", testData["BorrowerSsn2"]);
            Fill("social-security-three", testData["BorrowerSsn3"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep13(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 13 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElement(By.ClassName("step-13"), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName("step-13")), "Unable to verify the script is on Step 13. "
                    + "Cannot locate element of class 'step-13'.");
            }

            Fill("email", testData["EmailAddress"]);
            Fill("password", testData["Password"]);

            System.Threading.Thread.Sleep(500);
            WaitForAjaxToComplete(5);
            Common.ReportEvent(Common.INFO, "***** Submitting homeequity QF *****");
            ContinueToNextStep();
        }


        public void ContinueToNextStep()
        {
            if (IsElementDisplayed(By.Id("next")))
            {
                //Common.ReportEvent(Common.INFO, "***** Clicking next *****");
                ClickButton("next");
            }
        }


        public void ClickThroughSteps(Int32 intNumSteps)
        {
            // Loop through steps - Verify on expected step, delay for 1 sec, then click Continue/Submit
            for (int i = 1; i <= intNumSteps; i++)
            {
                strStepNum = "step-" + i.ToString();
                WaitForElementDisplayed(By.Id(strStepNum), 5);
                WaitForElement(By.ClassName(strStepNum), 5);
                Assert.IsTrue(IsElementPresent(By.ClassName(strStepNum)), "Unable to verify the script is on Step "
                    + i.ToString() + ".  Cannot locate element of class '" + strStepNum + "'.");
                System.Threading.Thread.Sleep(1000);
                ContinueToNextStep();
            }
        }
    }
}
