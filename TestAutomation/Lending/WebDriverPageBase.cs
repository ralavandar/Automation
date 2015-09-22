//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Support.UI;

//namespace TestAutomation.LendingTree
//{
//    //Page base for generic web forms
//    public abstract class WebDriverPageBase : PageBase
//    {

//        protected const double WAIT_TIME_ANIMATE = 1; //s
//        protected const int WAIT_TIME_AJAX = 10; //s
//        //Changing from 5 to 10  to 20 attempting to correct timeout issues. the test failed later with the wait of 10
//        protected const double WAIT_TIME_STEP_ADVANCE = 20; //s

//        //TODO: This delay before ending or beginning a step prevents certain mysterious issues like IE not hitting radio buttons. Increasing it might help if those issues resurface.
//        //
//        protected const int WAIT_TIME_INPUT_DELAY = 1000; //ms

//        protected uint currStep;

//        public Dictionary<string, string> testData;




//        // Constructor
//        public WebDriverPageBase(IWebDriver driver, Dictionary<string, string> testData)
//            : base(driver)
//        {
//            this.testData = testData;
//        }

//        #region Test Data Access
//        // should change
//        public abstract bool ShouldAutoAdvance { get; }
//        // this should probably come out or change.
//        public abstract void ReportAutoAdvance();
//        // this should probably come out.
//        public abstract bool IsUniformStyle { get; }
//        // this should probably come out.
//        public abstract void ReportUniformStyle();

//        protected uint? GetVariation(uint variationIndex)
//        {
//            string VID = testData["Variation"];

//            //If the VID isn't long enough, return null to indicate the default value.
//            if (VID.Length < (2 * variationIndex) + 1) return null;

//            return uint.Parse(VID.Substring((int)(2 * variationIndex), 1));
//        }

//        #endregion

//        #region Base Form Process Stuff

//        protected bool didLastFieldTriggerAdvance;

//        public abstract IFormField[][] ValidQFSteps { get; }

//        public IFormField[] Step(params IFormField[] fields) { return fields; }

//        public override void NavigateToFossaForm(string environment, string page, string tid, string vid = "", string queryString = "")
//        {
//            base.NavigateToFossaForm(environment, page, tid, vid, queryString);

//            // Capture/Report the GUID and QFVersion
//            //TODO: Added the below wait and moved the GUID capture to the Navigate method.
//            //WaitForElement(By.Id("GUID"), 10); //5 seconds isn't enough time when running for the first time in a while.
//            //strQFormUID = driver.FindElement(By.Id("GUID")).Text;
//            //Common.ReportEvent(Common.INFO, String.Format("The QForm GUID = {0}", strQFormUID));
//        }

//        //TODO: Removed testData from this, since it gets passed into the constructor now.
//        public virtual void FillOutValidQF()
//        {
//            FillOutFormWithAllSteps(ValidQFSteps);
//        }
//        // creating a method that will fill out general forms that do not have a step displayed on the page or url.
//        public void FillOutFormWithAllSteps(IFormField[][] steps)
//        {
//            StartForm();

//            PerformSteps(steps, 1, (uint)(steps.Length - 1));
//        }

//        public virtual void StartForm()
//        {
//            ReportAutoAdvance();

//            NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);
//        }

//        public virtual void FillOutGenericForm()
//        {
//            //FillOutGenericFormAllSteps(ValidQFSteps);
//            FillOutGenericFormAllSteps(ValidFormSteps);
//        }

//        public void FillOutGenericFormAllSteps(IFormField[][] steps)
//        {
//            StartGenericForm();

//            PerformSteps(steps, 1, (uint)(steps.Length - 1));
//        }

//        public virtual void StartGenericForm()
//        {
//            ReportAutoAdvance();

//            NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);
//        }


//        /// <summary>
//        /// Fills out and completes the provided form steps (inclusive). Does not prepare the form; for that, use StartForm().
//        /// </summary>
//        /// <param name="steps">the steps to fill out (try ValidQFSteps)</param>
//        /// <param name="startStep">the first step to fill out</param>
//        /// <param name="endStep">the final step to fill out</param>
//        public void PerformSteps(IFormField[][] steps, uint startStep, uint endStep)
//        {
//            for (uint i = startStep; i <= endStep; i++)
//            {
//                PerformStep(i, steps[i]);
//            }
//        }

//        public void PerformStep(uint step, params IFormField[] fields)
//        {
//            PrepareStep(step, true);

//            FillOutStep(fields);

//            ConcludeStep();
//        }

//        public void PrepareStep(uint step, Boolean shouldVerifyStep = true)
//        {
//            currStep = step;
//            didLastFieldTriggerAdvance = false;
//            Common.ReportEvent(Common.INFO, "***** Starting Step " + step + " *****");

//            if (shouldVerifyStep)
//            {
//                VerifyStep();
//            }

//            WaitUntilNoAnimation(WAIT_TIME_STEP_ADVANCE);

//            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
//        }

//        protected abstract void VerifyStep();



//        public void FillOutStep(IFormField[] fields)
//        {
//            foreach (IFormField field in fields)
//            {
//                //Common.ReportEvent("DEBUG", "Setting field ID " + field.FieldID + " to value in column " + field.ColumnName + " using method " + field.FillAction.Method.Name);
//                field.FillField(testData);
//            }
//        }

//        public void ConcludeStep()
//        {
//            //Common.ReportEvent("DEBUG", "didLastFieldTriggerAdvance is " + didLastFieldTriggerAdvance + ", ShouldAutoAdvance is " + ShouldAutoAdvance);

//            if (!(ShouldAutoAdvance && didLastFieldTriggerAdvance))
//            {
//                Validation.IsTrue(IsElementDisplayed(By.Id("step-" + currStep)));
//                ContinueToNextStep();
//            }
//            else
//            {
//                Common.ReportEvent("INFO", "Auto-advance expected.");
//            }
//        }

//        public abstract void ContinueToNextStep();

//        #endregion

//        #region Field Access Methods

//        protected abstract void FocusOnPage();

//        /// <summary>
//        /// Determines the appropriate radio button ID based on whether the provided test data value is "Y" or "N." The field ID should contain "{0}" in any places that should be replaced by "yes" or "no" (via a string.Format).
//        /// </summary>
//        /// <param name="strId">the ID of the field on the page with "{0}" wherever we want a "yes" or "no"</param>
//        /// <param name="strValue">the test data value</param>
//        /// <returns>the ID of the radio button to click</returns>
//        protected string GetRadioIDYesNo(string ID, string value)
//        {
//            bool isYes = value.Equals("Y", StringComparison.OrdinalIgnoreCase);
//            string yesno = isYes ? "yes" : "no";
//            //Common.ReportEvent("DEBUG", "Got a radio button ID of " + String.Format(ID, yesno));
//            return String.Format(ID, yesno);
//        }

//        /// <summary>
//        /// Clicks either a "yes" radio button or a "no" radio button depending on whether the provided test data value is "Y" or "N." The field ID should contain "{0}" in any places that should be replaced by "yes" or "no" (via a string.Format).
//        /// </summary>
//        /// <param name="ID">the ID of the field on the page with "{0}" wherever we want a "yes" or "no"</param>
//        /// <param name="value">the test data value</param>
//        public void ClickRadioYesNo(string ID, string value)
//        {
//            //Common.ReportEvent("DEBUG", "Setting radio button at " + ID + " to " + value);

//            ClickRadio(GetRadioIDYesNo(ID, value));

//            didLastFieldTriggerAdvance = true;
//        }

//        /// <summary>
//        /// Clicks the radio button that has the given ID with all instances of "{0}" replaced with the value.ToLower() (using String.Format).
//        /// </summary>
//        /// <param name="ID">the ID of the field on the page with "{0}" wherever we want the lowercase value</param>
//        /// <param name="value">the value to substitute into the ID</param>
//        public void ClickRadioWithValueID(string ID, string value)
//        {
//            //Common.ReportEvent("DEBUG", "Setting radio button at " + ID + " to " + value);

//            ClickRadio(String.Format(ID, value.ToLower()));

//            didLastFieldTriggerAdvance = true;
//        }

//        //TODO: Move this up into a higher class? Also, this works the same as other WaitFor functions; can we consolidate these?
//        /// <summary>
//        /// Waits a specified number of seconds for the Element's 'Displayed' property to be True.  Note: the .Displayed
//        /// property does not work if the page uses "uniform" css.  
//        /// </summary>
//        /// <param name="by">The By locator for the Element</param>
//        /// <param name="seconds">The number of seconds to wait</param>
//        public void WaitForElementEnabled(By by, double seconds)
//        {
//            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
//            //Common.ReportEvent("DEBUG", String.Format("Inside WaitForElementEnabled() for '{0}'", by.ToString()));

//            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
//            {
//                //Common.ReportEvent("DEBUG", String.Format("PageSource contains uniform?: {0}", d.PageSource.Contains("/assets/css/uniform.default.css").ToString()));
//                return d.FindElement(by).Enabled;
//            });
//        }

//        public override void SelectByText(String ID, String text)
//        {
//            WrapSelectWithAutoadvanceCheck(ID, text, base.SelectByText);
//        }

//        public override void SelectByValue(String ID, String value)
//        {
//            WrapSelectWithAutoadvanceCheck(ID, value, base.SelectByValue);
//        }

//        /// <summary>
//        /// Wraps a SELECT choice in a check to see if we autoadvance. 
//        /// If we changed the value of the SELECT, we should expect an autoadvance; 
//        /// if we picked the same value it had before, we don't expect the form to autoadvance.
//        /// </summary>
//        /// <param name="ID">ID of the select element</param>
//        /// <param name="value">"value" to select (may be text, not value)</param>
//        /// <param name="toWrap">function being wrapped</param>
//        protected virtual void WrapSelectWithAutoadvanceCheck(String ID, String value, Action<string, string> toWrap)
//        {
//            SelectElement element = new SelectElement(driver.FindElement(By.Id(ID)));
//            IWebElement oldSelection = null;
//            if (element.AllSelectedOptions.Count > 0) oldSelection = element.SelectedOption;

//            toWrap(ID, value);

//            if (oldSelection == null) didLastFieldTriggerAdvance = (null != (element.AllSelectedOptions.Count > 0 ? element.SelectedOption : null));
//            else didLastFieldTriggerAdvance = !oldSelection.GetAttribute("value").Equals(element.AllSelectedOptions.Count > 0 ? element.SelectedOption.GetAttribute("value") : null);

//            //Common.ReportEvent("DEBUG", "SelectByText autoAdvance trigger " + oldSelection.GetAttribute("value") + " ?= " + element.SelectedOption.GetAttribute("value"));
//        }

//        /// <summary>
//        /// Takes in a field-setting method and returns a method that only sets the field if the element is displayed within a timeout; otherwise, it just returns. 
//        /// This should not be used for elements such as dropdowns that have opacity 0 so that they can receive uniform styling.
//        /// </summary>
//        /// <param name="method">the field-setting method to wrap</param>
//        /// <returns>a method which only runs the field-setting method if the element is displayed within a timeout.</returns>
//        public Action<String, String> IfDisplayed(Action<String, String> method)
//        {
//            if (method == ClickRadioYesNo) return ClickRadioYesNoIfDisplayed;
//            return (string ID, string value) =>
//            {
//                try
//                {
//                    WaitForElementDisplayed(By.Id(ID), WAIT_TIME_ANIMATE);
//                    //Common.ReportEvent("DEBUG", "Field ID " + ID + " found and set.");
//                    method(ID, value);
//                }
//                catch (WebDriverTimeoutException)
//                {
//                    //Common.ReportEvent("DEBUG", "Field ID " + ID + " not found; continuing without error.");
//                    return;
//                }
//            };
//        }

//        /// <summary>
//        /// Takes in a field-setting method and returns a method that only sets the field if the element is not "display: none" or "type: hidden" within a timeout; otherwise, it just returns. Handy for "uniform" styled dropdowns with opacity set to 0.
//        /// </summary>
//        /// <param name="method">the field-setting method to wrap</param>
//        /// <returns>a method which only runs the field-setting method if the element is displayed within a timeout.</returns>
//        public Action<String, String> IfNotDisplayNone(Action<String, String> method)
//        {
//            if (method == ClickRadioYesNo) return ClickRadioYesNoIfDisplayed;
//            return (string ID, string value) =>
//            {
//                try
//                {
//                    WaitForElementNotDisplayNone(By.Id(ID), WAIT_TIME_ANIMATE);
//                    //Common.ReportEvent("DEBUG", "Element " + ID + " has display value " + objDriver.FindElement(By.Id(ID)).GetCssValue("display") + " and type value " + objDriver.FindElement(By.Id(ID)).GetAttribute("type"));
//                    //Common.ReportEvent("DEBUG", "Field ID " + ID + " found and set.");
//                    method(ID, value);
//                }
//                catch (WebDriverTimeoutException)
//                {
//                    //Common.ReportEvent("DEBUG", "Field ID " + ID + " not found; continuing without error.");
//                    return;
//                }
//            };
//        }

//        public Action<String, String> IfOtherElementDisplayed(string otherElementID, Action<String, String> method)
//        {
//            return (string ID, string value) =>
//            {
//                try
//                {
//                    WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(WAIT_TIME_ANIMATE));

//                    Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
//                    {
//                        return d.FindElement(By.Id(otherElementID)).Displayed;
//                    });

//                    //Common.ReportEvent("INFO", "Field ID " + ID + " found and set.");
//                    method(ID, value);
//                }
//                catch (WebDriverTimeoutException)
//                {
//                    //Common.ReportEvent("INFO", "Element ID " + otherElementID + " not displayed, so field ID " + ID + " not set; continuing without error.");
//                    return;
//                }
//            };
//        }

//        public Action<String, String> IfOtherClassDisplayed(string otherElementClass, Action<String, String> method)
//        {
//            return (string ID, string value) =>
//            {
//                try
//                {
//                    WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(WAIT_TIME_ANIMATE));

//                    Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
//                    {
//                        return d.FindElement(By.ClassName(otherElementClass)).Displayed;
//                    });

//                    //Common.ReportEvent("INFO", "Field ID " + ID + " found and set.");
//                    method(ID, value);
//                }
//                catch (WebDriverTimeoutException)
//                {
//                    //Common.ReportEvent("INFO", "Element with class " + otherElementClass + " not displayed, so field ID " + ID + " not set; continuing without error.");
//                    return;
//                }
//            };
//        }

//        /// <summary>
//        /// Waits a specified number of seconds for the Element's computed CSS display value to not be "none."
//        /// </summary>
//        /// <param name="by">The By locator for the Element</param>
//        /// <param name="seconds">The number of seconds to wait</param>
//        public void WaitForElementNotDisplayNone(By by, double seconds)
//        {
//            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

//            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
//            {
//                return d.FindElement(by).GetCssValue("display") != "none" && d.FindElement(by).GetAttribute("type") != "hidden";
//            });
//        }

//        /// <summary>
//        /// Takes in a field-setting method and returns a method that doesn't announce an auto-advance.
//        /// </summary>
//        /// <param name="method">the field-setting method to wrap</param>
//        /// <returns>a method which runs the field-setting method then marks the auto-advance as not triggered.</returns>
//        public Action<String, String> NoAutoAdvance(Action<String, String> method)
//        {
//            return (string ID, string value) =>
//            {
//                method(ID, value);
//                didLastFieldTriggerAdvance = false;
//            };
//        }

//        /// <summary>
//        /// Takes in a field-setting method and returns a method that waits for AJAX to complete afterward. Note that this should only be used after fields which are expected to initiate an AJAX request, such as fields which do special validation.
//        /// </summary>
//        /// <param name="method">the field-setting method to wrap</param>
//        /// <returns>a method which runs the field-setting method then waits for AJAX to complete.</returns>
//        public Action<String, String> AJAXWaitAfter(Action<String, String> method)
//        {
//            return (string ID, string value) =>
//            {
//                method(ID, value);

//                WaitForAjaxToComplete(WAIT_TIME_AJAX);
//            };
//        }

//        /// <summary>
//        /// Takes in a field-setting method and returns a method that deselects the field afterward by clicking on the body of the document.
//        /// </summary>
//        /// <param name="method">the field-setting method to wrap</param>
//        /// <returns>a method which runs the field-setting method then deselects the field afterward by clicking on the body of the document.</returns>
//        public Action<String, String> DeselectAfter(Action<String, String> method)
//        {
//            return (string ID, string value) =>
//            {
//                method(ID, value);

//                FocusOnPage();
//            };
//        }

//        public override void ClickRadio(string strId)
//        {
//            WaitForElementEnabled(By.Id(strId), 5);
//            FocusOnPage();
//            base.ClickRadio(strId);
//        }

//        /// <summary>
//        /// Waits for the appropriate radio button to be displayed; if doesn't get displayed before a timeout, return without clicking anything. If it does, click it.
//        /// </summary>
//        /// <param name="ID">the ID of the field on the page with "{0}" wherever we want a "yes" or "no"</param>
//        /// <param name="value">the test data value</param>
//        private void ClickRadioYesNoIfDisplayed(string ID, string value)
//        {
//            string fullID = GetRadioIDYesNo(ID, value);

//            try
//            {
//                WaitForElementDisplayed(By.Id(fullID), WAIT_TIME_ANIMATE);

//                ClickRadio(fullID);
//                //Common.ReportEvent("INFO", "Field ID " + fullID + " found and set.");

//                didLastFieldTriggerAdvance = true;
//            }
//            catch (WebDriverTimeoutException)
//            {
//                //Common.ReportEvent("INFO", "Field ID " + fullID + " not found; continuing without error.");
//                return;
//            }
//        }

//        //TODO: Move up into PageBase?
//        /// <summary>
//        /// Adds text to the contents of a text box without clearing its existing contents.
//        /// </summary>
//        /// <param name="ID">The ID of the text box</param>
//        /// <param name="value">The string value to append to the text box</param>
//        public void Append(String ID, String value)
//        {
//            var element = GetElement(ID);

//            element.SendKeys(value);
//        }

//        public void WaitUntilNoAnimation(double seconds)
//        {
//            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

//            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
//            {
//                //Use jQuery to see if there are any elements with the pseudoclass :animated. Probably a pretty slow check, so let's not use this in performance-critical code.
//                return ((IJavaScriptExecutor)d).ExecuteScript("return $(':animated').length").ToString() == "0";
//            });
//        }

//        #endregion
//    }
//}
