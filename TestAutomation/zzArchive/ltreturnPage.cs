using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.zzArchive
{
    public class ltreturnPage : PageBase
    {
        private readonly IWebDriver ltreturn;
        private const String strTid = "lt-return";

        // Constructor
        public ltreturnPage(IWebDriver driver) : base(driver)
        {
            ltreturn = driver;
        }


        public void FillOutValidQF(Dictionary<string, string> testData)
        {
            NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", strTid, testData["Variation"]);
            CompleteStep1(testData);
        }

    
        public void CompleteStep1(Dictionary<string, string> testData)
        {
            Common.ReportEvent(Common.INFO, "***** Starting lt-return *****");
            WaitForElement(By.Id("homeloan-product-type"), 5);
            Assert.IsTrue(IsElementPresent(By.Id("homeloan-product-type")), "Unable to verify the lt-return form displayed. "
                + "Cannot locate element by ID 'homeloan-product-type'.");

            // Capture/Report the GUID and QFVersion
            System.Threading.Thread.Sleep(5000);
            strQFormUID = ltreturn.FindElement(By.Id("GUID")).Text;
            Common.ReportEvent(Common.INFO, String.Format("The QForm GUID = {0}", strQFormUID));

            // Fill out the fields & click Continue
            SelectByValue("homeloan-product-type", testData["LoanType"]);
            SelectByText("property-type", testData["HomeDescription"]);

            // Pause while the form displays Purchase or Refinance fields, based on LOAN_TYPE selection above
            System.Threading.Thread.Sleep(1000);

            ////  Begin Refinance specific fields  ////
            if (testData["LoanType"].Equals("Refinance", StringComparison.OrdinalIgnoreCase))
            {
                SelectByText("property-use", testData["PropertyUseType"]);
                Fill("property-zip", testData["PropertyZipCode"]);
                SelectByText("estproperty-value", testData["RefiPropertyValue"]);
                System.Threading.Thread.Sleep(500);
                SelectByText("current-monthly-payment", testData["FirstMortgagePayment"]);
                SelectByText("est-mortgage-balance", testData["FirstMortgageBalance"]);
                SelectByText("1st-mortgage-interest-rate", testData["FirstMortgageRate"]);

                if (testData["SecondMortgageYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("second-mortgage-yes");
                    // Explicit wait to ensure the Browser displays the 2nd mortgage questions
                    WaitForElementDisplayed(By.Id("2nd-mortgage-monthly-payment"), 5);
                    SelectByText("2nd-mortgage-monthly-payment", testData["SecondMortgagePayment"]);
                    SelectByText("second-mortgage-balance", testData["SecondMortgageBalance"]);
                    System.Threading.Thread.Sleep(500);
                    SelectByText("2nd-mortgage-interest-rate", testData["SecondMortgageRate"]);
                }
                else
                {
                    ClickRadio("second-mortgage-no");
                }

                SelectByText("cash-out", testData["RefiCashoutAmount"]);
            }
            ////  Begin Purchase specific fields  ////
            else if (testData["LoanType"].Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                if (testData["FoundNewHomeYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("new-home-yes");
                }
                else
                {
                    ClickRadio("new-home-no");
                }

                SelectByText("property-state", testData["PropertyState"]);
                WaitForDropdownRefresh("property-city", 5);
                SelectByText("property-city", testData["PropertyCity"]);
                SelectByText("purchase-price", testData["PurchasePrice"]);
                SelectByText("down-payment-amt", testData["PurchaseDownPayment"]);

                if (testData["CurrentREAgentYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("current-realestate-agent_yes");
                }
                else
                {
                    ClickRadio("current-realestate-agent_no");
                }

                if (testData["RealtorConsultYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("inline_realtor_optin-yes");
                }
                else
                {
                    ClickRadio("inline_realtor_optin-no");
                }

                SelectByText("property-use", testData["PropertyUseType"]);
            }
            else
            {
                // Report invalid Loan_Type and stop the test

            }
            
            ////  Begin shared Purchase/Refi fields  ////
            if (IsElementDisplayed(By.Id("rLendingTreeOptInYes")))
            {
                if (testData["LTLOptInYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    ClickRadio("rLendingTreeOptInYes");
                }
                else
                {
                    ClickRadio("rLendingTreeOptInNo");
                }
            }

            SelectByText("birth-month", testData["DateOfBirthMonth"]);
            SelectByText("birth-day", testData["DateOfBirthDay"]);
            SelectByText("birth-year", testData["DateOfBirthYear"]);
            SelectByText("stated-credit-history", testData["CreditProfile"]);

            if (testData["MilitaryServiceYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("is-veteran-yes");
            }
            else
            {
                ClickRadio("is-veteran-no");
            }

            SelectByText("foreclosure-text", testData["ForeclosureHistory"]);

            // If Bankruptcy = Y, then complete bankruptcy specific fields
            if (testData["BankruptcyYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("declared-bankruptcy-yes");
                WaitForElementDisplayed(By.Id("bankruptcy-discharged"), 5);
                SelectByText("bankruptcy-discharged", testData["BankruptcyHistory"]);
            }
            else
            {
                ClickRadio("declared-bankruptcy-no");
            }

            // Handle debtConsolidate variation
            if (IsElementDisplayed(By.Id("ddcreditcarddebt")))
            {
                SelectByText("ddcreditcarddebt", testData["CreditCardDebtAmount"]);
                System.Threading.Thread.Sleep(1000);
                if (IsElementDisplayed(By.Id("rDebtConsultationOptIn")))  //displays if credit card debt > $10,000
                {
                    if (testData["DebtConsultYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        ClickRadio("debt-consultation-yes");
                    }
                    else
                    {
                        ClickRadio("debt-consultation-no");
                    }
                }
            }

            Fill("first-name", testData["BorrowerFirstName"]);
            Fill("last-name", testData["BorrowerLastName"]);
            Fill("street1", testData["BorrowerStreetAddress"]);

            // If Zip field is displayed, then fill it out
            if (IsElementDisplayed(By.Id("zip-code")))
            {
                Fill("zip-code", testData["BorrowerZipCode"]);
            }

            Fill("home-phone-one", testData["BorrowerHomePhone1"]);
            Fill("home-phone-two", testData["BorrowerHomePhone2"]);
            Fill("home-phone-three", testData["BorrowerHomePhone3"]);
            Fill("social-security-one", testData["BorrowerSsn1"]);
            Fill("social-security-two", testData["BorrowerSsn2"]);
            Fill("social-security-three", testData["BorrowerSsn3"]);
            Fill("email", testData["EmailAddress"]);
            Fill("password", testData["Password"]);

            System.Threading.Thread.Sleep(1000);
            ClickButton("next");
        }
    }
}
