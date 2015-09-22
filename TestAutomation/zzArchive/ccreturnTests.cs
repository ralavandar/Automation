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
    public class ccreturnTests : SeleniumTestBase
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
        public void CCReturn_RefinanceForm_PrimaryHome()
        {
            String strSignature;

            ccreturnPage cc_return = new ccreturnPage(driver);

            // Fill out and submit the initial cc-qf form
            cc_return.FillOutCcQF(testData);
            System.Threading.Thread.Sleep(5000);
            
            // Generate the Signature that will be used to return to cc-return
            strSignature = cc_return.GenerateSignature(testData, cc_return.strQFormUID);
            cc_return.NavigateToCcReturn(testData["TestEnvironment"], strSignature, testData["Variation"]);
            
            // Validate data is pre-popped on Step 1, then fill out Step 1
            cc_return.ValidateContactForm(testData);
            cc_return.CompleteStep1(testData, true);

            // Validate data is pre-popped on Step 2, then fill out Step 2
            cc_return.ValidateDateOfBirth(testData);
            cc_return.CompleteStep2(testData, true);

            // Fill out cc-return Step 3
            cc_return.CompleteStep3(testData, true);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            cc_return.BypassTlCrossSells();
            System.Threading.Thread.Sleep(1000);

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(cc_return.strQFormUID));
        }


        [Test]
        public void CCReturn_PurchaseForm_PrimaryHome()
        {
            String strSignature;

            ccreturnPage cc_return = new ccreturnPage(driver);

            // Fill out and submit the initial cc-qf form
            cc_return.FillOutCcQF(testData);
            System.Threading.Thread.Sleep(5000);

            // Generate the Signature that will be used to return to cc-return
            strSignature = cc_return.GenerateSignature(testData, cc_return.strQFormUID);
            cc_return.NavigateToCcReturn(testData["TestEnvironment"], strSignature, testData["Variation"]);

            // Validate data is pre-popped on Step 1, then fill out Step 1
            cc_return.ValidateContactForm(testData);
            cc_return.CompleteStep1(testData, true);

            // Validate data is pre-popped on Step 2, then fill out Step 2
            cc_return.ValidateDateOfBirth(testData);
            cc_return.CompleteStep2(testData, true);

            // Fill out cc-return Step 3
            cc_return.CompleteStep3(testData, true);

            // Handle the processing popup
            // TO DO...Handle the processing popup

            // Handle the cross-sells
            cc_return.BypassTlCrossSells();
            System.Threading.Thread.Sleep(1000);

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(cc_return.strQFormUID));
        }

        [Test]
        public void CCReturn_RequiredFieldTest()
        {
            String strSignature;

            ccreturnPage cc_return = new ccreturnPage(driver);

            // Navigate to initial cc-qf form and click through.
            cc_return.NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", "cc-qf", "");
            cc_return.CompleteCcQfStep1();
            Common.ReportEvent(Common.INFO, "***** Starting cc-qf Step 2 *****");
            cc_return.WaitForAjaxToComplete(10);
            cc_return.WaitForElementDisplayed(By.Id("step-2"), 5);
            cc_return.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            System.Threading.Thread.Sleep(1000);
            cc_return.ClickButton("next");

            // Verify all validation errors
            System.Threading.Thread.Sleep(2000);
            // Create collection of expected error text
            List<String> expectedccQFErrors = new List<String>()
            {
                "Please enter the first name.",
                "Please enter the last name.",
                "Please enter the date of birth.",
                "Please enter the street address.",
                "Please enter the ZIP code.",
                "This field is required.",
                "Please enter an email address."
            };

            // Get all elements where class = 'error' into a collection called 'actualErrors'
            IList<IWebElement> actualccQFErrors = new List<IWebElement>();
            IList<IWebElement> allccQFLabels = driver.FindElements(By.TagName("label"));

            // Loop all labels and pick out the ones that have class attribute = "error", generated attribute = "true"
            for (int i = 0; i < allccQFLabels.Count; i++)
            {
                //Common.ReportEvent("DEBUG", String.Format("Label number {0}: class attribute is '{1}', for attribute is '{2}', generated attribute is '{3}', innerText is '{4}'.",
                //    i, allccQFLabels[i].GetAttribute("class"), allccQFLabels[i].GetAttribute("for"), allccQFLabels[i].GetAttribute("generated"), allccQFLabels[i].Text));

                if (allccQFLabels[i].GetAttribute("class").Equals("error") && allccQFLabels[i].GetAttribute("generated").Equals("true") && allccQFLabels[i].Text.Length > 0)
                {
                    actualccQFErrors.Add(allccQFLabels[i]);
                }
            }

            if (actualccQFErrors.Count > 0)
            {
                // Check to see that counts of expectedErrors and actualErrors are equal.
                if (Validation.IsTrue(actualccQFErrors.Count.Equals(expectedccQFErrors.Count)))
                {
                    Common.ReportEvent(Common.PASS, String.Format
                        ("The counts of actual and expected error messages are equal.  Actual error msg count = {0}.  "
                        + "Expected error msg count = {1}.", actualccQFErrors.Count, expectedccQFErrors.Count));
                }
                else
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The counts of actual and expected error messages are NOT equal!  Actual error msg count = {0}.  "
                        + "Expected error msg count = {1}.", actualccQFErrors.Count, expectedccQFErrors.Count));
                }

                // Loop through IWebElements and verify 'Text' property matches expected, and 'Displayed' property = True
                for (int i = 0; i < expectedccQFErrors.Count; i++)
                {
                    // Verify the IWebElement display property = True
                    if (Validation.IsTrue(actualccQFErrors[i].Displayed))
                    {
                        Common.ReportEvent(Common.PASS, String.Format
                            ("The 'Displayed' property for IWebElement {0} matches the expected value, True.",
                            actualccQFErrors[i].GetAttribute("id")));
                    }
                    else
                    {
                        Common.ReportEvent(Common.FAIL, String.Format
                            ("The 'Displayed' property for IWebElement {0} DID NOT match the expected value, True.",
                            actualccQFErrors[i].GetAttribute("id")));
                    }

                    // Verify actualErrors(i).text = expectedErrors.Item(i)
                    Validation.StringCompare(expectedccQFErrors[i], actualccQFErrors[i].Text);
                }
            }
            else
            {
                //Report fail - no 'error' elements found on the page
                Common.ReportEvent(Common.FAIL, "The count of actual errors on the page was not > 0.  " +
                    "Unable to fine Elements of class name 'error' on the page.");
            }

            // Fill out and submit the initial cc-qf form
            cc_return.CompleteCcQfStep2(testData);
            // Need to wait for the "SUCCESS!  Please Close this window now" msg...
            System.Threading.Thread.Sleep(5000);
            cc_return.WaitForElement(By.ClassName("errorNotification"), 10);
            System.Threading.Thread.Sleep(5000);

            // Generate the Signature that will be used to return to cc-return
            strSignature = cc_return.GenerateSignature(testData, cc_return.strQFormUID);
            cc_return.NavigateToCcReturn(testData["TestEnvironment"], strSignature, testData["Variation"]);

            // Validate data is pre-popped on cc-return step-1 and then click through steps without filling out fields
            cc_return.ValidateContactForm(testData);
            cc_return.ClickThroughSteps(3);

            System.Threading.Thread.Sleep(2000);

            // Create collection of expected error text
            List<String> expectedErrors = new List<String>()
            {
                "Please enter the property ZIP code.",
                "Please select the home value.",
                "Please select the monthly payment.",
                "Please select your mortgage balance.",
                "Please enter your social security number.",
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
                    cc_return.RecordScreenshot("ValidationPageFail");
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
            cc_return.FillOutValidationStep(testData);

            // Handle the cross-sells
            cc_return.BypassTlCrossSells();
            System.Threading.Thread.Sleep(1000);

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(cc_return.strQFormUID));
        }
    }
}
