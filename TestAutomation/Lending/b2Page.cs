using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tlm
{
    class b2Page : FossaPageBase
    {
        public b2Page(IWebDriver driver, Dictionary<string, string> testData)
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

        public IFormField[][] ValidQFStepsWithCrossSell
        {
            get 
            {
                int numSteps = 13;
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
                int numSteps = 12;
                IFormField[][] Steps = new IFormField[numSteps][];

                // Business Type
                string selector = "";
                switch (testData["BusinessType"].ToUpper())
                {
                    case "SOLEPROPRIETORSHIP":
                        selector = "label[for=business-type-sole]";
                        break;
                    case "PARTNERSHIP":
                        selector = "label[for=business-type-partnership]";
                        break;
                    case "CORPORATION":
                        selector = "label[for=business-type-corp]";
                        break;
                    case "SCORPORATION":
                        selector = "label[for=business-type-scorp]";
                        break;
                    case "LLC":
                        selector = "label[for=business-type-llc]";
                        break;
                }
                Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector(selector)),
                                new FossaField(Wait, "Wait"));

                // Loan amount requested
                Steps[2] = Step(
                                new FossaField(Fill, By.Id("amount-requested"), "LoanAmountRequested"),
                                new FossaField(GetAngularQFormUID, "B2"),
                                new FossaField(Wait, "Wait"));
                // Business Inception Date
                Steps[3] = Step(
                                new FossaField(NoAutoAdvance(SelectByValue), "inception-month", "InceptionMonth"),
                                new FossaField(SelectByText, "inception-year", "InceptionYear"),
                                new FossaField(Wait, "Wait"));
                // Annual Revenue
                Steps[4] = Step(new FossaField(Fill, "annual-revenue", "AnnualRevenue"),
                                new FossaField(Wait, "Wait"));
                // is B2B
                switch (testData["B2BYesNo"])
                {
                    case "N":
                        Steps[5] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=business-b2b-no]")),
                            new FossaField(Wait, "Wait"));
                        break;
                    case "Y":
                        Steps[5] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=business-b2b-yes]")),
                            new FossaField(Wait, "Wait"));
                        break;
                }
                // Bankruptcy
                switch (testData["BankruptcyYesNo"])
                {
                    case "N":
                        Steps[6] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=declared-bankruptcy-no]")),
                            new FossaField(Wait, "Wait"));
                        break;
                    case "Y":
                        Steps[6] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=declared-bankruptcy-yes]")),
                            new FossaField(Wait, "Wait"));
                        break;
                }
                // Bankruptcy
                switch (testData["ProfitableYesNo"])
                {
                    case "N":
                        Steps[7] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=business-profitable-no]")),
                            new FossaField(Wait, "Wait"));
                        break;
                    case "Y":
                        Steps[7] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=business-profitable-yes]")),
                            new FossaField(Wait, "Wait"));
                        break;
                }
                // Business name and ZIP
                Steps[8] = Step(new FossaField(Fill, "business-name", "BusinessName"),
                                new FossaField(Fill, "business-zip-code-input", "BusinessZipCode"),
                                new FossaField(Wait, "Wait"));
                // Name
                Steps[9] = Step(
                                new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(Fill, "last-name", "BorrowerLastName"),
                                new FossaField(Wait, "Wait"));
                // Credit profile
                switch (testData["CreditProfile"].ToUpper())
                {
                    case "EXCELLENT":
                        Steps[10] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=owners-credit-history-excellent]")),
                                new FossaField(Wait5Sec, "Wait"));
                        break;
                    case "GOOD":
                        Steps[10] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=owners-credit-history-good]")),
                                new FossaField(Wait5Sec, "Wait"));
                        break;
                    case "FAIR":
                        Steps[10] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=owners-credit-history-fair]")),
                                new FossaField(Wait5Sec, "Wait"));
                        break;
                    case "POOR":
                        Steps[10] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=owners-credit-history-poor]")),
                                new FossaField(Wait5Sec, "Wait"));
                        break;
                }
                // Phone and Email
                Steps[11] = Step(
                                new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
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
