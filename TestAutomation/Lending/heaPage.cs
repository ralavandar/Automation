using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tla
{
    class heaPage : FossaPageBase
    {
        public heaPage(IWebDriver driver, Dictionary<string, string> testData)
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
                IFormField[][] Steps = new IFormField[13][];

                // Step 1
                switch (testData["PropertyType"])
                {
                    case "Single family home":
                        Steps[1] = Step(new FossaField(SelectByValue, "loan-type", "LoanType"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[1] = Step(new FossaField(SelectByValue, "loan-type", "LoanType"),
                                        new FossaField(SelectByText, "property-type", "PropertyType"));
                        break;
                }
                // Step 2
                switch (testData["PropertyUse"])
                {
                    case "Primary residence":
                        Steps[2] = Step(new FossaField(Fill, "property-zip-code-input", "PropertyZipCode"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[2] = Step(new FossaField(Fill, "property-zip-code-input", "PropertyZipCode"),
                                        new FossaField(SelectByText, "property-use", "PropertyUse"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(ClickButton, "next"));
                        break;
                }
                // Step 3
                Steps[3] = Step(new FossaField(SelectByText, "estproperty-value", "PropertyValue"),
                                new FossaField(GetAngularQFormUID, "hea"),
                                new FossaField(SelectByText, "purchase-price", "PurchasePrice"));
                // Step 4
                switch (testData["CurrentMortgages"])
                {
                    case "No current mortgage":
                        Steps[4] = Step(new FossaField(SelectByText, "purchase-year", "PurchaseYear"),
                                        new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[4] = Step(new FossaField(SelectByText, "purchase-year", "PurchaseYear"),
                                        new FossaField(SelectByText, "property-mortgage", "CurrentMortgages"));
                        break;
                }
                // Step 5
                switch (testData["CurrentMortgages"])
                {
                    case "No current mortgage":
                        Steps[5] = Step(new FossaField(SelectByText, "desired-loan-amount", "RequestedLoanAmount"),
                                        new FossaField(ClickButton,  "next"));
                        break;
                    case "First mortgage only":
                        Steps[5] = Step(new FossaField(SelectByText, "est-mortgage-balance", "FirstMortgageBalance"),
                                        new FossaField(SelectByText, "desired-loan-amount", "RequestedLoanAmount"),
                                        new FossaField(ClickButton,  "next"));
                        break;
                    case "First & second mortgages":
                        Steps[5] = Step(new FossaField(SelectByText, "est-mortgage-balance", "FirstMortgageBalance"),
                                        new FossaField(SelectByText, "second-mortgage-balance", "SecondMortgageBalance"),
                                        new FossaField(SelectByText, "desired-loan-amount", "RequestedLoanAmount"),
                                        new FossaField(ClickButton,  "next"));
                        break;
                    default:
                        Steps[5] = Step(new FossaField(Wait, "Wait"),
                                        new FossaField(ClickButton,  "next"));
                        break;
                }
                // Step 6
                switch (testData["CreditProfile"])
                {
                    case "stated-credit-history-good":
                        Steps[6] = Step(new FossaField(SelectByText, "birth-month", "DateOfBirthMonth"),
                                        new FossaField(SelectByText, "birth-day", "DateOfBirthDay"),
                                        new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));
                        break;

                    default:
                        Steps[6] = Step(new FossaField(ClickRadio, testData["CreditProfile"]),
                                        new FossaField(SelectByText, "birth-month", "DateOfBirthMonth"),
                                        new FossaField(SelectByText, "birth-day", "DateOfBirthDay"),
                                        new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));
                        break;
                }
                // Step 7
                switch (testData["BankruptcyHistory"])
                {
                    case "Never/Not in the last 7 years":
                        Steps[7] = Step(new FossaField(SelectByText, "foreclosure-text", "ForeclosureHistory"),
                                        new FossaField(ClickRadio, "declared-bankruptcy-no"),
                                        new FossaField(ClickButton, "next"));
                        break;

                    default:
                        Steps[7] = Step(new FossaField(SelectByText, "foreclosure-text", "ForeclosureHistory"),
                                        new FossaField(ClickRadio, "declared-bankruptcy-yes"),
                                        new FossaField(Wait, "Wait"),
                                        new FossaField(SelectByText, "bankruptcy-discharged", "BankruptcyHistory"));
                        break;
                }
                // Step 8
                Steps[8] = Step(new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(Fill, "last-name", "BorrowerLastName"),
                                new FossaField(ClickButton, "next"));
                // Step 9
                Steps[9] = Step(new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                // Step 10
                Steps[10] = Step(new FossaField(ClickElement, By.Id("bank-other")),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                // Step 11
                Steps[11] = Step(new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                new FossaField(ClickButton, "next"));
                // Step 12
                Steps[12] = Step(new FossaField(Fill, "email", "EmailAddress"),
                                new FossaField(Fill, "password", "Password"),
                                new FossaField(Wait5Sec, "Wait"),
                                new FossaField(ClickButton, "next"));

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
            System.Threading.Thread.Sleep(100);
        }
    }
}
