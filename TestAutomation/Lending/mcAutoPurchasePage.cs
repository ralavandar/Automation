using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.mc
{
    class mcAutoPurchasePage : FossaPageBase
    {
        public mcAutoPurchasePage(IWebDriver driver, Dictionary<string, string> testData)
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
                Int32 numSteps;

                // Decide number of Steps  if Co-Borrower is not as applicant
                numSteps = (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase)) ? 21 : 12;

                // Add a step if Borrower's Emploment is Full Time or Partime or Self Employed
                if ((testData["BorrowerEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                                    || testData["BorrowerEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase)
                                                || testData["BorrowerEmploymentStatus"].Equals("Self-Employed", StringComparison.OrdinalIgnoreCase)))
                {
                    numSteps = numSteps + 1;
                }

                // Add a step if Co-borrower's Emploment is Full Time or Partime or Self Employed
                if (testData["CoEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                                        || testData["CoEmploymentStatus"].Equals("Self-Employed", StringComparison.OrdinalIgnoreCase)
                                                    || testData["CoEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase))
                {
                    numSteps = numSteps + 1;
                }

                // Add a step if Auto Loan Type is 'Used Car Purchage' to accomodate VIN and Milage
                if (testData["AutoLoanType"].Equals("UsedCarPurchase", StringComparison.OrdinalIgnoreCase))
                {
                    numSteps = numSteps + 1;
                }

                //Add a step if the borrower SSN is populated from the test data
                if (!(testData["BorrowerSsn1"].Equals("") || testData["BorrowerSsn1"].Equals(null)))
                {
                    numSteps = numSteps + 1;
                }

                //Add a step if the Co-borrower SSN is populated from the test data
                if (!(testData["CoborrowerSsn1"].Equals("") || testData["CoborrowerSsn1"].Equals(null)))
                {
                    numSteps = numSteps + 1;
                }

                IFormField[][] Steps = new IFormField[numSteps][];

                // Step 1 Select Type of loan, Loan Term (New Car and Used Car                
                Steps[1] = Step(
                                new FossaField(SelectByValue, "loan-type", "AutoLoanType"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByValue, "loan-period", "AutoLoanTerm"),
                                new FossaField(Wait, "Wait"));

                // Step 2 select Coborrower (vehicle state and coborrower)
                Steps[2] = Step(
                                new FossaField(GetAngularQFormUID, "GUID"),
                                new FossaField(SelectByText, "vehicle-state", "VehicleState"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickRadioYesNo, "cb-relationship-{0}", "CoborrowerYesNo"),
                                new FossaField(Wait, "Wait"));

                // Step 3 Downpayment, Loan Amount
                Steps[3] = Step(
                                new FossaField(Fill, "down-payment", "PurchaseDownPayment"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "requestedLoanAmount", "RequestedLoanAmount"),
                                new FossaField(Wait, "Wait"));

                // Step 4 Fill Vehicle Details
                Steps[4] = Step(
                                new FossaField(SelectByText, By.Id("vehicle-year"), "VehicleYear"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, By.Id("vehicle-make"), "VehicleMake"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, By.Id("vehicle-model"), "VehicleModel"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, By.Id("vehicle-trim"), "VehicleTrim"),
                                new FossaField(Wait, "Wait"));

                int stepNum = 5;

                // Step 5 
                //VIN and Milage
                if (testData["AutoLoanType"].Equals("UsedCarPurchase", StringComparison.OrdinalIgnoreCase))
                {
                    Steps[stepNum] = Step(
                                          new FossaField(Fill, "vehicle-identification-number", "VehicleIdNumber"),
                                          new FossaField(Fill, "vehicle-mileage", "VehicleMileage"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                //Borrower bankruptcy
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

                // Borrower Employment Status
                // Borrower is Full Time or Part Time or Self-Employed
                if ((testData["BorrowerEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                            || testData["BorrowerEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase)
                                        || testData["BorrowerEmploymentStatus"].Equals("Self-Employed", StringComparison.OrdinalIgnoreCase)))
                {
                    Steps[stepNum] = Step(
                                          new FossaField(SelectByText, "employment-status", "BorrowerEmploymentStatus"),
                                          new FossaField(Wait, "Wait"),
                                          new FossaField(SelectByText, "job-start-month", "BorrowerEmploymentTimeMonths"),
                                          new FossaField(SelectByText, "job-start-year", "BorrowerEmploymentTimeYears"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                    // Borrower Employer Details
                    Steps[stepNum] = Step(
                                          new FossaField(Fill, "employer-name", "BorrowerEmployerName"),
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

                // Borrower Gross annual earnings and Other
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "income", "BorrowerIncome"),
                                      new FossaField(Fill, "other-income", "BorrowerOtherIncome"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Borrower Total liquid assets
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "total-liquid-assets", "BorrowerAssets"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Borrower House own or Rent and How long
                Steps[stepNum] = Step(
                                      new FossaField(SelectByValue, "own-or-rent", "BorrowerRentOwn"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(SelectByText, "time-at-address-year", "BorrowerYearsAtAddress"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(SelectByText, "time-at-address-month", "BorrowerMonthsAtAddress"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Borrower Monthly rent or mortage payment
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "current-housing-payment", "BorrowerHousingPayment"));
                stepNum = stepNum + 1;

                //Enter the Borrower SSN if its displayed
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

                //Confirm Borrower Information
                Steps[stepNum] = Step(
                                      new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                      new FossaField(Fill, "last-name", "BorrowerLastName"),
                                      new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                      new FossaField(Fill, By.Id("zip-code-input"), "BorrowerZipCode"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "city", "BorrowerCity"),
                                      new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                      new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                      new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "work-phone-area-code", "BorrowerWorkPhone1"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "work-phone-prefix", "BorrowerWorkPhone2"),
                                      new FossaField(Wait, "Wait"),
                                      new FossaField(Fill, "work-phone-line", "BorrowerWorkPhone3"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Steps for Co-brorrower Informations
                if (!testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    return Steps;
                }

                //Co-borrower Information 
                Steps[stepNum] = Step(
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(Fill, "cb-first-name", "CoborrowerFirstName"),
                                        new FossaField(Fill, "cb-last-name", "CoborrowerLastName"),
                                        new FossaField(Fill, "cb-email", "CoEmailAddress"),
                                        new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Co-borrower's date of birth
                Steps[stepNum] = Step(
                                        new FossaField(SelectByText, By.Id("cb-birth-month"), "CoDateOfBirthMonth"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(SelectByText, By.Id("cb-birth-day"), "CoDateOfBirthDay"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(SelectByText, By.Id("cb-birth-year"), "CoDateOfBirthYear"),
                                        new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Co-Borrower Bankruptcy
                if (testData["CoBankruptcyHistory"].Equals("N", StringComparison.OrdinalIgnoreCase)
                                    || testData["CoBankruptcyHistory"].Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    Steps[stepNum] = Step(
                                            new FossaField(ClickRadio, By.Id("cb-declared-bankruptcy-no")),
                                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else
                {
                    Steps[stepNum] = Step(
                                            new FossaField(ClickRadio, By.Id("uniform-cb-declared-bankruptcy-yes")),
                                            new FossaField(Wait, "Wait"),
                                            new FossaField(SelectByText, By.Id("cb-bankruptcy-discharged"), "CoBankruptcyHistory"),
                                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                //Co-Borrower Employment Status
                if (testData["CoEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                                || testData["CoEmploymentStatus"].Equals("Self-Employed", StringComparison.OrdinalIgnoreCase)
                                            || testData["CoEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase))
                {
                    Steps[stepNum] = Step(
                                            new FossaField(SelectByText, "cb-employment-status", "CoEmploymentStatus"),
                                            new FossaField(Wait, "Wait"),
                                            new FossaField(SelectByText, "cb-job-start-month", "CoEmploymentTimeMonths"),
                                            new FossaField(SelectByText, "cb-job-start-year", "CoEmploymentTimeYears"),
                                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;

                    //Co-Borrower Employer
                    Steps[stepNum] = Step(
                                            new FossaField(Fill, "cb-employer-name", "CoEmployerName"),
                                            new FossaField(Fill, "cb-job-title", "CoJobTitle"),
                                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else
                {
                    // Co-borrower is Home Maker or Student or Retired
                    Steps[stepNum] = Step(
                                            new FossaField(SelectByText, "cb-employment-status", "CoEmploymentStatus"),
                                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                //Co-borrower's gross annual earnings
                Steps[stepNum] = Step(
                                        new FossaField(Fill, "cb-income", "CoborrowerIncome"),
                                        new FossaField(Fill, "cb-other-income", "CoborrowerOtherIncome"),
                                        new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Co-borrower's total liquid assets
                Steps[stepNum] = Step(
                                        new FossaField(Fill, "cb-total-liquid-assets", "CoborrowerAssets"),
                                        new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Co-borrower's street address
                Steps[stepNum] = Step(
                                        new FossaField(Fill, "street2", "CoStreetAddress"),
                                        new FossaField(Fill, "cb-zip-code-input", "CoborrowerZipCode"),
                                        new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Does the co-borrower rent or own their house
                Steps[stepNum] = Step(
                                        new FossaField(SelectByValue, "cb-own-or-rent", "CoborrowerRentOwn"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(SelectByText, "cb-time-at-address-year", "CoYearsAtAddress"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(SelectByText, "cb-time-at-address-month", "CoMonthsAtAddress"),
                                        new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //co-borrower's phone number
                Steps[stepNum] = Step(
                                        new FossaField(Fill, "cb-home-phone-area-code", "CoborrowerHomePhone1"),
                                        new FossaField(Fill, "cb-home-phone-prefix", "CoborrowerHomePhone2"),
                                        new FossaField(Fill, "cb-home-phone-line", "CoborrowerHomePhone3"),
                                        new FossaField(Fill, "cb-work-phone-area-code", "CoborrowerWorkPhone1"),
                                        new FossaField(Fill, "cb-work-phone-prefix", "CoborrowerWorkPhone2"),
                                        new FossaField(Fill, "cb-work-phone-line", "CoborrowerWorkPhone3"),
                                        new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Enter Co-Borrower SSN if its displayed
                if (!(testData["CoborrowerSsn1"].Equals("") || testData["CoborrowerSsn1"].Equals(null)))
                {
                    Steps[stepNum] = Step(
                                            new FossaField(Fill, "cb-social-security-one", "CoborrowerSsn1"),
                                            new FossaField(Wait, "Wait"),
                                            new FossaField(ClickElement, By.XPath("//div[@class='col-xs-3 col-md-4']")),
                                            new FossaField(Fill, "cb-social-security-two", "CoborrowerSsn2"),
                                            new FossaField(Wait, "Wait"),
                                            new FossaField(ClickElement, By.XPath("//div[@class='col-xs-5 col-md-4']")),
                                            new FossaField(Wait, "Wait"),
                                            new FossaField(Fill, "cb-social-security-three", "CoborrowerSsn3"),
                                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
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

