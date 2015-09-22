using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.mc
{
    class mcPurchasePage : FossaPageBase
    {
        public mcPurchasePage(IWebDriver driver, Dictionary<string, string> testData)
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
                Int32 numSteps = 11;

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

                Steps[1] = Step(
                                new FossaField(SelectByText, By.Id("property-type"), "PropertyType"));

                Steps[2] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "mc-purchase"),
                                new FossaField(SelectByText, By.Id("property-use"), "PropertyUse"));

                Steps[3] = Step(
                                new FossaField(Fill, By.Name("propertyCityState"), "PropertyCity"),
                                new FossaField(Append, By.Name("propertyCityState"), "PropertyState"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickElement, By.ClassName("dropdown-menu")),
                                new FossaField(Wait, "Wait"));

                Steps[4] = Step(
                                new FossaField(ClickRadioYesNo, "new-home-{0}", "FoundNewHomeYesNo"));

                Steps[5] = Step(
                                new FossaField(SelectByText, By.Id("purchase-price"), "PurchasePrice"));

                Steps[6] = Step(
                                new FossaField(Fill, By.Id("down-payment-amt-dollars"), "PurchaseDownPayment"));

                // Step 7 Military
                Steps[7] = Step(
                        new FossaField(ClickRadioYesNo, "is-veteran-{0}", "MilitaryServiceYesNo"),
                        new FossaField(Wait, "Wait"));

                //Step 8 
                int stepNum = 8;

                if (testData["ForeclosureHistory"] == "Never Foreclosed" && testData["BankruptcyYesNo"] == "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(ClickRadioWithValueID, "bankruptcy-or-foreclosure-no"));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] == "Never Foreclosed" && testData["BankruptcyYesNo"] == "Y")
                {
                    Steps[stepNum] = Step(
                            new FossaField(ClickRadioWithValueID, "bankruptcy-or-foreclosure-bankruptcy"));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                                    new FossaField(SelectByText, By.Id("bankruptcy-discharged"), "BankruptcyHistory"));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] != "Never Foreclosed" && testData["BankruptcyYesNo"] == "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(ClickRadioWithValueID, "bankruptcy-or-foreclosure-foreclosure"));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                            new FossaField(SelectByText, By.Id("foreclosure-text"), "ForeclosureHistory"));
                    stepNum = stepNum + 1;

                }
                else if (testData["ForeclosureHistory"] != "Never Foreclosed" && testData["BankruptcyYesNo"] == "Y")
                {
                    Steps[stepNum] = Step(
                            new FossaField(ClickRadioWithValueID, "bankruptcy-or-foreclosure-both"));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                                    new FossaField(SelectByText, By.Id("bankruptcy-discharged"), "BankruptcyHistory"));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                            new FossaField(SelectByText, By.Id("foreclosure-text"), "ForeclosureHistory"));
                    stepNum = stepNum + 1;

                }
                else
                {
                    // Report invalid test data
                    Common.ReportEvent(Common.ERROR, "The values provided for 'ForeclosureHistory' and/or 'BankruptcyYesNo' are not valid. " +
                        "Please check the test case data and try again.");
                }

                Steps[stepNum] = Step(
                                new FossaField(Fill, By.Id("social-security-one"), "BorrowerSsn1"),
                                new FossaField(Fill, By.Id("social-security-two"), "BorrowerSsn2"),
                                new FossaField(Fill, By.Id("social-security-three"), "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                Steps[stepNum] = Step(
                                new FossaField(Fill, By.Id("first-name"), "BorrowerFirstName"),
                                new FossaField(Fill, By.Id("last-name"), "BorrowerLastName"),
                                new FossaField(Fill, By.Id("street1"), "BorrowerStreetAddress"),
                                //new FossaField(Fill, By.Id("city"), "BorrowerCity"),
                                //new FossaField(SelectByText, By.Id("state"), "BorrowerState"),
                                new FossaField(Fill, By.Id("zip-code-input"), "BorrowerZipCode"),
                                new FossaField(Fill, By.Id("home-phone-area-code"), "BorrowerHomePhone1"),
                                new FossaField(Fill, By.Id("home-phone-prefix"), "BorrowerHomePhone2"),
                                new FossaField(Fill, By.Id("home-phone-line"), "BorrowerHomePhone3"),
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
