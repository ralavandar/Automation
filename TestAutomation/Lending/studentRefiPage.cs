using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tla
{
    class studentRefiPage : FossaPageBase
    {
        public studentRefiPage(IWebDriver driver, Dictionary<string, string> testData)
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
                Int32 numSteps = 14;

                //If Highest Degree is Masters or Doctorate then step added
                if ((testData["BorrowerHighestDegree"].Equals("Masters", StringComparison.OrdinalIgnoreCase)
                            || testData["BorrowerHighestDegree"].Equals("Doctorate", StringComparison.OrdinalIgnoreCase)))
                    numSteps = numSteps + 1;

                //If Borrower Employemnt is Full Time or Part Time then increase step
                if ((testData["BorrowerEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                            || testData["BorrowerEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase)))
                    numSteps = numSteps + 1;

                // If targus fails, then show/handle Oops step (phone # confirmation step) - coming soon per LCO-1761
                if (testData["TargusPassYesNo"] == "N")
                    numSteps = numSteps + 1;

                // If creditpull fails, then show/handle Oops step (confirmation step)
                if (testData["CreditPullSuccessYesNo"] == "N")
                    numSteps = numSteps + 1;

                IFormField[][] Steps = new IFormField[numSteps][];

                // Select Borrower Highest Degree
                Steps[1] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, "degree-obtained", "BorrowerHighestDegree"),
                                new FossaField(Wait, "Wait"));

                int stepNum = 2;
                //Specify Graduate Degree when Borrower Highest degree is Masters or Doctorate
                if ((testData["BorrowerHighestDegree"].Equals("Masters", StringComparison.OrdinalIgnoreCase)
                            || testData["BorrowerHighestDegree"].Equals("Doctorate", StringComparison.OrdinalIgnoreCase)))
                {
                    Steps[stepNum] = Step(
                                          new FossaField(SelectByText, "graduate-degree", "BorrowerGraduateDegree"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;

                }

                // Specify Last Institution Attended                
                Steps[stepNum] = Step(
                                      new FossaField(GetAngularQFormUID, "GUID"),
                                      new FossaField(Fill, "college-name", "LastInstitutionAttended"),
                                      new FossaField(Wait5Sec, "Wait"),
                                      new FossaField(ClickElement, By.CssSelector("[ng-click=\"selectMatch($index)\"]")),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Specify student loan amount
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "requested-loan-amount-input", "RequestedLoanAmount"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Specify Borrower Employment Status
                Steps[stepNum] = Step(
                                    new FossaField(SelectByText, "employment-status", "BorrowerEmploymentStatus"),
                                    new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //If Borrower is Full Time or Part Time Employed then Fill Employer
                if ((testData["BorrowerEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                            || testData["BorrowerEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase)))
                {
                    Steps[stepNum] = Step(
                                          new FossaField(Fill, "employer-name", "BorrowerEmployerName"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                //Specify Borrower Annual Pre-Tax Income
                Steps[stepNum] = Step(
                                      new FossaField(SelectByText, "annual-income", "BorrowerPreTaxIncome"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Specify Citizenship Status
                Steps[stepNum] = Step(
                                      new FossaField(SelectByText, "citizenship-status", "Citizenship"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Specify Borrower estimated credit rating
                switch (testData["CreditProfile"].ToUpper())
                {
                    case "EXCELLENT":
                        Steps[stepNum] = Step(new FossaField(ClickRadio, "stated-credit-history-excellent"));
                        stepNum = stepNum + 1;
                        break;
                    case "GOOD":
                        //Steps[stepNum] = Step(new FossaField(ClickRadio, "stated-credit-history-good"));
                        Steps[stepNum] = Step(new FossaField(ClickButton, "next"));
                        stepNum = stepNum + 1;
                        break;
                    case "FAIR":
                        Steps[stepNum] = Step(new FossaField(ClickRadio, "stated-credit-history-fair"));
                        stepNum = stepNum + 1;
                        break;
                    case "POOR":
                        Steps[stepNum] = Step(new FossaField(ClickRadio, "stated-credit-history-poor"));
                        stepNum = stepNum + 1;
                        break;
                    default:
                        stepNum = stepNum + 1;
                        break;
                }

                //Specify Borrower Zip
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Borrower Date Of Birth
                Steps[stepNum] = Step(
                                      new FossaField(SelectByText, By.Id("birth-month"), "DateOfBirthMonth"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(SelectByText, By.Id("birth-day"), "DateOfBirthDay"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(SelectByText, By.Id("birth-year"), "DateOfBirthYear"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Borrower First Name Last Name
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                      new FossaField(Fill, "last-name", "BorrowerLastName"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Borrower Email Address and Password
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "email", "EmailAddress"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "password", "Password"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Borrower Phone Number
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                      new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                      new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Borrower SSN
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "social-security-four", "BorrowerSsn3"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Phone number Oops step (only if targus fails) - coming soon per LCO-1761
                if (testData["TargusPassYesNo"] == "N")
                {
                    Steps[stepNum] = Step(
                                      new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                      new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                      new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                      new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                      new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                      new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                      new FossaField(Fill, "last-name", "BorrowerLastName"),
                                      new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                // Credit Pull Oops step (only if credit pull fails)
                if (testData["CreditPullSuccessYesNo"] == "N")
                {
                    Steps[stepNum] = Step(
                                      new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                      new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                      new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
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
