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
    /// Note: The 'gsguyPage' class is based on the mortgagePage class.
    /// </summary>
    public class gsguyPage : PageBase
    {
        

        private const uint VARIATION_AUTOADVANCE = 3;

        private const int WAIT_TIME_ANIMATE = 2; //s
        private const int WAIT_TIME_AJAX = 10; //s
        private const int WAIT_TIME_STEP_ADVANCE = 5; //s

        //TODO: This delay before ending or beginning a step prevents certain mysterious issues like IE not hitting radio buttons. Increasing it might help if those issues resurface.
        private const int WAIT_TIME_INPUT_DELAY = 1000; //ms

        //TODO: Move testData and blnValidateHeader arguments into public properties.

        //TODO: Let's remove Hungarian notation for variables where the type makes such a thing unnecessary.

        protected enum LoanType { Refinance, Purchase, HomeEquity, AutoPurchase, AutoRefinance, Personal }

        protected uint currStep;

        public Dictionary<string, string> testData;

        //TODO: Let's make these private booleans, since there's no need for the code to know what the numeric values mean.
        //public Int16 intAutoAdvance;    //based on 'autoAdvance' variation setting

        private bool autoAdvanceReported = false;
        public bool ShouldAutoAdvance
        {
            get
            {
                var autoAdvanceVariation = GetVariation(VARIATION_AUTOADVANCE);
                if (autoAdvanceVariation == 0)
                {
                    if (!autoAdvanceReported) Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is turned OFF (0) for this test.");
                    autoAdvanceReported = true;
                    return false;
                }
                else if (autoAdvanceVariation == 1)
                {
                    if (!autoAdvanceReported) Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is turned ON (1) for this test.");
                    autoAdvanceReported = true;
                    return true;
                }
                else
                {
                    if (!autoAdvanceReported) Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is ON by default for this test.");
                    autoAdvanceReported = true;
                    return true;     //default value
                }
            }
        }

        

        public gsguyPage(IWebDriver driver, Dictionary<string, string> testData)
            : base(driver)
        {
            this.testData = testData;
        }

        private uint? GetVariation(uint variationIndex)
        {
            string VID = testData["Variation"];

            //If the VID isn't long enough, return null to indicate the default value.
            if(VID.Length < (2 * variationIndex) + 1) return null;

            return uint.Parse(VID.Substring((int) (2 * variationIndex), 1));
        }

        //public Int16 intInputStyle;     //based on 'inputStyle' variation setting (aka - Uniform Controls)

       

        private bool didLastFieldTriggerAdvance;

        //TODO: Step1, Step2, Step3 as functions is very rigid. Let's break out functions by purpose and figure out a more flexible way to define a default sequence.



        #region Mort-Tree QF Process - Possibly put abstract versions of this in FossaPageBase

        private IFormField[][] ValidQFStepsGeneric
        {
            get
            {
                //Needs to be one larger than the number of steps, since step numbers are one-indexed instead of zero-indexed.
                IFormField[][] Steps = new IFormField[5][]; 

                Steps[1]  = Step(
                                new FossaField(SelectByValue, "homeloan-product-type", "LoanType"));
                
                Steps[4]  = Step(
                                new FossaField(ClickRadioYesNo, "is-veteran-{0}", "MilitaryServiceYesNo"),
                                new FossaField(IfOtherClassDisplayed("vet-yes", ClickRadioYesNo), "current-loan-va-{0}", "CurrentLoanVAYesNo"),
                                new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(Fill, "last-name", "BorrowerLastName"),
                                new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                //TODO: I force a blur here because Selenium refuses to fire the proper events while filling out the field, causing intermittent failures.
                                new FossaField(DeselectAfter(Fill), "zip-code", "BorrowerZipCode"),
                                new FossaField(Fill, "password", "Password"),
                                new FossaField(IfDisplayed(Fill), "home-phone-one", "BorrowerHomePhone1"),
                                new FossaField(IfDisplayed(Fill), "home-phone-two", "BorrowerHomePhone2"),
                                //new FossaField(IfDisplayed(Fill), "home-phone-three", "BorrowerHomePhone3"),
                                new FossaField(IfDisplayed(DeselectAfter(NoAutoAdvance(Fill))), "home-phone-three", "BorrowerHomePhone3"),
                                
                                new FossaField(IfDisplayed(Fill), "home-phone", "BorrowerHomePhone1"),
                                new FossaField(IfDisplayed(Append), "home-phone", "BorrowerHomePhone2"),
                                new FossaField(IfDisplayed(Append), "home-phone", "BorrowerHomePhone3"),
                                new FossaField(IfDisplayed(Fill), "social-security-one", "BorrowerSsn1"),
                                new FossaField(IfDisplayed(Fill), "social-security-two", "BorrowerSsn2"),
                                new FossaField(IfDisplayed(NoAutoAdvance(Fill)), "social-security-three", "BorrowerSsn3"),
                                new FossaField(IfDisplayed(Fill), "ssn", "BorrowerSsn1"),
                                new FossaField(IfDisplayed(Append), "ssn", "BorrowerSsn2"),
                                new FossaField(IfDisplayed(NoAutoAdvance(Append)), "ssn", "BorrowerSsn3"));

                return Steps;
            }
        }

        public IFormField[][] ValidQFStepsPurchase
        {
             get
             {
                IFormField[][] Steps = ValidQFStepsGeneric;

                Steps[2]  = Step(
                                new FossaField(SelectByText, "property-type", "HomeDescription"), 
                                new FossaField(SelectByText, "property-use", "PropertyUseType"),
                                new FossaField(ClickRadioYesNo, "new-home-{0}", "FoundNewHomeYesNo"),
                                new FossaField(ClickRadioYesNo, "current-realestate-agent_{0}", "FoundNewHomeYesNo"),
                                new FossaField(IfOtherElementDisplayed("ig-realtor-optin", ClickRadioYesNo), "inline_realtor_optin-{0}", "RealtorConsultYesNo"),
                                new FossaField(SelectByText, "purchase-price", "PurchasePrice"),
                                new FossaField(SelectByText, "down-payment-amt", "PurchaseDownPayment"));
                                
                Steps[3]  = Step(
                                new FossaField(ClickRadioYesNo, "homeservice-optin-{0}", "HomeServicesOptInYesNo"),
                                new FossaField(ClickCreditRadio, "stated-credit-history-{0}", "CreditProfile"),
                                new FossaField(SelectByText, "birth-month", "DateOfBirthMonth"),
                                new FossaField(SelectByText, "birth-day", "DateOfBirthDay"),
                                new FossaField(SelectByText, "birth-year", "DateOfBirthYear"),
                                new FossaField(ClickRadioYesNo, "declared-bankruptcy-{0}", "BankruptcyYesNo"),
                                new FossaField(IfOtherClassDisplayed("bankruptcy-date", SelectByText), "bankruptcy-discharged", "BankruptcyHistory"),
                                new FossaField(SelectByText, "foreclosure-text", "ForeclosureHistory"),
                                new FossaField(AJAXWaitAfter(SelectByText), "property-state", "PropertyState"),
                                new FossaField(SelectByText, "property-city", "PropertyCity"),
                                new FossaField(DeselectAfter(NoAutoAdvance((Fill))), "email", "EmailAddress"));
                return Steps;
             }
        }

        public IFormField[][] ValidQFStepsRefinance
        {
            get
            {
                IFormField[][] Steps = ValidQFStepsGeneric;

                //TODO: Here we click through after the fill. Probably should test tabbing, clicking outside the box, or hitting ENTER. Also note that behavior is different after validation has fired.
                //TODO: Figure out where to put the following note: For zip code fields, validation (and advancement) waits until the first blur. After the first blur, validation happens whenever the field changes.
                Steps[2]  = Steps[2]  = Step(
                                new FossaField(SelectByText, "property-type", "HomeDescription"), 
                                new FossaField(SelectByText, "property-use", "PropertyUseType"),
                                new FossaField(SelectByText, "estproperty-value", "RefiPropertyValue"),
                                new FossaField(IfNotDisplayNone(SelectByText), "est-mortgage-balance", "FirstMortgageBalance"),
                                new FossaField(IfDisplayed(DeselectAfter(Fill)), "est-mortgage-balance-txt", "FirstMortgageBalance"),
                                new FossaField(ClickRadioYesNo, "second-mortgage-{0}", "SecondMortgageYesNo"),
                                new FossaField(IfOtherElementDisplayed("second-mortgage-yes-fields", IfNotDisplayNone(NoAutoAdvance(SelectByText))), "second-mortgage-balance", "SecondMortgageBalance"),
                                new FossaField(IfDisplayed(DeselectAfter(NoAutoAdvance(Fill))), "second-mortgage-balance-txt", "SecondMortgageBalance"),
                                new FossaField(SelectByText, "cash-out", "RefiCashoutAmount"));
                Steps[3]  = Step(
                                new FossaField(ClickRadioYesNo, "homeservice-optin-{0}", "HomeServicesOptInYesNo"),
                                new FossaField(ClickCreditRadio, "stated-credit-history-{0}", "CreditProfile"),
                                new FossaField(SelectByText, "birth-month", "DateOfBirthMonth"),
                                new FossaField(SelectByText, "birth-day", "DateOfBirthDay"),
                                new FossaField(SelectByText, "birth-year", "DateOfBirthYear"),
                                 new FossaField(ClickRadioYesNo, "declared-bankruptcy-{0}", "BankruptcyYesNo"),
                                new FossaField(IfOtherClassDisplayed("bankruptcy-date", SelectByText), "bankruptcy-discharged", "BankruptcyHistory"),
                                new FossaField(SelectByText, "foreclosure-text", "ForeclosureHistory"),
                                new FossaField(NoAutoAdvance(Fill), "property-zip", "PropertyZipCode"),
                                new FossaField(DeselectAfter(NoAutoAdvance((Fill))), "email", "EmailAddress"));
                return Steps;
            }
        }

        public IFormField[][] ValidQFSteps
        {
            get
            {
                if (GetLoanType() == LoanType.Refinance) return ValidQFStepsRefinance;
                else return ValidQFStepsPurchase; //if(GetLoanType() == LoanType.Purchase)
            }
        }

        #endregion

        #region Base Form Process Stuff - Refactor into PageBase or FossaPageBase

        public IFormField[] Step(params IFormField[] fields) { return fields; }

        //TODO: Removed testData from this, since it gets passed into the constructor now.
        public void FillOutValidQF()
        {
            FillOutFormWithAllSteps(ValidQFSteps);
        }

        public void FillOutFormWithAllSteps(IFormField[][] steps)
        {
            StartForm();

            PerformSteps(steps, 1, (uint)(steps.Length - 1));
        }

        public void StartForm()
        {
            NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);
        }

       
        /// <summary>
        /// Fills out and completes the provided form steps (inclusive). Does not prepare the form; for that, use StartForm().
        /// </summary>
        /// <param name="steps">the steps to fill out (try ValidQFSteps)</param>
        /// <param name="startStep">the first step to fill out</param>
        /// <param name="endStep">the final step to fill out</param>
        public void PerformSteps(IFormField[][] steps, uint startStep, uint endStep)
        {
            for (uint i = startStep; i <= endStep; i++)
            {
                PerformStep(i, steps[i]);
            }
        }

        //TODO: Move this up?
        public override void NavigateToFossaForm(string environment, string page, string tid, string vid = "", string queryString = "")
        {
            base.NavigateToFossaForm(environment, page, tid, vid, queryString);

            // Capture/Report the GUID and QFVersion
            //TODO: Added the below wait and moved the GUID capture to the Navigate method.
            WaitForElement(By.Id("GUID"), 10); //5 seconds isn't enough time when running for the first time in a while.
            strQFormUID = driver.FindElement(By.Id("GUID")).Text;
            Common.ReportEvent(Common.INFO, String.Format("The QForm GUID = {0}", strQFormUID));
        }

        
        public void PerformStep(uint step, params IFormField[] fields)
        {
            PrepareStep(step, true);

            FillOutStep(fields);

            ConcludeStep();
        }

        public void PrepareStep(uint step, Boolean shouldValidateHeader = true)
        {
            currStep = step;
            didLastFieldTriggerAdvance = false;
            Common.ReportEvent(Common.INFO, "***** Starting Step " + step + " *****");
            if (shouldValidateHeader)
            {
                //TODO: Commented this out. We don't care about AJAX here.
                //WaitForAjaxToComplete(10);

                //TODO: Added the below try/catch to provide more helpful messages.

                try
                {
                    WaitForElementDisplayed(By.Id("step-" + step), WAIT_TIME_STEP_ADVANCE);
                    WaitForElementDisplayed(By.ClassName("step-" + step), WAIT_TIME_STEP_ADVANCE);
                }
                catch (WebDriverTimeoutException)
                {
                    Common.ReportEvent("ERROR", "Step " + step + " did not appear before the timeout; the form may not have advanced to the next step as expected.");
                    throw;
                }
            }

            WaitUntilNoAnimation(WAIT_TIME_STEP_ADVANCE);

            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
        }

        public void ConcludeStep()
        {
            //Common.ReportEvent("DEBUG", "didLastFieldTriggerAdvance is " + didLastFieldTriggerAdvance + ", ShouldAutoAdvance is " + ShouldAutoAdvance);

            if (!(ShouldAutoAdvance && didLastFieldTriggerAdvance))
            {
                Validation.IsTrue(IsElementDisplayed(By.Id("step-" + currStep)));
                ContinueToNextStep();
            }
            else
            {
                Common.ReportEvent("INFO", "Auto-advance expected.");
            }
        }

        public void FillOutStep(IFormField[] fields)
        {
            foreach (IFormField field in fields)
            {
                field.FillField(testData);
            }
        }

        public void ContinueToNextStep()
        {
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);

            WaitForElementEnabled(By.Id("next"), WAIT_TIME_STEP_ADVANCE);

            if (IsElementDisplayed(By.Id("next")))
            {
                //TODO: Switched this from DEBUG to INFO, since it's pretty darn useful!
                Common.ReportEvent("INFO", "Clicking the 'next' button");
                ClickButton("next");
            }
            else
            {
                Common.ReportEvent("ERROR", "Tried to continue to next step but couldn't find the 'next' button."); //TODO: Made this an ERROR instead of a DEBUG.
            }
        }

        public int GetErrorLabelCount()
        {
            int errorLabelCount = 0;
            var elements = driver.FindElements(By.ClassName("error"));

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].GetAttribute("tagName").ToUpper().Equals("LABEL"))
                {
                    errorLabelCount++;
                }
            }

            return errorLabelCount;
        }


        private LoanType GetLoanType()
        {
            switch (testData["LoanType"].ToUpper())
            {
                case "REFINANCE":
                    return LoanType.Refinance;
                case "PURCHASE":
                    return LoanType.Purchase;
                case "HOMEEQUITY":
                    return LoanType.HomeEquity;
                case "AUTOPURCHASE":
                    return LoanType.AutoPurchase;
                case "AUTOREFINANCE":
                    return LoanType.AutoRefinance;
                case "PERSONAL":
                    return LoanType.Personal;
                default:
                    throw new ArgumentException("Value of LoanType data column is not an accepted loan type.");
            }
        }

        #endregion

        #region Field Access Methods - Refactor into PageBase or FossaPageBase

        //TODO: Moved the QF GUID capturing out of the first step.
        //TODO: We're no longer respecting blnValidateHeader. Fix that!
        //TODO FIRST!!!: Refactor commonalities into methods. Reporting events and validating headers for each step needs to be extracted in other pages.
       
        /// <summary>
        /// Determines the appropriate radio button ID based on whether the provided test data value is "Y" or "N." The field ID should contain "{0}" in any places that should be replaced by "yes" or "no" (via a string.Format).
        /// </summary>
        /// <param name="strId">the ID of the field on the page with "{0}" wherever we want a "yes" or "no"</param>
        /// <param name="strValue">the test data value</param>
        /// <returns>the ID of the radio button to click</returns>
        private string GetRadioIDYesNo(string ID, string value)
        {
            bool isYes = value.Equals("Y", StringComparison.OrdinalIgnoreCase);
            string yesno = isYes ? "yes" : "no";
            //Common.ReportEvent("DEBUG", "Got a radio button ID of " + String.Format(ID, yesno));
            return String.Format(ID, yesno);
        }

        /// <summary>
        /// Clicks either a "yes" radio button or a "no" radio button depending on whether the provided test data value is "Y" or "N." The field ID should contain "{0}" in any places that should be replaced by "yes" or "no" (via a string.Format).
        /// </summary>
        /// <param name="ID">the ID of the field on the page with "{0}" wherever we want a "yes" or "no"</param>
        /// <param name="value">the test data value</param>
        public void ClickRadioYesNo(string ID, string value)
        {
            //Common.ReportEvent("DEBUG", "Setting radio button at " + ID + " to " + value);

            ClickRadio(GetRadioIDYesNo(ID, value));
            
            didLastFieldTriggerAdvance = true;
        }

        /// <summary>
        /// Clicks the radio button that has the given ID with all instances of "{0}" replaced with the value.ToLower() (using String.Format).
        /// </summary>
        /// <param name="ID">the ID of the field on the page with "{0}" wherever we want the lowercase value</param>
        /// <param name="value">the value to substitute into the ID</param>
        public void ClickRadioWithValueID(string ID, string value)
        {
            //Common.ReportEvent("DEBUG", "Setting radio button at " + ID + " to " + value);

            ClickRadio(String.Format(ID, value.ToLower()));

            didLastFieldTriggerAdvance = true;
        }

        /// <summary>
        /// Clicks the appropriate credit radio button. Clicks the radio button that has the given ID with all instances of "{0}" replaced with the value.ToLower() (using String.Format), except that "excellent" is changed to "ex."
        /// </summary>
        /// <param name="ID">the ID of the field on the page with "{0}" wherever we want the lowercase value</param>
        /// <param name="value">the value to substitute into the ID</param>
        public void ClickCreditRadio(string ID, string value)
        {
            string modifiedValue;
            if (value.ToLower() == "excellent") modifiedValue = "ex";
            else modifiedValue = value;

            ClickRadioWithValueID(ID, modifiedValue);
        }

        //TODO: Move this up into a higher class? Also, this works the same as other WaitFor functions; can we consolidate these?
        /// <summary>
        /// Waits a specified number of seconds for the Element's 'Displayed' property to be True.  Note: the .Displayed
        /// property does not work if the page uses "uniform" css.  
        /// </summary>
        /// <param name="by">The By locator for the Element</param>
        /// <param name="intSeconds">The number of seconds to wait</param>
        public void WaitForElementEnabled(By by, Int32 intSeconds)
        {
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(intSeconds));
            //Common.ReportEvent("DEBUG", String.Format("Inside WaitForElementEnabled() for '{0}'", by.ToString()));

            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
            {
                //Common.ReportEvent("DEBUG", String.Format("PageSource contains uniform?: {0}", d.PageSource.Contains("/assets/css/uniform.default.css").ToString()));
                 return d.FindElement(by).Enabled;
            });
        }

        public override void SelectByText(String ID, String text)
        {
            WrapSelectWithAutoadvanceCheck(ID, text, base.SelectByText);
        }

        public override void SelectByValue(String ID, String value)
        {
            WrapSelectWithAutoadvanceCheck(ID, value, base.SelectByValue);
        }

        private void WrapSelectWithAutoadvanceCheck(String ID, String value, Action<string, string> toWrap)
        {
            SelectElement element = new SelectElement(driver.FindElement(By.Id(ID)));
            IWebElement oldSelection = null;
            if (element.AllSelectedOptions.Count > 0) oldSelection = element.SelectedOption;

            toWrap(ID, value);

            if (oldSelection == null) didLastFieldTriggerAdvance = (null != (element.AllSelectedOptions.Count > 0 ? element.SelectedOption : null));
            else didLastFieldTriggerAdvance = !oldSelection.GetAttribute("value").Equals(element.AllSelectedOptions.Count > 0 ? element.SelectedOption.GetAttribute("value") : null);
            //Common.ReportEvent("DEBUG", "SelectByText autoAdvance trigger " + oldSelection.GetAttribute("value") + " ?= " + element.SelectedOption.GetAttribute("value"));
        }

        /// <summary>
        /// Takes in a field-setting method and returns a method that only sets the field if the element is displayed within a timeout; otherwise, it just returns. 
        /// This should not be used for elements such as dropdowns that have opacity 0 so that they can receive uniform styling.
        /// </summary>
        /// <param name="method">the field-setting method to wrap</param>
        /// <returns>a method which only runs the field-setting method if the element is displayed within a timeout.</returns>
        public Action<String, String> IfDisplayed(Action<String, String> method)
        {
            if (method == ClickRadioYesNo) return ClickRadioYesNoIfDisplayed;
            return (string ID, string value) =>
                {
                    try
                    {
                        WaitForElementDisplayed(By.Id(ID), WAIT_TIME_ANIMATE);
                        //Common.ReportEvent("DEBUG", "Field ID " + ID + " found and set.");
                        method(ID, value);
                    }
                    catch (WebDriverTimeoutException)
                    {
                        //Common.ReportEvent("DEBUG", "Field ID " + ID + " not found; continuing without error.");
                        return;
                    }
                };
        }

        /// <summary>
        /// Takes in a field-setting method and returns a method that only sets the field if the element is not "display: none" or "type: hidden" within a timeout; otherwise, it just returns. Handy for "uniform" styled dropdowns with opacity set to 0.
        /// </summary>
        /// <param name="method">the field-setting method to wrap</param>
        /// <returns>a method which only runs the field-setting method if the element is displayed within a timeout.</returns>
        public Action<String, String> IfNotDisplayNone(Action<String, String> method)
        {
            if (method == ClickRadioYesNo) return ClickRadioYesNoIfDisplayed;
            return (string ID, string value) =>
            {
                try
                {
                    WaitForElementNotDisplayNone(By.Id(ID), WAIT_TIME_ANIMATE);
                    //Common.ReportEvent("DEBUG", "Element " + ID + " has display value " + objDriver.FindElement(By.Id(ID)).GetCssValue("display") + " and type value " + objDriver.FindElement(By.Id(ID)).GetAttribute("type"));
                    //Common.ReportEvent("DEBUG", "Field ID " + ID + " found and set.");
                    method(ID, value);
                }
                catch (WebDriverTimeoutException)
                {
                    //Common.ReportEvent("DEBUG", "Field ID " + ID + " not found; continuing without error.");
                    return;
                }
            };
        }

        public Action<String, String> IfOtherElementDisplayed(string otherElementID, Action<String, String> method)
        {
            return (string ID, string value) =>
            {
                try
                {
                    WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(WAIT_TIME_ANIMATE));
            
                    Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
                    {
                        return d.FindElement(By.Id(otherElementID)).Displayed;
                    });

                    //Common.ReportEvent("INFO", "Field ID " + ID + " found and set.");
                    method(ID, value);
                }
                catch (WebDriverTimeoutException)
                {
                    //Common.ReportEvent("INFO", "Element ID " + otherElementID + " not displayed, so field ID " + ID + " not set; continuing without error.");
                    return;
                }
            };
        }

        public Action<String, String> IfOtherClassDisplayed(string otherElementClass, Action<String, String> method)
        {
            return (string ID, string value) =>
            {
                try
                {
                    WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(WAIT_TIME_ANIMATE));

                    Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
                    {
                        return d.FindElement(By.ClassName(otherElementClass)).Displayed;
                    });

                    //Common.ReportEvent("INFO", "Field ID " + ID + " found and set.");
                    method(ID, value);
                }
                catch (WebDriverTimeoutException)
                {
                    //Common.ReportEvent("INFO", "Element with class " + otherElementClass + " not displayed, so field ID " + ID + " not set; continuing without error.");
                    return;
                }
            };
        }

        /// <summary>
        /// Waits a specified number of seconds for the Element's computed CSS display value to not be "none."
        /// </summary>
        /// <param name="by">The By locator for the Element</param>
        /// <param name="intSeconds">The number of seconds to wait</param>
        public void WaitForElementNotDisplayNone(By by, Int32 intSeconds)
        {
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(intSeconds));
            
            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
            {
                return d.FindElement(by).GetCssValue("display") != "none" && d.FindElement(by).GetAttribute("type") != "hidden";
            });
        }

        /// <summary>
        /// Takes in a field-setting method and returns a method that doesn't announce an auto-advance.
        /// </summary>
        /// <param name="method">the field-setting method to wrap</param>
        /// <returns>a method which runs the field-setting method then marks the auto-advance as not triggered.</returns>
        public Action<String, String> NoAutoAdvance(Action<String, String> method)
        {
            return (string ID, string value) =>
            {
                method(ID, value);
                didLastFieldTriggerAdvance = false;
            };
        }

        /// <summary>
        /// Takes in a field-setting method and returns a method that waits for AJAX to complete afterward. Note that this should only be used after fields which are expected to initiate an AJAX request, such as fields which do special validation.
        /// </summary>
        /// <param name="method">the field-setting method to wrap</param>
        /// <returns>a method which runs the field-setting method then waits for AJAX to complete.</returns>
        public Action<String, String> AJAXWaitAfter(Action<String, String> method)
        {
            return (string ID, string value) =>
            {
                method(ID, value);

                WaitForAjaxToComplete(WAIT_TIME_AJAX);
            };
        }

        /// <summary>
        /// Takes in a field-setting method and returns a method that deselects the field afterward by clicking on the body of the document.
        /// </summary>
        /// <param name="method">the field-setting method to wrap</param>
        /// <returns>a method which runs the field-setting method then deselects the field afterward by clicking on the body of the document.</returns>
        public Action<String, String> DeselectAfter(Action<String, String> method)
        {
            return (string ID, string value) =>
            {
                method(ID, value);

                driver.FindElement(By.Id("topSubHeader")).Click();
            };
        }

        public override void ClickRadio(string strId)
        {
            WaitForElementEnabled(By.Id(strId), 5);
            base.ClickRadio(strId);
        }

        /// <summary>
        /// Waits for the appropriate radio button to be displayed; if doesn't get displayed before a timeout, return without clicking anything. If it does, click it.
        /// </summary>
        /// <param name="ID">the ID of the field on the page with "{0}" wherever we want a "yes" or "no"</param>
        /// <param name="value">the test data value</param>
        private void ClickRadioYesNoIfDisplayed(string ID, string value)
        {
            string fullID = GetRadioIDYesNo(ID, value);

            try
            {
                WaitForElementDisplayed(By.Id(fullID), WAIT_TIME_ANIMATE);
                
                ClickRadio(fullID);
                //Common.ReportEvent("INFO", "Field ID " + fullID + " found and set.");

                didLastFieldTriggerAdvance = true;
            }
            catch (WebDriverTimeoutException)
            {
                //Common.ReportEvent("INFO", "Field ID " + fullID + " not found; continuing without error.");
                return;
            }
        }

        //TODO: Move up into PageBase?
        /// <summary>
        /// Adds text to the contents of a text box without clearing its existing contents.
        /// </summary>
        /// <param name="ID">The ID of the text box</param>
        /// <param name="value">The string value to append to the text box</param>
        public void Append(String ID, String value)
        {
            var element = GetElement(ID);

            element.SendKeys(value);
        }

        public void WaitUntilNoAnimation(Int32 intSeconds)
        {
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(intSeconds));
            
            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
            {
                //Use jQuery to see if there are any elements with the pseudoclass :animated. Probably a pretty slow check, so let's not use this in performance-critical code.
                return ((IJavaScriptExecutor)d).ExecuteScript("return $(':animated').length").ToString() == "0";
            });
        }

        #endregion
    }
}
