using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tla
{
    class mortgage2Page : FossaPageBase
    {
        public mortgage2Page(IWebDriver driver, Dictionary<string, string> testData)
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
                if (GetLoanType() == LoanType.Refinance) return ValidQFStepsDefaultRefinance;
                else return ValidQFStepsDefaultPurchase;
            }
        }

        private IFormField[][] ValidQFStepsDefault
        {
            get 
            {
                // Number of steps depends on SSN presense, targus pass/fail, and credit pull pass/fail
                Int32 numSteps;
                switch (testData["BorrowerSsn1"])
                {
                    case "":  // No SSN
                        if (testData["TargusPassYesNo"] == "Y")
                            numSteps = 18;
                        else
                            numSteps = 19;
                        break;

                    default:
                        if (testData["TargusPassYesNo"] == "Y" && testData["CreditPullSuccessYesNo"] == "Y")
                        {
                            numSteps = 18;
                        }
                        else if (testData["TargusPassYesNo"] == "Y" && testData["CreditPullSuccessYesNo"] == "N")
                        {
                            numSteps = 19;
                        }
                        else if (testData["TargusPassYesNo"] == "N" && testData["CreditPullSuccessYesNo"] == "Y")
                        {
                            numSteps = 19;
                        }
                        else if (testData["TargusPassYesNo"] == "N" && testData["CreditPullSuccessYesNo"] == "N")
                        {
                            numSteps = 20;
                        }
                        else
                        {
                            numSteps = 20;
                            // Report invalid test data
                            Common.ReportEvent(Common.ERROR, "The values provided for 'TargusPassYesNo' and 'CreditPullSuccessYesNo' are not valid. " +
                                "These values must be 'Y' or 'N'. Please check the test case data and try again.");
                        }
                        break;
                }

                IFormField[][] Steps = new IFormField[numSteps][];

                // Step 1
                switch (testData["PropertyType"])
                {
                    case "Single family home":
                        Steps[1] = Step(
                                new FossaField(SelectByValue, "loan-type", "LoanType"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[1] = Step(
                                new FossaField(SelectByValue, "loan-type", "LoanType"),
                                new FossaField(SelectByText, "property-type", "PropertyType"));
                        break;
                }

                // Step 2
                switch (testData["PropertyUse"])
                {
                    case "Primary residence":
                        Steps[2] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[2] = Step(
                                new FossaField(SelectByText, "property-use", "PropertyUse"));
                        break;
                }

                //Steps 3-7 depend on loan type.

                // Step 8 Home Services Opt-in
                // TODO: handle HomeServicesCategory drop-down and add data field to db!
                // TODO: Handle 'HomeServices' variation
                switch (testData["HomeServicesOptInYesNo"])
                {
                    case "N":
                        Steps[8] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[8] = Step(
                                new FossaField(ClickRadioYesNo, "home-services-optin-{0}", "HomeServicesOptInYesNo"));
                        break;
                }

                // Step 9 Credit profile
                switch (testData["CreditProfile"].ToUpper())
                {
                    case "EXCELLENT":
                        Steps[9] = Step(new FossaField(ClickRadio, "stated-credit-history-excellent"));
                        break;
                    case "GOOD":
                        Steps[9] = Step(new FossaField(ClickButton, "next"));
                        break;
                    case "FAIR":
                        Steps[9] = Step(new FossaField(ClickRadio, "stated-credit-history-fair"));
                        break;
                    case "POOR":
                        Steps[9] = Step(new FossaField(ClickRadio, "stated-credit-history-poor"));
                        break;
                    default:
                        Steps[9] = Step(new FossaField(ClickButton, "next"));
                        break;
                }

                Steps[10] = Step(
                                new FossaField(SelectByText, "birth-month", "DateOfBirthMonth"),
                                new FossaField(SelectByText, "birth-day", "DateOfBirthDay"),
                                new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));

                // Step 11 Military
                switch (testData["MilitaryServiceYesNo"])
                {
                    case "N":
                        Steps[11] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[11] = Step(
                                new FossaField(ClickRadioYesNo, "is-veteran-{0}", "MilitaryServiceYesNo"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(IfOtherElementDisplayed("current-loan-va-yes", ClickRadioYesNo), "current-loan-va-{0}", "CurrentLoanVAYesNo"));
                        break;
                }

                // Step 12 Bankruptcy
                switch (testData["BankruptcyYesNo"])
                {
                    case "N":
                        Steps[12] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[12] = Step(
                                new FossaField(ClickRadioYesNo, "declared-bankruptcy-{0}", "BankruptcyYesNo"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, "bankruptcy-discharged", "BankruptcyHistory"));
                        break;
                }

                // Step 13 Foreclosure
                switch (testData["ForeclosureHistory"])
                {
                    case "Never foreclosed":
                        Steps[13] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[13] = Step(
                                new FossaField(SelectByText, "foreclosure-text", "ForeclosureHistory"));
                        break;
                }

                Steps[14] = Step(
                                new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));

                Steps[15] = Step(
                                new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(Fill, "last-name", "BorrowerLastName"),
                                new FossaField(Fill, "email", "EmailAddress"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "password", "Password"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                Steps[16] = Step(
                                new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));

                // Step 17 SSN
                switch (testData["BorrowerSsn1"])
                {
                    case "":
                        Steps[17] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[17] = Step(
                                new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                }


                // Handle steps 18 and 19 together based on SSN, Targus check pass/fail, and Credit pull pass/fail
                switch (testData["BorrowerSsn1"])
                {
                    case "":  // no SSN
                        if (testData["TargusPassYesNo"] == "Y")
                        {
                            break;
                        }
                        else // "N"
                        {
                            Steps[18] = Step(
                                    new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                    new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                    new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                    new FossaField(Wait, "Wait"),
                                    new FossaField(ClickButton, "next"));
                        }
                        break;

                    default:  // we have an SSN
                        if (testData["TargusPassYesNo"] == "Y" && testData["CreditPullSuccessYesNo"] == "Y")
                        {
                            break;
                        }
                        else if (testData["TargusPassYesNo"] == "Y" && testData["CreditPullSuccessYesNo"] == "N")
                        {
                            Steps[18] = Step(
                                    new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                    new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                    new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                    new FossaField(Wait, "Wait"),
                                    new FossaField(ClickButton, "next"));
                        }
                        else if (testData["TargusPassYesNo"] == "N" && testData["CreditPullSuccessYesNo"] == "Y")
                        {
                            Steps[18] = Step(
                                    new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                    new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                    new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                    new FossaField(Wait, "Wait"),
                                    new FossaField(ClickButton, "next"));
                        }
                        else // both "N"
                        {
                            Steps[18] = Step(
                                    new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                    new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                    new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                    new FossaField(Wait, "Wait"),
                                    new FossaField(ClickButton, "next"));
                            Steps[19] = Step(
                                    new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                    new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                    new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                    new FossaField(Wait, "Wait"),
                                    new FossaField(ClickButton, "next"));
                        }
                        break;
                }
                       
                return Steps;
            }
        }

        public IFormField[][] ValidQFStepsDefaultPurchase
        {
            get
            {
                IFormField[][] Steps = ValidQFStepsDefault;

                Steps[3] = Step(
                                new FossaField(SelectByText, "property-state", "PropertyState"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, "property-city", "PropertyCity"));
                Steps[4] = Step(
                                new FossaField(ClickRadioYesNo, "new-home-{0}", "FoundNewHomeYesNo"));
                Steps[5] = Step(
                                new FossaField(ClickRadioYesNo, "current-realestate-agent-{0}", "CurrentREAgentYesNo"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "mortgage2"),
                                new FossaField(ClickRadioYesNo, "inline-realtor-optin-{0}", "RealtorConsultYesNo"));
                Steps[6] = Step(
                                new FossaField(SelectByText, "purchase-price", "PurchasePrice"));
                Steps[7] = Step(
                                new FossaField(SelectByText, "down-payment-amt", "PurchaseDownPayment"));
                return Steps;
            }
        }

        public IFormField[][] ValidQFStepsDefaultRefinance
        {
            get
            {
                IFormField[][] Steps = ValidQFStepsDefault;

                Steps[3] = Step(
                                new FossaField(Fill, "property-zip-code-input", "PropertyZipCode"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "mortgage2"),
                                new FossaField(ClickButton, "next"));
                Steps[4] = Step(
                                new FossaField(SelectByText, "estproperty-value", "RefiPropertyValue"));
                Steps[5] = Step(
                                new FossaField(SelectByText, "est-mortgage-balance", "FirstMortgageBalance"));

                // Step 6 2nd mortgage
                switch (testData["SecondMortgageYesNo"].ToUpper())
                {
                    case "N":
                        Steps[6] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[6] = Step(
                                new FossaField(ClickRadioYesNo, "have-multiple-mortgages-{0}", "SecondMortgageYesNo"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, "second-mortgage-balance", "SecondMortgageBalance"));
                        break;
                }

                // Step 7 Cashout
                switch (testData["RefiCashoutAmount"])
                {
                    case "$0 No Cash":
                        Steps[7] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[7] = Step(
                                new FossaField(SelectByText, "cash-out", "RefiCashoutAmount"));
                        break;
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
            System.Threading.Thread.Sleep(100);
        }
    }
}
