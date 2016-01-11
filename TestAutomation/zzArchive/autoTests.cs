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
    public class autoTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Auto";
        private autoPage auto;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            auto = new autoPage(driver, testData);
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
            // auto.BypassAutoCrossSells();

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(auto.strQFormUID));

            // Verify redirect to My LendingTree
            auto.VerifyRedirectToMyLtExpress(testData);
        }       

        [Test]
        public void auto_01_NewCarPurchase()
        {
            // Fill out and submit a QF
            auto.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void auto_02_UsedCarPurchase()
        {
            // Fill out and submit a QF
            auto.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void auto_03_Refinance()
        {
            // Fill out and submit a QF
            auto.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void auto_04_LeaseBuyOut()
        {
            // Fill out and submit a QF
            auto.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void auto_05_RequiredFieldTest_NoCobo()
        {
            // Navigate to valid QF
            auto.NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", "auto", testData["Variation"]);
            
            auto.PrepareStep("step-1");
            // Click through the steps without filling anything out
            auto.ClickThroughSteps(16);

            // Verify all validation errors
            System.Threading.Thread.Sleep(2000);

            // Create collection of expected error text
            List<String> expectedErrors = new List<String>()
            {
                "Please enter loan amount.",
                "Please select vehicle make.",
                "Please select vehicle model.",
                "Please select vehicle trim.",
                "Please enter your date of birth.",
                "This field is required.",
                "Please enter your employer's name.",
                "Please enter your job title.",
                "Please enter your annual income.",
                "Please enter your total liquid assets.",
                "Please enter your full first and last name.",
                "Please enter your email address.",
                "Please enter a password.",
                "Please enter your street address.",
                "Please enter the length of time at current residence.",
                "Please enter your monthly housing payment.",
                "This field is required.",
                "This field is required."

            };
            //todo: if variation for defaultGeo is off, need errors for vehicle-state and zip-code

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
                    "Unable to find Elements of class name 'error' on the page.");
            }

            // Fill out and submit the final page
            auto.FillOutValidationStep();
            auto.shouldAdvanceSteps = true;
            auto.ContinueToNextStep();
            System.Threading.Thread.Sleep(2000);
            FinishTest();
        }


        [Test]
        public void auto_06_RequiredFieldTest_Cobo()
        {
            // Navigate to valid QF and click cobo=yes
            auto.NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", "auto", testData["Variation"]);

            auto.PrepareStep("step-1");
            // Click through to coborrower step
            auto.ClickThroughSteps(1);
            auto.CompleteStep2(true);

            // Click through the steps without filling anything else out
            auto.ClickThroughSteps(26);

            // Verify all validation errors
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(auto.DoesPageContainText("Please correct any errors in entered information"));

            // Create collection of expected error text
            List<String> expectedErrors = new List<String>()
            {
                "Please enter loan amount.",
                "Please select vehicle make.",
                "Please select vehicle model.",
                "Please select vehicle trim.",
                "Please enter your date of birth.",
                "This field is required.",
                "Please enter your employer's name.",
                "Please enter your job title.",
                "Please enter your annual income.",
                "Please enter your total liquid assets.",
                "Please enter your full first and last name.",
                "Please enter your email address.",
                "Please enter a password.",
                "Please enter your street address.",
                "Please enter the length of time at current residence.",
                "Please enter your monthly housing payment.",
                "This field is required.",
                "This field is required.",

                "Please enter a first name for the co-borrower.",
                "Please enter a last name for the co-borrower.",
                "Please enter date of birth.",
                "Please select a job title for the co-borrower.",
                "Please enter employer's name for the co-borrower.",
                "Please enter the co-borrower's length of time with current employer.",
                "Please enter the income for your co-borrower.",
                "Please enter the co-borrower's total liquid assets.",
                "Please enter a street address for the co-borrower.",
                "Please enter co-borrower's zip code. Zip code must be 5 numeric characters.",
                "Please enter co-borrower's length of time at this address.",
                "Please enter a valid phone number for the co-borrower.",
                "Please enter a valid phone number for the co-borrower.",
                "Please enter a valid social security number for the co-borrower.",
                "Please enter email address for the co-borrower."
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
            auto.FillOutValidationStep();
            //System.Threading.Thread.Sleep(2000);
            auto.shouldAdvanceSteps = true;
            auto.ContinueToNextStep();

            FinishTest();
        }
    }
}
