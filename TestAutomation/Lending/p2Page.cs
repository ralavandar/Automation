using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tlm
{
    class p2Page : FossaPageBase
    {
        public p2Page(IWebDriver driver, Dictionary<string, string> testData)
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
                // Number of steps depends on credit pull pass/fail
                Int32 numSteps;
                if (testData["CreditPullSuccessYesNo"] == "N")
                    numSteps = 16;
                else
                    numSteps = 15;

                IFormField[][] Steps = new IFormField[numSteps][];

                // Loan purpose
                Steps[1] = Step(
                                //new FossaField(SelectByText, By.CssSelector("select[ng-model=inputs.loanPurpose]"), "LoanPurpose"),
                                new FossaField(SelectByText, By.CssSelector("[ng-model='inputs.loanPurpose']"), "LoanPurpose"),
                                new FossaField(Wait, "Wait"));
                // Loan amount requested
                Steps[2] = Step(
                                new FossaField(Fill, By.Name("requested-loan-amount"), "LoanAmountRequested"),
                                new FossaField(GetAngularQFormUID, "P2"),
                                new FossaField(Wait, "Wait"));
                // ZIP code
                Steps[3] = Step(new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                new FossaField(Wait, "Wait"));
                // Street address
                Steps[4] = Step(new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                new FossaField(Wait, "Wait"));
                // Residence type
                switch (testData["Residence"].ToUpper())
                {
                    case "OWN":
                        Steps[5] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=residence-type-own]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "RENT":
                        Steps[5] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=residence-type-rent]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "OTHER":
                        Steps[5] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=residence-type-other]")),
                                new FossaField(Wait, "Wait"));
                        break;
                }
                // Credit profile
                switch (testData["CreditProfile"].ToUpper())
                {
                    case "EXCELLENT":
                        Steps[6] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-excellent]")));
                        break;
                    case "GOOD":
                        Steps[6] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-good]")));
                        break;
                    case "FAIR":
                        Steps[6] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-fair]")));
                        break;
                    case "POOR":
                        Steps[6] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-poor]")));
                        break;
                }
                // Date of birth
                Steps[7] = Step(
                                new FossaField(NoAutoAdvance(SelectByText), "birth-month", "DateOfBirthMonth"),
                                new FossaField(NoAutoAdvance(SelectByText), "birth-day", "DateOfBirthDay"),
                                new FossaField(SelectByText, "birth-year", "DateOfBirthYear"),
                                new FossaField(Wait, "Wait"));
                // Employment status
                switch (testData["EmploymentStatus"])
                {
                    case "Full-time":
                        Steps[8] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=employment-status-fulltime]")));
                        break;
                    case "Part-time":
                        Steps[8] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=employment-status-parttime]")));
                        break;
                    case "Self-employed":
                        Steps[8] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=employment-status-selfemployed]")));
                        break;
                    case "Unemployed":
                        Steps[8] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=employment-status-unemployed]")));
                        break;
                    case "Other":
                        Steps[8] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=employment-status-other]")));
                        break;
                }
                // Income
                Steps[9] = Step(new FossaField(Fill, "income", "BorrowerIncome"),
                                new FossaField(Wait, "Wait"));
                // Name
                Steps[10] = Step(
                                new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(Fill, "last-name", "BorrowerLastName"),
                                new FossaField(Wait, "Wait"));
                // Home phone
                Steps[11] = Step(
                                new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"));
                // Email
                Steps[12] = Step(
                                new FossaField(DeselectAfter(Fill), "email", "EmailAddress"),
                                new FossaField(Wait5Sec, "Wait"));
                // Password
                Steps[13] = Step(
                                new FossaField(Fill, "password", "Password"),
                                new FossaField(Wait, "Wait"));
                // SSN
                switch (testData["BorrowerSsn3"])
                {
                    case "":
                        Steps[14] = Step(
                                new FossaField(Wait, "Wait"));
                        break;
                    default:
                        Steps[14] = Step(
                                new FossaField(Fill, "social-security-four", "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"));
                        break;
                }
                // Step 14 - Oops step if credit pull fails. 
                if (testData["CreditPullSuccessYesNo"] == "N")
                {
                    Steps[15] = Step(
                                new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(Fill, "last-name", "BorrowerLastName"),
                                new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                // The following is an 'xpath' hack b/c there are two identical 'social-security' elements on the oops page
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
