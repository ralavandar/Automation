using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestAutomation.LendingTree.zzArchive
{
    [TestFixture]
    public class homeequityTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_HomeEquity";
        private homeequityPage homeequity;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            homeequity = new homeequityPage(driver);
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
            // homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }       

        [Test]
        public void homeequity_01_Loan()
        {
            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            FinishTest();
        }

        [Test]
        public void homeequity_02_LineOfCredit()
        {
            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            FinishTest();
        }

        [Test]
        public void homeequity_03_NoPreference()
        {
            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            FinishTest();
        }

        [Test]
        public void homeequity_04_RequiredFieldTest()
        {
            // Navigate to valid change QF
            homeequity.NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", "homeequity", testData["Variation"], testData["QueryString"]);

            // Click through the steps without filling anything out
            homeequity.ClickThroughSteps(13);

            // Verify all validation errors
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(homeequity.DoesPageContainText("Please correct any errors in entered information."));

            // Create collection of expected error text
            List<String> expectedErrors = new List<String>()
            {
                "Please select at least one reason",
                "Please enter your property ZIP code.",
                "Please specify property value",
                "Please select your purchase price.",
                "Please specify year purchased",
                "Please enter your date of birth.",
                "Please enter your first name.",
                "Please enter your last name.",
                "Please enter your street address.",
                "Please enter your ZIP code.",
                "This field is required.",
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
                //Common.ReportEvent("DEBUG", String.Format("Label number {0}: class attribute is '{1}', for attribute is '{2}', generated attribute is '{3}', innerText is '{4}'.",
                //    i, allLabels[i].GetAttribute("class"), allLabels[i].GetAttribute("for"), allLabels[i].GetAttribute("generated"), allLabels[i].Text));

                if (allLabels[i].GetAttribute("class").Equals("error") && allLabels[i].GetAttribute("generated").Equals("true") && allLabels[i].Text.Length > 0)
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
            homeequity.FillOutValidationStep(testData);
            //System.Threading.Thread.Sleep(2000);
            homeequity.ContinueToNextStep();

            FinishTest();
        }

        [Test]
        public void homeequity_05_LtvTest()
        {
            DataTable objDataTable = new DataTable();

            // Fill out the first 4 steps
            homeequity.NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", "homeequity", testData["Variation"], testData["QueryString"]);
            homeequity.CompleteStep1(testData, true);
            homeequity.CompleteStep2(testData, true);
            homeequity.CompleteStep3(testData, true);
            homeequity.CompleteStep4(testData, true);
            homeequity.CompleteStep5(testData, true);
            homeequity.CompleteStep6(testData, true);

            // On step 7, verify the following:
            //    a. est-mortgage-balance defaults to “Select balance”
            //    b. est-mortgage-balance min value = 10000 ($10,000 or less)
            //    c. est-mortgage-balance max value = 190001 ($190,001 - $200,000)
            //    d. second-mortgage-balance defaults to “2nd mortgage balance”
            //    e. second-mortgage-balance min value = 10000 ($10,000 or less)
            //    f. second-mortgage-balance max value = 190001 ($190,001 - $200,000)
            //    g. desired-loan-amount defaults to “$140,001 - $150,000”
            //    h. desired-loan-amount min value = 5000 ($1 - $5,000)
            //    i. desired-loan-amount max value = 190001 ($190,001 - $200,000)
            homeequity.WaitForAjaxToComplete(10);
            homeequity.WaitForElement(By.ClassName("step-7"), 5);
            Assert.IsTrue(homeequity.IsElementPresent(By.ClassName("step-7")), "Unable to verify the script is on Step 7. "
                + "Cannot locate element of class 'step-7'.");
            Validation.VerifySelectedOption(driver, "est-mortgage-balance", "Select balance");
            Validation.VerifyDropdownListMinValue(driver, "est-mortgage-balance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "est-mortgage-balance", "$190,001 - $200,000");
            Validation.VerifySelectedOption(driver, "second-mortgage-balance", "2nd mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "second-mortgage-balance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "second-mortgage-balance", "$190,001 - $200,000");
            Validation.VerifySelectedOption(driver, "desired-loan-amount", "$140,001 - $150,000");
            Validation.VerifyDropdownListMinValue(driver, "desired-loan-amount", "$1 - $5,000");
            Validation.VerifyDropdownListMaxValue(driver, "desired-loan-amount", "$190,001 - $200,000");

            // Select a 1st Mortgage Balance of 160001 ($160,001 - $170,000)
            homeequity.SelectByText("est-mortgage-balance", testData["FirstMortgageBalance"]);
            homeequity.WaitForAjaxToComplete(5);

            // Verify the following:
            //    a. second-mortgage-balance defaults to “2nd mortgage balance”
            //    b. second-mortgage-balance min value = 10000 ($10,000 or less)
            //    c. second-mortgage-balance max value = 30001 ($30,001 - $40,000)
            //    d. desired-loan-amount defaults to “$1 - $5,000”
            //    e. desired-loan-amount min value = 5000 ($1 - $5,000)
            //    f. desired-loan-amount max value = 35000 ($30,001 - $35,000)
            Validation.VerifySelectedOption(driver, "second-mortgage-balance", "2nd mortgage balance");
            Validation.VerifyDropdownListMinValue(driver, "second-mortgage-balance", "$10,000 or less");
            Validation.VerifyDropdownListMaxValue(driver, "second-mortgage-balance", "$30,001 - $40,000");
            Validation.VerifySelectedOption(driver, "desired-loan-amount", "$1 - $5,000");
            Validation.VerifyDropdownListMinValue(driver, "desired-loan-amount", "$1 - $5,000");
            Validation.VerifyDropdownListMaxValue(driver, "desired-loan-amount", "$35,001 - $40,000");  // 9/19/2012 - Changed from "$30,001 - $35,000"

            // Select a 2nd Mortgage Balance of 20001 ($20,001 - $30,000)
            homeequity.SelectByText("second-mortgage-balance", testData["SecondMortgageBalance"]);
            homeequity.WaitForAjaxToComplete(5);

            // Verify the following:
            //  a. desired-loan-amount defaults to “$1 - $5,000”
            //  b. desired-loan-amount min value = 5000 ($1 - $5,000)
            //  c. desired-loan-amount max value = 15000 ($10,001 - $15,000)
            Validation.VerifySelectedOption(driver, "desired-loan-amount", "$1 - $5,000");
            Validation.VerifyDropdownListMinValue(driver, "desired-loan-amount", "$1 - $5,000");
            Validation.VerifyDropdownListMaxValue(driver, "desired-loan-amount", "$15,001 - $20,000"); // 9/19/2012 - Changed from $10,001 - $15,000

            // Complete the rest of the steps with valid data.
            homeequity.CompleteStep7(testData, false);
            homeequity.CompleteStep8(testData, true);
            homeequity.CompleteStep9(testData, true);
            homeequity.CompleteStep10(testData, true);
            homeequity.CompleteStep11(testData, true);
            homeequity.CompleteStep12(testData, true);
            homeequity.CompleteStep13(testData, true);

            FinishTest();

            // Verify Data saved to the DB correctly
            //  a. Verify LoanAmt = 15000
            //  b. Verify PresentLTV = 90.001%
            //  c. Verify ProposedLTV = 97.501%
            System.Threading.Thread.Sleep(2000);

            // Build the connection string
            String strDBConnectionString = Common.DBBuildLendXConnectionString(testData["TestEnvironment"]);

            // Here is the query
            String strQuery = "SELECT qf.QFormID, qf.AmountRequested, qfhec.PresentLTVPercent, qfhec.ProposedLTVPercent "
                    + "FROM LendX.dbo.tQForm qf with(nolock) "
                    + "JOIN LendX.dbo.tQFormHomeEquityCalculation qfhec ON qf.QFormID = qfhec.QFormID "
                    + "WHERE qf.QFormUID = '" + homeequity.strQFormUID + "'";

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
                Validation.AreEqual(15000.00, objDataTable.Rows[0].Field<Decimal>(objDataTable.Columns[1]), "AmountRequested");
                Validation.AreEqual(90.001, objDataTable.Rows[0].Field<Decimal>(objDataTable.Columns[2]), "PresentLTVPercent");
                Validation.AreEqual(97.501, objDataTable.Rows[0].Field<Decimal>(objDataTable.Columns[3]), "ProposedLTVPercent");
            }
        }
    }

/*
    [TestFixture]
    public class homeequityVolumeTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_HomeEquity";

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
        public void homeequity_8_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
        [Test]
        public void homeequity_9_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
        [Test]
        public void homeequity_10_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
        [Test]
        public void homeequity_11_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
        [Test]
        public void homeequity_12_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
        [Test]
        public void homeequity_13_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
        [Test]
        public void homeequity_14_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
        [Test]
        public void homeequity_15_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
        [Test]
        public void homeequity_16_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
        [Test]
        public void homeequity_17_Volume()
        {
            homeequityPage homeequity = new homeequityPage(driver);

            // Fill out and submit a QF
            homeequity.FillOutValidQF(testData);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            homeequity.BypassTlCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(homeequity.strQFormUID));

            // Verify redirect to My LendingTree
            homeequity.VerifyRedirectToMyLendingTree(testData);
        }
    }*/
}
