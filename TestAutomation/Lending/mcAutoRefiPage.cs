using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.mc
{
    class mcAutoRefiPage : FossaPageBase
    {
        public mcAutoRefiPage(IWebDriver driver, Dictionary<string, string> testData)
            : base(driver, testData)
        {

        }

        public override bool ShouldAutoAdvance
        {
            get { return true; }
        }

        public override void StartForm()
        {
            //Maximize the window
            driver.Manage().Window.Maximize();
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
                Int32 numSteps = 13;

                // Add a step if Borrower's Emploment is Full Time or Partime or Self Employed
                if ((testData["BorrowerEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                                    || testData["BorrowerEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase)
                                                || testData["BorrowerEmploymentStatus"].Equals("Self-Employed", StringComparison.OrdinalIgnoreCase)))
                {
                    numSteps = numSteps + 1;
                }

                // Add a step if the borrower SSN is populated from the test data
                if (!(testData["BorrowerSsn1"].Equals("") || testData["BorrowerSsn1"].Equals(null)))
                {
                    numSteps = numSteps + 1;
                }

                IFormField[][] Steps = new IFormField[numSteps][];

                // Select period of loan               
                Steps[1] = Step(
                                new FossaField(SelectByValue, "loan-period", "AutoLoanTerm"));

                // Select Vehicle info
                Steps[2] = Step(
                                new FossaField(GetAngularQFormUID, "GUID"),
                                new FossaField(SelectByText, "vehicle-year", "VehicleYear"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, "vehicle-make", "VehicleMake"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, "vehicle-model", "VehicleModel"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, "vehicle-trim", "VehicleTrim"),
                                new FossaField(Wait, "Wait"));

                // Enter the VIN and Mileage
                Steps[3] = Step(
                                new FossaField(Fill, "vehicle-identification-number", "VehicleIdNumber"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "vehicle-mileage", "VehicleMileage"),
                                new FossaField(Wait, "Wait"));

                // Enter the current interest rate and lender
                Steps[4] = Step(
                                new FossaField(Fill, "current-interest-rate", "CurrentRate"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "current-lender", "CurrentLender"),
                                new FossaField(Wait, "Wait"));
                // Enter the payoff amount, remaining payments and current monthly payment
                Steps[5] = Step(
                                new FossaField(Fill, "payoff-amount", "CurrentPayoffAmount"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, "remaining-terms", "CurrentRemainingTerms"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "current-vehicle-payment", "CurrentPayment"),
                                new FossaField(ClickButton, "next"),
                                new FossaField(Wait, "Wait"));
                // Select Borrower's bankruptcy option
                int stepNum = 6;
                if (testData["BankruptcyHistory"].Equals("N", StringComparison.OrdinalIgnoreCase)
                            || testData["BankruptcyHistory"].Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    Steps[stepNum] = Step(
                                          new FossaField(ClickRadio, By.Id("declared-bankruptcy-no")),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else
                {
                    Steps[stepNum] = Step(
                                          new FossaField(ClickRadio, By.Id("uniform-declared-bankruptcy-yes")),
                                          new FossaField(Wait, "Wait"),
                                          new FossaField(SelectByText, By.Id("bankruptcy-discharged"), "BankruptcyHistory"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                // Enter Borrower's Employment status
                // Borrower is Full Time or Part Time or Self-Employed
                if ((testData["BorrowerEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                            || testData["BorrowerEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase)
                                        || testData["BorrowerEmploymentStatus"].Equals("Self-Employed", StringComparison.OrdinalIgnoreCase)))
                {
                    Steps[stepNum] = Step(
                                          new FossaField(SelectByText, "employment-status", "BorrowerEmploymentStatus"),
                                          new FossaField(Wait, "Wait"),
                                          new FossaField(SelectByText, "job-start-month", "BorrowerEmploymentTimeMonths"),
                                          new FossaField(Wait, "Wait"),
                                          new FossaField(SelectByText, "job-start-year", "BorrowerEmploymentTimeYears"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                    // Borrower Employer Details
                    Steps[stepNum] = Step(
                                          new FossaField(Fill, "employer-name", "BorrowerEmployerName"),
                                          new FossaField(Wait, "Wait"),
                                          new FossaField(Fill, "job-title", "BorrowerJobTitle"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                // Borrower is Home Maker or Student or Retired
                else
                {
                    Steps[stepNum] = Step(
                                          new FossaField(SelectByText, "employment-status", "BorrowerEmploymentStatus"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                // Enter gross annual earnings and other income
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "income", "BorrowerIncome"),
                                      new FossaField(Fill, "other-income", "BorrowerOtherIncome"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Enter the total liquid assets
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "total-liquid-assets", "BorrowerAssets"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Step Rent/Own house and how long lived there
                Steps[stepNum] = Step(
                                      new FossaField(SelectByValue, "own-or-rent", "BorrowerRentOwn"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(SelectByText, "time-at-address-year", "BorrowerYearsAtAddress"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(SelectByText, "time-at-address-month", "BorrowerMonthsAtAddress"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Enter the mortgage payment
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "current-housing-payment", "BorrowerHousingPayment"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Enter the Borrower SSN if its displayed
                if (!(testData["BorrowerSsn1"].Equals("") || testData["BorrowerSsn1"].Equals(null)))
                {
                    Steps[stepNum] = Step(
                                          new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                          new FossaField(Wait, "Wait"),
                                          new FossaField(ClickElement, By.XPath("//div[@class='col-xs-3 col-md-4']")),
                                          new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                          new FossaField(Wait, "Wait"),
                                          new FossaField(ClickElement, By.XPath("//div[@class='col-xs-5 col-md-4']")),
                                          new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                // Enter/confirm the Borrower details
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                      new FossaField(Fill, "last-name", "BorrowerLastName"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                      new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "city", "BorrowerCity"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                      new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                      new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "work-phone-area-code", "BorrowerWorkPhone1"),
                                      new FossaField(Fill, "work-phone-prefix", "BorrowerWorkPhone2"),
                                      new FossaField(Fill, "work-phone-line", "BorrowerWorkPhone3"),
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
