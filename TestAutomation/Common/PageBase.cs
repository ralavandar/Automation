using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestAutomation
{
    //TODO: let's make this class abstract!
    public class PageBase
    {
        //TODO: Made this protected instead of private to allow adding certain functionality to morttreePage. If that gets moved here, switch it back to private!
        protected readonly IWebDriver driver;
        public String strQFormUID { get; set; } //TODO: Make the setter protected and remove the Hungarian notation.
        public String strQFVersion { get; set; }

        //Constructor
        public PageBase(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void ClickLink(String strId)
        {
            var objElement = GetElement(strId);
            objElement.Click();
        }

        public void ClickButton(String strId)
        {
            var objElement = GetElement(strId);
            objElement.Click();
        }

        public void ClickElement(By by)
        {
            var objElement = GetElement(by);
            objElement.Click();
            System.Threading.Thread.Sleep(125);
        }

        public void Wait(String strId)
        {
            System.Threading.Thread.Sleep(2000);
        }
        //TODO determine what would need to happen for FossaField to take different data times.
        // Creating seperate methods for each timespan because System.TimeSpan can not be used with a FossaField because they can currently only pass strings
        //System.TimeSpan can not handle strings 
        public void Wait10Sec(String strId)
        {

            System.Threading.Thread.Sleep(10000);
        }
        public void Wait20Sec(String strId)
        {
            
            System.Threading.Thread.Sleep(20000);
        }
        public void Wait5Sec(String strId)
        {

            System.Threading.Thread.Sleep(5000);
        }
        // Used for personal loan transition between ssn and oops step
        public void Wait7Sec(String strId)
        {

            System.Threading.Thread.Sleep(7000);
        }
        //TODO: Is clicking working in IE again?
        public virtual void ClickRadio(String strId)
        {
            // For whatever reason, the Click() method is not working in IE - works fine in Firefox and Chrome.
            // The workaround is to use the SendKeys() method.
            if (driver.GetType().ToString().EndsWith("InternetExplorerDriver"))
            {
               GetElement(strId).SendKeys(Keys.Space);
            }
            else
            {
                GetElement(strId).Click();
            }
        }

        //TODO: remove virtual if function gets moved up from morttreePage
        /// <summary>
        /// Clears the contents of an element (usually a text box) and then enters in some text.
        /// </summary>
        /// <param name="strId">The ID of the text box</param>
        /// <param name="strValue">The string value to enter in the text box</param>
        public virtual void Fill(String strId, String strValue)
        {
            var objElement = GetElement(strId);
            objElement.Click();
            System.Threading.Thread.Sleep(125);
            objElement.Clear();
            System.Threading.Thread.Sleep(125);
            objElement.SendKeys(strValue);
        }

        /// <summary>
        /// Clears the contents of an element (usually a text box) and then enters in some text.
        /// </summary>
        /// <param name="by">The locator object of the element</param>
        /// <param name="value">The string value to enter in the text box</param>
        public void Fill(By by, String value)
        {
            var objElement = GetElement(by);
            objElement.Click();
            System.Threading.Thread.Sleep(125);
            objElement.Clear();
            System.Threading.Thread.Sleep(125);
            objElement.SendKeys(value);
        }

        /// <summary>
        /// Enters in some text into a text box without clearing the text box.
        /// </summary>
        /// <param name="by">The locator object of the element</param>
        /// <param name="value">The string value to enter in the text box</param>
        public void Append(By by, String value)
        {
            var objElement = GetElement(by);
            System.Threading.Thread.Sleep(125);
            objElement.SendKeys(value);
        }

        //TODO: this was marked virtual so I could override it in morttreePage, but if that gets moved here unmark this as virtual. Same for all the other field-setting functions.
        /// <summary>
        /// Selects a single value from a dropdown list by the text displayed.
        /// </summary>
        /// <param name="strId">The ID of the dropdown list</param>
        /// <param name="strText">The text value to select</param>
        public virtual void SelectByText(String strId, String strText)
        {
            if (strText.Length > 0)
            {
                SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(strId)));
                
                System.Threading.Thread.Sleep(100); 

                try
                {
                    objSelect.SelectByText(strText);
                }
                catch
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByText failed to select the text '{0}' in "
                        + "dropdown '{1}'.", strText, strId));
                }
            }
        }

        /// <summary>
        /// Selects a single value from a dropdown list by the displayed text.
        /// </summary>
        /// <param name="by">The locator object of the dropdown list</param>
        /// <param name="text">The text to select in the dropdown list</param>
        public virtual void SelectByText(By by, String text)
        {
            if (text.Length > 0)
            {
                SelectElement objSelect = new SelectElement(driver.FindElement(by));
                System.Threading.Thread.Sleep(100);

                try
                {
                    objSelect.SelectByText(text);
                }
                catch
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByText failed to select the value '{0}' in "
                        + "dropdown '{1}'.", text, by.ToString()));
                }
            }
        }

        /// <summary>
        /// Selects a single value from a dropdown list by the actual VALUE.
        /// </summary>
        /// <param name="strId">The ID of the dropdown list</param>
        /// <param name="strValue">The value to select in the dropdown list</param>
        public virtual void SelectByValue(String strId, String strValue)
        {
            if (strValue.Length > 0)
            {
                SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(strId)));
                System.Threading.Thread.Sleep(100);

                try
                {
                    objSelect.SelectByValue(strValue);
                }
                catch
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByValue failed to select the value '{0}' in "
                        + "dropdown '{1}'.", strValue, strId));
                }
            }
        }

        /// <summary>
        /// Selects a single value from a dropdown list by the actual VALUE.
        /// </summary>
        /// <param name="by">The locator object of the dropdown list</param>
        /// <param name="value">The value to select in the dropdown list</param>
        public virtual void SelectByValue(By by, String value)
        {
            if (value.Length > 0)
            {
                SelectElement objSelect = new SelectElement(driver.FindElement(by));
                System.Threading.Thread.Sleep(100);

                try
                {
                    objSelect.SelectByValue(value);
                }
                catch
                {
                    Common.ReportEvent(Common.ERROR, String.Format("SelectByValue failed to select the value '{0}' in "
                        + "dropdown '{1}'.", value, by.ToString()));
                }
            }
        }

        /// <summary>
        /// Looks to see if a checkbox is already checked, and checks it (only if necessary).
        /// </summary>
        /// <param name="strId"></param>
        public void Check(String strId)
        {
            var objElement = GetElement(strId);

            if (objElement.Selected)
            {
                // do nothing
                Common.ReportEvent(Common.INFO, String.Format("Nothing to do.  The '{0}' checkbox is already checked.", strId));
            }
            else
            {
                objElement.Click(); // check the checkbox
            }
        }

        /// <summary>
        /// Moves a slider handle across a slider control by a given offset
        /// </summary>
        /// <param name="sliderHandle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveJQuerySlider(IWebElement sliderHandle, int x, int y)
        {
            Actions actions = new Actions(driver);
            IAction action = actions.ClickAndHold(sliderHandle).MoveByOffset(x, y).Release().Build();
            action.Perform();
            //sliderHandle.SendKeys("rightArrow");
        }

        /// <summary>
        /// Moves a sliderhandle across a slider control with the right-arrow key
        /// </summary>
        /// <param name="sliderHandle"></param>
        public void MoveJQuerySliderWithKeyboard(IWebElement sliderHandle)
        {
            sliderHandle.SendKeys(Keys.ArrowRight);
            // Need to slow it down here so that the slider controls and related text have time to update after moving the handle
            System.Threading.Thread.Sleep(100);
        }

        /// <summary>
        /// Looks to see if a checkbox is checked, and unchecks it (if necessary).
        /// </summary>
        /// <param name="strId"></param>
        public void Uncheck(String strId)
        {
            var objElement = GetElement(strId);
            if (objElement.Selected)
            {
                objElement.Click(); // uncheck the checkbox
            }
        }

       public IWebElement GetElement(String strId)
        {
            try
            {
                //return objDriver.FindElement(By.Id(strId));
                var e = driver.FindElement(By.Id(strId));
                System.Threading.Thread.Sleep(250);
                return e;
            }
            catch (NoSuchElementException)
            {
                Common.ReportEvent(Common.ERROR, String.Format("FindElement failed to find an object with id '{0}'. "
                    + "See following 'FindElementException' screenshot for more details.", strId));
                RecordScreenshot("FindElementException");
                return null;
            }
        }

        public IWebElement GetElement(By by)
        {
            try
            {
                var e = driver.FindElement(by);
                System.Threading.Thread.Sleep(250);
                return e;
            }
            catch (NoSuchElementException)
            {
                Common.ReportEvent(Common.ERROR, String.Format("FindElement failed to find an object using the provided " 
                    + "by locator {0}.  See following 'FindElementException' screenshot for more details.", by.ToString()));
                RecordScreenshot("FindElementException");
                return null;
            }
        }


        public bool IsElementPresent(By objBy)
        {
            try
            {
                driver.FindElement(objBy);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsElementDisplayed(By objBy)
        {
            try
            {
                var element = driver.FindElement(objBy);

                if (element.Displayed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks for a specified number of seconds for the element's 'Displayed' property to be True.
        /// Returns true if the 'Displayed' property is True; otherwise returns false
        /// </summary>
        /// <param name="by">The By locator for the Element</param>
        /// <param name="intSeconds">The number of seconds to check</param>
        public Boolean IsElementDisplayed(By by, Int32 intSecondsToCheck)
        {
            Int32 intCounter = 0;
            Boolean blnDisplayed = false;

            // System.Threading.Thread.Sleep(1000);

            // Loop until Displayed property is True, or until we reach intSecondsToWait
            do
            {
                try
                {
                    var element = driver.FindElement(by);

                    if (element.Displayed)
                        blnDisplayed = true;
                    else
                        blnDisplayed = false;
                }
                catch (NoSuchElementException)
                {
                    blnDisplayed = false;
                }

                //Common.ReportEvent("DEBUG", String.Format("intCounter = {0}; blnDisplayed = {1}",
                //    intCounter.ToString(), blnDisplayed.ToString()));

                if (blnDisplayed.Equals(false))
                {
                    //Common.ReportEvent("DEBUG", "Waiting for 1 second and incrementing intCounter");
                    System.Threading.Thread.Sleep(1000);
                    intCounter++;
                }

            } while (blnDisplayed.Equals(false) && intCounter < intSecondsToCheck);

            return blnDisplayed;
        }

        /// <summary>
        /// Waits a specified number of seconds for an Element to be found on the Page.  Note: the Element may or may
        /// not be visible/displayed.  Use WaitForElementDisplayed() for that purpose.
        /// </summary>
        /// <param name="by">The By locator for the Element</param>
        /// <param name="intSeconds">The number of seconds to wait</param>
        public void WaitForElement(By by, Int32 intSeconds)
        {
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(intSeconds));

            IWebElement myDynamicElement = objWait.Until<IWebElement>((d) =>
            {
                //Common.ReportEvent("DEBUG", String.Format("WaitForElementID for {0} is '{1}'", strID, IsElementPresent(by)));
                //Common.ReportEvent("DEBUG", String.Format("The Displayed property for {0} is '{1}'", strID, d.FindElement(by).Displayed.ToString()));
                return d.FindElement(by);
            });
        }

        //TODO: We may want to switch to checking for "display: none." The "uniform" css makes dropdowns have opacity 0, but they're still visible thanks to sham styling.
        //TODO: Let's make this log a relevant error if it fails.
        /// <summary>
        /// Waits a specified number of seconds for the Element's 'Displayed' property to be True.  Note: the .Displayed
        /// property does not work if the page uses "uniform" css.  
        /// </summary>
        /// <param name="by">The By locator for the Element</param>
        /// <param name="seconds">The number of seconds to wait</param>
        public void WaitForElementDisplayed(By by, double seconds)
        {
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            //Common.ReportEvent("DEBUG", String.Format("Inside WaitForElementDisplayed() for '{0}'", by.ToString()));

            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
            {
                //Common.ReportEvent("DEBUG", String.Format("PageSource contains uniform?: {0}", d.PageSource.Contains("/assets/css/uniform.default.css").ToString()));
                //Common.ReportEvent("DEBUG", String.Format("The Displayed property for {0} is '{1}'", by.ToString(), d.FindElement(by).Displayed.ToString()));
                //Common.ReportEvent("DEBUG", String.Format("The Uniform Displayed property for {0} is '{1}'", "uniform-" + strID, d.FindElement(By.Id("uniform-" + strID)).Displayed.ToString()));
                return d.FindElement(by).Displayed;
            });
        }

        /// <summary>
        /// Waits for a dropdown list to be populated with more than 2 options. For now, this is very specific to the
        /// City dropdown which is populated via an AJAX call after the user selects a State.  But if the >2 check
        /// works for other dropdowns, you can pass in the ID and this will work.
        /// </summary>
        /// <param name="ID">The ID of the dropdown list</param>
        /// <param name="seconds">The number of seconds to wait</param>
        public void WaitForDropdownRefresh(String ID, double seconds)
        {
            SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(ID)));
     
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            Boolean blnConditionMet = objWait.Until<Boolean>((d) =>
            {
                return (objSelect.Options.Count > 2);
            });
        }

        /// <summary>
        /// Waits a specified number of seconds for jQuery.active == 0, meaning all Ajax requests have completed.
        /// The function will throw an exception if AJAX is not complete, and the test will fail.
        /// </summary>
        /// <param name="intSeconds"></param>
        public void WaitForAjaxToCompleteAndFail(Int32 intSeconds)
        {
            System.Threading.Thread.Sleep(500);
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(intSeconds));
            Boolean blnConditionMet = objWait.Until<Boolean>((d) =>
            {
                Common.ReportEvent("DEBUG", String.Format("jQuery.active = {0}",
                     (d as IJavaScriptExecutor).ExecuteScript("return jQuery.active;").ToString()));
                return ((bool)(d as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"));
            });
        }

        //TODO GAW: Good candidate for refactoring! The loop success condition isn't too readable and it depends on a lot of variables that are essentially magic numbers. 
        // There are also duplicated lines in the loop that can be cleaned up to tighten up the logic.
        // Before I touched it, it was waiting 3s at minimum; I reduced that to 600ms minimum. If errors arise, increase each WAIT_TIME back to 1000.
        /// <summary>
        /// Waits a specified number of seconds for jQuery.active == 0, meaning all AJAX requests have completed.
        /// The test will continue if AJAX is not complete, and a WARNING will be logged 
        /// </summary>
        /// <param name="intSecondsToWait"></param>
        public void WaitForAjaxToComplete(Int32 intSecondsToWait)
        {
            // Check jQuery.active every second.  Do not advance until we get n seconds in a row of jQuery.active == 0.
            // If jQuery.active is > 0, reset the counter.
            // Also exit if these checks reach the number of seconds passed in without success
            //TODO GAW: Why N seconds in a row?

            int WAIT_TIME_AJAX_PREAMBLE = 0; //TODO: I set this at 0 instead of 1/10s. GAW
            int WAIT_TIME_AJAX_STEP = 100; //TODO: I set this to 100ms instead of 250. GAW

            Int32 intActive = 0;
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            //String strScript = "return jQuery.active;";

            int elapsedMs = 1;
            Int32 intSuccessCounter = 0;
            Int32 intSuccessCounterTarget = 2;
            Boolean blnAjaxComplete = false;

            //Common.ReportEvent("DEBUG", "Inside WaitForAjaxToComplete()'");

            System.Threading.Thread.Sleep(WAIT_TIME_AJAX_PREAMBLE); //TODO GAW: This wait seems like it may be unnecessary. Switched it from a second to 1/10 second.

            // Loop until our sucess counter indicates AJAX calls are complete, or until we reach intSecondsToWait
            do
            {
                //Common.ReportEvent("DEBUG", "Start executing java script 'return jQuery.active'");
                intActive = Convert.ToInt32(js.ExecuteScript("return jQuery.active;"));
                //Common.ReportEvent("DEBUG", "Finished executing java script 'return jQuery.active'");

                //Common.ReportEvent("DEBUG", String.Format("intTotalCounter = {0}; intSuccessCounter = {1}; jQuery.active = {2}",
                //    intTotalCounter.ToString(), intSuccessCounter.ToString(), intActive.ToString()));

                if (intActive == 0)
                {
                    // add 1 to the success counter - trying to get to intSuccessCounterTarget
                    intSuccessCounter++;
                    if (intSuccessCounter == intSuccessCounterTarget)
                    {
                        blnAjaxComplete = true;
                    }
                    else
                    {
                        //Common.ReportEvent("DEBUG", "Waiting for 1 second and incrementing intTotalCounter");
                        System.Threading.Thread.Sleep(WAIT_TIME_AJAX_STEP);
                        elapsedMs += WAIT_TIME_AJAX_STEP;
                    }
                }
                else
                {
                    intSuccessCounter = 0;
                    //Common.ReportEvent("DEBUG", "Waiting for " + WAIT_TIME_AJAX_STEP +  " ms and incrementing intTotalCounter");
                    System.Threading.Thread.Sleep(WAIT_TIME_AJAX_STEP);
                    elapsedMs += WAIT_TIME_AJAX_STEP;
                }


            } while (intSuccessCounter < intSuccessCounterTarget && elapsedMs < intSecondsToWait * 1000); 

            // Check blnAjaxComplete value and report
            if (!blnAjaxComplete)
                Common.ReportEvent("WARNING", String.Format("Waited {0} seconds for AJAX calls to complete, but they never did. "
                    + "Continuing with the test anyway, but could cause unpredictable results", intSecondsToWait.ToString()));
        }

        public bool DoesPageContainText(string textToFind)
        {
            Boolean blnFound = false;

            blnFound = driver.FindElement(By.TagName("body")).Text.Contains(textToFind);

            if (blnFound)
            {
                Common.ReportEvent(Common.PASS, String.Format("The expected text was found on the page: '{0}'",
                    textToFind));
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("The expected text was NOT found on the page: '{0}'",
                    textToFind));
                RecordScreenshot("FindTextFail");
            }

            return blnFound;
        }

        public void RecordScreenshot(String strFileName)
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            strFileName = strFileName + DateTime.Now.ToString("HHmmssffff") + ".gif";
            ss.SaveAsFile(strFileName, System.Drawing.Imaging.ImageFormat.Gif);
            Common.ReportEvent(Common.INFO, String.Format("Screenshot saved as: {0}\\{1}", 
                Directory.GetCurrentDirectory(), strFileName));
        }

        // TODO: This is Fossa-specific
        //TODO: Marked this as virtual to override it in morttreePage; might not want to keep it that way.
        public virtual void NavigateToFossaForm(string environment, string page, string tid, string vid = "", string queryString = "")
        {
            String strBaseUrl;
            String strUrl;

            switch (environment.ToUpper())
            {
                case "PROD":
                    strBaseUrl = "https://offers.lendingtree.com";
                    break;
                case "PREPROD":
                    strBaseUrl = "https://offers.preprod.lendingtree.com";
                    break;
                case "QA":
                case "STAGING":
                case "STAGE":
                    strBaseUrl = "https://offers.staging.lendingtree.com";
                    break;
                case "DEV":
                    strBaseUrl = "http://offers.dev.lendingtree.com";
                    break;
                default:    //QA
                    strBaseUrl = "http://offers.staging.lendingtree.com";
                    break;
            }

            // If a vid was provided, then use it.  Else, do not include it on the end of the URL
            if (vid.Length > 0)
            {
                strUrl = strBaseUrl + "/" + page + "?tid=" + tid + "&vid=" + vid;
            }
            else
            {
                strUrl = strBaseUrl + "/" + page + "?tid=" + tid;
            }

            // Add on queryString if provided (for testing ESourceID, siteId, etc - must begin with '&')
            if (queryString.Length > 0)
                strUrl = strUrl + queryString;

            // MPS, 08/27/2012 - This is a quick hack to get a unique id entered into the QueryString
            //if queryString contains 'selenium', then add on &seleniumid= + DateTime.Now.ToString("HHmmssffff")
            if ((queryString.Length > 0) && (queryString.Contains("selenium")))
                strUrl = strUrl + "&seleniumid=" + DateTime.Now.ToString("HHmmssffff");

            Common.ReportEvent(Common.INFO, String.Format("Navigating to Fossa Form URL: {0}", strUrl));
            driver.Navigate().GoToUrl(strUrl);
        }

        //TODO: this is a work in progress...commenting out for now
        /*public static void NavigateToWebDriverForm(string environment, string queryString = "")
        {
            String strBaseUrl;
            String strUrl;


           
            // Add on queryString if provided (for testing ESourceID, siteId, etc - must begin with '&')
            if (queryString.Length > 0)
                strUrl = strUrl + queryString;
                      
            Common.ReportEvent(Common.INFO, String.Format("Navigating to URL: {0}", strUrl));

            // This is the method that navigates 
            driver.Navigate().GoToUrl(strUrl);
        }*/
        
        //TODO - this is specific to Forms - works for Fossa and Wizard
        public string GetFormLeadId(string strFormName)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            String strScript = "return $($(\"" + strFormName + "\").get(0)).data(\"fossa\").lead.id";
            strQFormUID = (String)js.ExecuteScript(strScript);
            return strQFormUID;
        }

        //TODO: This seems to work, but look it over for refactoring. We may want to add data for the xsell order/presence per template
        public void BypassTlCrossSells()
        {
            // MPS, 4/3/14 - Removing this check as it is causing all kinds of issues: Check for Processing Dialog display
            // CheckForProcessing();
            System.Threading.Thread.Sleep(1000);

            //check for xsellModal info box
            if (IsElementDisplayed(By.Id("xsellModal"), 10))    // xsell modal dialog
            {
                Common.ReportEvent(Common.INFO, String.Format("Clicking close button on xsellModal dialog: {0}",
                    driver.FindElement(By.Id("xsellModal")).FindElement(By.ClassName("modalTitle")).Text));

                driver.FindElement(By.Id("xsellModal")).FindElement(By.ClassName("close")).Click();
            }
            else
            {
                Common.ReportEvent(Common.INFO, "The xsellModal did not display within 10 seconds.  This may be expected behavior depending on template and vid.");
            }

            // Check for first cross-sell display
            try
            {
                WaitForElement(By.Id("xsell-blue"), 30);     // Experian Credit Score cross-sell
                System.Threading.Thread.Sleep(1000);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on Experian cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.WARNING, "The first cross-sell did not display within 30 seconds. This may be expected behavior depending on template and vid.");
                RecordScreenshot("CrossSellException");
            }

            try
            {
                WaitForElement(By.Id("xsell-drahs_click"), 5);     // DR home warranty cross-sell
                System.Threading.Thread.Sleep(1000);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on DR home AHS warranty cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No DR home AHS warranty cross-sell shown, this may be expected behavior");
            }

            try
            {
                WaitForElement(By.Id("xsell-lexington"), 5);     // Lexington Law cross-sell
                System.Threading.Thread.Sleep(1000);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on Lexington cross-sell");
                //MPS, 4/21/2014 - Temporary work-around b/c there are two links with id=demoBtn :(
                //ClickButton("demoBtn");
                ClickElement(By.LinkText("Next>>"));
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No Lexington cross-sell shown, this may be expected behavior");
            }

            try
            {
                WaitForElement(By.Id("xsell-drhw_click"), 5);     // DR home warranty cross-sell
                System.Threading.Thread.Sleep(1000);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on DR home warranty cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No DR home warranty cross-sell shown, this may be expected behavior");
            }

            try
            {
                WaitForElement(By.Id("xsell-drhi_click"), 5);     // DR home insurance cross-sell
                System.Threading.Thread.Sleep(1000);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on DR home insurance cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No DR home insurance cross-sell shown, this may be expected behavior");
            }

            try
            {
                WaitForElement(By.Id("xsell-lexington"), 5);     // Lexington Law cross-sell
                System.Threading.Thread.Sleep(1000);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on Lexington cross-sell");
                //MPS, 4/21/2014 - Temporary work-around b/c there are two links with id=demoBtn :(
                //ClickButton("demoBtn");
                ClickElement(By.LinkText("Next>>"));
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No Lexington cross-sell shown, this may be expected behavior");
            }

            try
            {
                WaitForElement(By.ClassName("green-block"), 5);  // Homeowners Insurance cross-sell
                System.Threading.Thread.Sleep(1000);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on Homeowners Insurance cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No Homeowners Insurance cross-sell shown, this may be expected behavior");
            }

            try
            {
                WaitForElement(By.Id("securityXsell"), 5);  // Home Security System cross-sell
                System.Threading.Thread.Sleep(1000);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on Home Security System cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No Home Security System cross-sell shown, this may be expected behavior");
            }

            try
            {
                WaitForElement(By.Id("totalProtectXsellForm"), 5);  // Total Protect Home Warranty cross-sell
                System.Threading.Thread.Sleep(1000);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on Total Protect Home Warranty cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No Total Protect Home Warranty cross-sell shown, this may be expected behavior");
            }
        }

        public void CheckForProcessing()
        {
            if (IsElementDisplayed(By.Id("modalWrapper"), 5) && IsElementDisplayed(By.ClassName("errorNotification"), 5))   // Processing dialog image
            {
                Common.ReportEvent(Common.INFO, String.Format("The processing dialog displayed inside modalWrapper"));
            }
            else
            {
                Common.ReportEvent(Common.INFO, "The processing dialog did not display, " +
                    "or it displayed so quickly that automation did not detect it.");
            }
        }

        // TODO: This is Fossa-specific - probably belongs in FossaPageBase
        public void BypassCrossSells()
        {
            // Check for modal dialog display
            System.Threading.Thread.Sleep(2000);
            if (IsElementDisplayed(By.ClassName("modal-footer"), 8)) 
            {
                Common.ReportEvent(Common.INFO, "The modal dialog displayed. Clicking close.");
                ClickElement(By.CssSelector("input[value=Close]"));
            }
        }

        // TODO - this is specific to Forms - works for all Fossa forms
        /// <summary>
        /// Verifies redirect to MyLendingTree offers page
        /// </summary>
        /// <param name="testData"></param>
        public void VerifyRedirectToMyLendingTree(Dictionary<string, string> testData)
        {
            // Check for redirect to mc.aspx / offers page
            if (IsElementDisplayed(By.ClassName("user-icon"), 30))    // element inside nav bar at very top of MyLendingTree page
            {
                // Specific checks on MyLendingTree offers
                System.Threading.Thread.Sleep(1000);

                try
                {
                    Assert.IsTrue(driver.Url.Contains("/mc.aspx?tid=mc-"));

                    Common.ReportEvent(Common.PASS, String.Format
                        ("The TestString contains the expected value.  Expected: '/mc.aspx?tid=mc-'.  TestString: \"{0}\".",
                           driver.Url));
                }
                catch (AssertionException)
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The TestString does not contain the expected value.  Expected: '/mc.aspx?tid=mc-'.  TestString: \"{0}\".",
                            driver.Url));
                }

                //try
                //{
                //    Assert.IsTrue(driver.Url.Contains("&qformuid=" + strQFormUID));

                //    Common.ReportEvent(Common.PASS, String.Format
                //        ("The TestString contains the expected value.  Expected: '&qformuid=<QFormUID>'.  TestString: \"{0}\".",
                //           driver.Url));
                //}
                //catch (AssertionException)
                //{
                //    Common.ReportEvent(Common.FAIL, String.Format
                //        ("The TestString does not contain the expected value.  Expected: '&qformuid=<QFormUID>'.  TestString: \"{0}\".",
                //            driver.Url));
                //}

                // Validate page header/nav contains text <firstname> + " " + <lastname>
                Validation.StringContains(testData["BorrowerFirstName"] + " " + testData["BorrowerLastName"],
                    driver.FindElement(By.ClassName("user-icon")).FindElement(By.TagName("a")).Text);
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The redirect to My LendingTree did not display within 30 seconds. "
                    + "Expecting an element with ClassName 'user-icon'.");
                RecordScreenshot("MyLendingTreeOffersException");
                Assert.Fail();
            }
        }

        // TODO - this is specific to Forms - works for all Fossa forms
        /// <summary>
        /// Verifies redirect to MyLendingTree DHC page
        /// </summary>
        /// <param name="testData"></param>
        public void VerifyRedirectToMyLendingTreeDHC(Dictionary<string, string> testData)
        {
            // Check for redirect to mc.aspx / offers page
            if (IsElementDisplayed(By.ClassName("user-icon"), 30))    // element inside nav bar at very top of MyLendingTree page
            {
                // Specific checks on MyLendingTree DHC
                System.Threading.Thread.Sleep(1000);

                try
                {
                    Assert.IsTrue(driver.Url.Contains("/dream-home-companion?"));

                    Common.ReportEvent(Common.PASS, String.Format
                        ("The TestString contains the expected value.  Expected: '/dream-home-companion?'.  TestString: \"{0}\".",
                           driver.Url));
                }
                catch (AssertionException)
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The TestString does not contain the expected value.  Expected: '/dream-home-companion?'.  TestString: \"{0}\".",
                            driver.Url));
                }

                //try
                //{
                //    Assert.IsTrue(driver.Url.Contains("?qformuid=" + strQFormUID));

                //    Common.ReportEvent(Common.PASS, String.Format
                //        ("The TestString contains the expected value.  Expected: '?qformuid=<QFormUID>'.  TestString: \"{0}\".",
                //           driver.Url));
                //}
                //catch (AssertionException)
                //{
                //    Common.ReportEvent(Common.FAIL, String.Format
                //        ("The TestString does not contain the expected value.  Expected: '?qformuid=<QFormUID>'.  TestString: \"{0}\".",
                //            driver.Url));
                //}

                // Validate page header/nav contains text <firstname> + " " + <lastname>
                Validation.StringContains(testData["BorrowerFirstName"] + " " + testData["BorrowerLastName"],
                    driver.FindElement(By.ClassName("user-icon")).FindElement(By.TagName("a")).Text);
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The redirect to My LendingTree did not display within 30 seconds. "
                    + "Expecting an element with Id 'mc-header'.");
                RecordScreenshot("MyLendingTreeDHCException");
                Assert.Fail();
            }
        }
    }
}
