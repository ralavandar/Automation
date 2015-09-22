using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.zzArchive
{
    public class ltchangePage : PageBase
    {
        private readonly IWebDriver ltchange;
        private const String strTid = "lt-change";
        private String strStepNum;
        public Int16 intInputStyle;     //based on 'inputStyle' variation setting (aka - Uniform Controls)

        // Constructor
        public ltchangePage(IWebDriver driver) : base(driver)
        {
            ltchange = driver;
        }

        public void FillOutValidQF(Dictionary<string, string> testData)
        {
            InitializeMortgageVariables(testData);
            NavigateToFossaForm(testData["TestEnvironment"], "QuickMatchFormLoader.aspx", strTid, testData["Variation"]);
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
                WaitForElement(By.ClassName("step1"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step1")), "Unable to verify the script is on Step 1. "
                    + "Cannot locate element of class 'step1'.");
            }

            // Capture/Report the GUID and QFVersion
            strQFormUID = ltchange.FindElement(By.Id("GUID")).Text;
            strQFVersion = ltchange.FindElement(By.Id("QFVersion")).Text;
            Common.ReportEvent(Common.INFO, String.Format("The QForm GUID = {0}", strQFormUID));
            Common.ReportEvent(Common.INFO, String.Format("The QF version under test = {0}", strQFVersion));

            // Fill out the fields & click Continue
            SelectByValue("LOAN_TYPE", testData["LoanType"]);
            SelectByText("ctl00_Body_ctl00_ddStructure", testData["HomeDescription"]);

            System.Threading.Thread.Sleep(500);
            // Note: By design, autoAdvance does not function here, so we must click Continue.
            ContinueToNextStep();
        }


        public void CompleteStep2(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 2 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step2"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step2")), "Unable to verify the script is on Step 2. "
                    + "Cannot locate element of class 'step2'.");
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                if (testData["FoundNewHomeYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("rFundNewHouseYes");
                }
                else
                {
                    ClickRadio("rFundNewHouseNo");
                }
            }

            // Fill out Refinance-specific fields
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                Fill("ctl00_Body_ctl00_txtZip", testData["PropertyZipCode"]);
                //RecordScreenshot("Step2_Submit");
                ContinueToNextStep();
            }
            System.Threading.Thread.Sleep(500);
        }


        public void CompleteStep3(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 3 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step3"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step3")), "Unable to verify the script is on Step 3. "
                    + "Cannot locate element of class 'step3'.");
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                SelectElement objSelect = new SelectElement(ltchange.
                    FindElement(By.Id("ctl00_Body_ctl00_ddlCityProperty")));
                
                SelectByText("ctl00_Body_ctl00_ddlStateProperty", testData["PropertyState"]);

                // Wait for number of options to be greater than 2, then proceed
                WaitForAjaxToComplete(5);
                System.Threading.Thread.Sleep(500);
                WaitForDropdownRefresh("ctl00_Body_ctl00_ddlCityProperty", 5);
                SelectByText("ctl00_Body_ctl00_ddlCityProperty", testData["PropertyCity"]);
            }

            // Fill out Refinance-specific fields
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("ddHomeValue", testData["RefiPropertyValue"]);
            }
            System.Threading.Thread.Sleep(500);
        }

        public void CompleteStep4(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 4 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step4"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step4")), "Unable to verify the script is on Step 4. "
                    + "Cannot locate element of class 'step4'.");
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("ctl00_Body_ctl00_ddProperty", testData["PropertyUseType"]);

                if (IsElementDisplayed(By.Id("rCurrentREAgentYes")))
                {
                    if (testData["CurrentREAgentYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        ClickRadio("rCurrentREAgentYes");
                    }
                    else
                    {
                        ClickRadio("rCurrentREAgentNo");
                    }
                    // Note: the form will auto advance after selecting yes or no for CurrentREAgent
                }
                else
                {
                    // Note: By design, autoAdvance does not function here for Purchase, so we must click Continue.
                    ContinueToNextStep();
                }
            }

            // Fill out Refinance-specific fields
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("ctl00_Body_ctl00_ddProperty", testData["PropertyUseType"]);
                SelectByText("ctl00_Body_ctl00_ddHousing", testData["FirstMortgagePayment"]);
            }

            System.Threading.Thread.Sleep(500);
        }

        public void CompleteStep5(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 5 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step5"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step5")), "Unable to verify the script is on Step 5. "
                    + "Cannot locate element of class 'step5'.");
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("ddEstimated", testData["PurchasePrice"]);
            }

            // Fill out Refinance-specific fields
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("ctl00_Body_ctl00_ddRate", testData["FirstMortgageRate"]);
                SelectByText("ddBalance", testData["FirstMortgageBalance"]);
            }
            System.Threading.Thread.Sleep(500);
        }

        public void CompleteStep6(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 6 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step6"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step6")), "Unable to verify the script is on Step 6. "
                    + "Cannot locate element of class 'step6'.");
            }

            // Fill out Purchase-specific fields
            if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("ddDownPayment", testData["PurchaseDownPayment"]);

                if (testData["RealtorConsultYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("rConsultationYes");
                }
                else
                {
                    ClickRadio("rConsultationNo");
                }

                // Note: autoAdvance moves the user to the next step for Purchase
            }

            // Fill out Refinance-specific fields and click Continue
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                if (testData["SecondMortgageYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("rSecondMortgageYes");

                    // Explicit wait to ensure the Browser displays the 2nd mortgage questions
                    if (intInputStyle == 1) //uniform controls
                    {
                        WaitForElementDisplayed(By.Id("uniform-ctl00_Body_ctl00_ddHousing2"), 5);
                    }
                    else
                    {
                        WaitForElementDisplayed(By.Id("ctl00_Body_ctl00_ddHousing2"), 5);
                    }

                    SelectByText("ctl00_Body_ctl00_ddHousing2", testData["SecondMortgagePayment"]);
                    SelectByText("ddBalance2", testData["SecondMortgageBalance"]);
                    SelectByText("ctl00_Body_ctl00_ddRate2", testData["SecondMortgageRate"]);
                }
                else
                {
                    ClickRadio("rSecondMortgageNo");
                }

                SelectByText("ddAdditional", testData["RefiCashoutAmount"]);
                // Note: By design, autoAdvance does not function here for Refi, so we must click Continue.
                ContinueToNextStep();
            }

            System.Threading.Thread.Sleep(500);
        }

        public void CompleteStep7(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 7 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step7"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step7")), "Unable to verify the script is on Step 7. "
                    + "Cannot locate element of class 'step7'.");
            }

            SelectByText("ctl00_Body_ctl00_ddCredit", testData["CreditProfile"]);
            SelectByText("ddDOBMonth", testData["DateOfBirthMonth"]);
            SelectByText("ddDOBDay", testData["DateOfBirthDay"]);
            SelectByText("ddDOBYear", testData["DateOfBirthYear"]);
            System.Threading.Thread.Sleep(500);
        }

        public void CompleteStep8(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 8 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step8"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step8")), "Unable to verify the script is on Step 8. "
                    + "Cannot locate element of class 'step8'.");
            }

            // Handle 'HomeServices' variation
            if (IsElementDisplayed(By.Id("rHomeServiceOptinYes")))
            {
                if (testData["HomeServicesOptInYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("rHomeServiceOptinYes");
                }
                else
                {
                    ClickRadio("rHomeServiceOptinNo");
                }
            }
            System.Threading.Thread.Sleep(5000);

            if (testData["MilitaryServiceYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("rMilitaryYes");
            }
            else
            {
                ClickRadio("rMilitaryNo");
            }
            System.Threading.Thread.Sleep(500);
        }

        public void CompleteStep9(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 9 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step9"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step9")), "Unable to verify the script is on Step 9. "
                    + "Cannot locate element of class 'step9'.");
            }

            SelectByText("ddForeclosureText", testData["ForeclosureHistory"]);

            // Handle 'mortInsurance' variation
            if (IsElementDisplayed(By.Id("rMortgageInsuranceOptIn")))
            {
                if (testData["MortgageInsuranceYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("rMortgageInsuranceOptIn");
                }
                else
                {
                    ClickRadio("rMortgageInsuranceOptOut");
                }
            }

            // Handle 'debtConsolidate' variation
            if (IsElementDisplayed(By.Id("ddcreditcarddebt")))
            {
                SelectByText("ddcreditcarddebt", testData["CreditCardDebtAmount"]);
                System.Threading.Thread.Sleep(1000);

                // The following question displays if credit card debt > 10,000 ('debtConsolidate' variation)
                if (IsElementDisplayed(By.Id("rDebtConsultationOptIn")))
                {
                    if (testData["DebtConsultYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        ClickRadio("rDebtConsultationOptIn");
                    }
                    else
                    {
                        ClickRadio("rDebtConsultationOptOut");
                    }
                }
            }

            System.Threading.Thread.Sleep(500);
            if (testData["BankruptcyYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("rBankruptcyYes");

                if (intInputStyle == 1) //uniform controls
                {
                    WaitForElementDisplayed(By.Id("uniform-ctl00_Body_ctl00_ddBankruptcy"), 5);
                }
                else
                {
                    WaitForElementDisplayed(By.Id("ctl00_Body_ctl00_ddBankruptcy"), 5);
                }
                
                SelectByText("ctl00_Body_ctl00_ddBankruptcy", testData["BankruptcyHistory"]);
            }
            else
            {
                ClickRadio("rBankruptcyNo");
            }
            System.Threading.Thread.Sleep(500);
        }

        public void CompleteStep10(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 10 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step10"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step10")), "Unable to verify the script is on Step 10. "
                    + "Cannot locate element of class 'step10'.");
            }

            Fill("ctl00_Body_ctl00_txtFirstName", testData["BorrowerFirstName"]);
            Fill("ctl00_Body_ctl00_txtLastName", testData["BorrowerLastName"]);

            if (IsElementDisplayed(By.Id("rLendingTreeOptInYes")))
            {
                if (testData["LTLOptInYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("rLendingTreeOptInYes");
                }
                else
                {
                    ClickRadio("rLendingTreeOptInNo");
                }
            }
            System.Threading.Thread.Sleep(500);
            ContinueToNextStep();
        }

        public void CompleteStep11(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 11 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step11"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step11")), "Unable to verify the script is on Step 11. "
                    + "Cannot locate element of class 'step11'.");
            }

            Fill("ctl00_Body_ctl00_txtStreet", testData["BorrowerStreetAddress"]);

            // If Zip field is displayed, then fill it out
            if (IsElementDisplayed(By.Id("ctl00_Body_ctl00_txtContactZip")))
            {
                Fill("ctl00_Body_ctl00_txtContactZip", testData["BorrowerZipCode"]);
            }

            System.Threading.Thread.Sleep(500);
            //RecordScreenshot("Step11_Submit");
            ContinueToNextStep();
        }

        public void CompleteStep12(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 12 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step12"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step12")), "Unable to verify the script is on Step 12. "
                    + "Cannot locate element of class 'step12'.");
            }

            Fill("ctl00_Body_ctl00_txtHomePhone1", testData["BorrowerHomePhone1"]);
            Fill("ctl00_Body_ctl00_txtHomePhone2", testData["BorrowerHomePhone2"]);
            Fill("ctl00_Body_ctl00_txtHomePhone3", testData["BorrowerHomePhone3"]);
            Fill("ctl00_Body_ctl00_txtSsn1", testData["BorrowerSsn1"]);
            Fill("ctl00_Body_ctl00_txtSsn2", testData["BorrowerSsn2"]);
            Fill("ctl00_Body_ctl00_txtSsn3", testData["BorrowerSsn3"]);

            // If Work/Alternate Phone fields are displayed, then fill them out
            if (IsElementDisplayed(By.Id("ctl00_Body_ctl00_txtWorkPhone1")))
            {
                Fill("ctl00_Body_ctl00_txtWorkPhone1", testData["BorrowerWorkPhone1"]);
                Fill("ctl00_Body_ctl00_txtWorkPhone2", testData["BorrowerWorkPhone2"]);
                Fill("ctl00_Body_ctl00_txtWorkPhone3", testData["BorrowerWorkPhone3"]);
                Fill("ctl00_Body_ctl00_txtWorkPhoneExt", testData["BorrowerWorkPhoneExt"]);
            }

            System.Threading.Thread.Sleep(500);
            //RecordScreenshot("Step12_Submit");
            ContinueToNextStep();
        }

        public void CompleteStep13(Dictionary<string, string> testData, Boolean blnValidateHeader)
        {
            Common.ReportEvent(Common.INFO, "***** Starting Step 13 *****");
            if (blnValidateHeader)
            {
                WaitForElement(By.ClassName("step13"), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName("step13")), "Unable to verify the script is on Step 13. "
                    + "Cannot locate element of class 'step13'.");
            }

            Fill("txtEmail", testData["EmailAddress"]);

            // Handle emailOptin variation
            if (IsElementDisplayed(By.Id("rEmailOptinYes")))
            {
                if (testData["EmailOptInYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("rEmailOptinYes");
                }
                else
                {
                    ClickRadio("rEmailOptinNo");
                }
            }

            Fill("txtPassword1", testData["Password"]);

            System.Threading.Thread.Sleep(2000);
            SubmitQF();
        }

        public void FillSSN(Dictionary<string, string> testData)
        {
            this.Fill("ctl00_Body_ctl00_txtSsn1", testData["BorrowerSsn1"]);
            this.Fill("ctl00_Body_ctl00_txtSsn2", testData["BorrowerSsn2"]);
            this.Fill("ctl00_Body_ctl00_txtSsn3", testData["BorrowerSsn3"]);
        }

        public void FillEmailAddress(Dictionary<string, string> testData)
        {
            this.Fill("txtEmail", testData["EmailAddress"]);
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
                // In some cases, need to set focus on on the button prior to attempting the 'Click' method.
                // Otherwise it doesn't work.  So doing that here by sending the Tab key.
                ltchange.FindElement(By.Id("showMeMyResults")).SendKeys(Keys.Tab);
                System.Threading.Thread.Sleep(500);
                Common.ReportEvent(Common.INFO, "***** Submitting lt-change QF *****");
                ClickButton("showMeMyResults");
            }
            else if (IsElementDisplayed(By.Id("btn_acceptAndContinue")))
            {
                // In some cases, need to set focus on on the button prior to attempting the 'Click' method.
                // Otherwise it doesn't work.  So doing that here by sending the Tab key.
                ltchange.FindElement(By.Id("btn_acceptAndContinue")).SendKeys(Keys.Tab);
                System.Threading.Thread.Sleep(500);
                Common.ReportEvent(Common.INFO, "***** Submitting lt-change QF *****");
                ClickButton("btn_acceptAndContinue");
            }
            else
            {
                Common.ReportEvent(Common.WARNING, "Could not find a 'showMeMyResults' button or a 'btn_acceptAndContinue' button on the page. "
                    + " Double-check the actual ID for the button and try again.");
            }
        }

        public void ClickThroughSteps(Int32 intNumSteps)
        {
            // Loop through steps - Verify on expected step, delay for 1 sec, then click Continue/Submit
            for (int i = 1; i <= intNumSteps; i++)
            {
                strStepNum = "step" + i.ToString();
                WaitForElement(By.ClassName(strStepNum), 10);
                Assert.IsTrue(IsElementPresent(By.ClassName(strStepNum)), "Unable to verify the script is on Step "
                    + i.ToString() + ".  Cannot locate element of class '" + strStepNum + "'.");
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
            // Set intInputStyle based on 'inputStyle' variation (index 23 - Uniform Controls)
            if ((testData["Variation"].Length > 11) && (testData["Variation"][12] == '0'))
            {
                Common.ReportEvent(Common.INFO, "The 'inputStyle' (i.e. Uniform Controls) variation is turned OFF (0) for this test.");
                intInputStyle = 0;
            }
            else if ((testData["Variation"].Length > 11) && (testData["Variation"][12] == '1'))
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
    
    }
}
