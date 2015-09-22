//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Support.UI;

//namespace TestAutomation.LendingTree.tl
//{
//    class MyLendingTreePage : WebDriverPageBase
//    {
//        public MyLendingTreePage(IWebDriver driver, Dictionary<string, string> testData)
//            : base(driver, testData) 
//        { 
            
//        }

//        private const uint VARIATION_AUTOADVANCE = 4;
//        private const uint VARIATION_UNIFORMSTYLE = 8;

//        public override bool ShouldAutoAdvance
//        {
            
//            //get { return GetVariation(VARIATION_AUTOADVANCE) != 0; }
//            // set to true if we expect auto advance
           
//            get { return true; }
//        }

//        public override void StartForm()
//        {
//            ReportAutoAdvance();
            
//            NavigateToFossaForm(testData["TestEnvironment"], "tla.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);
          
//        }

//        public override void ReportAutoAdvance()
//        {
//            // this reports to event log
//            Common.ReportEvent(Common.INFO, "Personal loans currently has auto-advance.");

           

//            //var autoAdvanceVariation = GetVariation(VARIATION_AUTOADVANCE);
//            //if (autoAdvanceVariation == 0) Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is turned OFF (0) for this test.");
//            //else if (autoAdvanceVariation == 1) Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is turned ON (1) for this test.");
//            //else Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is ON by default for this test.");
//        }
//        //TODO determine if we using uniform style in this?
//        public override bool IsUniformStyle
//        {
//            get { return (GetVariation(VARIATION_UNIFORMSTYLE) != 3); }
//        }

//        public override void ReportUniformStyle()
//        {
//            var uniformStyle = GetVariation(VARIATION_UNIFORMSTYLE);
//            if (uniformStyle == 3) Common.ReportEvent(Common.INFO, "Uniform style is OFF due to the StyleSheets variation being 3 for this test.");
//            else Common.ReportEvent(Common.INFO, "Uniform style ON by default for this test.");
//        }

//        public override IFormField[][] ValidQFSteps
//        {
//            get 
//            {
//                IFormField[][] Steps = new IFormField[15][];
//                Steps[1] = Step(
//                            new FossaField(SelectByText, "loan-purpose", "LoanPurpose"));

//                Steps[2] = Step(
//                                new FossaField(Fill, "requested-loan-amount-input", "LoanAmountRequested"));

//                Steps[3] = Step(
//                               new FossaField(Fill, "zip-code-input", "BorrowerZipCode"));
                
//                Steps[4] = Step(
//                              new FossaField(Fill, "street1", "BorrowerStreetAddress"));
//                Steps[5] = Step(
//                             new FossaField(SelectByText, "residence", "Residence"));
                
               
//                switch(testData["CreditProfile"])
//                {
                  
//                    case "EXCELLENT":
//                    Steps[6] = Step(new FossaField(ClickRadio, "stated-credit-history-excellent"));
//                    break;
//                    case "MAJORCREDITPROBLEMS":
//                    Steps[6] = Step(new FossaField(ClickRadio, "stated-credit-history-fair"));
//                    break;
//                    case "SOMECREDITPROBLEMS":
//                    Steps[6] = Step(new FossaField(ClickRadio, "stated-credit-history-good"));
//                    break;
//                    case "LITTLEORNOCREDITHISTORY":
//                    Steps[6] = Step(new FossaField(ClickRadio, "stated-credit-history-poor"));
//                    break;
//                    default : 
//                    Steps[6] = Step(new FossaField(ClickRadio, "stated-credit-history-good"));
//                    break;
//                }
               
                                            
//                Steps[7] = Step(
//                              new FossaField(SelectByText, "birth-month", "DateOfBirthMonth"),
//                              new FossaField(SelectByText, "birth-day", "DateOfBirthDay"),
//                              new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));
//                Steps[8] = Step(
//                              new FossaField(SelectByText, "employment-status", "EmploymentStatus"));
//                Steps[9] = Step(
//                              new FossaField(Fill, "income", "BorrowerIncome"));
//                Steps[10] = Step(
//                              new FossaField(Fill, "first-name", "BorrowerFirstName"),
//                              new FossaField(Fill, "last-name", "BorrowerLastName"));
//                Steps[11] = Step(
//                              new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
//                              new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
//                              new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"));
//                Steps[12] = Step(
//                              new FossaField(Fill, "email", "EmailAddress"),
//                              new FossaField(Fill, "password", "Password"));
//                Steps[13] = Step(
//                              new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
//                              new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
//                              new FossaField(Fill, "social-security-three", "BorrowerSsn3"));
//                // To do, add case where you are not sent to oops page. 
//                Steps[14] = Step(
                   
//                    new FossaField(Fill, "first-name", "BorrowerFirstName"),
//                    //This oops step was working before a change to personal loan a week or so before September 03, 2013 
//                    //The second step is causing the page to submit the form. This causes the selenium test to fail as it is looking for the street fields and beyond.
//                    new FossaField(Fill, "last-name", "BorrowerLastName"),
//                    new FossaField(Fill, "street1", "BorrowerStreetAddress"),
//                    new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
//                    new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
//                    new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
//                    new FossaField(Fill, "social-security-three", "BorrowerSsn3"));

//                return Steps;
                
//            }
//        }

//        private void Switch(string p)
//        {
//            throw new NotImplementedException();
//        }

//        public override void FillOutValidQF()
//        {
//            //We need to resize the window wider than the form or Selenium gets terribly confused.
//            driver.Manage().Window.Maximize();

//            base.FillOutValidQF();
//        }

//        private void Click(string arg1, string arg2)
//        {
//            throw new NotImplementedException();
//        }

      
//        protected override void VerifyStep()
//        {
//            try
//            {
//                WaitForElementDisplayed(By.Id("step-" + currStep), WAIT_TIME_STEP_ADVANCE);
//                WaitForElementDisplayed(By.ClassName("step-" + currStep), WAIT_TIME_STEP_ADVANCE);
//            }
//            catch (WebDriverTimeoutException)
//            {
//                RecordScreenshot("VerifyStepTimeout");
//                Common.ReportEvent("ERROR", "Step " + currStep + " did not appear before the timeout; the form may not have advanced to the next step as expected.");
//                throw;
//            }
//            // return was added possibly due to a firefox specific error.
//            // Gets qform uid and stores it in strQFormUID

//            var obj = ((IJavaScriptExecutor)driver).ExecuteScript("return angular.element('[ng-controller=FossaCtrl]').scope().inputs.guid;");
//            strQFormUID = obj.ToString();
//            Common.ReportEvent(Common.INFO, "QForm UID is: " + strQFormUID);
//        }

//        public override void ContinueToNextStep()
//        {
//            WaitForElementEnabled(By.Id("next"), WAIT_TIME_STEP_ADVANCE);

//            //((IJavaScriptExecutor)driver).ExecuteScript("$('#next').click()");

//            if (IsElementDisplayed(By.Id("next")))
//            {
//                //adding to account for ContinueToNextStep button Not working as soon as it turns orange.
//                System.Threading.Thread.Sleep(1000);
//                Common.ReportEvent("INFO", "Clicking the 'next' button");
//                // commenting out due to zip code issue                
//                FocusOnPage();

//                var objElement = GetElement("next");
//                objElement.Click();
//            }
//            else
//            {
//                Common.ReportEvent("ERROR", "Tried to continue to next step but couldn't find the 'next' button.");
//            }
//        }

//        protected override void FocusOnPage()
//        {
//            driver.FindElement(By.Id("topSubHeader")).Click();

//        }

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

//        public override void Fill(string strId, string strValue)
//        {
//            base.Fill(strId, strValue);
//            FocusOnPage();
//        }

//        public override void SelectByText(String ID, String text)
//        {
//            SelectElement element = new SelectElement(driver.FindElement(By.Id(ID)));
//            if (element.AllSelectedOptions.Count > 0)
//            {
//                didLastFieldTriggerAdvance = element.SelectedOption.Text != text;
//            }

//            if (text.Length > 0)
//            {
//                SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(ID)));

//                System.Threading.Thread.Sleep(100);

//                try
//                {
//                    objSelect.SelectByText(text);
//                }
//                catch
//                {
//                    Common.ReportEvent(Common.ERROR, String.Format("SelectByText failed to select the text '{0}' in "
//                        + "dropdown '{1}'.", text, ID));
//                }
//            }
//        }

//        public override void SelectByValue(String ID, String value)
//        {
//            SelectElement element = new SelectElement(driver.FindElement(By.Id(ID)));
//            if (element.AllSelectedOptions.Count > 0)
//            {
//                didLastFieldTriggerAdvance = element.SelectedOption.GetAttribute("value") != value;
//            }

//            if (value.Length > 0)
//            {
//                SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(ID)));
//                System.Threading.Thread.Sleep(100);

//                try
//                {
//                    objSelect.SelectByValue(value);
//                }
//                catch (Exception)
//                {
//                    Common.ReportEvent(Common.ERROR, String.Format("SelectByValue failed to select the value '{0}' in "
//                        + "dropdown '{1}'.", value, ID));
//                }


//            }
//        }

//        public string CreditProfile { get; set; }
//    }
//}
