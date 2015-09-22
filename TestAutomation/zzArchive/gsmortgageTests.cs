using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using NUnit.Framework;
using TestAutomation.LendingTree.tl;

namespace TestAutomation.LendingTree.zzArchive
{
    [TestFixture]
    public class gsmortgageTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }


        [Test]
        public void gsmortgage_01_RefiPrimaryFirstAndSecond()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_02_RefiSecondaryFirstPlusCash()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_03_RefiInvestmentFirstOnlyNoCash()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_04_RefiSsnValidationTest()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Verify error message on SSN field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(gsmortgage.DoesPageContainText("Please enter your social security number."));

            // Populate the Dictionary with valid SSN and re-fill SSN
            testData["BorrowerSsn1"] = "980";
            testData["BorrowerSsn2"] = strRandom.Substring(0, 2);
            testData["BorrowerSsn3"] = strRandom.Substring(2, 4);
            gsmortgage.FillSSN(testData);
            gsmortgage.SubmitQF();

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_05_PurchasePrimary()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_06_PurchaseSecondary()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_07_PurchaseInvestmentProperty()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_08_RefiRequiredFieldTest()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);
            gsmortgage.InitializeMortgageVariables(testData);

            // Navigate to valid change QF
            gsmortgage.NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", testData["Template"], 
                testData["Variation"], testData["QueryString"]);

            // Click through the steps without filling anything out
            gsmortgage.ClickThroughSteps(13);

            System.Threading.Thread.Sleep(2000);

            // Create collection of expected error text
            List<String> expectedErrors = new List<String>()
            {
                "Please enter your property ZIP code.",
                "Please select the home value.",
                "Please select the monthly payment.",
                "Please select your mortgage balance.",
                "Please enter your date of birth.",
                "This field is required.",
                "This field is required.",
                "Please enter your first name.",
                "Please enter your last name.",
                "Please enter your street address.",
                "Please enter your ZIP code.",
                "This field is required.",
                "This field is required.",
                "Please enter your social security number.",
                "Please enter your email address.",
                "Please enter a password."
            };

            // Get all elements where class = 'error' into a collection called 'actualErrors'
            IList<IWebElement> actualErrors = new List<IWebElement>();
            IList<IWebElement> allLabels = driver.FindElements(By.TagName("label"));

            // Loop all labels and pick out the ones that have class attribute = "error", generated attribute = "true"
            for (int i = 0; i < allLabels.Count; i++)
            {
                Common.ReportEvent("DEBUG", String.Format("Label number {0}: class attribute is '{1}', for attribute is '{2}'," 
                    + " generated attribute is '{3}', innerText is '{4}'.", i, allLabels[i].GetAttribute("class"), 
                    allLabels[i].GetAttribute("for"), allLabels[i].GetAttribute("generated"), allLabels[i].Text));

                if (allLabels[i].GetAttribute("class").Equals("error") && 
                    allLabels[i].GetAttribute("generated").Equals("true") && 
                    allLabels[i].Text.Length > 0)
                {
                    actualErrors.Add(allLabels[i]);
                }
            }

            if (actualErrors.Count > 0)
            {
                // Check to see that counts of expectedErrors and actualErrors are equal.
                if (Validation.IsTrue(actualErrors.Count.Equals(expectedErrors.Count)))
                {
                    Common.ReportEvent(Common.PASS, String.Format
                        ("The counts of actual and expected error messages are equal.  Actual error msg count = {0}.  "
                        + "Expected error msg count = {1}.", actualErrors.Count, expectedErrors.Count));
                }
                else
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The counts of actual and expected error messages are NOT equal!  Actual error msg count = {0}.  "
                        + "Expected error msg count = {1}.", actualErrors.Count, expectedErrors.Count));
                    gsmortgage.RecordScreenshot("ValidationPageFail");
                }

                // Loop through IWebElements and verify 'Text' property matches expected, and 'Displayed' property = True
                for (int i = 0; i < expectedErrors.Count; i++)
                {
                    // Verify the IWebElement display property = True
                    if (Validation.IsTrue(actualErrors[i].Displayed))
                    {
                        Common.ReportEvent(Common.PASS, String.Format
                            ("The 'Displayed' property for IWebElement {0} matches the expected value, True.",
                            actualErrors[i].GetAttribute("id")));
                    }
                    else
                    {
                        Common.ReportEvent(Common.FAIL, String.Format
                            ("The 'Displayed' property for IWebElement {0} DID NOT match the expected value, True.",
                            actualErrors[i].GetAttribute("id")));
                    }

                    // Verify actualErrors(i).text = expectedErrors.Item(i)
                    Validation.StringCompare(expectedErrors[i], actualErrors[i].Text);
                }
            }
            else
            {
                //Report fail - no 'error' elements found on the page
                Common.ReportEvent(Common.FAIL, "The count of actual errors on the page was not > 0.  " +
                    "Unable to fine Elements of class name 'error' on the page.");
            }

            // Fill out and submit the final page
            gsmortgage.FillOutValidationStep(testData);
            System.Threading.Thread.Sleep(500);
            gsmortgage.WaitForAjaxToComplete(10);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_09_BriteVerifyOn()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Verify error message on Email Address field
            System.Threading.Thread.Sleep(2000);
            Validation.IsTrue(gsmortgage.DoesPageContainText(testData["BorrowerFirstName"] + 
                ", please correct any errors in entered information."));
            Validation.IsTrue(gsmortgage.DoesPageContainText("Please enter a valid email address."));

            // Populate the Dictionary with a valid Email Address and re-fill SSN
            testData["EmailAddress"] = "qa@lendingtree.com";
            gsmortgage.FillEmailAddress(testData);
            gsmortgage.SubmitQF();

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_10_UniformControlsIE()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_11_UniformControlsFirefox()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);

            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_12_MinLoanAmtVariation3()
        {
            String strExpected = "";
            mortgagePage gsmortgage = new mortgagePage(driver);
            gsmortgage.InitializeMortgageVariables(testData);

            // Call pages individually
            gsmortgage.NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", testData["Template"],
                testData["Variation"], testData["QueryString"]);
            gsmortgage.CompleteStep1(testData, true);
            gsmortgage.CompleteStep2(testData, true);
            gsmortgage.CompleteStep3(testData, true);
            gsmortgage.CompleteStep4(testData, true);

            // On step 5, verify the following for 1st Mortgage Balance dropdown:
            //   a. est-mortgage-balance defaults to “select mortgage balance”
            //   b. est-mortgage-balance min value = 10000 ($10,000 or less)
            //   c. est-mortgage-balance max value = 190001 ($190,001 - $200,000)
            gsmortgage.WaitForElement(By.ClassName("step-5"), 10);
            Assert.IsTrue(gsmortgage.IsElementPresent(By.ClassName("step-5")), "Unable to verify the script is on Step 5. "
                + "Cannot locate element of class 'step-5'.");
            Validation.VerifySelectedOption(driver, "est-mortgage-balance", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "est-mortgage-balance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "est-mortgage-balance", "$190,001 - $200,000");

            gsmortgage.CompleteStep5(testData, true);

            // On step 6, input “Yes” for 2nd Mortgage.
            gsmortgage.WaitForElement(By.ClassName("step-6"), 10);
            Assert.IsTrue(gsmortgage.IsElementPresent(By.ClassName("step-6")), "Unable to verify the script is on Step 6. "
                + "Cannot locate element of class 'step-6'.");
            gsmortgage.ClickRadio("second-mortgage-yes");
            System.Threading.Thread.Sleep(2000);

            // On step 6, verify the following for the 2nd Mortgage Balance and Additional Cash dropdowns:
            //   a. second-mortgage-balance defaults to “select mortgage balance”
            //   b. second-mortgage-balance min value = 10000 ($10,000 or less)
            //   c. second-mortgage-balance max value = 140001 ($140,001 - $150,000)
            //   d. cash-out defaults to 100000 ($95,001 - $100,000)
            //   e. cash-out min value = 0 ($0 No Cash)
            //   f. cash-out max value = 150000 ($140,001 - $150,000)
            Validation.VerifySelectedOption(driver, "second-mortgage-balance", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "second-mortgage-balance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "second-mortgage-balance", "$140,001 - $150,000");
            Validation.VerifySelectedOption(driver, "cash-out", "$95,001 - $100,000");
            Validation.VerifyDropdownListMinValue(driver, "cash-out", "$0 No Cash");
            Validation.VerifyDropdownListMaxValue(driver, "cash-out", "$140,001 - $150,000");

            // On step 6, select a 2nd Mortgage Balance of 10000 ($10,000 or less)
            gsmortgage.SelectByText("second-mortgage-balance", "$10,000 or less");
            System.Threading.Thread.Sleep(2000);

            // On Step 6, verify the following for Additional Cash dropdown:
            //  a. cash-out defaults to 90000 ($85,001 - $90,000)
            //  b. cash-out min value = 0 ($0 No Cash)
            //  c. cash-out max value = 140000 ($130,001 - $140,000)
            Validation.VerifySelectedOption(driver, "cash-out", "$85,001 - $90,000");
            Validation.VerifyDropdownListMinValue(driver, "cash-out", "$0 No Cash");
            Validation.VerifyDropdownListMaxValue(driver, "cash-out", "$130,001 - $140,000");

            // Enter Additional Cash of 15000 ($10,001 - $15,000)
            // Verify div id=”cashOutTip” is visible (“At this time, most lenders…)
            gsmortgage.SelectByText("cash-out", "$10,001 - $15,000");
            System.Threading.Thread.Sleep(2000);
            strExpected = "At this time most lenders in our network do not accept loan requests with amounts less "
                    + "than $80,000 so it could be difficult to match you. If possible, please revisit your "
                    + "selections for mortgage balances and cash-out and see if you can meet this criteria.";
            Validation.StringCompare(strExpected, gsmortgage.GetElement("cashOutTip").Text);

            // Enter Additional Cash of 20000 ($15,001 - $20,000)
            // Verify div id=”cashOutTip” is NOT visible (innerText is empty)
            gsmortgage.SelectByText("cash-out", "$15,001 - $20,000");
            System.Threading.Thread.Sleep(2000);
            strExpected = "";
            Validation.StringCompare(strExpected, gsmortgage.GetElement("cashOutTip").Text);

            gsmortgage.CompleteStep6(testData, true);

            // Complete the rest of the steps with valid data.
            gsmortgage.CompleteStep7(testData, true);
            gsmortgage.CompleteStep8(testData, true);
            gsmortgage.CompleteStep9(testData, true);
            gsmortgage.CompleteStep10(testData, true);
            gsmortgage.CompleteStep11(testData, true);
            gsmortgage.CompleteStep12(testData, true);
            gsmortgage.CompleteStep13(testData, true);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void gsmortgage_13_MinLoanAmtVariation1()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);
            DataTable objDataTable = new DataTable();

            // Call pages individually
            gsmortgage.InitializeMortgageVariables(testData);
            gsmortgage.NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", testData["Template"], 
                testData["Variation"], testData["QueryString"]);
            gsmortgage.CompleteStep1(testData, true);
            gsmortgage.CompleteStep2(testData, true);
            gsmortgage.CompleteStep3(testData, true);
            gsmortgage.CompleteStep4(testData, true);

            // On step 5, verify the following for 1st Mortgage Balance dropdown:
            //   a. est-mortgage-balance defaults to “select mortgage balance”
            //   b. est-mortgage-balance min value = 10000 ($10,000 or less)
            //   c. est-mortgage-balance max value = 180001 ($180,001 - $190,000)
            gsmortgage.WaitForElement(By.ClassName("step-5"), 10);
            Assert.IsTrue(gsmortgage.IsElementPresent(By.ClassName("step-5")), "Unable to verify the script is on Step 5. "
                + "Cannot locate element of class 'step-5'.");
            Validation.VerifySelectedOption(driver, "est-mortgage-balance", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "est-mortgage-balance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "est-mortgage-balance", "$180,001 - $190,000");

            gsmortgage.CompleteStep5(testData, true);

            // On step 6, input “Yes” for 2nd Mortgage.
            gsmortgage.WaitForElement(By.ClassName("step-6"), 10);
            Assert.IsTrue(gsmortgage.IsElementPresent(By.ClassName("step-6")), "Unable to verify the script is on Step 6. "
                + "Cannot locate element of class 'step-6'.");
            gsmortgage.ClickRadio("second-mortgage-yes");
            //System.Threading.Thread.Sleep(2000);
            gsmortgage.WaitForAjaxToComplete(2);

            // On step 6, verify the following for the 2nd Mortgage Balance and Additional Cash dropdowns:
            //   a. second-mortgage-balance defaults to “select mortgage balance”
            //   b. second-mortgage-balance min value = 10000 ($10,000 or less)
            //   c. second-mortgage-balance max value = 100001 ($100,001 - $110,000)
            //   d. cash-out defaults to 100000 ($95,001 - $100,000)
            //   e. cash-out min value = 50000 ($45,001 - $50,000)
            //   f. cash-out max value = 110000 ($105,001 - $110,000)
            Validation.VerifySelectedOption(driver, "second-mortgage-balance", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "second-mortgage-balance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "second-mortgage-balance", "$100,001 - $110,000");
            Validation.VerifySelectedOption(driver, "cash-out", "$95,001 - $100,000");
            Validation.VerifyDropdownListMinValue(driver, "cash-out", "$45,001 - $50,000");
            Validation.VerifyDropdownListMaxValue(driver, "cash-out", "$105,001 - $110,000");

            // On step 6, select a 2nd Mortgage Balance of 10000 ($10,000 or less)
            gsmortgage.SelectByText("second-mortgage-balance", "$10,000 or less");
            //System.Threading.Thread.Sleep(2000);
            gsmortgage.WaitForAjaxToComplete(2);

            // On Step 6, verify the following for Additional Cash dropdown:
            //  a. cash-out defaults to 90000 ($85,001 - $90,000)
            //  b. cash-out min value = 40000 ($35,001 - $40,000)
            //  c. cash-out max value = 100000 ($95,001 - $100,000)
            Validation.VerifySelectedOption(driver, "cash-out", "$85,001 - $90,000");
            Validation.VerifyDropdownListMinValue(driver, "cash-out", "$35,001 - $40,000");
            Validation.VerifyDropdownListMaxValue(driver, "cash-out", "$95,001 - $100,000");

            gsmortgage.CompleteStep6(testData, true);

            // Complete the rest of the steps with valid data.
            gsmortgage.CompleteStep7(testData, true);
            gsmortgage.CompleteStep8(testData, true);
            gsmortgage.CompleteStep9(testData, true);
            gsmortgage.CompleteStep10(testData, true);
            gsmortgage.CompleteStep11(testData, true);
            gsmortgage.CompleteStep12(testData, true);
            gsmortgage.CompleteStep13(testData, true);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);

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
                    + "WHERE qf.QFormUID = '" + gsmortgage.strQFormUID + "'";

            // Query the DB and get back a DataTable object representing the rows returned by the query.
            objDataTable = Common.DBGetDataTable(strDBConnectionString, strQuery);

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

        [Test]
        public void gsmortgage_14_MtgInputStyleTextbox()
        {
            mortgagePage gsmortgage = new mortgagePage(driver);


            // Fill out and submit a QF
            gsmortgage.FillOutValidQF(testData);

            // Handle the cross-sells
            gsmortgage.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(gsmortgage.strQFormUID));

            // Verify redirect to My LendingTree
            gsmortgage.VerifyRedirectToMyLendingTree(testData);
        }
    }
}
