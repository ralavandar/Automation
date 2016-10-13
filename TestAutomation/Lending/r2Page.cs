using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tlm
{
    class r2Page : FossaPageBase
    {
        public r2Page(IWebDriver driver, Dictionary<string, string> testData)
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
                // Number of steps depends on targus pass/fail
                Int32 numSteps;
                if (testData["TargusPassYesNo"] == "N")
                    numSteps = 11;
                else
                    numSteps = 10;

                IFormField[][] Steps = new IFormField[numSteps][];

                // STEP 1 What Type Of Property?
                switch (testData["PropertyType"])
                {
                    case "Single Family Home":
                        Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-single-fam]")),
                                new FossaField(Wait, "Wait"));
                        break;

                    case "Townhome":
                        Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-townhome]")),
                                new FossaField(Wait, "Wait"));
                        break;

                    case "Condominium":
                        Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-condo]")),
                                new FossaField(Wait, "Wait"));
                        break;

                    case "Multi family home":
                        Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-multi]")),
                                new FossaField(Wait, "Wait"));
                        break;

                    case "Manufactured / Mobile":
                        Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-mobile]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    default:
                        Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-single-fam]")),
                                new FossaField(Wait, "Wait"));
                        break;
                }

                // STEP 2  How Will This Property Be Used?
                switch (testData["PropertyUse"])
                {
                    case "Primary Home":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-primary]")),
                                new FossaField(Wait, "Wait"));
                        break;

                    case "Secondary Home":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-secondary]")),
                                new FossaField(Wait, "Wait"));
                        break;

                    case "Rental Property":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-rental]")),
                                new FossaField(Wait, "Wait"));
                        break;

                    default:
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-primary]")),
                                new FossaField(Wait, "Wait"));
                        break;
                }

                //STEP 3 Estimated Property Value
                Steps[3] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "r2"),
                                new FossaField(SelectSliderValueByText, testData["RefiPropertyValue"]));

                //STEP 4 What's Your Mortgage Balance?
                Steps[4] = Step(
                                new FossaField(SelectSliderValueByText, testData["FirstMortgageBalance"]),
                                new FossaField(Wait, "Wait"));

                //STEP 5 Property ZIP Code
                Steps[5] = Step(
                                new FossaField(NoAutoAdvance(Fill), "property-zip-code-input", "PropertyZipCode"),
                                new FossaField(Wait, "Wait"));

                // STEP 6 If age >= 62, then answer Yes and select the age from the dropdown; else, answer No.
                if (Convert.ToInt32(testData["Age"]) >= 62)
                {
                    Steps[6] = Step(
                                new FossaField(ClickElement, By.CssSelector("label[for=ig-age-eligible-yes]")),
                                new FossaField(SelectByText, By.Name("eligibleAge"), "Age"),
                                new FossaField(Wait, "Wait"));
                }
                else
                {
                    Steps[6] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=ig-age-eligible-no]")),
                                new FossaField(Wait, "Wait"));
                }

                //STEP 7 Fisrts Name/ Last Name
                Steps[7] = Step(
                                new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(NoAutoAdvance(Fill), "last-name", "BorrowerLastName"));

                //STEP 8 Email Adress and Phone Number
                Steps[8] = Step(
                                new FossaField(Fill, "email", "EmailAddress"),
                                new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"));

                //STEP 9 Stree Address
                Steps[9] = Step(
                                new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(NoAutoAdvance(Fill), "password", "Password"));

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
