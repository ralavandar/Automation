using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tla
{
    class blPage : FossaPageBase
    {
        public blPage(IWebDriver driver, Dictionary<string, string> testData)
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

        public IFormField[][] ValidQFStepsWithCrossSell
        {
            get
            {
                int numSteps = 12;
                IFormField[][] steps = ValidQFSteps;

                IFormField[][] stepsWithXsell = new IFormField[numSteps][];

                // copy over Steps 1-4
                for (int i = 1; i < 5; i++)
                {
                    stepsWithXsell.SetValue(steps.GetValue(i), i);
                }

                // insert step 5 cross-sell
                stepsWithXsell[5] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.ClassName("continue-link")),
                                new FossaField(Wait, "Wait"));

                // copy over Steps 5 to End
                for (int i = 5; i < steps.Length; i++)
                {
                    stepsWithXsell.SetValue(steps.GetValue(i), i + 1);
                }

                return stepsWithXsell;
            }
        }

        public override IFormField[][] ValidQFSteps
        {
            get 
            {
                int numSteps = 11;
                IFormField[][] Steps = new IFormField[numSteps][];

                // Business Type
                Steps[1] = Step(
                                new FossaField(SelectByValue, "business-type", "BusinessType"),
                                new FossaField(Wait, "Wait"));
                // Loan amount requested
                Steps[2] = Step(
                                //new FossaField(SelectByText, "ig-requested-loan-amount", "LoanAmountRequested"),
                                new FossaField(SelectByText, By.Name("requested-loan-amount"), "LoanAmountRequested"),
                                new FossaField(GetAngularQFormUID, "BL"),
                                new FossaField(Wait, "Wait"));
                // Business Inception Date
                Steps[3] = Step(
                                new FossaField(NoAutoAdvance(SelectByText), "inception-month", "InceptionMonth"),
                                new FossaField(SelectByText, "inception-year", "InceptionYear"),
                                new FossaField(Wait, "Wait"));
                // Annual Revenue
                Steps[4] = Step(
                                new FossaField(Fill, "annual-revenue", "AnnualRevenue"),
                                new FossaField(Wait, "Wait"));
                // Bankruptcy
                Steps[5] = Step(
                                new FossaField(ClickRadioYesNo, "declared-bankruptcy-{0}", "BankruptcyYesNo"),
                                new FossaField(Wait, "Wait"));
                // Profitable
                Steps[6] = Step(
                                new FossaField(ClickRadioYesNo, "business-profitable-{0}", "ProfitableYesNo"),
                                new FossaField(Wait, "Wait"));
                // Business name and ZIP
                Steps[7] = Step(new FossaField(Fill, "business-name", "BusinessName"),
                                new FossaField(Fill, "business-zip-code-input", "BusinessZipCode"),
                                new FossaField(Wait, "Wait"));
                // Name
                Steps[8] = Step(
                                new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(Fill, "last-name", "BorrowerLastName"),
                                new FossaField(Wait, "Wait"));
                // Credit rating
                switch (testData["CreditProfile"].ToUpper())
                {
                    case "EXCELLENT":
                        Steps[9] = Step(
                                new FossaField(ClickRadioWithValueID, "stated-credit-history-excellent"),
                                new FossaField(Wait5Sec, "Wait"));
                        break;
                    case "GOOD":
                        Steps[9] = Step(
                                new FossaField(ClickRadioWithValueID, "stated-credit-history-good"),
                                new FossaField(Wait5Sec, "Wait"));
                        break;
                    case "FAIR":
                        Steps[9] = Step(
                                new FossaField(ClickRadioWithValueID, "stated-credit-history-fair"),
                                new FossaField(Wait5Sec, "Wait"));
                        break;
                    case "POOR":
                        Steps[9] = Step(
                                new FossaField(ClickRadioWithValueID, "stated-credit-history-poor"),
                                new FossaField(Wait5Sec, "Wait"));
                        break;
                    default:
                        // Report invalid test data
                        Common.ReportEvent(Common.ERROR, "The value provided for 'CreditProfile' is not valid. " +
                            "Please check the test case data and try again.");
                        break;
                }
                // Phone and Email
                Steps[10] = Step(
                                new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                new FossaField(DeselectAfter(Fill), "email", "EmailAddress"),
                                new FossaField(Wait5Sec, "Wait"));

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
