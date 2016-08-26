using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.mc
{    
    class mcHomeEquityPage :FossaPageBase
    {
        public mcHomeEquityPage(IWebDriver driver, Dictionary<string, string> testData)
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

            NavigateToFossaForm(testData["TestEnvironment"], "mc.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);

            // Login to MC using the email address and pwd in the test case data
            Fill(By.Id("UserName"), testData["EmailAddress"]);
            Fill(By.Id("Password"), testData["Password"]);
            ClickElement(By.ClassName("sign-in-button"));
        }

        public override IFormField[][] ValidQFSteps
        {
            get
            {
                Int32 numSteps = 15;

                //Add a step for First mortgage only
                if (testData["CurrentMortgages"] == "First mortgage only")
                {
                    numSteps = numSteps + 1;
                }

                //Add 2 steps for First and Second mortgage
                if (testData["CurrentMortgages"] == "First & second mortgages")
                {
                    numSteps = numSteps + 2;
                }
                // Add two steps eigther bankruptcy is Y or Forceclouser have some value other than N
                if ((testData["BankruptcyHistory"] != "N") || (testData["ForeclosureHistory"] != "N"))
                {
                    numSteps = numSteps + 2;
                }

                ////Phone number page its only for test case 1
                //if (testData["TestCaseName"].Equals("mcHomeEquity_01_Primary"))
                //{
                //    numSteps = numSteps + 1;
                //}
                
                IFormField[][] Steps = new IFormField[numSteps][];

                // Step 1 Select Property type               
                Steps[1] = Step(
                                new FossaField(SelectByText, "property-type", "PropertyType"));

                //Ste 2 Property Zip code
                Steps[2] = Step(                                
                                new FossaField(Fill, By.Name("propertyZipCodeInput"), "PropertyZipCode"),
                                new FossaField(Wait, "Wait"));
                
                // Step 3 Select Property use and GUID
                Steps[3] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "mc-HomeEquity"),
                                new FossaField(SelectByText, By.Id("property-use"), "PropertyUse"));

                //Step 4 Property value                
                Steps[4] = Step(
                                new FossaField(SelectByText, By.Id("estproperty-value"), "PropertyValue"),
                                new FossaField(Wait, "Wait"));

                //Step 5 Property purchase price                
                Steps[5] = Step(
                                new FossaField(SelectByText, By.Id("purchase-price"), "PurchasePrice"),
                                new FossaField(Wait, "Wait"));

                //Step 6 Year purchase                 
                Steps[6] = Step(
                                new FossaField(SelectByText, By.Id("purchase-year"), "PurchaseYear"), 
                                new FossaField(Wait, "Wait"));

                //Step 7 have a morgage ?  
                Steps[7] = Step(
                                new FossaField(SelectByText, By.Id("property-mortgage"), "CurrentMortgages"),
                                new FossaField(Wait, "Wait"));
                //Step Check for First Mortgage and First and Second Mortgage
                int stepNum = 8;                                                         
                switch (testData["CurrentMortgages"])
                {
                    case "First mortgage only":
                        Steps[stepNum] = Step(
                                               new FossaField(SelectByText, By.Id("est-mortgage-balance"), "FirstMortgageBalance"),
                                               new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;

                    case "First & second mortgages":
                        Steps[stepNum] = Step(
                                               new FossaField(SelectByText, By.Id("est-mortgage-balance"), "FirstMortgageBalance"),
                                               new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;

                        Steps[stepNum] = Step(
                                               new FossaField(SelectByText, By.Id("second-mortgage-balance"), "SecondMortgageBalance"),
                                               new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;
                    default:
                        Steps[stepNum] = Step(
                                               new FossaField(SelectByText, By.Id("est-mortgage-balance"), "FirstMortgageBalance"),
                                               new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;
                }

                // Step  Amount you would like to borrow  
                Steps[stepNum] = Step(
                                        new FossaField(Fill, "income", "RequestedLoanAmount"),
                                        new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //DOB
                //Your date of birth
                Steps[stepNum] = Step(
                                new FossaField(NoAutoAdvance(SelectByText), "birth-month", "DateOfBirthMonth"),
                                new FossaField(NoAutoAdvance(SelectByText), "birth-day", "DateOfBirthDay"),
                                new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));
                stepNum = stepNum + 1;

                // Step Bankruptcy and Force Clouser                      
                if (testData["ForeclosureHistory"] == "N" && testData["BankruptcyHistory"] == "N")
                {
                    Steps[stepNum] = Step(
                                           new FossaField(ClickRadioWithValueID, "declared-bankruptcy-no"),
                                           new FossaField(Wait, "Wait"));
                                           
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] != "N" && testData["BankruptcyHistory"] != "N")
                {
                    Steps[stepNum] = Step(
                                          new FossaField(ClickRadioWithValueID, "declared-bankruptcy-yes"),
                                          new FossaField(Wait, "Wait"));

                    stepNum = stepNum + 1;

                    //How long ago was the bankruptcy? 
                    Steps[stepNum] = Step(
                                          new FossaField(SelectByText, "bankruptcy-discharged", "BankruptcyHistory"),
                                          new FossaField(Wait, "Wait"));

                    stepNum = stepNum + 1;

                    //How long ago was the foreclosure?
                    Steps[stepNum] = Step(
                                          new FossaField(SelectByText, "foreclosure-text", "ForeclosureHistory"),
                                          new FossaField(Wait, "Wait"));

                    stepNum = stepNum + 1;
                }
                else
                {
                    // Report invalid test data
                    Common.ReportEvent(Common.ERROR, "The values provided for 'ForeclosureHistory' and/or 'BankruptcyHistory' are not valid. " +
                        "Please check the test case data and try again.");
                }

                //Who do you bank with
                Steps[stepNum] = Step(
                                        new FossaField(ClickElement, By.CssSelector("label[for=bank-other]")),
                                        new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //SSN
                Steps[stepNum] = Step(
                                        new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                        new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                        new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                        new FossaField(Wait, "Wait"));

                stepNum = stepNum + 1;

                //Phone number
                Steps[stepNum] = Step(
                                        new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                        new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                        new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                        new FossaField(Wait, "Wait"));

                stepNum = stepNum + 1;
                                                   
                //Detail page
                Steps[stepNum] = Step(
                                        new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                        new FossaField(Fill, "last-name", "BorrowerLastName"),
                                        new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                        new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                        new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                        new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                        new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                        new FossaField(Wait, "Wait"));
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
                Common.ReportEvent("INFO", "Clicking the 'next' button");
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
