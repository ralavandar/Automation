using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.mc
{
    class mcRefiPage : FossaPageBase
    {
        public mcRefiPage(IWebDriver driver, Dictionary<string, string> testData)
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
                Int32 numSteps = 12;

                // Add a step if bankruptcy is Y
                if (testData["BankruptcyYesNo"] == "Y")
                {
                    numSteps = numSteps + 1;
                }

                // Add a step if foreclosure is Y
                if (testData["ForeclosureHistory"] != "Never Foreclosed")
                {
                    numSteps = numSteps + 1;
                }

                IFormField[][] Steps = new IFormField[numSteps][];

                // Step 1 Select Property type               
                Steps[1] = Step(
                        new FossaField(SelectByText, "property-type", "PropertyType"));

                // Step 2 Select Property use

                Steps[2] = Step(
                                new FossaField(SelectByText, By.Id("property-use"), "PropertyUse"));

                //Find QFID and enter Borrower Zip code
                Steps[3] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "mc-refi"),
                                new FossaField(Fill, By.Name("propertyZipCodeInput"), "PropertyZipCode"),
                                new FossaField(Wait, "Wait"));

                //Property value                
                Steps[4] = Step(
                                new FossaField(SelectByText, By.Id("estproperty-value"), "RefiPropertyValue"),
                                new FossaField(Wait, "Wait"));

                //First Mortgage balance 
                Steps[5] = Step(
                                new FossaField(SelectByText, By.Id("est-mortgage-balance"), "FirstMortgageBalance"),
                                new FossaField(Wait, "Wait"));

                //Step 6  Second Mortgate Yes No                                              
                switch (testData["SecondMortgageYesNo"])
                {

                    case "N":
                        Steps[6] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickRadioYesNo, "have-multiple-mortgages-{0}", "SecondMortgageYesNo"),
                                new FossaField(Wait, "Wait"));

                        break;
                    default:
                        Steps[6] = Step(
                                new FossaField(ClickRadio, By.XPath("//label[@for='have-multiple-mortgages-yes']")),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, By.Id("second-mortgage-balance"), "SecondMortgageBalance"));
                        break;
                }

                // Step 7 RefinCashOut Amount  

                Steps[7] = Step(
                        new FossaField(SelectByText, By.Id("cash-out"), "RefiCashoutAmount"),
                        new FossaField(Wait, "Wait"));

                // Step 8 Military                      
                switch (testData["MilitaryServiceYesNo"])
                {
                    case "N":
                        Steps[8] = Step(
                            new FossaField(ClickRadioYesNo, "is-veteran-{0}", "MilitaryServiceYesNo"),
                            new FossaField(Wait, "Wait"));
                        break;
                    case "Y":
                        if (testData["CurrentLoanVAYesNo"] == "Y")
                        {
                            Steps[8] = Step(
                               new FossaField(ClickRadio, By.XPath("//label[@for='is-veteran-yes']")),
                               new FossaField(Wait, "Wait"),
                               new FossaField(ClickRadio, By.XPath("//label[@for='current-loan-va-yes']")),
                               new FossaField(Wait, "Wait"));
                        }
                        else
                        {
                            Steps[8] = Step(
                               new FossaField(ClickRadio, By.XPath("//label[@for='is-veteran-yes']")),
                               new FossaField(Wait, "Wait"),
                               new FossaField(ClickRadio, By.XPath("//label[@for='current-loan-va-no']")),
                               new FossaField(Wait, "Wait"));
                        }
                        break;
                }

                //Step 9 
                int stepNum = 9;

                if (testData["ForeclosureHistory"] == "Never Foreclosed" && testData["BankruptcyYesNo"] == "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(ClickRadio, By.XPath("//label[@for='bankruptcy-or-foreclosure-no']")));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] == "Never Foreclosed" && testData["BankruptcyYesNo"] == "Y")
                {
                    Steps[stepNum] = Step(
                            new FossaField(ClickRadio, By.XPath("//label[@for='bankruptcy-or-foreclosure-bankruptcy']")));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                                    new FossaField(SelectByText, "bankruptcy-discharged", "BankruptcyHistory"));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] != "Never Foreclosed" && testData["BankruptcyYesNo"] == "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(ClickRadio, By.XPath("//label[@for='bankruptcy-or-foreclosure-foreclosure']")));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                            new FossaField(SelectByText, "foreclosure-text", "ForeclosureHistory"));
                    stepNum = stepNum + 1;

                }
                else if (testData["ForeclosureHistory"] != "Never Foreclosed" && testData["BankruptcyYesNo"] == "Y")
                {
                    Steps[stepNum] = Step(
                            new FossaField(ClickRadio, By.XPath("//label[@for='bankruptcy-or-foreclosure-both']")));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                                    new FossaField(SelectByText, "bankruptcy-discharged", "BankruptcyHistory"),
                                    new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                            new FossaField(SelectByText, "foreclosure-text", "ForeclosureHistory"),
                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;

                }
                else
                {
                    // Report invalid test data
                    Common.ReportEvent(Common.ERROR, "The values provided for 'ForeclosureHistory' and/or 'BankruptcyYesNo' are not valid. " +
                        "Please check the test case data and try again.");
                }

                Steps[stepNum] = Step(
                                new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"));

                stepNum = stepNum + 1;

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
