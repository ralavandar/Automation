using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.zzArchive
{
    /// <summary>
    /// Note: The 'mortgagePage' class is used for both the mortgage and gs-mortgage templates
    /// </summary>
    public class mortgagePage : PageBase
    {
        private readonly IWebDriver mortgageDriver;
        private String strStepNum;
        public Int16 intAutoAdvance;    //based on 'autoAdvance' variation setting
        public Int16 intInputStyle;     //based on 'inputStyle' variation setting (aka - Uniform Controls)

        // Constructor
        public mortgagePage(IWebDriver driver) : base(driver)
        {
            mortgageDriver = driver;
        }

        public void FillOutValidQF(Dictionary<string, string> testData)
        {
            InitializeMortgageVariables(testData);
            NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);
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
            System.Threading.Thread.Sleep(2000);
            CompleteStep13(testData, false);
        }


        public void CompleteStep1(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 1 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-1"), 5);
                // Having all kinds of timing issues despite the above Wait statements.
                System.Threading.Thread.Sleep(1000);
            }

            // Capture/Report the GUID and QFVersion
            strQFormUID = mortgageDriver.FindElement(By.Id("GUID")).Text;
            Common.ReportEvent(Common.INFO, String.Format("The QForm GUID = {0}", strQFormUID));

            // Fill out the fields & click Continue
            SelectByValue("homeloan-product-type", testData["LoanType"]);
            SelectByText("property-type", testData["HomeDescription"]);

            //DEBUG - having some sort of timing issue when I run this on the VMs.  The form does not advance to step-2 in certain cases.
            System.Threading.Thread.Sleep(1000);

            // if auto advance is OFF, then need to click 'Continue'...
            // OR if "Single family home" is selected, then auto-advance won't fire, so we need to click 'Continue'...
            if ((intAutoAdvance == 0) || (testData["HomeDescription"].Equals("Single family home")))
            {
                ContinueToNextStep();
            }
        }


        public void CompleteStep2(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 2 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-2"), 5);
                System.Threading.Thread.Sleep(500);
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                if (testData["FoundNewHomeYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("new-home-yes");
                }
                else
                {
                    ClickRadio("new-home-no");
                }

                if (intAutoAdvance == 0)
                {
                    ContinueToNextStep();
                }
            }

            // Fill out Refinance-specific fields
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                Fill("property-zip", testData["PropertyZipCode"]);
                ContinueToNextStep();
            }
        }


        public void CompleteStep3(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 3 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-3"), 5);
                System.Threading.Thread.Sleep(500);
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                SelectElement objSelect = new SelectElement(mortgageDriver.
                    FindElement(By.Id("property-city")));

                SelectByText("property-state", testData["PropertyState"]);

                // Wait for number of options to be greater than 2, then proceed
                //WaitForDropdownRefresh("property-city", 5);
                WaitForAjaxToComplete(10);
                System.Threading.Thread.Sleep(500);
                SelectByText("property-city", testData["PropertyCity"]);
            }

            // Fill out Refinance-specific fields
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("estproperty-value", testData["RefiPropertyValue"]);
            }

            if (intAutoAdvance == 0)
            {
                ContinueToNextStep();
            }
        }

        public void CompleteStep4(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 4 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-4"), 5);
                WaitForElementDisplayed(By.ClassName("step-4"), 5);
                System.Threading.Thread.Sleep(500);
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("property-use", testData["PropertyUseType"]);
                if (IsElementDisplayed(By.Id("current-realestate-agent_yes")))
                {
                    // click radio button, then process autoAdvance
                    if (testData["CurrentREAgentYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        ClickRadio("current-realestate-agent_yes");
                    }
                    else
                    {
                        ClickRadio("current-realestate-agent_no");
                    }

                    // if autoAdvance is turned OFF, then we need to click 'Continue'
                    if (intAutoAdvance == 0)
                    {
                        ContinueToNextStep();
                    }
                }
                else
                {
                    // process really ugly autoAdvance scenario: basically, another case where 'autoAdvance' does not
                    // advance the user when trying to select the default choice - in this case "Primary residence".
                    // if autoAdvance is turned OFF, then click 'Continue'.  Also covers the case where autoAdvance
                    // is ON, but the current-realestate-agent question is not shown, and we're selecting the default
                    // answer, which does not trigger autoAdvance.
                    if ((intAutoAdvance == 0) || (testData["PropertyUseType"].Contains("Primary")))
                    {
                        ContinueToNextStep();
                    }
                }
            }

            // Fill out Refinance-specific fields
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("property-use", testData["PropertyUseType"]);
                SelectByText("current-monthly-payment", testData["FirstMortgagePayment"]);

                if (intAutoAdvance == 0)
                {
                    ContinueToNextStep();
                }
            }
        }

        public void CompleteStep5(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 5 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-5"), 5);
                WaitForElementDisplayed(By.ClassName("step-5"), 5);
                System.Threading.Thread.Sleep(500);
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                //The select is firing before auto-advance is ready...not sure what else I can do but sleep for 1 sec
                System.Threading.Thread.Sleep(1000);
                SelectByText("purchase-price", testData["PurchasePrice"]);
                if (intAutoAdvance == 0)
                {
                    ContinueToNextStep();
                }
            }

            // Fill out Refinance-specific fields
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("1st-mortgage-interest-rate", testData["FirstMortgageRate"]);

                // Need to determine if est-mortgage-balance is a dropdown or textbox
                if ((IsElementDisplayed(By.Id("est-mortgage-balance"))) || (IsElementDisplayed(By.Id("uniform-est-mortgage-balance")))) // dropdown
                {
                    SelectByText("est-mortgage-balance", testData["FirstMortgageBalance"]);
                    if (intAutoAdvance == 0)
                    {
                        ContinueToNextStep();
                    }
                }
                else // must be a textbox
                {
                    Fill("est-mortgage-balance-txt", testData["FirstMortgageBalance"]);
                    ContinueToNextStep();
                }
            }
        }

        public void CompleteStep6(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 6 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-6"), 5);
                WaitForElementDisplayed(By.ClassName("step-6"), 5);
                System.Threading.Thread.Sleep(500);
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("down-payment-amt", testData["PurchaseDownPayment"]);

                if (testData["RealtorConsultYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("inline_realtor_optin-yes");
                }
                else
                {
                    ClickRadio("inline_realtor_optin-no");
                }

                if (intAutoAdvance == 0)
                {
                    ContinueToNextStep();
                }
            }

            // Fill out Refinance-specific fields and click Continue
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                if (testData["SecondMortgageYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("second-mortgage-yes");
                    // Explicit wait to ensure the Browser displays the 2nd mortgage questions
                    if (intInputStyle == 1) //uniform controls
                    {
                        WaitForElementDisplayed(By.Id("uniform-2nd-mortgage-monthly-payment"), 5);
                    }
                    else
                    {
                        WaitForElementDisplayed(By.Id("2nd-mortgage-monthly-payment"), 5);
                    }

                    SelectByText("2nd-mortgage-monthly-payment", testData["SecondMortgagePayment"]);

                    // Need to determine if second-mortgage-balance is a dropdown or textbox
                    if ((IsElementDisplayed(By.Id("second-mortgage-balance"))) || (IsElementDisplayed(By.Id("uniform-second-mortgage-balance"))))// dropdown
                        SelectByText("second-mortgage-balance", testData["SecondMortgageBalance"]);
                    else
                        Fill("second-mortgage-balance-txt", testData["SecondMortgageBalance"]);

                    SelectByText("2nd-mortgage-interest-rate", testData["SecondMortgageRate"]);
                }
                else
                {
                    ClickRadio("second-mortgage-no");
                }
                
                SelectByText("cash-out", testData["RefiCashoutAmount"]);
                // Note: By design, autoAdvance does not function here for Refi, so we always click Continue (05/21/2012).
                ContinueToNextStep();
            }
        }

        public void CompleteStep7(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 7 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-7"), 5);
                WaitForElementDisplayed(By.ClassName("step-7"), 5);
                System.Threading.Thread.Sleep(500);
            }

            SelectByText("stated-credit-history", testData["CreditProfile"]);
            SelectByText("birth-month", testData["DateOfBirthMonth"]);
            SelectByText("birth-day", testData["DateOfBirthDay"]);
            SelectByText("birth-year", testData["DateOfBirthYear"]);

            if (intAutoAdvance == 0)
            {
                ContinueToNextStep();
            }
        }

        public void CompleteStep8(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 8 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-8"), 5);
                WaitForElementDisplayed(By.ClassName("step-8"), 5);
                System.Threading.Thread.Sleep(500);
            }

            // TODO: handle HomeServicesCategory drop-down and add data field to db!
            // Handle 'HomeServices' variation
            if (IsElementDisplayed(By.Id("homeservice-optin-yes")))
            {
                if (testData["HomeServicesOptInYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("homeservice-optin-yes");
                    //System.Threading.Thread.Sleep(500);
                    //if (IsElementDisplayed(By.Id("home-service-category")))
                    //{
                    //    SelectByText("home-service-category", testData["HomeServicesCategory"]);
                    //}
                }
                else
                {
                    ClickRadio("homeservice-optin-no");
                }
            }
            //System.Threading.Thread.Sleep(5000);

            if (testData["MilitaryServiceYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("is-veteran-yes");

                // If Refi, then wait for the IsCurrentLoanVA question to display and fill it out
                if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
                {
                    WaitForElementDisplayed(By.Id("current-loan-va-no"), 5);
                    if (testData["CurrentLoanVAYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        ClickRadio("current-loan-va-yes");
                    }
                    else
                    {
                        ClickRadio("current-loan-va-no");
                    }
                }
            }
            else
            {
                ClickRadio("is-veteran-no");
            }

            if (intAutoAdvance == 0)
            {
                ContinueToNextStep();
            }
        }

        public void CompleteStep9(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 9 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-9"), 5);
                WaitForElementDisplayed(By.ClassName("step-9"), 5);
                System.Threading.Thread.Sleep(500);
            }

            SelectByText("foreclosure-text", testData["ForeclosureHistory"]);

            // Handle 'mortInsurance' variation
            if (IsElementDisplayed(By.Id("mortgage-ins-yes")))
            {
                if (testData["MortgageInsuranceYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("mortgage-ins-yes");
                }
                else
                {
                    ClickRadio("mortgage-ins-no");
                }
            }

            // Handle 'debtConsolidate' variation
            if (IsElementDisplayed(By.Id("ddcreditcarddebt")) || IsElementDisplayed(By.Id("uniform-ddcreditcarddebt")))
            {
                SelectByText("ddcreditcarddebt", testData["CreditCardDebtAmount"]);
                System.Threading.Thread.Sleep(1000);

                // The following question displays if credit card debt > 10,000 ('debtConsolidate' variation)
                if (IsElementDisplayed(By.Id("debt-consultation-yes")))
                {
                    if (testData["DebtConsultYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        ClickRadio("debt-consultation-yes");
                    }
                    else
                    {
                        ClickRadio("debt-consultation-no");
                    }
                }
            }

            System.Threading.Thread.Sleep(500);
            if (testData["BankruptcyYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("declared-bankruptcy-yes");
                if (intInputStyle == 1) //uniform controls
                {
                    WaitForElementDisplayed(By.Id("uniform-bankruptcy-discharged"), 5);
                }
                else
                {
                    WaitForElementDisplayed(By.Id("bankruptcy-discharged"), 5);
                }
                SelectByText("bankruptcy-discharged", testData["BankruptcyHistory"]);

                // This is a case where autoAdvance does not advance by design...
                // So if there is no edu opt-in question displayed, then click 'Continue'
                if (IsElementDisplayed(By.Id("edu-optin-yes")) == false)
                {
                    ContinueToNextStep();
                }
            }
            else
            {
                ClickRadio("declared-bankruptcy-no");
                // if autoAdvance is OFF and the edu opt-in is not displayed, then click Continue
                if ((intAutoAdvance == 0) && (IsElementDisplayed(By.Id("edu-optin-yes")) == false))
                {
                    ContinueToNextStep();
                }
            }
        }

        public void CompleteStep10(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 10 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-10"), 5);
                WaitForElementDisplayed(By.ClassName("step-10"), 5);
                System.Threading.Thread.Sleep(500);
            }

            Fill("first-name", testData["BorrowerFirstName"]);
            Fill("last-name", testData["BorrowerLastName"]);

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
                if (intAutoAdvance == 0)
                {
                    ContinueToNextStep();
                }
            }
            else
            {
                ContinueToNextStep();
            }
        }

        public void CompleteStep11(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 11 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-11"), 5);
                WaitForElementDisplayed(By.ClassName("step-11"), 5);
                System.Threading.Thread.Sleep(500);
            }

            Fill("street1", testData["BorrowerStreetAddress"]);

            // If ZIP field is displayed, then fill it out
            if (IsElementDisplayed(By.Id("zip-code")))
            {
                Fill("zip-code", testData["BorrowerZipCode"]);
            }

            ContinueToNextStep();
        }

        public void CompleteStep12(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 12 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-12"), 5);
                WaitForElementDisplayed(By.ClassName("step-12"), 5);
                System.Threading.Thread.Sleep(500);
            }

            FillHomePhone(testData);
            FillWorkPhone(testData);
            FillSSN(testData);
            ContinueToNextStep();
        }

        public void CompleteStep13(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 13 *****");
            if (blnValidateHeader)
            {
                WaitForAjaxToComplete(10);
                WaitForElementDisplayed(By.Id("step-13"), 5);
                WaitForElementDisplayed(By.ClassName("step-13"), 5);
                System.Threading.Thread.Sleep(500);
            }

            FillEmailAddress(testData);

            // Handle emailOptin variation
            if (IsElementDisplayed(By.Id("email-optin-yes")))
            {
                if (testData["EmailOptInYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("email-optin-yes");
                }
                else
                {
                    ClickRadio("email-optin-no");
                }
            }

            Fill("password", testData["Password"]);
            SubmitQF();
        }

        public void FillSSN(Dictionary<string, string> testData)
        {
            if (this.IsElementDisplayed(By.Id("social-security-one")))   // 3 fields
            {
                this.Fill("social-security-one", testData["BorrowerSsn1"]);
                this.Fill("social-security-two", testData["BorrowerSsn2"]);
                this.Fill("social-security-three", testData["BorrowerSsn3"]);
            }

            if (this.IsElementDisplayed(By.Id("ssn"))) // 1 field
            {
                this.Fill("ssn", testData["BorrowerSsn1"] + testData["BorrowerSsn2"] + testData["BorrowerSsn3"]);
            }
        }

        public void FillHomePhone(Dictionary<string, string> testData)
        {
            if (this.IsElementDisplayed(By.Id("home-phone-one"))) // 3 fields
            {
                this.Fill("home-phone-one", testData["BorrowerHomePhone1"]);
                this.Fill("home-phone-two", testData["BorrowerHomePhone2"]);
                this.Fill("home-phone-three", testData["BorrowerHomePhone3"]);
            }

            if (this.IsElementDisplayed(By.Id("home-phone"))) // 1 field
            {
                this.Fill("home-phone", testData["BorrowerHomePhone1"] + testData["BorrowerHomePhone2"] + testData["BorrowerHomePhone3"]);
            }
        }

        public void FillHomePhone(string phone1, string phone2, string phone3)
        {
            if (this.IsElementDisplayed(By.Id("home-phone-one"))) // 3 fields
            {
                this.Fill("home-phone-one", phone1);
                this.Fill("home-phone-two", phone2);
                this.Fill("home-phone-three", phone3);
            }

            if (this.IsElementDisplayed(By.Id("home-phone"))) // 1 field
            {
                this.Fill("home-phone", phone1 + phone2 + phone3);
            }
        }

        public void FillWorkPhone(Dictionary<string, string> testData)
        {
            if (this.IsElementDisplayed(By.Id("work-phone-one"))) // 3 fields
            {
                Fill("work-phone-one", testData["BorrowerWorkPhone1"]);
                Fill("work-phone-two", testData["BorrowerWorkPhone2"]);
                Fill("work-phone-three", testData["BorrowerWorkPhone3"]);
                Fill("work-phone-ext", testData["BorrowerWorkPhoneExt"]);
            }

            if (this.IsElementDisplayed(By.Id("work-phone"))) // 1 field
            {
                this.Fill("work-phone", testData["BorrowerWorkPhone1"] + testData["BorrowerWorkPhone2"] + testData["BorrowerWorkPhone3"]);
            }
        }

        public void FillEmailAddress(Dictionary<string, string> testData)
        {
            this.GetElement("email").Clear();
            this.Fill("email", testData["EmailAddress"]);
        }

        public void ContinueToNextStep()
        {
            System.Threading.Thread.Sleep(500);
            WaitForAjaxToComplete(5);

            if (IsElementDisplayed(By.Id("next")))
            {
                Common.ReportEvent("DEBUG", "Clicking the 'next' button");
                ClickButton("next");
            }
        }

        public void SubmitQF()
        {
            System.Threading.Thread.Sleep(500);
            WaitForAjaxToComplete(5);

            if (IsElementDisplayed(By.Id("next")))
            {
                // In some cases, need to set focus on the button prior to attempting the 'Click' method.
                // Otherwise it doesn't work.  So doing that here by sending the Tab key.
                //mortgageDriver.FindElement(By.Id("next")).SendKeys(Keys.Tab);
                //System.Threading.Thread.Sleep(500);
                Common.ReportEvent(Common.INFO, "***** Submitting mortgage QF *****");
                ClickButton("next");
            }
            else
            {
                Common.ReportEvent(Common.WARNING, "The 'next' button is NOT displayed on the page. "
                    + " Double-check the actual ID for the button.");
            }
        }

        public void ClickThroughSteps(Int32 intNumSteps)
        {
            // Loop through steps - Verify on expected step, delay for 1 sec, then click Continue/Submit
            for (int i = 1; i <= intNumSteps; i++)
            {
                strStepNum = "step-" + i.ToString();
                WaitForElementDisplayed(By.Id(strStepNum), 10);
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
        

        public void InitializeMortgageVariables(Dictionary<string, string> testData)
        {
            // Set intAutoAdvance based on 'autoAdvance' variation
            if ((testData["Variation"].Length > 21) && (testData["Variation"][22] == '0'))
            {
                Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is turned OFF (0) for this test.");
                intAutoAdvance = 0;
            }
            else if ((testData["Variation"].Length > 21) && (testData["Variation"][22] == '1'))
            {
                Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is turned ON (1) for this test.");
                intAutoAdvance = 1;
            }
            else
            {
                Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is ON by default for this test.");
                intAutoAdvance = 1;     //default value
            }

            // Set intInputStyle based on 'inputStyle' variation (index 23 - Uniform Controls)
            if ((testData["Variation"].Length > 45) && (testData["Variation"][46] == '0'))
            {
                Common.ReportEvent(Common.INFO, "The 'inputStyle' (i.e. Uniform Controls) variation is turned OFF (0) for this test.");
                intInputStyle = 0;
            }
            else if ((testData["Variation"].Length > 45) && (testData["Variation"][46] == '1'))
            {
                Common.ReportEvent(Common.INFO, "The 'inputStyle' (i.e. Uniform Controls) variation is turned ON (1) for this test.");
                intInputStyle = 1;
            }
            else
            {
                Common.ReportEvent(Common.INFO, "The 'inputStyle' (i.e. Uniform Controls) variation is OFF by default for this test.");
                intInputStyle = 0;     //default value
            }
        }

        public int GetErrorLabelCount()
        {
            int errorLabelCount = 0;
            var elements = mortgageDriver.FindElements(By.ClassName("error"));

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].GetAttribute("tagName").ToUpper().Equals("LABEL"))
                {
                    Common.ReportEvent(Common.INFO, "The error label is: " + elements[i].Text);
                    errorLabelCount++;
                }
            }

            return errorLabelCount;
        }
    }
}
