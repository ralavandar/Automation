using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestAutomation.LendingTree
{
    //TODO: Make this an abstract class.
    public abstract class FossaPageBase : PageBase
    {
        protected const double WAIT_TIME_ANIMATE = 1; //s
        protected const int WAIT_TIME_AJAX = 10; //s
        //Changing from 5 to 10  to 20 attempting to correct timeout issues. the test failed later with the wait of 10
        protected const double WAIT_TIME_STEP_ADVANCE = 30; //s

        //TODO: This delay before ending or beginning a step prevents certain mysterious issues like IE not hitting radio buttons. Increasing it might help if those issues resurface.
        //
        protected const int WAIT_TIME_INPUT_DELAY = 1000; //ms
        protected uint currStep;
        public Dictionary<string, string> testData;
        public enum LoanType { Refinance, Purchase, HomeEquity, AutoPurchase, AutoRefinance, Personal }

        // Constructor
        public FossaPageBase(IWebDriver driver, Dictionary<string, string> testData)
            : base(driver)
        {
            this.testData = testData;
        }

        #region Test Data Access

        public abstract bool ShouldAutoAdvance { get; }

        public virtual void ReportAutoAdvance()
        {
            Common.ReportEvent(Common.INFO, "The form has auto-advance by default.");
        }

        protected uint? GetVariation(uint variationIndex)
        {
            string VID = testData["Variation"];

            //If the VID isn't long enough, return null to indicate the default value.
            if (VID.Length < (2 * variationIndex) + 1) return null;

            return uint.Parse(VID.Substring((int)(2 * variationIndex), 1));
        }

        #endregion

        #region Base Form Process Stuff

        protected bool didLastFieldTriggerAdvance;

        public abstract IFormField[][] ValidQFSteps { get; }

        public IFormField[] Step(params IFormField[] fields) { return fields; }

        public override void NavigateToFossaForm(string environment, string page, string tid, string vid = "", string queryString = "")
        {
            base.NavigateToFossaForm(environment, page, tid, vid, queryString);

        }

        //TODO: Removed testData from this, since it gets passed into the constructor now.
        public virtual void FillOutValidQF()
        {
            FillOutFormWithAllSteps(ValidQFSteps);
        }

        public void FillOutFormWithAllSteps(IFormField[][] steps)
        {
            StartForm();

            PerformSteps(steps, 1, (uint)(steps.Length - 1));
        }

        public virtual void StartForm()
        {
            ReportAutoAdvance();

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

        public void PerformStep(uint step, params IFormField[] fields)
        {
            PrepareStep(step, true);

            FillOutStep(fields);

            ConcludeStep();
        }

        public void PrepareStep(uint step, Boolean shouldVerifyStep = true)
        {
            currStep = step;
            didLastFieldTriggerAdvance = false;
            Common.ReportEvent(Common.INFO, "***** Starting Step " + step + " *****");

            if (shouldVerifyStep)
            {
                VerifyStep();
            }

            WaitUntilNoAnimation(WAIT_TIME_STEP_ADVANCE);

            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
        }

        public virtual void VerifyStep()
        {
            try
            {
                WaitForElementDisplayed(By.Id("step-" + currStep), WAIT_TIME_STEP_ADVANCE);
            }
            catch (WebDriverTimeoutException)
            {
                RecordScreenshot("VerifyStepTimeout");
                Common.ReportEvent("ERROR", "Step " + currStep + " did not appear before the timeout; the form may not have advanced to the next step as expected.");
                throw;
            }
        }

        public void FillOutStep(IFormField[] fields)
        {
            foreach (IFormField field in fields)
            {
                field.FillField(testData);
            }
        }

        public void ConcludeStep()
        {
            if (!(ShouldAutoAdvance && didLastFieldTriggerAdvance))
            {
                ContinueToNextStep();
            }
            else
            {
                Common.ReportEvent("INFO", "Auto-advance expected.");
            }
        }

        public abstract void ContinueToNextStep();

        /// <summary>
        /// Verifies redirect to MyLendingTree express-offers page
        /// </summary>
        /// <param name="testData"></param>
        public void VerifyRedirectToMyLtExpress(Dictionary<string, string> testData)
        {
            System.Threading.Thread.Sleep(5000);

            // Check for redirect to mc.aspx / offers page
            if (IsElementDisplayed(By.ClassName("user-icon"), 25))    // element inside nav bar at very top of MyLendingTree page
            {
                // Specific checks on MyLendingTree
                System.Threading.Thread.Sleep(1000);

                try
                {
                    Assert.IsTrue(driver.Url.Contains("/express-offers"));

                    Common.ReportEvent(Common.PASS, String.Format
                        ("The TestString contains the expected value.  Expected: '/express-offers'.  TestString: \"{0}\".",
                           driver.Url));
                }
                catch (AssertionException)
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The TestString does not contain the expected value.  Expected: '/express-offers'.  TestString: \"{0}\".",
                            driver.Url));
                }

                try
                {
                    Assert.IsTrue(driver.Url.Contains("&guid=" + strQFormUID));

                    Common.ReportEvent(Common.PASS, String.Format
                        ("The TestString contains the expected value.  Expected: '&guid=<QFormUID>'.  TestString: \"{0}\".",
                           driver.Url));
                }
                catch (AssertionException)
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The TestString does not contain the expected value.  Expected: '&guid=<QFormUID>'.  TestString: \"{0}\".",
                            driver.Url));
                }

                // Validate page header/nav contains text <firstname> + " " + <lastname>
                Validation.StringContains(testData["BorrowerFirstName"] + " " + testData["BorrowerLastName"],
                    driver.FindElement(By.ClassName("user-icon")).FindElement(By.TagName("a")).Text);
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The redirect to My LendingTree did not display within 30 seconds. "
                    + "Expecting an element with ClassName 'user-icon'.");
                RecordScreenshot("MyLendingTreeException");
                Assert.Fail();
            }
        }

        /// <summary>
        /// Verifies redirect to MyLendingTree express-offers page, unauthorized
        /// </summary>
        /// <param name="testData"></param>
        public void VerifyRedirectToMyLtExpressUnauthorized(Dictionary<string, string> testData)
        {
            System.Threading.Thread.Sleep(5000);

            // Check for redirect to mc.aspx / offers page
            if (IsElementDisplayed(By.ClassName("offersDisplay"), 25)) 
            {
                // Specific checks on MyLendingTree
                System.Threading.Thread.Sleep(1000);

                // Verify URL contains express-offers
                try
                {
                    Assert.IsTrue(driver.Url.Contains("/express-offers"));

                    Common.ReportEvent(Common.PASS, String.Format
                        ("The TestString contains the expected value.  Expected: '/express-offers'.  TestString: \"{0}\".",
                           driver.Url));
                }
                catch (AssertionException)
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The TestString does not contain the expected value.  Expected: '/express-offers'.  TestString: \"{0}\".",
                            driver.Url));
                }

                // Verify URL contains expected GUID
                try
                {
                    Assert.IsTrue(driver.Url.Contains("&guid=" + strQFormUID));

                    Common.ReportEvent(Common.PASS, String.Format
                        ("The TestString contains the expected value.  Expected: '&guid=<QFormUID>'.  TestString: \"{0}\".",
                           driver.Url));
                }
                catch (AssertionException)
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The TestString does not contain the expected value.  Expected: '&guid=<QFormUID>'.  TestString: \"{0}\".",
                            driver.Url));
                }

                // Verify page header/nav contains unauthorized text
                try
                {
                    WaitForElementDisplayed(By.ClassName("message"), 15);
                    Validation.StringContains("Sorry, we're having trouble accessing some of your account information",
                        driver.FindElement(By.ClassName("message")).Text);       
                }
                catch (AssertionException)
                {
                    Common.ReportEvent(Common.FAIL,
                        "The TestString does not contain the expected value, or the element was not found/visible.  Expected: 'Sorry, we're having trouble accessing some of your account information'.");
                }
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The redirect to My LendingTree did not display within 30 seconds. "
                    + "Expecting an element with ClassName 'offersDisplay'.");
                RecordScreenshot("MyLendingTreeException");
                Assert.Fail();
            }
        }

        #endregion

        #region Field Access Methods

        /// <summary>
        /// Determines the appropriate radio button ID based on whether the provided test data value is "Y" or "N." The field ID should contain "{0}" in any places that should be replaced by "yes" or "no" (via a string.Format).
        /// </summary>
        /// <param name="strId">the ID of the field on the page with "{0}" wherever we want a "yes" or "no"</param>
        /// <param name="strValue">the test data value</param>
        /// <returns>the ID of the radio button to click</returns>
        protected string GetRadioIDYesNo(string ID, string value)
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
            var element = GetElement(GetRadioIDYesNo(ID, value));

            if (element.Selected)
            {
                // do nothing
                Common.ReportEvent(Common.INFO, String.Format("Nothing to do.  The '{0}' element is already selected.", ID));
                didLastFieldTriggerAdvance = false;
            }
            else
            {
                // For whatever reason, the Click() method does not work on our radio buttons in IEDriver - works fine in Firefox and ChromeDriver.
                // The workaround is to click on the radio button's parent object when using IEDriver :(
                if (driver.GetType().ToString().EndsWith("InternetExplorerDriver"))
                {
                    element.FindElement(By.XPath("..")).Click();
                }
                else
                {
                    element.Click();
                }
                didLastFieldTriggerAdvance = true;
            }
        }

        /// <summary>
        /// Clicks the radio button that has the given ID.
        /// </summary>
        /// <param name="ID">the ID of the element on the page</param>
        public void ClickRadioWithValueID(string ID)
        {
            var element = GetElement(By.Id(ID));

            if (element.Selected)
            {
                // do nothing
                Common.ReportEvent(Common.INFO, String.Format("Nothing to do.  The '{0}' element is already selected.", ID));
                didLastFieldTriggerAdvance = false;
            }
            else
            {
                element.Click();
                didLastFieldTriggerAdvance = true;
            }
        }

        //Click on Radio button By by
        public void ClickRadio(By by)
        {
            //Search the desired radio button
            var element = driver.FindElement(by);

            if (element.Selected)
            {
                // do nothing
                Common.ReportEvent(Common.INFO, String.Format("Radio button '{0}' is already selected.", by));
                didLastFieldTriggerAdvance = false;
            }
            else
            {
                // click the element
                element.Click();
                didLastFieldTriggerAdvance = true;
            }
        }

        //Click on Radio button By ID
        public override void ClickRadio(string strId)
        {
            WaitForElementEnabled(By.Id(strId), 5);
            //FocusOnPage();
            base.ClickRadio(strId);
        }


        //TODO: Move this up into a higher class? Also, this works the same as other WaitFor functions; can we consolidate these?
        /// <summary>
        /// Waits a specified number of seconds for the Element's 'Displayed' property to be True.  Note: the .Displayed
        /// property does not work if the page uses "uniform" css.  
        /// </summary>
        /// <param name="by">The By locator for the Element</param>
        /// <param name="seconds">The number of seconds to wait</param>
        public void WaitForElementEnabled(By by, double seconds)
        {
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
            {
                return d.FindElement(by).Enabled;
            });
        }

        public override void Fill(string strId, string strValue)
        {
            base.Fill(strId, strValue);
        }

        /// <summary>
        /// Selects a single value from a dropdown list by the displayed text.
        /// </summary>
        /// <param name="by">The locator object of the dropdown list</param>
        /// <param name="text">The text to select in the dropdown list</param>
        public override void SelectByText(By by, String text)
        {
            if (text.Length > 0)
            {
                SelectElement element = new SelectElement(driver.FindElement(by));
                System.Threading.Thread.Sleep(100);

                if (element.AllSelectedOptions.Count > 0)
                {
                    didLastFieldTriggerAdvance = element.SelectedOption.Text != text;
                }

                try
                {
                    element.SelectByText(text);
                }
                catch
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByText failed to select the value '{0}' in "
                        + "dropdown '{1}'.", text, by.ToString()));
                }
            }
        }


        public override void SelectByText(String ID, String text)
        {
            SelectElement element = new SelectElement(driver.FindElement(By.Id(ID)));
            if (element.AllSelectedOptions.Count > 0)
            {
                didLastFieldTriggerAdvance = element.SelectedOption.Text != text;
            }

            if (text.Length > 0)
            {
                SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(ID)));

                System.Threading.Thread.Sleep(100);

                try
                {
                    objSelect.SelectByText(text);
                }
                catch
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByText failed to select the text '{0}' in "
                        + "dropdown '{1}'.", text, ID));
                }
            }
        }

        /// <summary>
        /// Selects a single value from a dropdown list by the actual VALUE.
        /// </summary>
        /// <param name="by">The locator object of the dropdown list</param>
        /// <param name="value">The value to select in the dropdown list</param>
        public override void SelectByValue(By by, String value)
        {
            if (value.Length > 0)
            {
                SelectElement element = new SelectElement(driver.FindElement(by));
                System.Threading.Thread.Sleep(100);

                if (element.AllSelectedOptions.Count > 0)
                {
                    didLastFieldTriggerAdvance = element.SelectedOption.GetAttribute("value") != value;
                }

                try
                {
                    element.SelectByValue(value);
                }
                catch
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByValue failed to select the value '{0}' in "
                        + "dropdown '{1}'.", value, by.ToString()));
                }
            }
        }
 
        public override void SelectByValue(String ID, String value)
        {
            SelectElement element = new SelectElement(driver.FindElement(By.Id(ID)));
            if (element.AllSelectedOptions.Count > 0)
            {
                didLastFieldTriggerAdvance = element.SelectedOption.GetAttribute("value") != value;
            }

            if (value.Length > 0)
            {
                SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(ID)));
                System.Threading.Thread.Sleep(100);

                try
                {
                    objSelect.SelectByValue(value);
                }
                catch (Exception)
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByValue failed to select the value '{0}' in "
                        + "dropdown '{1}'.", value, ID));
                }
            }
        }

        public void SelectSliderValueByText(String valueToSelect)
        {
            // identify slider controls and initialize values
            var sliderHandle = driver.FindElement(By.CssSelector("a[class*=ui-slider-handle]"));
            var sliderLabel = driver.FindElement(By.CssSelector("div[class*=ui-slider-value-text]"));
            var handlePosition = Convert.ToDecimal(sliderHandle.GetAttribute("style").Trim('l', 'e', 'f', 't', ':', ' ', ';', '%'));
            string value = sliderLabel.Text;

            // If text already equals valueToSelect, then do nothing
            if (value == valueToSelect)
            {
                Common.ReportEvent(Common.INFO, String.Format("Nothing to do; the slider value '{0}' is already selected.", value));
            }
            else
            {
                // Move slider to beginning of range (far left) and set values
                MoveJQuerySlider(sliderHandle, -500, 0);
                // Need to slow it down here so that the slider controls and related text have time to update after moving the handle
                System.Threading.Thread.Sleep(100);
                value = sliderLabel.Text;
                handlePosition = Convert.ToDecimal(sliderHandle.GetAttribute("style").Trim('l', 'e', 'f', 't', ':', ' ', ';', '%'));

                // Move slider to right until value matches desired value, or we hit the far-right side (100%)
                while ((value != valueToSelect) && (handlePosition < 100))
                {
                    MoveJQuerySliderWithKeyboard(sliderHandle);
                    value = sliderLabel.Text;
                    handlePosition = Convert.ToDecimal(sliderHandle.GetAttribute("style").Trim('l', 'e', 'f', 't', ':', ' ', ';', '%'));

                    // Report error if we make it to the far right and still haven't found the valueToSelect
                    if ((handlePosition == 100) && (value != valueToSelect))
                    {
                        Common.ReportEvent(Common.ERROR, String.Format("Unable to select the value '{0}' on "
                            + "the slider.", valueToSelect));
                    }
                }
                Common.ReportEvent(Common.INFO, String.Format("Selected slider value '{0}'", sliderLabel.Text));
            }
        }

        /// <summary>
        /// Wraps a SELECT choice in a check to see if we autoadvance. 
        /// If we changed the value of the SELECT, we should expect an autoadvance; 
        /// if we picked the same value it had before, we don't expect the form to autoadvance.
        /// </summary>
        /// <param name="ID">ID of the select element</param>
        /// <param name="value">"value" to select (may be text, not value)</param>
        /// <param name="toWrap">function being wrapped</param>
        protected virtual void WrapSelectWithAutoadvanceCheck(String ID, String value, Action<string, string> toWrap)
        {
            SelectElement element = new SelectElement(driver.FindElement(By.Id(ID)));
            IWebElement oldSelection = null;
            if (element.AllSelectedOptions.Count > 0) oldSelection = element.SelectedOption;

            toWrap(ID, value);

            if (oldSelection == null) didLastFieldTriggerAdvance = (null != (element.AllSelectedOptions.Count > 0 ? element.SelectedOption : null));
            else didLastFieldTriggerAdvance = !oldSelection.GetAttribute("value").Equals(element.AllSelectedOptions.Count > 0 ? element.SelectedOption.GetAttribute("value") : null);
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
                    method(ID, value);
                }
                catch (WebDriverTimeoutException)
                {
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
                    method(ID, value);
                }
                catch (WebDriverTimeoutException)
                {
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
        /// <param name="seconds">The number of seconds to wait</param>
        public void WaitForElementNotDisplayNone(By by, double seconds)
        {
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

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
        /// Takes in a method (like clicking on an element) and returns a method that announces an auto-advance.
        /// </summary>
        /// <param name="method">the method to wrap (Ex: ClickElement() </param>
        /// <returns>a method that runs and then triggers an auto-advance on the form</returns>
        public Action<By> AutoAdvance(Action<By> method)
        {
            return (By by) =>
            {
                method(by);
                didLastFieldTriggerAdvance = true;
            };
        }

        /// <summary>
        /// Takes in a field-setting method and returns a method that waits for AJAX to complete afterward. 
        ///   Note that this should only be used after fields which are expected to initiate an AJAX request, such as fields which do special validation.
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

                FocusOnPage();
            };
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

        public void FocusOnPage()
        {
            driver.FindElement(By.ClassName("form-header")).Click();
        }

        public void WaitUntilNoAnimation(double seconds)
        {
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));

            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
            {
                //Use jQuery to see if there are any elements with the pseudoclass :animated. Probably a pretty slow check, so let's not use this in performance-critical code.
                return ((IJavaScriptExecutor)d).ExecuteScript("return $(':animated').length").ToString() == "0";
            });
        }

        #endregion

        public void GetAngularQFormUID(string form)
        {
            var obj = ((IJavaScriptExecutor)driver).ExecuteScript("return angular.element(document).injector().get('data').getData().guid;");
            strQFormUID = obj.ToString();
            Common.ReportEvent(Common.INFO, "QForm UID is: " + strQFormUID);
        }

        public LoanType GetLoanType()
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
    }
}
