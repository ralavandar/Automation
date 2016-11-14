using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tlm
{
    class m2Page : FossaPageBase
    {
        public m2Page(IWebDriver driver, Dictionary<string, string> testData)
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
                // The minimum number of steps for a purchase scenario is 19, Refi is 20
                // Initialize the array to one larger than we need, since we don't use index 0.
                Int32 numSteps = 19;
                if (testData["LoanType"].ToUpper() == "REFINANCE")
                {
                    numSteps++;

                    // Add a step for refi second mortgage balance if applicable
                    if (testData["SecondMortgageYesNo"] == "Y")
                        numSteps++;

                    // Add a step for refi military va loan if applicable
                    if (testData["MilitaryServiceYesNo"] == "Y")
                        numSteps++;
                }

                // Add steps for purchase current realtor and realtor consult
                if (testData["LoanType"].ToUpper() == "PURCHASE")
                {
                    if ((testData["FoundNewHomeYesNo"].ToUpper() == "N") && (testData["CurrentREAgentYesNo"].ToUpper() == "N"))
                        // 'Current RE Agent' and 'Realtor Consult steps'
                        numSteps = numSteps + 2;
                    else
                        // Just 'Current RE Agent' step
                        numSteps++;
                }

                // Add two steps for home services opt-in questions if applicable
                if (testData["HomeServicesOptInYesNo"] == "Y")
                    numSteps = numSteps + 2;

                // Add a step for bankruptcy if applicable
                if (testData["BankruptcyYesNo"] == "Y")
                    numSteps++;

                // Add a step for foreclosure if applicable
                if (testData["ForeclosureHistory"] != "N")
                    numSteps++;

                // Number of steps also depends on SSN presence, targus pass/fail, and credit pull pass/fail
                switch (testData["BorrowerSsn3"])
                {
                    case "":  // No SSN
                        if (testData["TargusPassYesNo"] == "N")
                            numSteps++;
                        break;

                    default:  // SSN
                        if (testData["TargusPassYesNo"] == "N")
                            numSteps++;
                        if (testData["CreditPullSuccessYesNo"] == "N")
                            numSteps++;
                        break;
                }

                IFormField[][] Steps = new IFormField[numSteps][];

                // Loan Type
                switch (testData["LoanType"].ToUpper())
                {
                    case "REFINANCE":
                        Steps[1] = Step(
                                //new FossaField(ClickElement, By.CssSelector("label[for=loan-type-refinance]")),
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=loan-type-refinance]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "PURCHASE":
                        Steps[1] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=loan-type-purchase]")),
                                new FossaField(Wait, "Wait"));
                        break;
                }

                // Property Type
                switch (testData["PropertyType"])
                {
                    case "Single Family Home":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-single-fam]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Townhome":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-townhome]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Condominium":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-condo]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Multi Family Home":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-multi]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Manufactured / Mobile":
                        Steps[2] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-type-mobile]")),
                                new FossaField(Wait, "Wait"));
                        break;
                }

                // Property Use
                switch (testData["PropertyUse"])
                {
                    case "Primary Home":
                        Steps[3] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-primary]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Secondary Home":
                        Steps[3] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-secondary]")),
                                new FossaField(Wait, "Wait"));
                        break;
                    case "Rental Property":
                        Steps[3] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=property-use-rental]")),
                                new FossaField(Wait, "Wait"));
                        break;
                }

                //Steps 4-n depend on loan type.
                int stepNum = 4;

                if (testData["LoanType"].ToUpper() == "PURCHASE")
                {
                    Steps[stepNum] = Step(
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "m2"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, By.Id("property-geo-search"), "PropertyCity"),
                                new FossaField(Append, By.Id("property-geo-search"), "PropertyState"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickElement, By.ClassName("dropdown-menu")),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickElement, By.ClassName("form-header")),
                                new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                    // Found home
                    switch (testData["FoundNewHomeYesNo"])
                    {
                        case "N":
                            Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=new-home-no]")),
                                new FossaField(Wait, "Wait"));
                            break;
                        case "Y":
                            Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=new-home-yes]")),
                                new FossaField(Wait, "Wait"));
                            break;
                    }
                    stepNum = stepNum + 1;
                    // Current real estate agent
                    switch (testData["CurrentREAgentYesNo"])
                    {
                        case "N":
                            Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=current-realestate-agent-no]")),
                                new FossaField(Wait, "Wait"));
                            break;
                        case "Y":
                            Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=current-realestate-agent-yes]")),
                                new FossaField(Wait, "Wait"));
                            break;
                    }
                    stepNum = stepNum + 1;

                    // Real estate agent consult (only if CurrentREAgentYesNo = No and FoundNewHome = NO) 
                    if ((testData["FoundNewHomeYesNo"].ToUpper() == "N") && (testData["CurrentREAgentYesNo"].ToUpper() == "N"))
                    {
                        switch (testData["RealtorConsultYesNo"])
                        {
                            case "Y":
                                Steps[stepNum] = Step(
                                    new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=realtor-optin-yes]")),
                                    new FossaField(Wait, "Wait"));
                                break;
                            default:
                                Steps[stepNum] = Step(
                                    new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=realtor-optin-no]")),
                                    new FossaField(Wait, "Wait"));
                                break;
                        }
                        stepNum = stepNum + 1;
                    }

                    // Purchase Price
                    Steps[stepNum] = Step(
                                new FossaField(SelectSliderValueByText, testData["PurchasePrice"]),
                                new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                    // Purchase Down Payment
                    Steps[stepNum] = Step(
                                new FossaField(SelectSliderValueByText, testData["PurchaseDownPayment"]),
                                new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }

                if (testData["LoanType"].ToUpper() == "REFINANCE")
                {
                    Steps[stepNum] = Step(
                                new FossaField(Fill, "property-zip-code-input", "PropertyZipCode"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(GetAngularQFormUID, "m2"));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                                new FossaField(SelectSliderValueByText, testData["RefiPropertyValue"]),
                                new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;

                    Steps[stepNum] = Step(
                                new FossaField(SelectSliderValueByText, testData["FirstMortgageBalance"]),
                                new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;

                    // 2nd mortgage
                    switch (testData["SecondMortgageYesNo"].ToUpper())
                    {
                        case "N":
                            Steps[stepNum] = Step(
                                    new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=have-multiple-mortgages-no]")),
                                    new FossaField(Wait, "Wait"));
                            stepNum = stepNum + 1;
                            break;
                        default:
                            Steps[stepNum] = Step(
                                    new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=have-multiple-mortgages-yes]")),
                                    new FossaField(Wait, "Wait"));
                            stepNum = stepNum + 1;

                            Steps[stepNum] = Step(
                                    new FossaField(SelectSliderValueByText, testData["SecondMortgageBalance"]),
                                    new FossaField(Wait, "Wait"));
                                    //new FossaField(ClickButton, "next"));
                            stepNum = stepNum + 1;
                            break;
                    }

                    // Cashout
                    Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["RefiCashoutAmount"]),
                            new FossaField(Wait, "Wait"));
                            //new FossaField(ClickButton, "next"));
                    stepNum = stepNum + 1;
                }

                // Credit profile
                switch (testData["CreditProfile"].ToUpper())
                {
                    case "EXCELLENT":
                        Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-excellent]")));
                        break;
                    case "GOOD":
                        Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-good]")));
                        break;
                    case "FAIR":
                        Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-fair]")));
                        break;
                    case "POOR":
                        Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=stated-credit-history-poor]")));
                        break;
                }
                stepNum = stepNum + 1;

                //Date of Birth
                Steps[stepNum] = Step(
                                new FossaField(NoAutoAdvance(SelectByText), "birth-month", "DateOfBirthMonth"),
                                new FossaField(NoAutoAdvance(SelectByText), "birth-day", "DateOfBirthDay"),
                                new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));
                stepNum = stepNum + 1;

                // Military and VA Loan
                switch (testData["MilitaryServiceYesNo"])
                {
                    case "N":
                        Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=is-veteran-no]")),
                                new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;
                        break;
                    case "Y":
                        Steps[stepNum] = Step(
                                new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=is-veteran-yes]")),
                                new FossaField(Wait, "Wait"));
                        stepNum = stepNum + 1;

                        if ((testData["LoanType"].ToUpper() == "REFINANCE") && (testData["CurrentLoanVAYesNo"] == "Y"))
                        {
                            Steps[stepNum] = Step(
                                    new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=current-loan-va-yes]")),
                                    new FossaField(Wait, "Wait"));
                            stepNum = stepNum + 1;
                        }
                        else if ((testData["LoanType"].ToUpper() == "REFINANCE") && (testData["CurrentLoanVAYesNo"] == "N"))
                        {
                            Steps[stepNum] = Step(
                                    new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=current-loan-va-no]")),
                                    new FossaField(Wait, "Wait"));
                            stepNum = stepNum + 1;
                        }
                        break;
                }

                // Bankruptcy / Foreclosure
                if (testData["ForeclosureHistory"] == "N" && testData["BankruptcyYesNo"] == "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=bankruptcy-or-foreclosure-no]")));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] == "N" && testData["BankruptcyYesNo"] == "Y")
                {
                    Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=bankruptcy-or-foreclosure-bankruptcy]")));
                    stepNum = stepNum + 1;
                    Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["BankruptcyHistory"]),
                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] != "N" && testData["BankruptcyYesNo"] == "N")
                {
                    Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=bankruptcy-or-foreclosure-foreclosure]")));
                    stepNum = stepNum + 1;
                    Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["ForeclosureHistory"]),
                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else if (testData["ForeclosureHistory"] != "N" && testData["BankruptcyYesNo"] == "Y")
                {
                    Steps[stepNum] = Step(
                            new FossaField(AutoAdvance(ClickElement), By.CssSelector("label[for=bankruptcy-or-foreclosure-both]")));
                    stepNum = stepNum + 1;
                    Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["BankruptcyHistory"]),
                            new FossaField(Wait, "Wait"));

                    stepNum = stepNum + 1;
                    Steps[stepNum] = Step(
                            new FossaField(SelectSliderValueByText, testData["ForeclosureHistory"]),
                            new FossaField(Wait, "Wait"));
                    stepNum = stepNum + 1;
                }
                else
                {
                    // Report invalid test data
                    Common.ReportEvent(Common.ERROR, "The values provided for 'ForeclosureHistory' and/or 'BankruptcyYesNo' are not valid. " +
                        "Please check the test case data and try again.");
                }

                // Address
                Steps[stepNum] = Step(
                                new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickElement, By.ClassName("form-header")),
                                new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;
                // Name
                Steps[stepNum] = Step(
                                new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                new FossaField(Fill, "last-name", "BorrowerLastName"),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;
                // Email and Password
                Steps[stepNum] = Step(
                                new FossaField(DeselectAfter(Fill), "email", "EmailAddress"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "password", "Password"),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;
                // Home Phone
                Steps[stepNum] = Step(
                                new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;
                // SSN
                switch (testData["BorrowerSsn1"])
                {
                    case "":
                        Steps[stepNum] = Step(
                                new FossaField(Wait, "Wait"));
                        break;
                    default:
                        Steps[stepNum] = Step(
                                new FossaField(Fill, "social-security", "BorrowerSsn1"),
                                new FossaField(Append, "social-security", "BorrowerSsn2"),
                                new FossaField(Append, "social-security", "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"));
                        break;
                }
                stepNum = stepNum + 1;

                // Handle "Who do you bank with?" step
                // Skip = <a class="skip-step ng-scope" skip-step="">Skip this step</a>
                // or 
                // Click checkbox "None of the above": 
                //    <label for="bank-wellsfargo">Wells Fargo</label>    
                //    <label for="bank-other">None of the above</label>
                Steps[stepNum] = Step(
                    new FossaField(ClickElement, By.CssSelector("label[for=bank-other]")),
                    new FossaField(Wait, "Wait"));
                stepNum = stepNum + 1;

                // Handle the following steps together based on SSN, Targus check pass/fail, and Credit pull pass/fail
                switch (testData["BorrowerSsn1"])
                {
                    case "":  // no SSN
                        if (testData["TargusPassYesNo"] == "Y")
                        {
                            break;
                        }
                        else // "N"
                        {
                            Steps[stepNum] = Step(
                                new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"));
                            stepNum = stepNum + 1;
                        }
                        break;

                    default:  // we have an SSN
                        if (testData["TargusPassYesNo"] == "Y" && testData["CreditPullSuccessYesNo"] == "Y")
                        {
                            break;
                        }
                        else if (testData["TargusPassYesNo"] == "Y" && testData["CreditPullSuccessYesNo"] == "N")
                        {
                            Steps[stepNum] = Step(
                                new FossaField(Fill, By.XPath("//input[@id='social-security'][2]"), "BorrowerSsn1"),
                                new FossaField(Append, By.XPath("//input[@id='social-security'][2]"), "BorrowerSsn2"),
                                new FossaField(Append, By.XPath("//input[@id='social-security'][2]"), "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"));
                            stepNum = stepNum + 1;
                        }
                        else if (testData["TargusPassYesNo"] == "N" && testData["CreditPullSuccessYesNo"] == "Y")
                        {
                            Steps[stepNum] = Step(
                                new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"));
                            stepNum = stepNum + 1;
                        }
                        else // both "N"
                        {
                            Steps[stepNum] = Step(
                                new FossaField(Fill, "home-phone", "BorrowerHomePhone1"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone2"),
                                new FossaField(Append, "home-phone", "BorrowerHomePhone3"),
                                new FossaField(Wait, "Wait"));
                            stepNum = stepNum + 1;
                            Steps[stepNum] = Step(
                                new FossaField(Fill, By.XPath("//input[@id='social-security'][2]"), "BorrowerSsn1"),
                                new FossaField(Append, By.XPath("//input[@id='social-security'][2]"), "BorrowerSsn2"),
                                new FossaField(Append, By.XPath("//input[@id='social-security'][2]"), "BorrowerSsn3"),
                                new FossaField(Wait, "Wait"));
                            stepNum = stepNum + 1;
                        }
                        break;
                }

                // 'What's Next' step (the last step in the form)
                Steps[stepNum] = Step(
                                new FossaField(Wait5Sec, "Wait"),
                                new FossaField(ClickElement, By.ClassName("form-header")),
                                new FossaField(AutoAdvance(ClickElement), By.Id("next")));
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
