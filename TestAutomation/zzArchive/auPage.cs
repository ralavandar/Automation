using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tl
{
    public class auPage : PageBase
    {
        private readonly IWebDriver autoDriver;
        private const String strTid = "au";
        private String strStepNum;

        // Constructor
        public auPage(IWebDriver driver) : base(driver)
        {
            autoDriver = driver;
        }

        public void FillOutValidQF(Dictionary<string, string> testData)
        {
            NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", strTid, testData["Variation"]);
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
            
            // if there is a coborrower, then complete the remaining steps
            if (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                CompleteStep11(testData, true);
                CompleteStep12(testData, true);
                CompleteStep13(testData, true);
                CompleteStep14(testData, true);
                CompleteStep15(testData, true);
                CompleteStep16(testData, true);
            }
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

            // if there is a coborrower, then complete the remaining steps
            if (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                CompleteStep11(testData, false);
                CompleteStep12(testData, false);
                CompleteStep13(testData, false);
                CompleteStep14(testData, true);
                CompleteStep15(testData, true);
                System.Threading.Thread.Sleep(2000);
                CompleteStep16(testData, true);
            }
        }


        public void CompleteStep1(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 1 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step1"), 5);
                System.Threading.Thread.Sleep(1000);
            }

            // Capture/Report the GUID and QFVersion
            strQFormUID = autoDriver.FindElement(By.Id("GUID")).Text;
            Common.ReportEvent(Common.INFO, String.Format("The QForm GUID = {0}", strQFormUID));

            // Fill out the fields & click Continue
            SelectByValue("loan-type", testData["AutoLoanType"]);
            Fill("loan-period", testData["AutoLoanTerm"]);
            if (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("radHasCBYes");
            }
            else
            {
                ClickRadio("radHasCBNo");
            }

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep2(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 2 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step2"), 5);
                System.Threading.Thread.Sleep(500);
            }

            // New Car Purchase OR Used Car Purchase
            if (testData["AutoLoanType"].Equals("NewCarPurchase", StringComparison.OrdinalIgnoreCase)
                || testData["AutoLoanType"].Equals("UsedCarPurchase", StringComparison.OrdinalIgnoreCase))
            {
                Fill("down-payment", testData["PurchaseDownPayment"]);
                Fill("requested-loan-amount", testData["RequestedLoanAmount"]);
                SelectByText("vehicle-state", testData["VehicleState"]);
            }

            // Refinance OR Lease Buy Out
            if (testData["AutoLoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase)
                || testData["AutoLoanType"].Equals("BUYOUTLEASE", StringComparison.OrdinalIgnoreCase))
            {
                Fill("current-interest-rate", testData["CurrentRate"]);
                Fill("current-lender", testData["CurrentLender"]);
                Fill("payoff-amount", testData["CurrentPayoffAmount"]);
                SelectByText("remaining-terms", testData["CurrentRemainingTerms"]);
                Fill("current-vehicle-payment", testData["CurrentPayment"]);
                Fill("requested-loan-amount", testData["RequestedLoanAmount"]);
                SelectByText("vehicle-state", testData["VehicleState"]);
            }

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }


        public void CompleteStep3(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 3 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step3"), 5);
                System.Threading.Thread.Sleep(500);
            }

            SelectByText("vehicle-year", testData["VehicleYear"]);
            WaitForAjaxToComplete(5);
            SelectByText("vehicle-make-id", testData["VehicleMake"]);
            WaitForAjaxToComplete(5);
            SelectByText("vehicle-model-id", testData["VehicleModel"]);
            WaitForAjaxToComplete(5);
            SelectByText("vehicle-trim-id", testData["VehicleTrim"]);

            // Refinance OR Lease Buy Out
            if (testData["AutoLoanType"].Equals("UsedCarPurchase", StringComparison.OrdinalIgnoreCase)
                || testData["AutoLoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase)
                || testData["AutoLoanType"].Equals("BUYOUTLEASE", StringComparison.OrdinalIgnoreCase))
            {
                Fill("vehicle-identification-number", testData["VehicleIdNumber"]);
                Fill("vehicle-mileage", testData["VehicleMileage"]);
            }

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep4(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 4 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step4"), 5);
            }

            if (testData["MilitaryServiceYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("isveteranYes");
            }
            else
            {
                ClickRadio("isveteranNo");
            }

            SelectByText("dob-bo-month", testData["DateOfBirthMonth"]);
            SelectByText("dob-bo-day", testData["DateOfBirthDay"]);
            SelectByText("dob-bo-year", testData["DateOfBirthYear"]);

            if (testData["USCitizenYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("USCitizenYes");
            }
            else
            {
                ClickRadio("USCitizenNo");
            }

            SelectByText("stated-credit-history", testData["CreditProfile"]);
            SelectByText("bankruptcy-discharged", testData["BankruptcyHistory"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep5(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 5 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step5"), 5);
                System.Threading.Thread.Sleep(500);
            }

            SelectByText("employment-status", testData["BorrowerEmploymentStatus"]);
            WaitForAjaxToComplete(5);
            //Common.ReportEvent("DEBUG", String.Format
            //        ("employment-status value = {0}.", GetElement("employment-status").GetAttribute("value")));

            // If Employment status is one of the 'working' statuses, then complete these questions.  Otherwise, skip them.
            if ("FULLTIME, PARTTIME, SELFEMPLOYED".Contains(GetElement("employment-status").GetAttribute("value")))
            {
                Fill("job-title", testData["BorrowerJobTitle"]);
                Fill("employer-name", testData["BorrowerEmployerName"]);
                SelectByText("job-time-year", testData["BorrowerEmploymentTimeYears"]);
                SelectByText("job-time-month", testData["BorrowerEmploymentTimeMonths"]);
            }

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep6(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 6 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step6"), 5);
                System.Threading.Thread.Sleep(500);
            }

            Fill("income", testData["BorrowerIncome"]);
            Fill("other-income", testData["BorrowerOtherIncome"]);
            Fill("total-liquid-assets", testData["BorrowerAssets"]);
            SelectByText("bank-account-type", testData["BorrowerAccountType"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep7(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 7 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step7"), 5);
                System.Threading.Thread.Sleep(500);
            }

            Fill("first-name", testData["BorrowerFirstName"]);
            Fill("middle-name", testData["BorrowerMiddleName"]);
            Fill("last-name", testData["BorrowerLastName"]);
            SelectByText("name-suffix", testData["BorrowerSuffix"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep8(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 8 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step8"), 5);
                System.Threading.Thread.Sleep(500);
            }

            Fill("street1", testData["BorrowerStreetAddress"]);
            Fill("zip-code", testData["BorrowerZipCode"]);
            SelectByText("own-or-rent", testData["BorrowerRentOwn"]);
            SelectByText("time-at-address-year", testData["BorrowerYearsAtAddress"]);
            SelectByText("time-at-address-month", testData["BorrowerMonthsAtAddress"]);
            Fill("current-housing-payment", testData["BorrowerHousingPayment"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep9(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 9 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step9"), 5);
                System.Threading.Thread.Sleep(500);
            }

            Fill("home-phone-one", testData["BorrowerHomePhone1"]);
            Fill("home-phone-two", testData["BorrowerHomePhone2"]);
            Fill("home-phone-three", testData["BorrowerHomePhone3"]);
            Fill("work-phone-one", testData["BorrowerWorkPhone1"]);
            Fill("work-phone-two", testData["BorrowerWorkPhone2"]);
            Fill("work-phone-three", testData["BorrowerWorkPhone3"]);
            Fill("work-phone-ext", testData["BorrowerWorkPhoneExt"]);
            FillSSN(testData);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep10(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 10 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step10"), 5);
                System.Threading.Thread.Sleep(500);
            }

            FillEmailAddress(testData);
            Fill("password", testData["Password"]);
            Fill("mothers-maiden-name", testData["MothersMaidenName"]);
            System.Threading.Thread.Sleep(500);

            // If there is a Coborrower, then continue to next step.  Otherwise, submit the form
            if (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ContinueToNextStep();
            }
            else
            {
                SubmitQF();
            }
        }

        public void CompleteStep11(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 11 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step11"), 5);
                System.Threading.Thread.Sleep(500);
            }

            Fill("cb-first-name", testData["CoborrowerFirstName"]);
            Fill("cb-middle-name", testData["CoborrowerMiddleName"]);
            Fill("cb-last-name", testData["CoborrowerLastName"]);
            SelectByText("cb-name-suffix", testData["CoborrowerSuffix"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep12(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 12 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step12"), 5);
                System.Threading.Thread.Sleep(500);
            }

            SelectByText("cb-dob-month", testData["CoDateOfBirthMonth"]);
            SelectByText("cb-dob-day", testData["CoDateOfBirthDay"]);
            SelectByText("cb-dob-year", testData["CoDateOfBirthYear"]);

            if (testData["CoUSCitizenYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("cbUSCitizenYes");
            }
            else
            {
                ClickRadio("cbUSCitizenNo");
            }

            SelectByText("cb-bankruptcy-discharged", testData["CoBankruptcyHistory"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep13(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 13 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step13"), 5);
                System.Threading.Thread.Sleep(500);
            }

            SelectByText("cb-employment-status", testData["CoEmploymentStatus"]);
            WaitForAjaxToComplete(5);
            //Common.ReportEvent("DEBUG", String.Format
            //        ("cb-employment-status value = {0}.", GetElement("cb-employment-status").GetAttribute("value")));

            // If Employment status is one of the 'working' statuses, then complete these questions.  Otherwise, skip them.
            if ("FULLTIME, PARTTIME, SELFEMPLOYED".Contains(GetElement("cb-employment-status").GetAttribute("value")))
            {
                Fill("cb-job-title", testData["CoJobTitle"]);
                Fill("cb-employer-name", testData["CoEmployerName"]);
                SelectByText("cb-job-time-year", testData["CoEmploymentTimeYears"]);
                SelectByText("cb-job-time-month", testData["CoEmploymentTimeMonths"]);
            }

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep14(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 14 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step14"), 5);
                System.Threading.Thread.Sleep(500);
            }

            Fill("cb-income", testData["CoborrowerIncome"]);
            Fill("cb-other-income", testData["CoborrowerOtherIncome"]);
            Fill("cb-total-liquid-assets", testData["CoborrowerAssets"]);
            SelectByText("cb-bank-account-type", testData["CoborrowerAccountType"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep15(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 15 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step15"), 5);
                System.Threading.Thread.Sleep(500);
            }

            if (testData["CoSameAddressYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("cb-same-address-yes");
            }
            else
            {
                ClickRadio("cb-same-address-no");
                Fill("cb-street1", testData["CoStreetAddress"]);
                Fill("cb-zip-code", testData["CoborrowerZipCode"]);
            }

            SelectByText("cb-own-or-rent", testData["CoborrowerRentOwn"]);
            SelectByText("cb-time-at-address-year", testData["CoYearsAtAddress"]);
            SelectByText("cb-time-at-address-month", testData["CoMonthsAtAddress"]);

            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep16(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 16 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step16"), 5);
                System.Threading.Thread.Sleep(500);
            }

            Fill("cb-home-phone-1", testData["CoborrowerHomePhone1"]);
            Fill("cb-home-phone-2", testData["CoborrowerHomePhone2"]);
            Fill("cb-home-phone-3", testData["CoborrowerHomePhone3"]);
            Fill("cb-work-phone-1", testData["CoborrowerWorkPhone1"]);
            Fill("cb-work-phone-2", testData["CoborrowerWorkPhone2"]);
            Fill("cb-work-phone-3", testData["CoborrowerWorkPhone3"]);
            Fill("cb-work-phone-ext", testData["CoborrowerWorkPhoneExt"]);
            Fill("cb-ssn-1", testData["CoborrowerSsn1"]);
            Fill("cb-ssn-2", testData["CoborrowerSsn2"]);
            Fill("cb-ssn-3", testData["CoborrowerSsn3"]);
            Fill("cb-email", testData["CoEmailAddress"]);
            SubmitQF();
        }

        public void FillSSN(Dictionary<string, string> testData)
        {
            this.Fill("social-security-one", testData["BorrowerSsn1"]);
            this.Fill("social-security-two", testData["BorrowerSsn2"]);
            this.Fill("social-security-three", testData["BorrowerSsn3"]);
        }

        public void FillEmailAddress(Dictionary<string, string> testData)
        {
            this.Fill("email", testData["EmailAddress"]);
        }

        public void ContinueToNextStep()
        {
            if (IsElementDisplayed(By.Id("btnContinue")))
            {
                ClickButton("btnContinue");
            }
        }

        public void SubmitQF()
        {
            if (IsElementDisplayed(By.Id("showMeMyResults")))
            {
                System.Threading.Thread.Sleep(500);
                WaitForAjaxToComplete(5);
                Common.ReportEvent(Common.INFO, "***** Submitting auto QF *****");
                ClickButton("showMeMyResults");
            }
            else
            {
                Common.ReportEvent(Common.WARNING, "The 'showMeMyResults' button is NOT displayed on the page. "
                    + " Double-check the actual ID for the button.");
            }
        }

        public void ClickThroughSteps(Int32 intNumSteps)
        {
            // Loop through steps - Verify on expected step, delay for 1 sec, then click Continue/Submit
            for (int i = 1; i <= intNumSteps; i++)
            {
                strStepNum = "step" + i.ToString();
                WaitForElement(By.Id(strStepNum), 10);
                Assert.IsTrue(IsElementPresent(By.Id(strStepNum)), "Unable to verify the script is on Step "
                    + i.ToString() + ".  Cannot locate element with an id of '" + strStepNum + "'.");
                System.Threading.Thread.Sleep(1000);

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


        public void BypassAutoCrossSells()
        {
            // Click the 'next' link on the 3 distinct cross sells
            WaitForElement(By.Id("txtCrossSellZipcode"), 30);  // Auto Insurance cross-sell
            System.Threading.Thread.Sleep(500);
            ClickButton("demoBtn");

            //WaitForElement(By.Id("btnExperianSubmit"), 5);     // Experian Credit Score cross-sell - this one retired.
            WaitForElement(By.CssSelector("[alt='Continue to Free Score']"), 5);  //New Credit Score cross-sell
            System.Threading.Thread.Sleep(500);
            ClickButton("demoBtn");

            WaitForElement(By.ClassName("skip_three"), 5);     // Carfax Report cross-sell
            System.Threading.Thread.Sleep(500);
            ClickButton("demoBtn");

        }
    }
}
