using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tlm
{
    class h2Page : FossaPageBase
    {
        public h2Page(IWebDriver driver, Dictionary<string, string> testData)
            : base(driver, testData) 
        { 
            
        }

        public override bool ShouldAutoAdvance
        {
            get { return true; }
        }

        public override void StartForm()
        {
            ReportAutoAdvance();

            NavigateToFossaForm(testData["TestEnvironment"], "tlm.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);
        }

        public override IFormField[][] ValidQFSteps
        {
            get 
            {                
                // Initialize the array 
                Int32 numSteps = 19;

                // Add a step for First mortgage balance if applicable
                if (testData["CurrentMortgages"] == "First mortgage only")
                    numSteps++;
                // Add a step for First and second mortgage balance if applicable
                if (testData["CurrentMortgages"] == "First & second mortgages")
                    numSteps = numSteps + 2;

                // Add a step for bankruptcy if applicable
                if (testData["BankruptcyHistory"] != "N")
                    numSteps++;

                // Add a step for foreclosure if applicable
                if (testData["ForeclosureHistory"] != "N")
                    numSteps++;

                // Number of steps also depends on SSN presence, credit pull pass/fail
                if (testData["CreditPullSuccessYesNo"] == "N")
                    numSteps++;

                IFormField[][] Steps = new IFormField[numSteps][];

                //Loan Type
                Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='Home equity loan/line']")),
                                new FossaField(Wait, "Wait"));

                // Property Type
                switch (testData["PropertyType"])
                {
                    case "Single Family Home":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-single-fam]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Townhome":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-townhome]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Condominium":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-condo]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Multi Family Home":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-multi]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Manufactured / Mobile":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-mobile]")),
                                new FossaField(Wait, "Wait"));
                        break;
                }

                // Property Zip
                Steps[3] = Step(
                                new FossaField(Fill, "property-zip-code-input", "PropertyZipCode"),
                                new FossaField(Wait, "Wait"));

                // Property Use
                switch (testData["PropertyUse"])
                {
                    case "Primary Home":
                        Steps[4] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-primary]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Secondary Home":
                        Steps[4] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-secondary]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Rental Property":
                        Steps[4] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-rental]")),
                                new FossaField(Wait, "Wait"));
                        break;
                }

                //Estimated property value                
                Steps[5] = Step(
                            new FossaField(Wait, "Wait"),
                            new FossaField(GetAngularQFormUID, "h2"),
                            new FossaField(SelectSliderValueByText, testData["PropertyValue"]),
                            new FossaField(Wait, "Wait"));

                //What was the purchase price of the property?                
                Steps[6] = Step(
                            new FossaField(SelectSliderValueByText, testData["PurchasePrice"]),
                            new FossaField(Wait, "Wait"));

                //Which year did you purchase it?
                Steps[7] = Step(                            
                             new FossaField(SelectByText, "purchase-year", "PurchaseYear"),
                             new FossaField(Wait, "Wait"));

                //Do you have a mortgage on this property?
                int stepNum = 8;
                switch (testData["CurrentMortgages"])
                {                        
                    case "No current mortgage":
                        Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='No current mortgage']")),
                                new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;

                    case "First mortgage only":
                        Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='First mortgage only']")),
                                new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //Select First Mortgage Balance
                        Steps[stepNum] = Step(
                                new FossaField(SelectSliderValueByText, testData["FirstMortgageBalance"]),
                                new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;

                    case "First & second mortgages":
                        Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='First & second mortgages']")),
                                new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //Select First Mortgage Balance
                        Steps[stepNum] = Step(
                                new FossaField(SelectSliderValueByText, testData["FirstMortgageBalance"]),
                                new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //Select Second Mortgage Balance
                        Steps[stepNum] = Step(
                                new FossaField(SelectSliderValueByText, testData["SecondMortgageBalance"]),
                                new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;

                    default:
                        //No Mortgage
                        Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='No current mortgage']")),
                                new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;
                }

                //Amount you would like to borrow
                Steps[stepNum] = Step(
                                new FossaField(SelectSliderValueByText, testData["RequestedLoanAmount"]),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Describe your credit
                switch (testData["CreditProfile"])
                {
                    case "stated-credit-history-excellent":
                        Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-excellent]")));
                        break;
                    case "stated-credit-history-good":
                        Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-good]")));
                        break;
                    case "stated-credit-history-fair":
                        Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-fair]")));
                        break;
                    case "stated-credit-history-poor":
                        Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-poor]")));
                        break;
                }
                stepNum = stepNum + 1;

                //Your date of birth
                Steps[stepNum] = Step(
                                new FossaField(NoAutoAdvance(SelectByText), "birth-month", "DateOfBirthMonth"),
                                new FossaField(NoAutoAdvance(SelectByText), "birth-day", "DateOfBirthDay"),
                                new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));
                stepNum = stepNum + 1;

                //Have you had a bankruptcy or foreclosure in the last 7 years?
                if (testData["ForeclosureHistory"] == "N" && testData["BankruptcyHistory"] == "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=bankruptcy-or-foreclosure-no]")));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] == "N" && testData["BankruptcyHistory"] != "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=bankruptcy-or-foreclosure-bankruptcy]")));
                    stepNum = stepNum + 1;
                    Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["BankruptcyHistory"]),
                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] != "N" && testData["BankruptcyHistory"] == "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=bankruptcy-or-foreclosure-foreclosure]")));
                    stepNum = stepNum + 1;
                    Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["ForeclosureHistory"]),
                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] != "N" && testData["ForeclosureHistory"] != "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=bankruptcy-or-foreclosure-both]")));
                    stepNum = stepNum + 1;
                    Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["BankruptcyHistory"]),
                            new FossaField(Wait, "Wait"));

                    stepNum = stepNum + 1;
                    Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["ForeclosureHistory"]),
                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else
                {
                    // Report invalid test data
                    Common.ReportEvent(Common.ERROR, "The values provided for 'ForeclosureHistory' and/or 'BankruptcyYesNo' are not valid. " +
                        "Please check the test case data and try again.");
                }

                //Your name
                Steps[stepNum] = Step(
                               new FossaField(Fill, "first-name", "BorrowerFirstName"),
                               new FossaField(Fill, "last-name", "BorrowerLastName"),
                               new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Current street address
                Steps[stepNum] = Step(
                                new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickElement, By.ClassName("form-header")),
                                new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Do you currently have accounts with any of the following banks?
                Steps[stepNum] = Step(
                    new FossaField(ClickElement, By.CssSelector("label[for=bank-other]")),
                    new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Home Phone
                Steps[stepNum] = Step(
                                new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;
               
                //Please enter the last 4 digits of your SSN
                Steps[stepNum] = Step(
                                new FossaField(Fill, "social-security-four", "BorrowerSsn3"),                               
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Email and Password
                Steps[stepNum] = Step(
                                new FossaField(DeselectAfter(Fill), "email", "EmailAddress"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "password", "Password"),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;
                //Credit Pull check
                if (testData["CreditPullSuccessYesNo"] == "N")
                {
                    Steps[stepNum] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, By.XPath("//input[@id='social-security'][2]"), "BorrowerSsn1"),
                                new FossaField(Append, By.XPath("//input[@id='social-security'][2]"), "BorrowerSsn2"),
                                new FossaField(Append, By.XPath("//input[@id='social-security'][2]"), "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"));
                }                
               
                return Steps;
            }
        }

        public override void FillOutValidQF()
        {
            //We need to resize the window wider than the form or Selenium gets terribly confused.
            driver.Manage().Window.Maximize();

            base.FillOutValidQF();
        }

        public override void ContinueToNextStep()
        {
            WaitForElementEnabled(By.Id("next"), WAIT_TIME_STEP_ADVANCE);

            if (IsElementDisplayed(By.Id("next")))
            {
                Common.ReportEvent("INFO", "Clicking 'next' button");
                var objElement = GetElement("next");
                objElement.Click();
            }
            else
            {
                Common.ReportEvent("ERROR", "Tried to continue to next step but couldn't find the 'next' button.");
            }
        }
    }
}
