using System;
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
    public class ltchangeTests : SeleniumTestBase
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
        public void ltchange_01_RefiPrimaryFirstAndSecond()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_02_RefiSecondaryFirstPlusCash()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_03_RefiInvestmentFirstOnlyNoCash()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_04_RefiSsnValidationTest()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Verify error message on SSN field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(lt_change.DoesPageContainText("Please enter a complete SSN."));

            // Populate the Dictionary with valid SSN and re-fill SSN
            testData["BorrowerSsn1"] = "980";
            testData["BorrowerSsn2"] = strRandom.Substring(0, 2);
            testData["BorrowerSsn3"] = strRandom.Substring(2, 4);
            lt_change.FillSSN(testData);
            lt_change.SubmitQF();

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_05_PurchasePrimary()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_06_PurchaseSecondary()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_07_PurchaseInvestmentProperty()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_08_RefiRequiredFieldTest()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Navigate to valid change QF
            lt_change.InitializeMortgageVariables(testData);
            lt_change.NavigateToFossaForm(testData["TestEnvironment"], "QuickMatchFormLoader.aspx", "lt-change", testData["Variation"]);

            // Click through the steps without filling anything out
            lt_change.ClickThroughSteps(13);

            // Verify all validation errors - this will be huge
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(lt_change.DoesPageContainText("Please correct any errors in entered information."));

            // Create collection of expected error text
            List<String> expectedErrors = new List<String>()
            {
                "Please enter the valid ZIP code.",
                "Please select the home value.",
                "Please select the monthly payment.",
                "Please select the mortgage balance.",
                "Please enter valid birth date.",
                "Please select military service.",
                "Please select bankruptcy.",
                "Please enter your valid first name.",
                "Please enter your valid last name.",
                "Please enter a Mailing Address containing one space between the street number and the street name.",
                "Please enter your ZIP code.",
                "Please enter a valid phone number",
                "Please enter a valid phone number",
                "Please enter a complete SSN.",
                "Please enter a valid email address.",
                "Please enter password."
            };

            // Get all elements where class name = 'error-text' into a collection called 'actualErrors'
            IList<IWebElement> actualErrors = driver.FindElements(By.ClassName("error-text"));

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
                //end loop
            }
            else
            {
                //Report fail - no 'error-text' elements found on the page
                Common.ReportEvent(Common.FAIL, "The count of actual errors on the page was not > 0.  " +
                    "Unable to fine Elements of class name 'error-text' on the page.");
            }
 
            // Fill out and submit the final page
            lt_change.FillOutValidationStep(testData);
            System.Threading.Thread.Sleep(2000);
            //lt_change.SubmitQF();

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_09_BriteVerifyOn()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Verify error message on Email Address field
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(lt_change.DoesPageContainText("Please correct any errors in entered information."));
            Assert.IsTrue(lt_change.DoesPageContainText("Please enter a valid email address."));

            // Populate the Dictionary with a valid Email Address and re-fill SSN
            testData["EmailAddress"] = "qa@lendingtree.com";
            lt_change.FillEmailAddress(testData);
            System.Threading.Thread.Sleep(2000);
            lt_change.SubmitQF();

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_10_UniformControlsIE()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_11_UniformControlsFirefox()
        {
            ltchangePage lt_change = new ltchangePage(driver);

            // Fill out and submit a QF
            lt_change.FillOutValidQF(testData);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_12_MinLoanAmtVariation4()
        {
            String strExpected = "";
            ltchangePage lt_change = new ltchangePage(driver);

            // Call pages individually
            lt_change.InitializeMortgageVariables(testData);
            lt_change.NavigateToFossaForm(testData["TestEnvironment"], "QuickMatchFormLoader.aspx", "lt-change", testData["Variation"]);
            lt_change.CompleteStep1(testData, true);
            lt_change.CompleteStep2(testData, true);
            lt_change.CompleteStep3(testData, true);
            lt_change.CompleteStep4(testData, true);

            // On step 5, verify the following for 1st Mortgage Balance dropdown:
            //   a. ddBalance defaults to “select mortgage balance”
            //   b. ddBalance min value = 10000 ($10,000 or less)
            //   c. ddBalance max value = 190001 ($190,001 - $200,000)
            lt_change.WaitForElement(By.ClassName("step5"), 10);
            Assert.IsTrue(lt_change.IsElementPresent(By.ClassName("step5")), "Unable to verify the script is on Step 5. "
                + "Cannot locate element of class 'step5'.");
            Validation.VerifySelectedOption(driver, "ddBalance", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "ddBalance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "ddBalance", "$190,001 - $200,000");

            lt_change.CompleteStep5(testData, true);

            // On step 6, input “Yes” for 2nd Mortgage.
            lt_change.WaitForElement(By.ClassName("step6"), 10);
            Assert.IsTrue(lt_change.IsElementPresent(By.ClassName("step6")), "Unable to verify the script is on Step 6. "
                + "Cannot locate element of class 'step6'.");
            lt_change.ClickRadio("rSecondMortgageYes");
            System.Threading.Thread.Sleep(2000);

            // On step 6, verify the following for the 2nd Mortgage Balance and Additional Cash dropdowns:
            //   a. ddBalance2 defaults to “select mortgage balance”
            //   b. ddBalance2 min value = 10000 ($10,000 or less)
            //   c. ddBalance2 max value = 140001 ($140,001 - $150,000)
            //   d. ddAdditional defaults to 100000 ($95,001 - $100,000)
            //   e. ddAdditional min value = 0 ($0 No Cash)
            //   f. ddAdditional max value = 150000 ($140,001 - $150,000)
            Validation.VerifySelectedOption(driver, "ddBalance2", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "ddBalance2", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "ddBalance2", "$140,001 - $150,000");
            Validation.VerifySelectedOption(driver, "ddAdditional", "$95,001 - $100,000");
            Validation.VerifyDropdownListMinValue(driver, "ddAdditional", "$0 No Cash");
            Validation.VerifyDropdownListMaxValue(driver, "ddAdditional", "$140,001 - $150,000");

            // On step 6, select a 2nd Mortgage Balance of 10000 ($10,000 or less)
            lt_change.SelectByText("ddBalance2", "$10,000 or less");
            System.Threading.Thread.Sleep(2000);

            // On Step 6, verify the following for Additional Cash dropdown:
            //  a. ddAdditional defaults to 90000 ($85,001 - $90,000)
            //  b. ddAdditional min value = 0 ($0 No Cash)
            //  c. ddAdditional max value = 140000 ($130,001 - $140,000)
            Validation.VerifySelectedOption(driver, "ddAdditional", "$85,001 - $90,000");
            Validation.VerifyDropdownListMinValue(driver, "ddAdditional", "$0 No Cash");
            Validation.VerifyDropdownListMaxValue(driver, "ddAdditional", "$130,001 - $140,000");

            // Enter Additional Cash of 15000 ($10,001 - $15,000)
            // Verify div id=”cashOutTip” is visible (“At this time, most lenders…)
            lt_change.SelectByText("ddAdditional", "$10,001 - $15,000");
            System.Threading.Thread.Sleep(2000);
            strExpected = "At this time most lenders in our network do not accept loan requests with amounts less "
                    + "than $80,000 so it could be difficult to match you. If possible, please revisit your "
                    + "selections for mortgage balances and cash-out and see if you can meet this criteria.";
            Validation.StringCompare(strExpected, lt_change.GetElement("cashOutTip").Text);

            // Enter Additional Cash of 20000 ($15,001 - $20,000)
            // Verify div id=”cashOutTip” is NOT visible (innerText is empty)
            lt_change.SelectByText("ddAdditional", "$15,001 - $20,000");
            System.Threading.Thread.Sleep(2000);
            strExpected = "";
            Validation.StringCompare(strExpected, lt_change.GetElement("cashOutTip").Text);

            lt_change.CompleteStep6(testData, true);

            // Complete the rest of the steps with valid data.
            lt_change.CompleteStep7(testData, true);
            lt_change.CompleteStep8(testData, true);
            lt_change.CompleteStep9(testData, true);
            lt_change.CompleteStep10(testData, true);
            lt_change.CompleteStep11(testData, true);
            lt_change.CompleteStep12(testData, true);
            lt_change.CompleteStep13(testData, true);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);
        }


        [Test]
        public void ltchange_13_MinLoanAmtVariation1()
        {
            ltchangePage lt_change = new ltchangePage(driver);
            DataTable objDataTable = new DataTable();

            // Call pages individually
            lt_change.InitializeMortgageVariables(testData);
            lt_change.NavigateToFossaForm(testData["TestEnvironment"], "QuickMatchFormLoader.aspx", "lt-change", testData["Variation"]);
            lt_change.CompleteStep1(testData, true);
            lt_change.CompleteStep2(testData, true);
            lt_change.CompleteStep3(testData, true);
            lt_change.CompleteStep4(testData, true);

            // On step 5, verify the following for 1st Mortgage Balance dropdown:
            //   a. ddBalance defaults to “select mortgage balance”
            //   b. ddBalance min value = 10000 ($10,000 or less)
            //   c. ddBalance max value = 180001 ($180,001 - $190,000)
            lt_change.WaitForElement(By.ClassName("step5"), 10);
            Assert.IsTrue(lt_change.IsElementPresent(By.ClassName("step5")), "Unable to verify the script is on Step 5. "
                + "Cannot locate element of class 'step5'.");
            Validation.VerifySelectedOption(driver, "ddBalance", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "ddBalance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "ddBalance", "$180,001 - $190,000");

            lt_change.CompleteStep5(testData, true);

            // On step 6, input “Yes” for 2nd Mortgage.
            lt_change.WaitForElement(By.ClassName("step6"), 10);
            Assert.IsTrue(lt_change.IsElementPresent(By.ClassName("step6")), "Unable to verify the script is on Step 6. "
                + "Cannot locate element of class 'step6'.");
            lt_change.ClickRadio("rSecondMortgageYes");
            System.Threading.Thread.Sleep(2000);

            // On step 6, verify the following for the 2nd Mortgage Balance and Additional Cash dropdowns:
            //   a. ddBalance2 defaults to “select mortgage balance”
            //   b. ddBalance2 min value = 10000 ($10,000 or less)
            //   c. ddBalance2 max value = 100001 ($100,001 - $110,000)
            //   d. ddAdditional defaults to 100000 ($95,001 - $100,000)
            //   e. ddAdditional min value = 50000 ($45,001 - $50,000)
            //   f. ddAdditional max value = 110000 ($105,001 - $110,000)
            Validation.VerifySelectedOption(driver, "ddBalance2", "Select mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "ddBalance2", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "ddBalance2", "$100,001 - $110,000");
            Validation.VerifySelectedOption(driver, "ddAdditional", "$95,001 - $100,000");
            Validation.VerifyDropdownListMinValue(driver, "ddAdditional", "$45,001 - $50,000");
            Validation.VerifyDropdownListMaxValue(driver, "ddAdditional", "$105,001 - $110,000");

            // On step 6, select a 2nd Mortgage Balance of 10000 ($10,000 or less)
            lt_change.SelectByText("ddBalance2", "$10,000 or less");
            System.Threading.Thread.Sleep(2000);

            // On Step 6, verify the following for Additional Cash dropdown:
            //  a. ddAdditional defaults to 90000 ($85,001 - $90,000)
            //  b. ddAdditional min value = 40000 ($35,001 - $40,000)
            //  c. ddAdditional max value = 100000 ($95,001 - $100,000)
            Validation.VerifySelectedOption(driver, "ddAdditional", "$85,001 - $90,000");
            Validation.VerifyDropdownListMinValue(driver, "ddAdditional", "$35,001 - $40,000");
            Validation.VerifyDropdownListMaxValue(driver, "ddAdditional", "$95,001 - $100,000");

            lt_change.CompleteStep6(testData, true);

            // Complete the rest of the steps with valid data.
            lt_change.CompleteStep7(testData, true);
            lt_change.CompleteStep8(testData, true);
            lt_change.CompleteStep9(testData, true);
            lt_change.CompleteStep10(testData, true);
            lt_change.CompleteStep11(testData, true);
            lt_change.CompleteStep12(testData, true);
            lt_change.CompleteStep13(testData, true);

            // Handle the cross-sells
            lt_change.BypassCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(lt_change.strQFormUID));

            // Verify redirect to My LendingTree
            lt_change.VerifyRedirectToMyLendingTree(testData);

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
                    + "WHERE qf.QFormUID = '" + lt_change.strQFormUID + "'";

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
    }
}
