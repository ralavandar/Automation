using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tlm
{
    class a2Page : FossaPageBase
    {
        public a2Page(IWebDriver driver, Dictionary<string, string> testData)
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
                // Initialize the array 
                Int32 numSteps = 22;

                // Add a step if Borrower's Emploment is Full Time or Partime or Self Employed
                if ((testData["BorrowerEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                                    || testData["BorrowerEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase)
                                                || testData["BorrowerEmploymentStatus"].Equals("Self-Employed", StringComparison.OrdinalIgnoreCase)))
                {
                    numSteps = numSteps + 2;
                }

                // Add a steps if Auto Loan Type is 'Used Car Purchase' to accomodate 'From Whom Are You Buying?', 'Found Vehicle?' and 'Milage'.
                //Add a step if Auto Loan Type is 'Refinance' or 'Lease Buy Out'to accomodate VIN etc
                if (!(testData["AutoLoanType"].Equals("New Car Purchase", StringComparison.OrdinalIgnoreCase)))
                {
                    numSteps = numSteps + 3;
                }

                //Add a step if Bankruptcy is Yes
                if (!(testData["BankruptcyHistory"].Equals("N", StringComparison.OrdinalIgnoreCase)
                            || testData["BankruptcyHistory"].Equals("", StringComparison.OrdinalIgnoreCase)))
                {
                    numSteps = numSteps + 1;
                }

                //Credit pull and Targus both fail then add 2 steps (1. Phone number check and 2 SSN verification)
                if ((testData["CreditPullSuccessYesNo"] == "N") & (testData["TargusPassYesNo"] == "N"))
                {
                    numSteps = numSteps + 2;
                }

                //Credit pull fail and Targus pass then Phone number verification not happen only SSN is verified
                if ((testData["CreditPullSuccessYesNo"] == "N") & (testData["TargusPassYesNo"] == "Y"))
                {
                    numSteps = numSteps + 1;
                }
                

                IFormField[][] Steps = new IFormField[numSteps][];

                //Loan Type 
                Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='" + testData["AutoLoanType"] + "']")),
                                new FossaField(Wait, "Wait"));
                //Desired Loan Term
                Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.XPath("//div[contains(text(),'" + testData["AutoLoanTerm"] + "')]")),
                                new FossaField(GetAngularQFormUID, "a2"),
                                new FossaField(Wait, "Wait"));
                int stepNum = 3;

                switch (testData["AutoLoanType"])
                {
                    case "New Car Purchase" :
                    case "Used Car Purchase":
                        //State of Purchase?
                        Steps[stepNum] = Step(
                                       new FossaField(NoAutoAdvance(SelectByText), "vehicle-state", "VehicleState"),                                       
                                       new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //Down Payment?
                        Steps[stepNum] = Step(
                                       new FossaField(NoAutoAdvance(Fill), "down-payment", "PurchaseDownPayment"),
                                       new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;

                        //Loan Amount?
                        Steps[stepNum] = Step(
                                       new FossaField(NoAutoAdvance(Fill), "requested-loan-amount", "RequestedLoanAmount"),
                                       new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;

                        //What model car?
                        Steps[stepNum] = Step(
                                        new FossaField(NoAutoAdvance(SelectByText), "vehicle-year", "VehicleYear"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(NoAutoAdvance(SelectByText), "vehicle-make", "VehicleMake"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(NoAutoAdvance(SelectByText), "vehicle-model", "VehicleModel"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(SelectByText, "vehicle-trim", "VehicleTrim"),
                                        new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;
                    case "Refinance":
                    case "Lease Buy Out":
                        //What model car?
                        Steps[stepNum] = Step(
                                        new FossaField(NoAutoAdvance(SelectByText), "vehicle-year", "VehicleYear"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(NoAutoAdvance(SelectByText), "vehicle-make", "VehicleMake"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(NoAutoAdvance(SelectByText), "vehicle-model", "VehicleModel"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(SelectByText, "vehicle-trim", "VehicleTrim"),
                                        new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //VIN
                        Steps[stepNum] = Step(
                               new FossaField(NoAutoAdvance(Fill), "vehicle-identification-number", "VehicleIdNumber"),
                               new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //Mileage
                        Steps[stepNum] = Step(
                                   new FossaField(NoAutoAdvance(Fill), "vehicle-mileage", "VehicleMileage"),
                                   new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //Current Interest rate
                        Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["CurrentRate"]),
                            new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //Current Lender
                        Steps[stepNum] = Step(
                               new FossaField(Fill, By.XPath("//input[@ng-model='inputs.currentLender']"), "CurrentLender"),
                               new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //Current Pay Off amount and Current Monthly Payment
                        Steps[stepNum] = Step(
                               new FossaField(NoAutoAdvance(Fill), "payoff-amount", "CurrentPayoffAmount"),
                               new FossaField(NoAutoAdvance(Fill), "current-vehicle-payment", "CurrentPayment"),
                               new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        //Remaining Payment
                        Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["CurrentRemainingTerms"]),
                            new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;
                }
                
                //Loan type is used car purchase  add below steps                
                if (testData["AutoLoanType"].Equals("Used Car Purchase", StringComparison.OrdinalIgnoreCase))
                {
                    //From Whom Are You Buying?
                    Steps[stepNum] = Step(
                                          new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='" + testData["VehiclePurchaseSource"] + "']")),                                          
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                    //Have you found a car already?
                    Steps[stepNum] = Step(
                                          new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='" + testData["FoundVehicle"] + "']")),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                    //Mileage
                    Steps[stepNum] = Step(
                               new FossaField(NoAutoAdvance(Fill), "vehicle-mileage", "VehicleMileage"),
                               new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                //When Were You Born?
                Steps[stepNum] = Step(
                                new FossaField(NoAutoAdvance(SelectByText), "birth-month", "DateOfBirthMonth"),
                                new FossaField(NoAutoAdvance(SelectByText), "birth-day", "DateOfBirthDay"),
                                new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));
                stepNum = stepNum + 1;

                //Any Bankruptcy In The Past 7 Years?                
                if (testData["BankruptcyHistory"].Equals("N", StringComparison.OrdinalIgnoreCase)
                            || testData["BankruptcyHistory"].Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    Steps[stepNum] = Step(
                                    new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for='declared-bankruptcy-no']")),
                                    new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else
                {
                    Steps[stepNum] = Step(
                                    new FossaField(AutoAdvance(ClickElement),By.CssSelector("label[for='declared-bankruptcy-yes']")),                                         
                                    new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;

                    //When Was it Discharged?
                    Steps[stepNum] = Step(
                                    new FossaField(SelectSliderValueByText, testData["BankruptcyHistory"]),
                                    new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                //What's Your Employment Status 
                Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='" + testData["BorrowerEmploymentStatus"] + "']")),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;
                
                // Borrower is Full Time or Part Time or Self-Employed
                if ((testData["BorrowerEmploymentStatus"].Equals("Full Time", StringComparison.OrdinalIgnoreCase)
                            || testData["BorrowerEmploymentStatus"].Equals("Part Time", StringComparison.OrdinalIgnoreCase)
                                        || testData["BorrowerEmploymentStatus"].Equals("Self-Employed", StringComparison.OrdinalIgnoreCase)))
                {
                    //When Did You Start?
                    Steps[stepNum] = Step(                                          
                                          new FossaField(NoAutoAdvance(SelectByValue), "job-start-month", "BorrowerEmploymentTimeMonths"),
                                          new FossaField(SelectByText, "job-start-year", "BorrowerEmploymentTimeYears"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                    // Borrower Employer Details
                    Steps[stepNum] = Step(
                                          new FossaField(NoAutoAdvance(Fill), "employer-name", "BorrowerEmployerName"),
                                          new FossaField(NoAutoAdvance(Fill), "job-title", "BorrowerJobTitle"),
                                          new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                // Your Pre-tax Yearly Income
                Steps[stepNum] = Step(
                                      new FossaField(NoAutoAdvance(Fill), "income", "BorrowerIncome"),
                                      new FossaField(NoAutoAdvance(Fill), "other-income", "BorrowerOtherIncome"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Total Liquid Assets?
                Steps[stepNum] = Step(
                                      new FossaField(NoAutoAdvance(Fill), "asset-value", "BorrowerAssets"),                                      
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Your Name
                Steps[stepNum] = Step(
                                      new FossaField(NoAutoAdvance(Fill), "first-name", "BorrowerFirstName"),
                                      new FossaField(NoAutoAdvance(Fill), "last-name", "BorrowerLastName"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Email
                Steps[stepNum] = Step(
                                      new FossaField(NoAutoAdvance(Fill), "email", "EmailAddress"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Create A Password
                Steps[stepNum] = Step(
                                      new FossaField(NoAutoAdvance(Fill), "password", "Password"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Street Address and Zip
                Steps[stepNum] = Step(
                                      new FossaField(NoAutoAdvance(Fill), "street1", "BorrowerStreetAddress"),
                                      new FossaField(NoAutoAdvance(Fill), "zip-code", "BorrowerZipCode"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Residence Type
                Steps[stepNum] = Step(
                                 new FossaField(AutoAdvance(ClickElement), By.XPath("//div[text()='" + testData["BorrowerRentOwn"] + "']")),
                                 new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //How Long Have You Lived Here?
                Steps[stepNum] = Step(
                                      new FossaField(NoAutoAdvance(SelectByText), "time-at-address-year", "BorrowerYearsAtAddress"),
                                      new FossaField(SelectByText, "time-at-address-month", "BorrowerMonthsAtAddress"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Monthly Housing Payment
                Steps[stepNum] = Step(
                                      new FossaField(NoAutoAdvance(Fill), "monthly-housing-payment", "BorrowerHousingPayment"),
                                      new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Phone Number
                Steps[stepNum] = Step(
                                       new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                       new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                       new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
                                       new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Alternate Phone Number 20
                Steps[stepNum] = Step(
                                       new FossaField(Fill, "secondary-phone", "BorrowerWorkPhone1"),
                                       new FossaField(Append, "secondary-phone", "BorrowerWorkPhone2"),
                                       new FossaField(Append, "secondary-phone", "BorrowerWorkPhone3"),
                                       new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                //Please enter the last 4 digits of your SSN
                Steps[stepNum] = Step(
                                new FossaField(Fill, "social-security-four", "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;
               
                // Handle the following steps together based on SSN, Targus check pass/fail, and Credit pull pass/fail
                if ((testData["CreditPullSuccessYesNo"] == "N") & (testData["TargusPassYesNo"] == "N"))
                {
                    //Confirm Phone number
                    Steps[stepNum] = Step(
                                       new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                       new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                       new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
                                       new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                    //Enter SSN
                    Steps[stepNum] = Step(
                                        new FossaField(Fill, "social-security", "BorrowerSsn1"),
                                        new FossaField(Append, "social-security", "BorrowerSsn2"),
                                        new FossaField(Append, "social-security", "BorrowerSsn3"),
                                        new FossaField(Wait, "Wait"));
                }

                if ((testData["CreditPullSuccessYesNo"] == "N") & (testData["TargusPassYesNo"] == "Y"))
                {
                    //Enter SSN
                    Steps[stepNum] = Step(
                                        new FossaField(Fill, "social-security", "BorrowerSsn1"),
                                        new FossaField(Append, "social-security", "BorrowerSsn2"),
                                        new FossaField(Append, "social-security", "BorrowerSsn3"),
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
