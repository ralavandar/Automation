﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.zzArchive
{
    [TestFixture]
    public class gsguyTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private gsguyPage gsguy;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            gsguy = new gsguyPage(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        private void FinishTest()
        {
            // Handle the cross-sells
            gsguy.BypassTlCrossSells();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(gsguy.strQFormUID));

            // Verify redirect to My LendingTree
            gsguy.VerifyRedirectToMyLendingTree(testData);
        }

        //TODO: Configure this to be "random" browser again!
        //TODO GAW: I copy-pasted the default VID from the variation list on google docs into this test's definition row. Was that correct to do?
        //TODO: Configure this to be in STAGE instead of DEV.
        //TODO: mortgage's FirstMortgageBalance was $275,001 - $300,000, but that's higher than the max value on mort-tree. Switched it for $105,001 - $110,000. Is that okay?
        //TODO: Turned briteverify off.
        [Test]
        public void gsguy_01_RefiPrimaryFirstAndSecond()
        {
            // Fill out and submit a QF
            gsguy.FillOutValidQF();

            FinishTest();
        }


        [Test]
        public void gsguy_02_RefiSecondaryFirstPlusCash()
        {
            // Fill out and submit a QF
            gsguy.FillOutValidQF();

            FinishTest();
        }


        [Test]
        public void gsguy_03_RefiInvestmentFirstOnlyNoCash()
        {
            // Fill out and submit a QF
            gsguy.FillOutValidQF();

            FinishTest();
        }


        [Test]
        public void gsguy_04_RefiSsnValidationTest()
        {
            IFormField[][] steps = gsguy.ValidQFSteps;

            gsguy.StartForm();
            gsguy.PerformSteps(steps, 1, 3);

            gsguy.PrepareStep(4, true);
            gsguy.FillOutStep(steps[4]);
            gsguy.ClickButton("next"); //Shouldn't actually advance, but we need to blur the SSN fields.

            // Verify error message on SSN field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(gsguy.DoesPageContainText("Please enter your social security number."));

            // Populate the Dictionary with valid SSN and re-fill SSN
            testData["BorrowerSsn1"] = "980";
            testData["BorrowerSsn2"] = strRandom.Substring(0, 2);
            testData["BorrowerSsn3"] = strRandom.Substring(2, 4);
            gsguy.FillOutStep(steps[4]);
            gsguy.ConcludeStep();

            FinishTest();
        }


        [Test]
        public void gsguy_05_PurchasePrimary()
        {
            // Fill out and submit a QF
            gsguy.FillOutValidQF();

            FinishTest();
        }


        [Test]
        public void gsguy_06_PurchaseSecondary()
        {
            // Fill out and submit a QF
            gsguy.FillOutValidQF();

            FinishTest();
        }


        [Test]
        public void gsguy_07_PurchaseInvestmentProperty()
        {
            // Fill out and submit a QF
            gsguy.FillOutValidQF();

            FinishTest();
        }

        //TODO: I don't think this works, since gsguy does validation along the way.
        //[Test]
        //public void mortgage_08_RefiRequiredFieldTest()
        //{
        //    gsguyPage gsguy = new gsguyPage(driver);
        //    gsguy.InitializeMortgageVariables(testData);

        //    // Navigate to valid change QF
        //    gsguy.NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", testData["Template"], testData["Variation"], testData["QueryString"]);

        //    // Click through the steps without filling anything out
        //    gsguy.ClickThroughSteps(13);

        //    System.Threading.Thread.Sleep(2000);

        //    // Create collection of expected error text
        //    List<String> expectedErrors = new List<String>()
        //    {
        //        "Please enter your property ZIP code.",
        //        "Please select the home value.",
        //        "Please select the monthly payment.",
        //        "Please select your mortgage balance.",
        //        "Please enter your date of birth.",
        //        "This field is required.",
        //        "This field is required.",
        //        "Please enter your first name.",
        //        "Please enter your last name.",
        //        "Please enter your street address.",
        //        "Please enter your ZIP code.",
        //        "This field is required.",
        //        "This field is required.",
        //        "Please enter your social security number.",
        //        "Please enter your email address.",
        //        "Please enter a password."
        //    };

        //    // Get all elements where class = 'error' into a collection called 'actualErrors'
        //    IList<IWebElement> actualErrors = new List<IWebElement>();
        //    IList<IWebElement> allLabels = driver.FindElements(By.TagName("label"));

        //    // Loop all labels and pick out the ones that have class attribute = "error", generated attribute = "true"
        //    for (int i = 0; i < allLabels.Count; i++)
        //    {
        //        //Common.ReportEvent("DEBUG", String.Format("Label number {0}: class attribute is '{1}', for attribute is '{2}', generated attribute is '{3}', innerText is '{4}'.",
        //        //    i, allLabels[i].GetAttribute("class"), allLabels[i].GetAttribute("for"), allLabels[i].GetAttribute("generated"), allLabels[i].Text));

        //        if (allLabels[i].GetAttribute("class").Equals("error") && allLabels[i].GetAttribute("generated").Equals("true") && allLabels[i].Text.Length > 0)
        //        {
        //            actualErrors.Add(allLabels[i]);
        //        }
        //    }

        //    if (actualErrors.Count > 0)
        //    {
        //        // Check to see that counts of expectedErrors and actualErrors are equal.
        //        if (Validation.IsTrue(actualErrors.Count.Equals(expectedErrors.Count)))
        //        {
        //            Common.ReportEvent(Common.PASS, String.Format
        //                ("The counts of actual and expected error messages are equal.  Actual error msg count = {0}.  "
        //                + "Expected error msg count = {1}.", actualErrors.Count, expectedErrors.Count));
        //        }
        //        else
        //        {
        //            Common.ReportEvent(Common.FAIL, String.Format
        //                ("The counts of actual and expected error messages are NOT equal!  Actual error msg count = {0}.  "
        //                + "Expected error msg count = {1}.", actualErrors.Count, expectedErrors.Count));
        //            gsguy.RecordScreenshot("ValidationPageFail");
        //        }

        //        // Loop through IWebElements and verify 'Text' property matches expected, and 'Displayed' property = True
        //        for (int i = 0; i < expectedErrors.Count; i++)
        //        {
        //            // Verify the IWebElement display property = True
        //            if (Validation.IsTrue(actualErrors[i].Displayed))
        //            {
        //                Common.ReportEvent(Common.PASS, String.Format
        //                    ("The 'Displayed' property for IWebElement {0} matches the expected value, True.",
        //                    actualErrors[i].GetAttribute("id")));
        //            }
        //            else
        //            {
        //                Common.ReportEvent(Common.FAIL, String.Format
        //                    ("The 'Displayed' property for IWebElement {0} DID NOT match the expected value, True.",
        //                    actualErrors[i].GetAttribute("id")));
        //            }

        //            // Verify actualErrors(i).text = expectedErrors.Item(i)
        //            Validation.StringCompare(expectedErrors[i], actualErrors[i].Text);
        //        }
        //    }
        //    else
        //    {
        //        //Report fail - no 'error' elements found on the page
        //        Common.ReportEvent(Common.FAIL, "The count of actual errors on the page was not > 0.  " +
        //            "Unable to fine Elements of class name 'error' on the page.");
        //    }

        //    // Fill out and submit the final page
        //    gsguy.FillOutValidationStep(testData);
        //    System.Threading.Thread.Sleep(500);
        //    gsguy.WaitForAjaxToComplete(10);

        //    // Handle the cross-sells
        //    gsguy.BypassTlCrossSells();

        //    // Check for the QForm in the DB
        //    Assert.IsTrue(VerifytQFormRecord(gsguy.strQFormUID));

        //    // Verify redirect to My LendingTree
        //    gsguy.VerifyRedirectToMyLendingTree(testData);
        //}


        [Test]
        public void gsguy_09_BriteVerifyOn()
        {
            IFormField[][] steps = gsguy.ValidQFSteps;

            gsguy.StartForm();
            gsguy.PerformSteps(steps, 1, 2);


            gsguy.PrepareStep(3, true);
            gsguy.FillOutStep(steps[3]);
            gsguy.ClickButton("next"); //Shouldn't actually advance, but we need to blur the field.

            // Verify error message on Email Address field
            System.Threading.Thread.Sleep(2000);

            //Validation.IsTrue(gsguy.DoesPageContainText(testData["BorrowerFirstName"] + ", please correct any errors in entered information."));
            Validation.IsTrue(gsguy.DoesPageContainText("Please enter a valid email address."));

            // Populate the Dictionary with a valid Email Address and re-fill SSN
            testData["EmailAddress"] = "qa@lendingtree.com";

            gsguy.FillOutStep(steps[3]);
            gsguy.ConcludeStep();

            gsguy.PerformStep(4, steps[4]);

            FinishTest();
        }

        [Test]
        public void gsguy_12_MinLoanAmtVariation3()
        {
            TestMinLoanAmount(firstMortgageMax: "$190,001 - $200,000",
                            secondMortgageMax: "$140,001 - $150,000", 
                            cashOutMin: "$0 No Cash", 
                            cashOutMax: "$130,001 - $140,000",
                            cashOutSelect: "$15,001 - $20,000");
        }

        
        [Test]
        public void gsguy_13_MinLoanAmtVariation1()
        {
            TestMinLoanAmount(firstMortgageMax: "$180,001 - $190,000",
                            secondMortgageMax: "$100,001 - $110,000",
                            cashOutMin: "$35,001 - $40,000",
                            cashOutMax: "$95,001 - $100,000");

            // Insert a 5 second pause here.  Apparently after the record is written to tQForm, there is a delay in getting 
            // the remaining data written to subsequent tables, specifically tQFormMortgageCalculation
            System.Threading.Thread.Sleep(5000);

            // Verify Data saved to the DB correctly
            // a. Verify ProposedLTV = 80.0005 (tQFormMortgageCalculation.ProposedLTVPercent)
            // b. Verify LoanAmt = 160001 (tQForm.AmountRequested)
            // c. Verify PresentLTV = 30.0005 (tQFormMortgageCalculation.PresentLTVPercent)

            // Build the connection string
            String strDBConnectionString = Common.DBBuildLendXConnectionString(testData["TestEnvironment"]);

            // Here is the query
            String strQuery = "SELECT qf.QFormID, qf.AmountRequested, qfmc.PresentLTVPercent, qfmc.ProposedLTVPercent "
                    + "FROM LendX.dbo.tQForm qf with(nolock) "
                    + "JOIN LendX.dbo.tQFormMortgageCalculation qfmc ON qf.QFormID = qfmc.QFormID "
                    + "WHERE qf.QFormUID = '" + gsguy.strQFormUID + "'";

            // Query the DB and get back a DataTable object representing the rows returned by the query.
            var objDataTable = Common.DBGetDataTable(strDBConnectionString, strQuery);

            // Verify the query returned 1 and only 1 record.
            if (objDataTable.Rows.Count != 1)  // Report a FAIL
            {
                Common.ReportEvent(Common.FAIL, String.Format("The number of rows returned by the query ({0}) did not "
                    + "match the expected row count (1).", objDataTable.Rows.Count));
            }
            else  // Continue on with the validation
            {
                // Validate the 3 fields against their expected values.
                Validation.AreEqual(160001.00, objDataTable.Rows[0].Field<Decimal>(objDataTable.Columns[1]), "AmountRequested");
                Validation.AreEqual(30.0005, objDataTable.Rows[0].Field<Decimal>(objDataTable.Columns[2]), "PresentLTVPercent");
                Validation.AreEqual(80.0005, objDataTable.Rows[0].Field<Decimal>(objDataTable.Columns[3]), "ProposedLTVPercent");
            }
        }

        private void TestMinLoanAmount(string firstMortgageMax, string secondMortgageMax, string cashOutMin, string cashOutMax, string cashOutSelect = null)
        {
            IFormField[][] steps = gsguy.ValidQFSteps;

            gsguy.StartForm();
            gsguy.PerformSteps(steps, 1, 1);

            //gsguy.PrepareStep(5);

            // On step 5, verify the following for 1st Mortgage Balance dropdown:
            //   a. est-mortgage-balance defaults to “select mortgage balance”
            //   b. est-mortgage-balance min value = 10000 ($10,000 or less)
            //   c. est-mortgage-balance max value = 190001 ($190,001 - $200,000)
            Validation.IsTrue(gsguy.IsElementPresent(By.ClassName("step-2")), "Verifying that the script is on step 2.");
            Validation.VerifySelectedOption(driver, "est-mortgage-balance", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "est-mortgage-balance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "est-mortgage-balance", firstMortgageMax);

            gsguy.FillOutStep(steps[2]);
            
            gsguy.ClickRadio("second-mortgage-yes");
            gsguy.WaitForElementNotDisplayNone(By.Id("second-mortgage-balance"), 10);

            // On step 2, verify the following for the 2nd Mortgage Balance and Additional Cash dropdowns:
            //   a. second-mortgage-balance defaults to “select mortgage balance”
            //   b. second-mortgage-balance min value = 10000 ($10,000 or less)
            //   c. second-mortgage-balance max value = 140001 ($140,001 - $150,000)
            //   d. cash-out defaults to 100000 ($95,001 - $100,000)
            //   e. cash-out min value = 0 ($0 No Cash)
            //   f. cash-out max value = 150000 ($140,001 - $150,000)
            Validation.VerifySelectedOption(driver, "second-mortgage-balance", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "second-mortgage-balance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "second-mortgage-balance", secondMortgageMax);


            // On step 2, select a 2nd Mortgage Balance of 10000 ($10,000 or less)
            gsguy.SelectByText("second-mortgage-balance", "$10,000 or less");
            System.Threading.Thread.Sleep(2000);

            
            // On Step 2, verify the following for Additional Cash dropdown:
            //  a. cash-out defaults to 90000 ($85,001 - $90,000)
            //  b. cash-out min value = 0 ($0 No Cash)
            //  c. cash-out max value = 140000 ($130,001 - $140,000)
            Validation.VerifySelectedOption(driver, "cash-out", "$85,001 - $90,000");
            Validation.VerifyDropdownListMinValue(driver, "cash-out", cashOutMin);
            Validation.VerifyDropdownListMaxValue(driver, "cash-out", cashOutMax);

            // Enter Additional Cash of 20000 ($15,001 - $20,000)
            // Verify div id=”cashOutTip” is NOT visible (innerText is empty)
            if(cashOutSelect != null) gsguy.SelectByText("cash-out", cashOutSelect);

            gsguy.ConcludeStep();

            // Complete the rest of the steps with valid data.
            gsguy.PerformSteps(steps, 3, 4);

            FinishTest();
        }

    }
}
