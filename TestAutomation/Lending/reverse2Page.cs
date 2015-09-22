using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tla
{
    class reverse2Page : FossaPageBase
    {
        public reverse2Page(IWebDriver driver, Dictionary<string, string> testData)
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

            NavigateToFossaForm(testData["TestEnvironment"], "tla.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);
        }

        public override IFormField[][] ValidQFSteps
        {
            get 
            {
                // Number of steps depends on targus pass/fail
                Int32 numSteps;
                if (testData["TargusPassYesNo"] == "N")
                    numSteps = 11;
                else
                    numSteps = 10;

                IFormField[][] Steps = new IFormField[numSteps][];

                // Step 1
                switch (testData["PropertyType"])
                {
                    case "Single family home":
                        Steps[1] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[1] = Step(
                                new FossaField(SelectByText, "property-type", "PropertyType"));
                        break;
                }

                // Step 2
                switch (testData["PropertyUse"])
                {
                    case "Primary residence":
                        Steps[2] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[2] = Step(
                                new FossaField(SelectByText, "property-use", "PropertyUse"));
                        break;
                }

                Steps[3] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "reverse2"),
                                new FossaField(SelectByText, "estproperty-value", "RefiPropertyValue"));

                Steps[4] = Step(
                                new FossaField(SelectByText, "est-mortgage-balance", "FirstMortgageBalance"));

                Steps[5] = Step(
                                new FossaField(Fill, "property-zip-code-input", "PropertyZipCode"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));

                // If age >= 62, then answer Yes and select the age from the dropdown; else, answer No.
                if (Convert.ToInt32(testData["Age"]) >= 62)
                {
                    Steps[6] = Step(
                                new FossaField(ClickRadio, "ig-age-eligible-yes"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, By.Name("eligibleAge"), "Age"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                }
                else
                {
                    Steps[6] = Step(
                                new FossaField(ClickRadio, "ig-age-eligible-no"),
                                new FossaField(Wait, "Wait"));
                }

                Steps[7] = Step(
                                new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(Fill, "last-name", "BorrowerLastName"),
                                new FossaField(ClickButton, "next"));

                Steps[8] = Step(
                                new FossaField(Fill, "email", "EmailAddress"),
                                new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));

                Steps[9] = Step(
                                new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "password", "Password"),
                                new FossaField(ClickButton, "next"));

                // Handle Oops step (displays if targus call fails to validate user data)
                if (testData["TargusPassYesNo"] == "N")
                {
                    Steps[10] = Step(
                            new FossaField(Wait, "Wait"),
                            new FossaField(ClickButton, "next"));
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
            System.Threading.Thread.Sleep(100);
        }
    }
}
