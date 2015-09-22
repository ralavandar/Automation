using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.mc
{
    class mcPersonalLoanPage : FossaPageBase
    {
        public mcPersonalLoanPage(IWebDriver driver, Dictionary<string, string> testData)
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

            NavigateToFossaForm(testData["TestEnvironment"], "mc.aspx", "mc-personal", testData["Variation"], testData["QueryString"]);

            // Login to MC using the email address and pwd in the test case data
            Fill(By.Id("UserName"), testData["EmailAddress"]);
            Fill(By.Id("Password"), testData["Password"]);
            ClickElement(By.ClassName("sign-in-button"));
        }

        public override IFormField[][] ValidQFSteps
        {
            get
            {
                //Defining Number of steps 
                Int32 numSteps = 7;

                if (!testData["DateOfBirthMonth"].Equals(""))
                {
                    numSteps = numSteps + 1;
                }

                if (!testData["BorrowerSsn1"].Equals(""))
                {
                    numSteps = numSteps + 1;
                }

                IFormField[][] Steps = new IFormField[numSteps][];

                //Step 1 Select the Loan purpose
                Steps[1] = Step(new FossaField(SelectByText, "loan-purpose", "LoanPurpose"));

                //Step 2 Select the Loan loan amount and fetch the Qform UID
                Steps[2] = Step(new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "requestedLoanAmount", "LoanAmountRequested"),
                                new FossaField(GetAngularQFormUID, "mc-personal"));

                // Step 3 Select Residence type
                Steps[3] = Step(new FossaField(SelectByValue, "residence", "Residence"),
                                new FossaField(Wait, "Wait"));

                // Step 4 Select the Employment status
                Steps[4] = Step(new FossaField(SelectByText, "employment-status", "EmploymentStatus"));

                // Step 5 Enter the borrower income
                Steps[5] = Step(new FossaField(Fill, "income", "BorrowerIncome"),
                                    new FossaField(Wait, "Wait"));

                int stepNum = 6;

                // Step N - If the SSN field is displayed, enter it
                if (!testData["BorrowerSsn1"].Equals(""))
                {
                    Steps[stepNum] = Step(
                                new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                // Step N - If the DOB field is displayed, enter it
                if (!testData["DateOfBirthMonth"].Equals(""))
                {
                    Steps[stepNum] = Step(new FossaField(SelectByText, "birth-month", "DateOfBirthMonth"),
                                new FossaField(SelectByText, "birth-day", "DateOfBirthDay"),
                                new FossaField(SelectByText, "birth-year", "DateOfBirthYear"),
                                new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                
                // Step N - Enter the personal information
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


        private void Switch(string p)
        {
            throw new NotImplementedException();
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
