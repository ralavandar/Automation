using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.mc
{
    class mcStudentRefiPage : FossaPageBase
    {
        public mcStudentRefiPage(IWebDriver driver, Dictionary<string, string> testData)
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
                Int32 numSteps = 8;

                //If Highest Degree is Masters or Doctorate then step added
                if ((testData["BorrowerHighestDegree"].Equals("Masters", StringComparison.OrdinalIgnoreCase)
                            || testData["BorrowerHighestDegree"].Equals("Doctorate", StringComparison.OrdinalIgnoreCase)))
                    numSteps = numSteps + 1;

                //If Borrower Employemnt is Full Time or Part Time then increase step
                if ((testData["BorrowerEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                            || testData["BorrowerEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase)))
                    numSteps = numSteps + 1;

                IFormField[][] Steps = new IFormField[numSteps][];

                Steps[1] = Step(
                                new FossaField(SelectByValue, "loan-type", "LoanType"),
                                new FossaField(Wait, "Wait"));

                Steps[2] = Step(
                                new FossaField(SelectByText, "degree-obtained", "BorrowerHighestDegree"),
                                new FossaField(Wait, "Wait"));

                int stepNum = 3;
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
                                      new FossaField(Wait, "Wait"),
                    //new FossaField(SelectByText, By.XPath("//*[@class='dropdown-menu']//li"), "LastInstitutionAttended"),
                                      new FossaField(ClickElement, By.XPath("//*[@class='dropdown-menu ng-isolate-scope']//li")),
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

                // Borrower S.S.N
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                      new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                      new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // confirm the Borrower information
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                      new FossaField(Fill, "last-name", "BorrowerLastName"),
                                      new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                      new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                      new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                      new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

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