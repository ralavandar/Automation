using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tla
{
    class personalLoanPage : FossaPageBase
    {
        public personalLoanPage(IWebDriver driver, Dictionary<string, string> testData)
            : base(driver, testData) 
        { 
            
        }

        //private const uint VARIATION_AUTOADVANCE = 4;
        //private const uint VARIATION_UNIFORMSTYLE = 8;

        public override bool ShouldAutoAdvance
        {
            get { return true; }
        }

        public override void StartForm()
        {
            ReportAutoAdvance();
            
            NavigateToFossaForm(testData["TestEnvironment"], "tla.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);
        }

/*
        public override bool IsUniformStyle
        {
            get { return (GetVariation(VARIATION_UNIFORMSTYLE) != 3); }
        }

        public override void ReportUniformStyle()
        {
            var uniformStyle = GetVariation(VARIATION_UNIFORMSTYLE);
            if (uniformStyle == 3) Common.ReportEvent(Common.INFO, "Uniform style is OFF due to the StyleSheets variation being 3 for this test.");
            else Common.ReportEvent(Common.INFO, "Uniform style ON by default for this test.");
        }
*/
        public override IFormField[][] ValidQFSteps
        {
            get 
            {
                // Number of steps depends on credit pull pass/fail
                Int32 numSteps;
                if (testData["CreditPullSuccessYesNo"] == "N")
                    numSteps = 15;
                else
                    numSteps = 14;

                IFormField[][] Steps = new IFormField[numSteps][];
               
                Steps[1] = Step(new FossaField(SelectByText, "loan-purpose", "LoanPurpose"));


                Steps[2] = Step(new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "requested-loan-amount-input", "LoanAmountRequested"),
                                new FossaField(GetAngularQFormUID, "PLA"),
                                new FossaField(ClickButton, "next"));

                // Step 3
                switch (testData["CreditProfile"])
                {
                    case "EXCELLENT":
                        Steps[3] = Step(new FossaField(ClickRadio, "stated-credit-history-excellent"));
                        break;
                    case "MAJORCREDITPROBLEMS":
                        Steps[3] = Step(new FossaField(ClickRadio, "stated-credit-history-fair"));
                        break;
                    case "SOMECREDITPROBLEMS":
                        Steps[3] = Step(new FossaField(ClickButton, "next"));
                        break;
                    case "LITTLEORNOCREDITHISTORY":
                        Steps[3] = Step(new FossaField(ClickRadio, "stated-credit-history-poor"));
                        break;
                    default:
                        Steps[3] = Step(new FossaField(ClickRadio, "stated-credit-history-good"));
                        break;
                }

                // Step 4
                switch (testData["EmploymentStatus"])
                {
                    case "Full-time":
                        Steps[4] = Step(new FossaField(ClickButton, "next"));
                        break;
                    default:
                        Steps[4] = Step(new FossaField(SelectByText, "employment-status", "EmploymentStatus"));
                        break;
                }

                Steps[5] = Step(new FossaField(Fill, "income", "BorrowerIncome"),
                                new FossaField(ClickButton, "next"));

                Steps[6] = Step(new FossaField(SelectByText, "residence", "Residence"));

                Steps[7] = Step(new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(ClickButton, "next"));

                Steps[8] = Step(new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                new FossaField(ClickButton, "next"));

                Steps[9] = Step(new FossaField(Fill, "email", "EmailAddress"),
                                new FossaField(Wait, "Wait"),
                                new FossaField(Fill, "password", "Password"),
                                new FossaField(Wait, "Wait"),
                                //new FossaField(ClickButton, "next"),
                                new FossaField(ClickButton, "next"));

                Steps[10] = Step(new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                 new FossaField(Fill, "last-name", "BorrowerLastName"),
                                 new FossaField(ClickButton, "next"));

                Steps[11] = Step(new FossaField(SelectByText, "birth-month", "DateOfBirthMonth"),
                                 new FossaField(SelectByText, "birth-day", "DateOfBirthDay"),
                                 new FossaField(SelectByText, "birth-year", "DateOfBirthYear"));

                Steps[12] = Step(new FossaField(Fill, "home-phone-area-code", "BorrowerHomePhone1"),
                                 new FossaField(Fill, "home-phone-prefix", "BorrowerHomePhone2"),
                                 new FossaField(Fill, "home-phone-line", "BorrowerHomePhone3"),
                                 new FossaField(Wait, "Wait"),
                                 new FossaField(ClickButton, "next"));

                Steps[13] = Step(new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                 new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                 new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                 new FossaField(Wait, "Wait"),
                                 new FossaField(ClickButton, "next"),
                                 new FossaField(WaitForOops, "Wait"));

                // Step 14 - Oops step if credit pull fails. 
                if (testData["CreditPullSuccessYesNo"] == "N")
                {
                    Steps[14] = Step(
                                     new FossaField(Fill, "first-name", "BorrowerFirstName"),
                                     new FossaField(Fill, "last-name", "BorrowerLastName"),
                                     new FossaField(Fill, "street1", "BorrowerStreetAddress"),
                                     new FossaField(Fill, "zip-code-input", "BorrowerZipCode"),
                                     new FossaField(Fill, "social-security-one", "BorrowerSsn1"),
                                     new FossaField(Fill, "social-security-two", "BorrowerSsn2"),
                                     new FossaField(Fill, "social-security-three", "BorrowerSsn3"),
                                     new FossaField(Wait, "Wait"),
                                     new FossaField(ClickButton, "next"));
                }

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
            System.Threading.Thread.Sleep(100);
        }

        public void WaitForOops(String strId)
        {
            //MPS, 3/24/14 - TO DO: put logic in here to wait up to 30 secs for Oops step to appear
            //We need this b/c we can't predict how long the 3rd party credit pull svc call will take to respond in staging :(
            System.Threading.Thread.Sleep(10000);
        }
    }
}
