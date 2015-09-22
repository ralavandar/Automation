using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tla
{
    class prequalPage : FossaPageBase
    {
        public prequalPage(IWebDriver driver, Dictionary<string, string> testData)
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
                Int32 numSteps = 18;

                // Add a step if TargusPassYesNo = N
                if (testData["TargusPassYesNo"] == "N")
                {
                    numSteps = numSteps + 1;
                }

                IFormField[][] Steps = new IFormField[numSteps][];

                Steps[1] = Step(
                                new FossaField(ClickRadioYesNo, "has-found-home-{0}", "FoundNewHomeYesNo"));

                // Step 2
                switch (testData["PropertyType"])
                {
                    case "Single-Family Detached":
                        Steps[2] = Step(
                                new FossaField(ClickRadioWithValueID, "property-type-singlefamdet"));
                        break;
                    case "Town House":
                        Steps[2] = Step(
                                new FossaField(ClickRadioWithValueID, "property-type-singlefamatt"));
                        break;
                    case "Condo":
                        Steps[2] = Step(
                                new FossaField(ClickRadioWithValueID, "property-type-lowrisecondo"));
                        break;
                    case "Multiple Family Dwelling":
                        Steps[2] = Step(
                                new FossaField(ClickRadioWithValueID, "property-type-2to4unitfam"));
                        break;
                    case "Mobile/Manufactured Home":
                        Steps[2] = Step(
                                new FossaField(ClickRadioWithValueID, "property-type-mobilepermanent"));
                        break;
                    default:
                        // Report invalid test data
                        Common.ReportEvent(Common.ERROR, "The value provided for 'PropertyType' is not valid. " +
                            "Please check the test case data and try again.");
                        break;
                }

                // Step 3
                switch (testData["PropertyUse"])
                {
                    case "Primary Home":
                        Steps[3] = Step(
                                new FossaField(ClickRadioWithValueID, "property-use-owneroccupied"));
                        break;
                    case "Secondary Home":
                        Steps[3] = Step(
                                new FossaField(ClickRadioWithValueID, "property-use-secondhome"));
                        break;
                    case "Investment Property":
                        Steps[3] = Step(
                                new FossaField(ClickRadioWithValueID, "property-use-investmentproperty"));
                        break;
                    default:
                        // Report invalid test data
                        Common.ReportEvent(Common.ERROR, "The value provided for 'PropertyUse' is not valid. " +
                            "Please check the test case data and try again.");
                        break;
                }

                // Step 4
                if (testData["FoundNewHomeYesNo"] == "Y")
                {
                    Steps[4] = Step(
                                new FossaField(Fill, "property-street", "PropertyStreet"),
                                new FossaField(Fill, "property-zip-code-input", "PropertyZipCode"),
                                new FossaField(Wait, "Wait"));
                }
                else
                {
                    Steps[4] = Step(
                                new FossaField(Fill, By.Id("property-geo-search"), "PropertyCity"),
                                new FossaField(Append, By.Id("property-geo-search"), "PropertyState"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickElement, By.ClassName("dropdown-menu")),
                                new FossaField(Wait, "Wait"));
                }
                        
                Steps[5] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "prequal"),
                                new FossaField(Fill, "purchase-price", "PurchasePrice"),
                                new FossaField(Wait, "Wait"));
               
                Steps[6] = Step(new FossaField(Fill, "down-payment", "PurchaseDownPayment"));

                Steps[7] = Step(new FossaField(ClickRadioYesNo, "current-realestate-agent-{0}", "CurrentREAgentYesNo"));
                                
                // Step 8
                switch (testData["TimeToPurchase"])
                {
                    case "30DAYS":
                        Steps[8] = Step(
                                new FossaField(ClickRadioWithValueID, "time-to-purchase-30days"));
                        break;
                    case "60DAYS":
                        Steps[8] = Step(
                                new FossaField(ClickRadioWithValueID, "time-to-purchase-60days"));
                        break;
                    case "90DAYS":
                        Steps[8] = Step(
                                new FossaField(ClickRadioWithValueID, "time-to-purchase-90days"));
                        break;
                    case "MORETHAN90DAYS":
                        Steps[8] = Step(
                                new FossaField(ClickRadioWithValueID, "time-to-purchase-morethan90days"));
                        break;
                    case "NOTIMECONSTRAINT":
                        Steps[8] = Step(
                                new FossaField(ClickRadioWithValueID, "time-to-purchase-notimeconstraint"));
                        break;
                    default:
                        // Report invalid test data
                        Common.ReportEvent(Common.ERROR, "The value provided for 'TimeToPurchase' is not valid. " +
                            "Please check the test case data and try again.");
                        break;
                }
                                               
                // Step 9
                switch (testData["CreditProfile"])
                {
                    case "Excellent":
                        Steps[9] = Step(
                                new FossaField(ClickRadioWithValueID, "stated-credit-history-excellent"));
                        break;
                    case "Good":
                        Steps[9] = Step(
                                new FossaField(ClickRadioWithValueID, "stated-credit-history-somecreditproblems"));
                        break;
                    case "Fair":
                        Steps[9] = Step(
                                new FossaField(ClickRadioWithValueID, "stated-credit-history-majorcreditproblems"));
                        break;
                    case "Poor":
                        Steps[9] = Step(
                                new FossaField(ClickRadioWithValueID, "stated-credit-history-littleornocredithistory"));
                        break;
                    default:
                        // Report invalid test data
                        Common.ReportEvent(Common.ERROR, "The value provided for 'CreditProfile' is not valid. " +
                            "Please check the test case data and try again.");
                        break;
                }

                Steps[10] = Step(
                                 new FossaField(SelectByText, "birth-month", "DateOfBirthMonth"),
                                 new FossaField(SelectByText, "birth-day", "DateOfBirthDay"),
                                 new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));

                // Step 11    
                switch (testData["BankruptcyYesNo"])
                {
                    case "N":
                        Steps[11] = Step(
                                new FossaField(ClickRadioYesNo, "declared-bankruptcy-{0}", "BankruptcyYesNo"));
                        break;
                    default:
                        Steps[11] = Step(
                                new FossaField(ClickRadioYesNo, "declared-bankruptcy-{0}", "BankruptcyYesNo"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(SelectByText, "bankruptcy-discharged", "BankruptcyHistory"));
                        break;
                }

                // Step 12 Foreclosure
                switch (testData["ForeclosureHistory"])
                {
                    case "Never Foreclosed":
                        Steps[12] = Step(
                                new FossaField(Wait, "Wait"));
                        break;
                    default:
                        Steps[12] = Step(
                                new FossaField(SelectByText, "foreclosure-text", "ForeclosureHistory"));
                        break;
                }
               
                Steps[13] = Step(new FossaField(ClickRadioYesNo, "is-veteran-{0}", "MilitaryServiceYesNo"));

                Steps[14] = Step(new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                 new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                 new FossaField(Wait, "Wait"));

                Steps[15] = Step(new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                 new FossaField(Fill, "last-name", "BorrowerLastName"));
               
                Steps[16] = Step(new FossaField(Fill, "email", "EmailAddress"),
                                 new FossaField(Fill, "password", "Password"));

                Steps[17] = Step(new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                 new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                 new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                 new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                 new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                 new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                 new FossaField(Wait, "Wait"));

                // Step 18 - Targus validation 
                if (testData["TargusPassYesNo"] == "N")
                    {
                        Steps[18] = Step(
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
