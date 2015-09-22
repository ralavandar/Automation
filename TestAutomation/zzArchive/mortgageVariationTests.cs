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
    public class mortgageVariationTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private const String strBrowser = "IE";
        private const String strEnv = "STAGING";
        private const String strTid = "mortgage";

        [SetUp]
        public void SetupTest()
        {
            //Common.InitializeTestResults();

            // Since these are more like quick-hitting functional tests, not every test will require test data in tTestData_Mortgage
            //GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            //InitializeTestData();
            //driver = StartBrowser(testData["BrowserType"]);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            //Common.ReportFinalResults();
        }

        [Test]
        public void mortgage_00_ssnReq()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test ssnReq = 0 (not shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ig-ssn")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ssn")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("social-security-one")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("social-security-two")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("social-security-three")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ssnTip")));

            // Test ssnReq = 1 (shown, optional)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ig-ssn")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ssn")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ssnTip")));
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-13"), 5);
            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-13")));

            // Test ssnReq = 2 (shown, required)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ig-ssn")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ssn")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ssnTip")));
            mortgage.ContinueToNextStep();
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-12")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ig-ssn")));
            StringAssert.IsMatch("true", driver.FindElement(By.Id("ig-ssn")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("ssn-fields", driver.FindElement(By.Id("ig-ssn")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("Please enter your social security number.", driver.FindElement(By.Id("ig-ssn")).FindElements(By.TagName("label"))[1].Text);

            // Test ssnReq = <default> (shown, required)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-12", "");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ig-ssn")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ssn")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ssnTip")));
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-13"), 5);
            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-13")));
            mortgage.SubmitQF();
            mortgage.WaitForElementDisplayed(By.Id("step-13"), 5);
            System.Threading.Thread.Sleep(500);
            StringAssert.IsMatch("true", driver.FindElement(By.Id("ig-ssn")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("ssn-fields", driver.FindElement(By.Id("ig-ssn")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("Please enter your social security number.", driver.FindElement(By.Id("ig-ssn")).FindElements(By.TagName("label"))[1].Text);
        }


        [Test]
        public void mortgage_01_AltPhoneRequired()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test AltPhoneRequired = 0 (not shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("altPhone")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone-one")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone-two")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone-three")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone-ext")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("altPhoneSameAsPrimary")));

            // Test AltPhoneRequired = 1 (shown, optional)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            System.Threading.Thread.Sleep(250);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("altPhone")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-ext")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("altPhoneSameAsPrimary")));
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-13"), 5);
            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-13")));
            
            // Test AltPhoneRequired = 2 (shown, required)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-2-0-0-0-0-0-0-0-0-0-0-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-2-0-0-0-0-0-0-0-0-0-0-0-0-0-2#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            System.Threading.Thread.Sleep(250);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("altPhone")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-ext")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("altPhoneSameAsPrimary")));
            mortgage.ContinueToNextStep();
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-12")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("divWorkPhone")));
            StringAssert.IsMatch("true", driver.FindElement(By.Id("divWorkPhone")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("work-phone-fields", driver.FindElement(By.Id("divWorkPhone")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("divWorkPhone")).FindElements(By.TagName("label"))[1].Text);

            // Test AltPhoneRequired = <default> (shown, optional)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-12", "");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            System.Threading.Thread.Sleep(250);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("altPhone")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-ext")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("altPhoneSameAsPrimary")));
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-13"), 5);
            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-13")));
        }

        [Test]
        public void mortgage_02_HomePhoneLabel()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test HomePhoneLabel = 0 ("Phone Number")
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch("Phone Number", driver.FindElement(By.Id("homePhone")).GetAttribute("innerHTML"));

            // Test HomePhoneLabel = 1 ("Primary Phone Number")
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch("Primary Phone Number", driver.FindElement(By.Id("homePhone")).GetAttribute("innerHTML"));

            // Test HomePhoneLabel = <default> ("Primary Phone Number")
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch("Primary Phone Number", driver.FindElement(By.Id("homePhone")).GetAttribute("innerHTML"));
        }

        [Test]
        public void mortgage_03_AltPhoneLabel()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test AltPhoneLabel = 0 ("Alternate Phone Number")
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0");
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch("Alternate Phone Number", driver.FindElement(By.Id("altPhone")).GetAttribute("innerHTML"));

            // Test AltPhoneLabel = 1 ("Alternate Phone Number (Optional)")
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-1");
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch(@"Alternate Phone Number \(Optional\)", driver.FindElement(By.Id("altPhone")).GetAttribute("innerHTML"));

            // Test AltPhoneLabel = <default> ("Alternate Phone Number (Optional)")
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch(@"Alternate Phone Number \(Optional\)", driver.FindElement(By.Id("altPhone")).GetAttribute("innerHTML"));
        }

        [Test]
        public void mortgage_04_submitarea()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test submitarea = 0 ("Get Your Free Results!")
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0#_aspnetform=step-13");
            WaitForNextButtonValueUpdate("Get Your Free Results!", 5);
            StringAssert.IsMatch("Get Your Free Results!", driver.FindElement(By.Id("next")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("on a federal or state Do-Not-Call (DNC) registry"));

            // Test submitarea = 1 ("Get Your Free Results!", same as 0)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-1#_aspnetform=step-13");
            WaitForNextButtonValueUpdate("Get Your Free Results!", 5);
            StringAssert.IsMatch("Get Your Free Results!", driver.FindElement(By.Id("next")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("on a federal or state Do-Not-Call (DNC) registry"));

            // Test submitarea = 2 ("Show Me My Results")
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-2#_aspnetform=step-13");
            WaitForNextButtonValueUpdate("Show Me My Results", 5);
            StringAssert.IsMatch("Show Me My Results", driver.FindElement(By.Id("next")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("on a federal or state Do-Not-Call (DNC) registry"));

            // Test submitarea = 3 ("Show Me My Results", same as 2)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-3");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-3#_aspnetform=step-13");
            WaitForNextButtonValueUpdate("Show Me My Results", 5);
            StringAssert.IsMatch("Show Me My Results", driver.FindElement(By.Id("next")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("on a federal or state Do-Not-Call (DNC) registry"));

            // Test submitarea = 4 ("Accept and Continue")
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-4");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-4#_aspnetform=step-13");
            WaitForNextButtonValueUpdate("Accept and Continue", 5);
            StringAssert.IsMatch("Accept and Continue", driver.FindElement(By.Id("next")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("on a federal or state Do-Not-Call (DNC) registry"));

            // Test submitarea = 5 ("Accept and Continue", same as 4)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-5");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-5#_aspnetform=step-13");
            WaitForNextButtonValueUpdate("Accept and Continue", 5);
            StringAssert.IsMatch("Accept and Continue", driver.FindElement(By.Id("next")).GetAttribute("value"));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("on a federal or state Do-Not-Call (DNC) registry"));
        }

        [Test]
        public void mortgage_05_briteVerify()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test briteVerify = 0 (none)
            // (note: AltPhoneRequired (index 1) is turned on for this test too)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-0#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            mortgage.Fill("home-phone-one", "704");
            mortgage.Fill("home-phone-two", "555");
            mortgage.Fill("home-phone-three", "0100");
            mortgage.Fill("work-phone-one", "704");
            mortgage.Fill("work-phone-two", "555");
            mortgage.Fill("work-phone-three", "0199");
            mortgage.Fill("work-phone-ext", "8320");
            System.Threading.Thread.Sleep(500);
            mortgage.WaitForAjaxToComplete(10);
            Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("Please enter a valid phone number"));
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("email"), 5);
            mortgage.Fill("email", "ottotest@lendingtree.com");
            mortgage.Fill("password", "password101");
            System.Threading.Thread.Sleep(500);
            mortgage.WaitForAjaxToComplete(10);
            Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("Please enter a valid email address"));

            // Test briteVerify = 1 (verify primary phone, alt phone, and email)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-1#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            mortgage.Fill("home-phone-one", "704");
            mortgage.Fill("home-phone-two", "555");
            mortgage.Fill("home-phone-three", "0100");
            mortgage.Fill("work-phone-one", "704");
            mortgage.Fill("work-phone-two", "555");
            mortgage.Fill("work-phone-three", "0199");
            mortgage.Fill("work-phone-ext", "8320");
            System.Threading.Thread.Sleep(500);
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch("true", driver.FindElement(By.Id("ig-home-phone")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("home-phone-fields", driver.FindElement(By.Id("ig-home-phone")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("Please enter a valid phone number.", driver.FindElement(By.Id("ig-home-phone")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("true", driver.FindElement(By.Id("divWorkPhone")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("work-phone-fields", driver.FindElement(By.Id("divWorkPhone")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("Please enter a valid phone number.", driver.FindElement(By.Id("divWorkPhone")).FindElements(By.TagName("label"))[1].Text);
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Please enter a valid phone number"));
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("email"), 5);
            mortgage.Fill("email", "ottotest@lendingtree.com");
            mortgage.Fill("password", "password101");
            System.Threading.Thread.Sleep(500);
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch("true", driver.FindElement(By.Id("ig-email")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("email", driver.FindElement(By.Id("ig-email")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("Please enter a valid email address.", driver.FindElement(By.Id("ig-email")).FindElements(By.TagName("label"))[1].Text);
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Please enter a valid email address"));

            // Test briteVerify = <default> (verify primary phone, alt phone, and email)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-12", "");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            mortgage.Fill("home-phone-one", "704");
            mortgage.Fill("home-phone-two", "555");
            mortgage.Fill("home-phone-three", "0100");
            mortgage.Fill("work-phone-one", "704");
            mortgage.Fill("work-phone-two", "555");
            mortgage.Fill("work-phone-three", "0199");
            mortgage.Fill("work-phone-ext", "8320");
            System.Threading.Thread.Sleep(500);
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch("true", driver.FindElement(By.Id("ig-home-phone")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("home-phone-fields", driver.FindElement(By.Id("ig-home-phone")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("Please enter a valid phone number.", driver.FindElement(By.Id("ig-home-phone")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("true", driver.FindElement(By.Id("divWorkPhone")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("work-phone-fields", driver.FindElement(By.Id("divWorkPhone")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("Please enter a valid phone number.", driver.FindElement(By.Id("divWorkPhone")).FindElements(By.TagName("label"))[1].Text);
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Please enter a valid phone number"));
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("email"), 5);
            mortgage.Fill("email", "ottotest@lendingtree.com");
            mortgage.Fill("password", "password101");
            System.Threading.Thread.Sleep(500);
            mortgage.WaitForAjaxToComplete(10);
            StringAssert.IsMatch("true", driver.FindElement(By.Id("ig-email")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("email", driver.FindElement(By.Id("ig-email")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("Please enter a valid email address.", driver.FindElement(By.Id("ig-email")).FindElements(By.TagName("label"))[1].Text);
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Please enter a valid email address"));
        }

        [Test]
        public void mortgage_06_PhoneFieldsStyle()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test PhoneFieldsStyle = 0 (3 textboxes)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-0-0#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("home-phone")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone-three")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-ext")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("altPhoneSameAsPrimary")));

            // Test PhoneFieldsStyle = 1 (1 textbox))
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-0-1#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("home-phone-one")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("home-phone-two")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("home-phone-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone-one")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone-two")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone-three")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone-ext")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("altPhoneSameAsPrimary")));

            // Test PhoneFieldsStyle = 2 (show 'same as primary')
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-1-0-0-0-0-2#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("home-phone")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone-three")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-ext")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("altPhoneSameAsPrimary")));
            
            // Test PhoneFieldsStyle = <default> (3 text boxes)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-12", "");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("home-phone")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-phone-three")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("work-phone")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-three")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("work-phone-ext")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("altPhoneSameAsPrimary")));
        }

        [Test]
        public void mortgage_07_ssnStyle()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test ssnStyle = 0 (3 textboxes)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("social-security-one"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ssn")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-three")));

            // Test ssnStyle = 1 (1 textbox)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-1#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("ssn"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ssn")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("social-security-one")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("social-security-two")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("social-security-three")));

            // Test ssnStyle = <default> (3 textboxes)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-12", "");
            mortgage.WaitForElementDisplayed(By.Id("social-security-one"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ssn")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-one")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-two")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("social-security-three")));
        }

        [Test]
        public void mortgage_08_ErrorDisplay()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test ErrorDisplay = 0 (display all fields/steps on last step, errors inline)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0#_aspnetform=step-13");
            mortgage.WaitForElementDisplayed(By.Id("password"), 5);
            mortgage.SubmitQF();
            mortgage.WaitForElementDisplayed(By.Id("error-summary-header"), 5);
            //Assert 14 validation error messages
            Assert.AreEqual(15, mortgage.GetErrorLabelCount(), "Final count of error labels did not match!");
            StringAssert.IsMatch("Please enter your property ZIP code.", driver.FindElement(By.Id("ig-property-zip")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please select the home value.", driver.FindElement(By.Id("ig-property-value")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please select the monthly payment.", driver.FindElement(By.Id("ig-monthly-payment")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please select your mortgage balance.", driver.FindElement(By.Id("divFirstMortgage")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your date of birth.", driver.FindElement(By.Id("ig-dob")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-home-services")).FindElements(By.TagName("label"))[4].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-veteran")).FindElements(By.TagName("label"))[3].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-bankruptcy")).FindElements(By.TagName("label"))[3].Text);
            StringAssert.IsMatch("Please enter your first name.", driver.FindElement(By.Id("ig-first-name")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your last name.", driver.FindElement(By.Id("ig-last-name")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your street address.", driver.FindElement(By.Id("ig-street-address")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your ZIP code.", driver.FindElement(By.Id("ig-zip-code")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-home-phone")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your email address.", driver.FindElement(By.Id("ig-email")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter a password.", driver.FindElement(By.Id("ig-password")).FindElements(By.TagName("label"))[1].Text);
            // Assert the valid fields display in-line, within their normal input groups
            Assert.IsTrue(driver.FindElement(By.Id("ig-property-type")).FindElement(By.Id("property-type")).Displayed, "Valid field 'property-type' did not display!");
            Assert.IsTrue(driver.FindElement(By.Id("ig-property-use")).FindElement(By.Id("property-use")).Displayed, "Valid field 'property-use' did not display!");
            Assert.IsTrue(driver.FindElement(By.Id("ig-have-multiple-mortgages")).FindElement(By.Id("second-mortgage-yes")).Displayed, "Valid field 'second-mortgage-yes' did not display!");
            Assert.IsTrue(driver.FindElement(By.Id("ig-cash-out")).FindElement(By.Id("cash-out")).Displayed, "Valid field 'cash-out' did not display!");
            // Assert the valid fields do not display in top header of form
            Assert.IsFalse(driver.FindElement(By.Id("topSubHeader")).FindElement(By.Id("validForm")).Displayed, "The validForm element did not display!");
            Assert.IsFalse(driver.FindElement(By.Id("validForm")).FindElement(By.Id("view_valid_info")).Displayed, "The 'view_valid_info' link did not display!");
            Assert.IsFalse(driver.FindElement(By.Id("validForm")).FindElement(By.Id("edit_valid_info")).Displayed, "The 'edit_valid_info' link did not display!");
            Assert.IsFalse(mortgage.IsElementDisplayed(By.ClassName("have-multiple-mortgages")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.ClassName("cash-out")));

            // Test ErrorDisplay = 1 (display only fields with errors on last step, valid fields in summary window)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-1#_aspnetform=step-13");
            mortgage.WaitForElementDisplayed(By.Id("password"), 5);
            mortgage.SubmitQF();
            mortgage.WaitForElementDisplayed(By.Id("error-summary-header"), 5);
            //Assert 14 validation error messages
            Assert.AreEqual(15, mortgage.GetErrorLabelCount(), "Final count of error labels did not match!");
            StringAssert.IsMatch("Please enter your property ZIP code.", driver.FindElement(By.Id("ig-property-zip")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please select the home value.", driver.FindElement(By.Id("ig-property-value")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please select the monthly payment.", driver.FindElement(By.Id("ig-monthly-payment")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please select your mortgage balance.", driver.FindElement(By.Id("divFirstMortgage")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your date of birth.", driver.FindElement(By.Id("ig-dob")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-home-services")).FindElements(By.TagName("label"))[4].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-veteran")).FindElements(By.TagName("label"))[3].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-bankruptcy")).FindElements(By.TagName("label"))[3].Text);
            StringAssert.IsMatch("Please enter your first name.", driver.FindElement(By.Id("ig-first-name")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your last name.", driver.FindElement(By.Id("ig-last-name")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your street address.", driver.FindElement(By.Id("ig-street-address")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your ZIP code.", driver.FindElement(By.Id("ig-zip-code")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-home-phone")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your email address.", driver.FindElement(By.Id("ig-email")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter a password.", driver.FindElement(By.Id("ig-password")).FindElements(By.TagName("label"))[1].Text);
            // Assert the valid fields display in top header of form
            Assert.IsTrue(driver.FindElement(By.Id("topSubHeader")).FindElement(By.Id("validForm")).Displayed, "The validForm element did not display!");
            Assert.IsTrue(driver.FindElement(By.Id("validForm")).FindElement(By.Id("view_valid_info")).Displayed, "The 'view_valid_info' link was not displayed!");
            Assert.IsTrue(driver.FindElement(By.Id("validForm")).FindElement(By.Id("edit_valid_info")).Displayed, "The 'edit_valid_info' link was not displayed!");
            //Assert.IsTrue(driver.FindElement(By.Id("validForm")).FindElement(By.ClassName("have-multiple-mortgages")).Displayed);
            Assert.IsTrue(driver.FindElement(By.Id("validForm")).FindElement(By.ClassName("cash-out")).Displayed);
            // Assert the valid fields do not display in-line, within their normal input groups
            Assert.IsFalse(driver.FindElement(By.Id("ig-property-type")).FindElement(By.Id("property-type")).Displayed, "Valid field 'property-type' did not display!");
            Assert.IsFalse(driver.FindElement(By.Id("ig-property-use")).FindElement(By.Id("property-use")).Displayed, "Valid field 'property-use' did not display!");
            Assert.IsFalse(driver.FindElement(By.Id("ig-have-multiple-mortgages")).FindElement(By.Id("second-mortgage-yes")).Displayed, "Valid field 'second-mortgage-yes' did not display!");
            Assert.IsFalse(driver.FindElement(By.Id("ig-cash-out")).FindElement(By.Id("cash-out")).Displayed, "Valid field 'cash-out' did not display!");
            // Click into and verify additional valid fields.
            mortgage.ClickLink("view_valid_info");
            mortgage.WaitForElementDisplayed(By.ClassName("ui-dialog"), 5);
            Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("property-type")).Displayed, "The 'property-type' dropdown did not display!");
            Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("property-use")).Displayed, "The 'property-use' dropdown did not display!");
            //Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("1st-mortgage-interest-rate")).Displayed, "The '1st-mortgage-interest-rate' dropdown did not display!");
            Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("second-mortgage-yes")).Displayed, "The 'second-mortgage-yes' radio button did not display!");
            Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("cash-out")).Displayed, "The 'cash-out' dropdown did not display!");
            Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("stated-credit-history")).Displayed, "The 'stated-credit-history' dropdown did not display!");
            Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("foreclosure-text")).Displayed, "The 'foreclosure-text' dropdown did not display!");
            //Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("ddcreditcarddebt")).Displayed, "The 'ddcreditcarddebt' dropdown did not display!");
            //Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("ltl-optin-no")).Displayed, "The 'ltl-optin-no' radio button did not display!");
            Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.Id("save_valid_info")).Displayed, "The 'save_valid_info' button did not display!");
            Assert.IsTrue(driver.FindElement(By.ClassName("ui-dialog")).FindElement(By.ClassName("ui-dialog-titlebar-close")).Displayed, "The 'ui-dialog-titlebar-close' button did not display!");
            driver.FindElement(By.ClassName("ui-dialog-titlebar-close")).Click();

            // Test ErrorDisplay = <default> (display all fields/steps on last step, errors inline)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-13", "");
            mortgage.WaitForElementDisplayed(By.Id("password"), 5);
            mortgage.SubmitQF();
            mortgage.WaitForElementDisplayed(By.Id("error-summary-header"), 5);
            //Assert 14 validation error messages
            Assert.AreEqual(16, mortgage.GetErrorLabelCount(), "Final count of error labels did not match!");
            StringAssert.IsMatch("Please enter your property ZIP code.", driver.FindElement(By.Id("ig-property-zip")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please select the home value.", driver.FindElement(By.Id("ig-property-value")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please select the monthly payment.", driver.FindElement(By.Id("ig-monthly-payment")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please select your mortgage balance.", driver.FindElement(By.Id("divFirstMortgage")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your date of birth.", driver.FindElement(By.Id("ig-dob")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-home-services")).FindElements(By.TagName("label"))[4].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-veteran")).FindElements(By.TagName("label"))[3].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-bankruptcy")).FindElements(By.TagName("label"))[3].Text);
            StringAssert.IsMatch("Please enter your first name.", driver.FindElement(By.Id("ig-first-name")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your last name.", driver.FindElement(By.Id("ig-last-name")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your street address.", driver.FindElement(By.Id("ig-street-address")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your ZIP code.", driver.FindElement(By.Id("ig-zip-code")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-home-phone")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your social security number.", driver.FindElement(By.Id("ig-ssn")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter your email address.", driver.FindElement(By.Id("ig-email")).FindElements(By.TagName("label"))[1].Text);
            StringAssert.IsMatch("Please enter a password.", driver.FindElement(By.Id("ig-password")).FindElements(By.TagName("label"))[1].Text);
            // Assert the valid fields display in-line, within their normal input groups
            Assert.IsTrue(driver.FindElement(By.Id("ig-property-type")).FindElement(By.Id("property-type")).Displayed, "Valid field 'property-type' did not display!");
            Assert.IsTrue(driver.FindElement(By.Id("ig-property-use")).FindElement(By.Id("property-use")).Displayed, "Valid field 'property-use' did not display!");
            Assert.IsTrue(driver.FindElement(By.Id("ig-have-multiple-mortgages")).FindElement(By.Id("second-mortgage-yes")).Displayed, "Valid field 'second-mortgage-yes' did not display!");
            Assert.IsTrue(driver.FindElement(By.Id("ig-cash-out")).FindElement(By.Id("cash-out")).Displayed, "Valid field 'cash-out' did not display!");
            // Assert the valid fields do not display in top header of form
            Assert.IsFalse(driver.FindElement(By.Id("topSubHeader")).FindElement(By.Id("validForm")).Displayed, "The validForm element did not display!");
            Assert.IsFalse(driver.FindElement(By.Id("validForm")).FindElement(By.Id("view_valid_info")).Displayed, "The 'view_valid_info' link did not display!");
            Assert.IsFalse(driver.FindElement(By.Id("validForm")).FindElement(By.Id("edit_valid_info")).Displayed, "The 'edit_valid_info' link did not display!");
            Assert.IsFalse(mortgage.IsElementDisplayed(By.ClassName("have-multiple-mortgages")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.ClassName("cash-out")));
        }


        [Test]
        public void mortgage_09_fieldSets()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test fieldSets = 0 (lt-change copy)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-1-1-1-0-1-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            var fieldSets = driver.FindElements(By.TagName("fieldset"));
            Assert.IsTrue(fieldSets.Count.Equals(13), "The number of fieldsets (i.e. steps) did not match the expected value!");
            // Step 1
            Assert.IsTrue(fieldSets[0].GetAttribute("id").Equals("step-1"), "The id of the fieldset did not match the expected value!");
            var inputGroups = fieldSets[0].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-1 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-product-type"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-type"), "The input-group ID did not match the expected value!");
            // Step 2
            Assert.IsTrue(fieldSets[1].GetAttribute("id").Equals("step-2"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[1].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-2 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-zip"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-new-home"), "The input-group ID did not match the expected value!");
            // Step 3
            Assert.IsTrue(fieldSets[2].GetAttribute("id").Equals("step-3"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[2].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-3 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-value"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-state"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-property-city"), "The input-group ID did not match the expected value!");
            // Step 4
            Assert.IsTrue(fieldSets[3].GetAttribute("id").Equals("step-4"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[3].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-4 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-use"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-current-realestate-agent"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-monthly-payment"), "The input-group ID did not match the expected value!");
            // Step 5
            Assert.IsTrue(fieldSets[4].GetAttribute("id").Equals("step-5"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[4].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-5 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("first-mortgage-fields"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-purchase-price"), "The input-group ID did not match the expected value!");
            // Step 6
            Assert.IsTrue(fieldSets[5].GetAttribute("id").Equals("step-6"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[5].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(4), "The number of expected input groups on step-6 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-have-multiple-mortgages"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-cash-out"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-down-payment"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-realtor-optin"), "The input-group ID did not match the expected value!");
            // Step 7
            Assert.IsTrue(fieldSets[6].GetAttribute("id").Equals("step-7"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[6].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-7 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-credit-history"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-dob"), "The input-group ID did not match the expected value!");
            // Step 8
            Assert.IsTrue(fieldSets[7].GetAttribute("id").Equals("step-8"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[7].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-8 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-home-services"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-veteran"), "The input-group ID did not match the expected value!");
            // Step 9
            Assert.IsTrue(fieldSets[8].GetAttribute("id").Equals("step-9"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[8].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-9 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-foreclosure"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-debt-consolidation"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-bankruptcy"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-lt-edu"), "The input-group ID did not match the expected value!");
            // Step 10
            Assert.IsTrue(fieldSets[9].GetAttribute("id").Equals("step-10"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[9].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-10 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-first-name"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-last-name"), "The input-group ID did not match the expected value!");
            // Step 11
            Assert.IsTrue(fieldSets[10].GetAttribute("id").Equals("step-11"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[10].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-11 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-street-address"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-zip-code"), "The input-group ID did not match the expected value!");
            // Step 12
            Assert.IsTrue(fieldSets[11].GetAttribute("id").Equals("step-12"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[11].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-12 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-home-phone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("divWorkPhone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-ssn"), "The input-group ID did not match the expected value!");
            // Step 13
            Assert.IsTrue(fieldSets[12].GetAttribute("id").Equals("step-13"), "The id of the fieldset did not match the expected value! Was: " + inputGroups.Count);
            inputGroups = fieldSets[12].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-13 did not match the expected value!");
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-email"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-password"), "The input-group ID did not match the expected value!");

            // Test fieldSets = 1 (email moved to step 2)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-1-1-1-0-1-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            fieldSets = driver.FindElements(By.TagName("fieldset"));
            Assert.IsTrue(fieldSets.Count.Equals(13), "The number of fieldsets (i.e. steps) did not match the expected value!");
            // Step 1
            Assert.IsTrue(fieldSets[0].GetAttribute("id").Equals("step-1"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[0].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-1 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-product-type"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-type"), "The input-group ID did not match the expected value!");
            // Step 2
            Assert.IsTrue(fieldSets[1].GetAttribute("id").Equals("step-2"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[1].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-2 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-email"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-zip"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-new-home"), "The input-group ID did not match the expected value!");
            // Step 3
            Assert.IsTrue(fieldSets[2].GetAttribute("id").Equals("step-3"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[2].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-3 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-value"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-state"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-property-city"), "The input-group ID did not match the expected value!");
            // Step 4
            Assert.IsTrue(fieldSets[3].GetAttribute("id").Equals("step-4"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[3].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-4 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-use"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-current-realestate-agent"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-monthly-payment"), "The input-group ID did not match the expected value!");
            // Step 5
            Assert.IsTrue(fieldSets[4].GetAttribute("id").Equals("step-5"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[4].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-5 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("first-mortgage-fields"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-purchase-price"), "The input-group ID did not match the expected value!");
            // Step 6
            Assert.IsTrue(fieldSets[5].GetAttribute("id").Equals("step-6"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[5].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(4), "The number of expected input groups on step-6 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-have-multiple-mortgages"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-cash-out"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-down-payment"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-realtor-optin"), "The input-group ID did not match the expected value!");
            // Step 7
            Assert.IsTrue(fieldSets[6].GetAttribute("id").Equals("step-7"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[6].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-7 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-credit-history"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-dob"), "The input-group ID did not match the expected value!");
            // Step 8
            Assert.IsTrue(fieldSets[7].GetAttribute("id").Equals("step-8"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[7].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-8 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-home-services"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-veteran"), "The input-group ID did not match the expected value!");
            // Step 9
            Assert.IsTrue(fieldSets[8].GetAttribute("id").Equals("step-9"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[8].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-9 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-foreclosure"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-debt-consolidation"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-bankruptcy"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-lt-edu"), "The input-group ID did not match the expected value!");
            // Step 10
            Assert.IsTrue(fieldSets[9].GetAttribute("id").Equals("step-10"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[9].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-10 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-first-name"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-last-name"), "The input-group ID did not match the expected value!");
            // Step 11
            Assert.IsTrue(fieldSets[10].GetAttribute("id").Equals("step-11"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[10].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-11 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-street-address"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-zip-code"), "The input-group ID did not match the expected value!");
            // Step 12
            Assert.IsTrue(fieldSets[11].GetAttribute("id").Equals("step-12"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[11].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-12 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-home-phone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("divWorkPhone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-ssn"), "The input-group ID did not match the expected value!");
            // Step 13
            Assert.IsTrue(fieldSets[12].GetAttribute("id").Equals("step-13"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[12].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(1), "The number of expected input groups on step-13 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-password"), "The input-group ID did not match the expected value!");

            // Test fieldSets = 2 (lt-4step)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-1-1-1-0-1-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            fieldSets = driver.FindElements(By.TagName("fieldset"));
            Assert.IsTrue(fieldSets.Count.Equals(4), "The number of fieldsets (i.e. steps) did not match the expected value!");
            // Step 1
            Assert.IsTrue(fieldSets[0].GetAttribute("id").Equals("step-1"), "The id of the first fieldset did not match the expected value!");
            inputGroups = fieldSets[0].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(4), "The number of expected input groups on step-1 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-product-type"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-type"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-email"), "The input-group ID did not match the expected value!");
            // Step 2
            Assert.IsTrue(fieldSets[1].GetAttribute("id").Equals("step-2"), "The id of the 2nd fieldset did not match the expected value!");
            inputGroups = fieldSets[1].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(14), "The number of expected input groups on step-2 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-new-home"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-state"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-property-city"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-purchase-price"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[4].GetAttribute("id").Equals("ig-down-payment"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[5].GetAttribute("id").Equals("ig-realtor-optin"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[6].GetAttribute("id").Equals("ig-property-use"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[7].GetAttribute("id").Equals("ig-current-realestate-agent"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[8].GetAttribute("id").Equals("ig-property-zip"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[9].GetAttribute("id").Equals("ig-property-value"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[10].GetAttribute("id").Equals("ig-monthly-payment"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[11].GetAttribute("id").Equals("first-mortgage-fields"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[12].GetAttribute("id").Equals("ig-have-multiple-mortgages"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[13].GetAttribute("id").Equals("ig-cash-out"), "The input-group ID did not match the expected value!");
            // Step 3
            Assert.IsTrue(fieldSets[2].GetAttribute("id").Equals("step-3"), "The id of the 3rd fieldset did not match the expected value!");
            inputGroups = fieldSets[2].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(7), "The number of expected input groups on step-3 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-dob"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-credit-history"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-veteran"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-foreclosure"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[4].GetAttribute("id").Equals("ig-bankruptcy"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[5].GetAttribute("id").Equals("ig-debt-consolidation"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[6].GetAttribute("id").Equals("ig-home-services"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[7].GetAttribute("id").Equals("ig-lt-edu"), "The input-group ID did not match the expected value!");
            // Step 4
            Assert.IsTrue(fieldSets[3].GetAttribute("id").Equals("step-4"), "The id of the 4th fieldset did not match the expected value!");
            inputGroups = fieldSets[3].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(8), "The number of expected input groups on step-4 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-first-name"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-last-name"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-street-address"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-zip-code"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[4].GetAttribute("id").Equals("ig-home-phone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[5].GetAttribute("id").Equals("divWorkPhone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[6].GetAttribute("id").Equals("ig-ssn"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[7].GetAttribute("id").Equals("ig-password"), "The input-group ID did not match the expected value!");

            // Test fieldSets = 3 (lt-progress copy)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-1-1-1-0-1-0-0-0-3");
            mortgage.WaitForAjaxToComplete(10);
            fieldSets = driver.FindElements(By.TagName("fieldset"));
            Assert.IsTrue(fieldSets.Count.Equals(13), "The number of fieldsets (i.e. steps) did not match the expected value!");
            // Step 1
            Assert.IsTrue(fieldSets[0].GetAttribute("id").Equals("step-1"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[0].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-1 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-product-type"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-type"), "The input-group ID did not match the expected value!");
            // Step 2
            Assert.IsTrue(fieldSets[1].GetAttribute("id").Equals("step-2"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[1].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-2 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-zip"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-new-home"), "The input-group ID did not match the expected value!");
            // Step 3
            Assert.IsTrue(fieldSets[2].GetAttribute("id").Equals("step-3"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[2].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-3 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-value"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-state"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-property-city"), "The input-group ID did not match the expected value!");
            // Step 4
            Assert.IsTrue(fieldSets[3].GetAttribute("id").Equals("step-4"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[3].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-4 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-use"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-current-realestate-agent"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-monthly-payment"), "The input-group ID did not match the expected value!");
            // Step 5
            Assert.IsTrue(fieldSets[4].GetAttribute("id").Equals("step-5"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[4].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-5 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("first-mortgage-fields"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-purchase-price"), "The input-group ID did not match the expected value!");
            // Step 6
            Assert.IsTrue(fieldSets[5].GetAttribute("id").Equals("step-6"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[5].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(4), "The number of expected input groups on step-6 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-have-multiple-mortgages"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-cash-out"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-down-payment"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-realtor-optin"), "The input-group ID did not match the expected value!");
            // Step 7
            Assert.IsTrue(fieldSets[6].GetAttribute("id").Equals("step-7"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[6].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-7 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-credit-history"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-dob"), "The input-group ID did not match the expected value!");
            // Step 8
            Assert.IsTrue(fieldSets[7].GetAttribute("id").Equals("step-8"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[7].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-8 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-home-services"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-veteran"), "The input-group ID did not match the expected value!");
            // Step 9
            Assert.IsTrue(fieldSets[8].GetAttribute("id").Equals("step-9"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[8].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-9 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-foreclosure"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-debt-consolidation"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-bankruptcy"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-lt-edu"), "The input-group ID did not match the expected value!");
            // Step 10
            Assert.IsTrue(fieldSets[9].GetAttribute("id").Equals("step-10"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[9].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-10 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-first-name"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-last-name"), "The input-group ID did not match the expected value!");
            // Step 11
            Assert.IsTrue(fieldSets[10].GetAttribute("id").Equals("step-11"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[10].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-11 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-street-address"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-zip-code"), "The input-group ID did not match the expected value!");
            // Step 12
            Assert.IsTrue(fieldSets[11].GetAttribute("id").Equals("step-12"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[11].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-12 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-email"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-password"), "The input-group ID did not match the expected value!");
            // Step 13
            Assert.IsTrue(fieldSets[12].GetAttribute("id").Equals("step-13"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[12].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-13 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-home-phone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("divWorkPhone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-ssn"), "The input-group ID did not match the expected value!");

            // Test fieldSets = 4 (lt-change copy, combined LF/SF treatment)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-1-1-1-0-1-0-0-0-4");
            mortgage.WaitForAjaxToComplete(10);
            fieldSets = driver.FindElements(By.TagName("fieldset"));
            Assert.IsTrue(fieldSets.Count.Equals(13), "The number of fieldsets (i.e. steps) did not match the expected value!");
            // Step 1
            Assert.IsTrue(fieldSets[0].GetAttribute("id").Equals("step-1"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[0].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-1 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-product-type"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-type"), "The input-group ID did not match the expected value!");
            // Step 2
            Assert.IsTrue(fieldSets[1].GetAttribute("id").Equals("step-2"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[1].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-2 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-zip"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-new-home"), "The input-group ID did not match the expected value!");
            // Step 3
            Assert.IsTrue(fieldSets[2].GetAttribute("id").Equals("step-3"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[2].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-3 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-value"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-state"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-property-city"), "The input-group ID did not match the expected value!");
            // Step 4
            Assert.IsTrue(fieldSets[3].GetAttribute("id").Equals("step-4"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[3].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-4 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-use"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-current-realestate-agent"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-monthly-payment"), "The input-group ID did not match the expected value!");
            // Step 5
            Assert.IsTrue(fieldSets[4].GetAttribute("id").Equals("step-5"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[4].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-5 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("first-mortgage-fields"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-purchase-price"), "The input-group ID did not match the expected value!");
            // Step 6
            Assert.IsTrue(fieldSets[5].GetAttribute("id").Equals("step-6"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[5].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(4), "The number of expected input groups on step-6 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-have-multiple-mortgages"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-cash-out"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-down-payment"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-realtor-optin"), "The input-group ID did not match the expected value!");
            // Step 7
            Assert.IsTrue(fieldSets[6].GetAttribute("id").Equals("step-7"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[6].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-7 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-credit-history"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-dob"), "The input-group ID did not match the expected value!");
            // Step 8
            Assert.IsTrue(fieldSets[7].GetAttribute("id").Equals("step-8"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[7].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-8 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-home-services"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-veteran"), "The input-group ID did not match the expected value!");
            // Step 9
            Assert.IsTrue(fieldSets[8].GetAttribute("id").Equals("step-9"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[8].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-9 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-foreclosure"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-debt-consolidation"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-bankruptcy"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-lt-edu"), "The input-group ID did not match the expected value!");
            // Step 10
            Assert.IsTrue(fieldSets[9].GetAttribute("id").Equals("step-10"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[9].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-10 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-first-name"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-last-name"), "The input-group ID did not match the expected value!");
            // Step 11
            Assert.IsTrue(fieldSets[10].GetAttribute("id").Equals("step-11"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[10].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-11 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-street-address"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-zip-code"), "The input-group ID did not match the expected value!");
            // Step 12
            Assert.IsTrue(fieldSets[11].GetAttribute("id").Equals("step-12"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[11].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(4), "The number of expected input groups on step-12 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-email"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-password"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-home-phone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("divWorkPhone"), "The input-group ID did not match the expected value!");
            // Step 13
            Assert.IsTrue(fieldSets[12].GetAttribute("id").Equals("step-13"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[12].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(1), "The number of expected input groups on step-13 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-ssn"), "The input-group ID did not match the expected value!");

            // Test fieldSets = 5 (lt-4step copy, combined LF/SF treatment)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-1-1-1-0-1-0-0-0-5");
            mortgage.WaitForAjaxToComplete(10);
            fieldSets = driver.FindElements(By.TagName("fieldset"));
            Assert.IsTrue(fieldSets.Count.Equals(5), "The number of fieldsets (i.e. steps) did not match the expected value!");
            // Step 1
            Assert.IsTrue(fieldSets[0].GetAttribute("id").Equals("step-1"), "The id of the first fieldset did not match the expected value!");
            inputGroups = fieldSets[0].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(4), "The number of expected input groups on step-1 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-product-type"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-type"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-email"), "The input-group ID did not match the expected value!");
            // Step 2
            Assert.IsTrue(fieldSets[1].GetAttribute("id").Equals("step-2"), "The id of the 2nd fieldset did not match the expected value!");
            inputGroups = fieldSets[1].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(14), "The number of expected input groups on step-2 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-new-home"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-state"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-property-city"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-purchase-price"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[4].GetAttribute("id").Equals("ig-down-payment"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[5].GetAttribute("id").Equals("ig-realtor-optin"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[6].GetAttribute("id").Equals("ig-property-use"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[7].GetAttribute("id").Equals("ig-current-realestate-agent"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[8].GetAttribute("id").Equals("ig-property-zip"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[9].GetAttribute("id").Equals("ig-property-value"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[10].GetAttribute("id").Equals("ig-monthly-payment"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[11].GetAttribute("id").Equals("first-mortgage-fields"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[12].GetAttribute("id").Equals("ig-have-multiple-mortgages"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[13].GetAttribute("id").Equals("ig-cash-out"), "The input-group ID did not match the expected value!");
            // Step 3
            Assert.IsTrue(fieldSets[2].GetAttribute("id").Equals("step-3"), "The id of the 3rd fieldset did not match the expected value!");
            inputGroups = fieldSets[2].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(7), "The number of expected input groups on step-3 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-dob"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-credit-history"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-veteran"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-foreclosure"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[4].GetAttribute("id").Equals("ig-bankruptcy"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[5].GetAttribute("id").Equals("ig-debt-consolidation"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[6].GetAttribute("id").Equals("ig-home-services"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[7].GetAttribute("id").Equals("ig-lt-edu"), "The input-group ID did not match the expected value!");
            // Step 4
            Assert.IsTrue(fieldSets[3].GetAttribute("id").Equals("step-4"), "The id of the 4th fieldset did not match the expected value!");
            inputGroups = fieldSets[3].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(7), "The number of expected input groups on step-4 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-first-name"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-last-name"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-street-address"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-zip-code"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[4].GetAttribute("id").Equals("ig-home-phone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[5].GetAttribute("id").Equals("divWorkPhone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[6].GetAttribute("id").Equals("ig-password"), "The input-group ID did not match the expected value!");
            // Step 5
            Assert.IsTrue(fieldSets[4].GetAttribute("id").Equals("step-5"), "The id of the 5th fieldset did not match the expected value!");
            inputGroups = fieldSets[4].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(1), "The number of expected input groups on step-5 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-ssn"), "The input-group ID did not match the expected value!");

            // Test fieldSets = <default> (same as 0: lt-change copy)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            fieldSets = driver.FindElements(By.TagName("fieldset"));
            Assert.IsTrue(fieldSets.Count.Equals(13), "The number of fieldsets (i.e. steps) did not match the expected value!");
            // Step 1
            Assert.IsTrue(fieldSets[0].GetAttribute("id").Equals("step-1"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[0].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-1 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-product-type"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-type"), "The input-group ID did not match the expected value!");
            // Step 2
            Assert.IsTrue(fieldSets[1].GetAttribute("id").Equals("step-2"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[1].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-2 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-zip"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-new-home"), "The input-group ID did not match the expected value!");
            // Step 3
            Assert.IsTrue(fieldSets[2].GetAttribute("id").Equals("step-3"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[2].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-3 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-value"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-property-state"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-property-city"), "The input-group ID did not match the expected value!");
            // Step 4
            Assert.IsTrue(fieldSets[3].GetAttribute("id").Equals("step-4"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[3].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-4 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-property-use"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-current-realestate-agent"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-monthly-payment"), "The input-group ID did not match the expected value!");
            // Step 5
            Assert.IsTrue(fieldSets[4].GetAttribute("id").Equals("step-5"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[4].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-5 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("first-mortgage-fields"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-purchase-price"), "The input-group ID did not match the expected value!");
            // Step 6
            Assert.IsTrue(fieldSets[5].GetAttribute("id").Equals("step-6"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[5].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(4), "The number of expected input groups on step-6 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-have-multiple-mortgages"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-cash-out"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-down-payment"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-realtor-optin"), "The input-group ID did not match the expected value!");
            // Step 7
            Assert.IsTrue(fieldSets[6].GetAttribute("id").Equals("step-7"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[6].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-7 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-credit-history"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-dob"), "The input-group ID did not match the expected value!");
            // Step 8
            Assert.IsTrue(fieldSets[7].GetAttribute("id").Equals("step-8"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[7].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-8 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-home-services"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-veteran"), "The input-group ID did not match the expected value!");
            // Step 9
            Assert.IsTrue(fieldSets[8].GetAttribute("id").Equals("step-9"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[8].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-9 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-foreclosure"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-debt-consolidation"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-bankruptcy"), "The input-group ID did not match the expected value!");
            //Assert.IsTrue(inputGroups[3].GetAttribute("id").Equals("ig-lt-edu"), "The input-group ID did not match the expected value!");
            // Step 10
            Assert.IsTrue(fieldSets[9].GetAttribute("id").Equals("step-10"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[9].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-10 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-first-name"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-last-name"), "The input-group ID did not match the expected value!");
            // Step 11
            Assert.IsTrue(fieldSets[10].GetAttribute("id").Equals("step-11"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[10].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-11 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-street-address"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-zip-code"), "The input-group ID did not match the expected value!");
            // Step 12
            Assert.IsTrue(fieldSets[11].GetAttribute("id").Equals("step-12"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[11].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(3), "The number of expected input groups on step-12 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-home-phone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("divWorkPhone"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[2].GetAttribute("id").Equals("ig-ssn"), "The input-group ID did not match the expected value!");
            // Step 13
            Assert.IsTrue(fieldSets[12].GetAttribute("id").Equals("step-13"), "The id of the fieldset did not match the expected value!");
            inputGroups = fieldSets[12].FindElements(By.ClassName("input-group"));
            Assert.IsTrue(inputGroups.Count.Equals(2), "The number of expected input groups on step-13 did not match the expected value! Was: " + inputGroups.Count);
            Assert.IsTrue(inputGroups[0].GetAttribute("id").Equals("ig-email"), "The input-group ID did not match the expected value!");
            Assert.IsTrue(inputGroups[1].GetAttribute("id").Equals("ig-password"), "The input-group ID did not match the expected value!");

        }


        [Test]
        public void mortgage_10_HomeServices()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test HomeServices = 0 (nothing shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-8");
            mortgage.WaitForElementDisplayed(By.Id("is-veteran-yes"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.ClassName("lt-home-services-label")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("homeservice-optin-yes")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("homeservice-optin-no")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("home-service-category")));

            // Test HomeServices = 1 (yes/no opt-in, defaulted to yes)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-8");
            mortgage.WaitForElementDisplayed(By.Id("is-veteran-yes"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.ClassName("lt-home-services-label")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-yes")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-no")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("home-service-category")));

            // Test HomeServices = 2 (yes/no opt-in, defaulted to yes, category list #1)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-2#_aspnetform=step-8");
            mortgage.WaitForElementDisplayed(By.Id("is-veteran-yes"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.ClassName("lt-home-services-label")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-yes")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-no")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-service-category")));
            SelectElement objSelect2 = new SelectElement(driver.FindElement(By.Id("home-service-category")));
            Assert.IsTrue(objSelect2.Options.Count.Equals(155));
            StringAssert.IsMatch("Please select", objSelect2.Options.First().Text);
            StringAssert.IsMatch("Concrete - Waterproofing", objSelect2.Options.Last().Text);

            // Test HomeServices = 3 (yes/no opt-in, defaulted to yes, category list #2)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-3");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-3#_aspnetform=step-8");
            mortgage.WaitForElementDisplayed(By.Id("is-veteran-yes"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.ClassName("lt-home-services-label")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-yes")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-no")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-service-category")));
            SelectElement objSelect3 = new SelectElement(driver.FindElement(By.Id("home-service-category")));
            Assert.IsTrue(objSelect3.Options.Count.Equals(116));
            StringAssert.IsMatch("None", objSelect3.Options.First().Text);
            StringAssert.IsMatch("Other Home Improvement Services", objSelect3.Options.Last().Text);

            // Test HomeServices = 4 (yes/no opt-in, defaulted to yes, category list #3)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-4");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-4#_aspnetform=step-8");
            mortgage.WaitForElementDisplayed(By.Id("is-veteran-yes"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.ClassName("lt-home-services-label")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-yes")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-no")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-service-category")));
            SelectElement objSelect4 = new SelectElement(driver.FindElement(By.Id("home-service-category")));
            Assert.IsTrue(objSelect4.Options.Count.Equals(152));
            StringAssert.IsMatch("None", objSelect4.Options.First().Text);
            StringAssert.IsMatch("Other Home Improvement Service", objSelect4.Options.Last().Text);

            // Test HomeServices = 5 (yes/no opt-in, defaulted to yes, category list #3)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-5");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-5#_aspnetform=step-8");
            mortgage.WaitForElementDisplayed(By.Id("is-veteran-yes"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.ClassName("lt-home-services-label")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-yes")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-no")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-service-category")));
            SelectElement objSelect5 = new SelectElement(driver.FindElement(By.Id("home-service-category")));
            Assert.IsTrue(objSelect5.Options.Count.Equals(152));
            StringAssert.IsMatch("None", objSelect5.Options.First().Text);
            StringAssert.IsMatch("Other Home Improvement Service", objSelect5.Options.Last().Text);
            mortgage.SelectByText("home-service-category", "Heating & Cooling");
            mortgage.WaitForElementDisplayed(By.Id("home-service-category-addl"), 5);
            SelectElement objSelect5b = new SelectElement(driver.FindElement(By.Id("home-service-category-addl")));
            Assert.IsTrue(objSelect5b.Options.Count.Equals(152));
            StringAssert.IsMatch("None", objSelect5b.Options.First().Text);
            StringAssert.IsMatch("Other Home Improvement Service", objSelect5b.Options.Last().Text);

            // Test HomeServices = <default>
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-8", "");
            mortgage.WaitForElementDisplayed(By.Id("is-veteran-yes"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.ClassName("lt-home-services-label")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-yes")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("homeservice-optin-no")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("home-service-category")));
            SelectElement objSelectDefault = new SelectElement(driver.FindElement(By.Id("home-service-category")));
            Assert.IsTrue(objSelectDefault.Options.Count.Equals(155));
            StringAssert.IsMatch("Please select", objSelectDefault.Options.First().Text);
            StringAssert.IsMatch("Concrete - Waterproofing", objSelectDefault.Options.Last().Text);
        }

        [Test]
        public void mortgage_11_autoAdvance()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test autoAdvance = 0 (Off)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("property-type"), 5);
            mortgage.SelectByValue("property-type", "LOWRISECONDO");
            mortgage.WaitForAjaxToComplete(10);
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-1")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("property-type")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("step-2")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("property-zip")));
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-2"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("step-1")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("property-type")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-2")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("property-zip")));

            // Test autoAdvance = 1 (On)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("property-type"), 5);
            mortgage.SelectByValue("property-type", "LOWRISECONDO");
            mortgage.WaitForAjaxToComplete(10);
            System.Threading.Thread.Sleep(1000);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("step-1")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("property-type")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-2")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("property-zip")));

            // Test autoAdvance = <default> (On by default)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("property-type"), 5);
            mortgage.SelectByValue("property-type", "LOWRISECONDO");
            mortgage.WaitForAjaxToComplete(10);
            System.Threading.Thread.Sleep(1000);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("step-1")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("property-type")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-2")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("property-zip")));
        }

        [Test]
        public void mortgage_12_mortgageInputStyle()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test mortgageInputStyle = 0 (dropdowns)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-5");
            mortgage.WaitForElementDisplayed(By.Id("est-mortgage-balance"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("est-mortgage-balance")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("est-mortgage-balance-txt")));
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-6");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-yes"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("2nd-mortgage-monthly-payment"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("second-mortgage-balance")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("second-mortgage-balance-txt")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("cash-out")));

            // Test mortgageInputStyle = 1 (textboxes)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-5");
            mortgage.WaitForElementDisplayed(By.Id("est-mortgage-balance-txt"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("est-mortgage-balance")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("est-mortgage-balance-txt")));
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-6");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-yes"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("2nd-mortgage-monthly-payment"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("second-mortgage-balance")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("second-mortgage-balance-txt")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("cash-out")));

            // Test mortgageInputStyle = 2 (dropdown with exact amt option, conditional textbox)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-2#_aspnetform=step-3");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("estproperty-value"), 5);
            mortgage.SelectByValue("estproperty-value", "400000");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-2#_aspnetform=step-5");
            mortgage.WaitForElementDisplayed(By.Id("est-mortgage-balance"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("est-mortgage-balance")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("est-mortgage-balance-txt")));
            mortgage.SelectByText("est-mortgage-balance", "Enter Exact Amount");
            mortgage.WaitForElementDisplayed(By.Id("est-mortgage-balance-txt"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("est-mortgage-balance-txt")));
            mortgage.Fill("est-mortgage-balance-txt", "300000");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-yes"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("2nd-mortgage-monthly-payment"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("second-mortgage-balance")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("second-mortgage-balance-txt")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("cash-out")));
            mortgage.SelectByText("second-mortgage-balance", "Enter Exact Amount");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-balance-txt"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("second-mortgage-balance-txt")));

            // Test mortgageInputStyle = <default> (dropdowns)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-5", "");
            mortgage.WaitForElementDisplayed(By.Id("est-mortgage-balance"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("est-mortgage-balance")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("est-mortgage-balance-txt")));
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-6", "");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-yes"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("2nd-mortgage-monthly-payment"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("second-mortgage-balance")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("second-mortgage-balance-txt")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("cash-out")));

        }

        //[Test]
        //public void mortgage_13_StyleSheets()
        //{
        //    driver = StartBrowser(strBrowser);
        //    mortgagePage mortgage = new mortgagePage(driver);

        //    // Test StyleSheets = 0 (default stylesheets)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
        //    Assert.IsTrue(driver.FindElements(By.TagName("link")).Count.Equals(2));
        //    StringAssert.IsMatch(@"templates/mortgage/css/structure\.css", driver.FindElements(By.TagName("link"))[0].GetAttribute("href"));
        //    StringAssert.IsMatch(@"templates/mortgage/css/quickmatch\.css", driver.FindElements(By.TagName("link"))[1].GetAttribute("href"));
        //    StringAssert.IsMatch("", driver.FindElement(By.TagName("body")).GetAttribute("className"));

        //    // Test StyleSheets = 1 (default stylesheets + popup stylesheets)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-1");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
        //    //Common.ReportEvent("DEBUG", String.Format("document.stylesheets = {0}", (driver as IJavaScriptExecutor).ExecuteScript("return document.styleSheets[2].href;")));
        //    //Common.ReportEvent("DEBUG", String.Format("document.stylesheets = {0}", (driver as IJavaScriptExecutor).ExecuteScript("return document.styleSheets[3].href;")));
        //    //Common.ReportEvent("DEBUG", String.Format("document.stylesheets = {0}", (driver as IJavaScriptExecutor).ExecuteScript("return document.styleSheets[4].href;")));
        //    Assert.IsTrue(driver.FindElements(By.TagName("link")).Count.Equals(3));
        //    StringAssert.IsMatch(@"templates/mortgage/css/structure\.css", driver.FindElements(By.TagName("link"))[0].GetAttribute("href"));
        //    StringAssert.IsMatch(@"templates/mortgage/css/quickmatch\.css", driver.FindElements(By.TagName("link"))[1].GetAttribute("href"));
        //    StringAssert.IsMatch(@"templates/mortgage/css/popup\.css", driver.FindElements(By.TagName("link"))[2].GetAttribute("href"));
        //    StringAssert.IsMatch("popup", driver.FindElement(By.TagName("body")).GetAttribute("className"));

        //    // Test StyleSheets = 2 (no stylesheets)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-2");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
        //    Assert.IsTrue(driver.FindElements(By.TagName("link")).Count.Equals(0));
        //    StringAssert.IsMatch("", driver.FindElement(By.TagName("body")).GetAttribute("className"));

        //    // Test StyleSheets = <default> (default stylesheets)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
        //    Assert.IsTrue(driver.FindElements(By.TagName("link")).Count.Equals(3));
        //    StringAssert.IsMatch(@"templates/mortgage/css/structure\.css", driver.FindElements(By.TagName("link"))[1].GetAttribute("href"));
        //    StringAssert.IsMatch(@"templates/mortgage/css/quickmatch\.css", driver.FindElements(By.TagName("link"))[2].GetAttribute("href"));
        //    StringAssert.IsMatch("", driver.FindElement(By.TagName("body")).GetAttribute("className"));
        //}

        //[Test]
        //public void mortgage_14_debtConsolidate()
        //{
        //    //question removed
        //    return;
        //    driver = StartBrowser(strBrowser);
        //    mortgagePage mortgage = new mortgagePage(driver);

        //    // Test debtConsolidate = 0 (not shown)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-9");
        //    mortgage.WaitForElementDisplayed(By.Id("declared-bankruptcy-yes"), 5);
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ddcreditcarddebt")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("debt-consultation-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("debt-consultation-no")));

        //    // Test debtConsolidate = 1 (shown)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-9");
        //    mortgage.WaitForElementDisplayed(By.Id("declared-bankruptcy-yes"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ddcreditcarddebt")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("debt-consultation-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("debt-consultation-no")));
        //    mortgage.SelectByValue("ddcreditcarddebt", "12500");
        //    mortgage.WaitForElementDisplayed(By.Id("debt-consultation-yes"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("debt-consultation-yes")));
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("debt-consultation-no")));

        //    // Test debtConsolidate = <default> (shown)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-9", "");
        //    mortgage.WaitForElementDisplayed(By.Id("declared-bankruptcy-yes"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ddcreditcarddebt")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("debt-consultation-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("debt-consultation-no")));
        //    mortgage.SelectByValue("ddcreditcarddebt", "12500");
        //    mortgage.WaitForElementDisplayed(By.Id("debt-consultation-yes"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("debt-consultation-yes")));
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("debt-consultation-no")));
        //}

        [Test]
        public void mortgage_15_InputValidation()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test InputValidation = 0 (validate-on-last-step)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            //Common.ReportEvent("DEBUG", String.Format("step-1 count of error labels = {0}", mortgage.GetErrorLabelCount())); //0
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-1 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-2"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-2 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-3"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-3 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-4"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-4 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-5 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-6"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-6 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-7"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-7 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-8"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-8 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-9"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-9 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-10"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-10 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-11"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-11 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-12"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-12 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-13"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-13 count of error labels did not match!");
            System.Threading.Thread.Sleep(500);
            mortgage.SubmitQF();
            mortgage.WaitForElementDisplayed(By.Id("error-summary-header"), 5);
            Assert.AreEqual(14, mortgage.GetErrorLabelCount(), "Final count of error labels did not match!");

            // Test InputValidation = 1 (inline-allow-progress)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-1 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-2"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-2 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-3"), 5);
            Assert.AreEqual(1, mortgage.GetErrorLabelCount(), "step-3 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-4"), 5);
            Assert.AreEqual(2, mortgage.GetErrorLabelCount(), "step-4 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            Assert.AreEqual(3, mortgage.GetErrorLabelCount(), "step-5 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-6"), 5);
            Assert.AreEqual(4, mortgage.GetErrorLabelCount(), "step-6 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-7"), 5);
            Assert.AreEqual(4, mortgage.GetErrorLabelCount(), "step-7 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-8"), 5);
            Assert.AreEqual(5, mortgage.GetErrorLabelCount(), "step-8 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-9"), 5);
            Assert.AreEqual(6, mortgage.GetErrorLabelCount(), "step-9 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-10"), 5);
            Assert.AreEqual(7, mortgage.GetErrorLabelCount(), "step-10 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-11"), 5);
            Assert.AreEqual(9, mortgage.GetErrorLabelCount(), "step-11 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-12"), 5);
            Assert.AreEqual(11, mortgage.GetErrorLabelCount(), "step-12 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-13"), 5);
            Assert.AreEqual(12, mortgage.GetErrorLabelCount(), "step-13 count of error labels did not match!");
            System.Threading.Thread.Sleep(500);
            mortgage.SubmitQF();
            mortgage.WaitForElementDisplayed(By.Id("error-summary-header"), 5);
            Assert.AreEqual(14, mortgage.GetErrorLabelCount(), "Final step count of error labels did not match!");

            // Test InputValidation = 2 (inline-stop-progress)
            // TODO - may want to create another test case in 'mortgageTests' that goes through each page
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-1 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-2"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-2 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-2"), 5);
            Assert.AreEqual(1, mortgage.GetErrorLabelCount(), "step-2 count of error labels did not match!");
            StringAssert.IsMatch("true", driver.FindElement(By.Id("ig-property-zip")).FindElements(By.TagName("label"))[1].GetAttribute("generated"));
            StringAssert.IsMatch("property-zip", driver.FindElement(By.Id("ig-property-zip")).FindElements(By.TagName("label"))[1].GetAttribute("for"));
            StringAssert.IsMatch("Please enter your property ZIP code.", driver.FindElement(By.Id("ig-property-zip")).FindElements(By.TagName("label"))[1].Text);

            // Test InputValidation = <default> (validate-on-last-step)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            //Common.ReportEvent("DEBUG", String.Format("step-1 count of error labels = {0}", mortgage.GetErrorLabelCount())); //0
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-1 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-2"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-2 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-3"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-3 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-4"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-4 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-5 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-6"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-6 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-7"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-7 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-8"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-8 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-9"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-9 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-10"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-10 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-11"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-11 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-12"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-12 count of error labels did not match!");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-13"), 5);
            Assert.AreEqual(0, mortgage.GetErrorLabelCount(), "step-13 count of error labels did not match!");
            System.Threading.Thread.Sleep(500);
            mortgage.SubmitQF();
            mortgage.WaitForElementDisplayed(By.Id("error-summary-header"), 5);
            Assert.AreEqual(16, mortgage.GetErrorLabelCount(), "Final count of error labels did not match!");
        }

        [Test]
        public void mortgage_16_emailOptin()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test emailOptin = 0 (not shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-13");
            mortgage.WaitForElementDisplayed(By.Id("password"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("email-optin-yes")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("email-optin-no")));

            // Test emailOptin = 1 (shown, defaults to yes)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-13");
            mortgage.WaitForElementDisplayed(By.Id("password"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("email-optin-yes")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("email-optin-no")));

            // Test emailOptin = <default> (not shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-13", "");
            mortgage.WaitForElementDisplayed(By.Id("password"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("email-optin-yes")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("email-optin-no")));

        }

        [Test]
        public void mortgage_17_mortgageTip()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test mortgageTip = 0 (no tip)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-3");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("estproperty-value"), 5);
            mortgage.SelectByValue("estproperty-value", "100000");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-5");
            mortgage.WaitForElementDisplayed(By.Id("est-mortgage-balance"), 5);
            mortgage.SelectByValue("est-mortgage-balance", "50001");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-yes"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-balance"), 5);
            mortgage.SelectByValue("second-mortgage-balance", "20001");
            mortgage.SelectByValue("cash-out", "5000");
            System.Threading.Thread.Sleep(1000);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("cashOutTip")));
            mortgage.SelectByValue("cash-out", "10000");
            System.Threading.Thread.Sleep(1000);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("cashOutTip")));

            // Test mortgageTip = 1 (show tip)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-3");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("estproperty-value"), 5);
            mortgage.SelectByValue("estproperty-value", "100000");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-5");
            mortgage.WaitForElementDisplayed(By.Id("est-mortgage-balance"), 5);
            mortgage.SelectByValue("est-mortgage-balance", "50001");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-yes"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-balance"), 5);
            mortgage.SelectByValue("second-mortgage-balance", "20001");
            mortgage.SelectByValue("cash-out", "5000");
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("cashOutTip")));
            mortgage.SelectByValue("cash-out", "10000");
            System.Threading.Thread.Sleep(1000);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("cashOutTip")));

            // Test mortgageTip = <default> (show tip)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-3", "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("estproperty-value"), 5);
            mortgage.SelectByValue("estproperty-value", "100000");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-5", "");
            mortgage.WaitForElementDisplayed(By.Id("est-mortgage-balance"), 5);
            System.Threading.Thread.Sleep(1000);
            mortgage.SelectByValue("est-mortgage-balance", "50001");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-yes"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-balance"), 5);
            mortgage.SelectByValue("second-mortgage-balance", "20001");
            mortgage.SelectByValue("cash-out", "5000");
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("cashOutTip")));
            mortgage.SelectByValue("cash-out", "10000");
            System.Threading.Thread.Sleep(1000);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("cashOutTip")));
        }

        [Test]
        public void mortgage_18_logoHotlink()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test logoHotlink = 0 (no hotlink)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            // Make sure 1st link is 'Advertising Disclosures'.
            IWebElement firstLink = driver.FindElements(By.TagName("a"))[0];
            IWebElement secondLink = null;
            Assert.IsTrue(firstLink.Text.Equals("Advertising Disclosures"));
            firstLink = null;

            // Test logoHotlink = 1 (hotlink logo to www.lendingtree.com)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            // Make sure 1st link is lendingtree logo, 2nd link is Advertising Disclosures
            firstLink = driver.FindElements(By.TagName("a"))[0];
            secondLink = driver.FindElements(By.TagName("a"))[1];
            Assert.IsTrue(firstLink.GetAttribute("href").Equals("http://www.lendingtree.com/"));
            Assert.IsTrue(secondLink.Text.Equals("Advertising Disclosures"));
            firstLink = null;
            secondLink = null;

            // Test logoHotlink = <default> (no hotlink)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            // Make sure 1st link is 'Advertising Disclosures'.
            firstLink = driver.FindElements(By.TagName("a"))[0];
            Assert.IsTrue(firstLink.Text.Equals("Advertising Disclosures"));
        }

        //[Test]
        //public void mortgage_19_ProgressStyle()
        //{
        //    driver = StartBrowser(strBrowser);
        //    mortgagePage mortgage = new mortgagePage(driver);

        //    //TODO: how to id the bubble style bar?

        //    // Test ProgressStyle = 0 (no progress bar)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);

        //    // Test ProgressStyle = 1 (bubble style bar)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);

        //    // Test ProgressStyle = <default> (bubble style bar)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
        //}

        [Test]
        public void mortgage_20_minLoanAmount()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test minLoanAmount = 0
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-3");
            mortgage.WaitForElementDisplayed(By.Id("step-3"), 5);
            mortgage.SelectByValue("estproperty-value", "200000");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-4"), 5);
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            SelectElement firstBalance0 = new SelectElement(driver.FindElement(By.Id("est-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", firstBalance0.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", firstBalance0.Options[1].Text);
            StringAssert.IsMatch(@"\$180,001 - \$190,000", firstBalance0.Options.Last().Text);
            mortgage.SelectByValue("est-mortgage-balance", "50001");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-6"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-balance"), 5);
            SelectElement secondBalance0 = new SelectElement(driver.FindElement(By.Id("second-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", secondBalance0.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", secondBalance0.Options[1].Text);
            StringAssert.IsMatch(@"\$130,001 - \$140,000", secondBalance0.Options.Last().Text);
            SelectElement cashout0 = new SelectElement(driver.FindElement(By.Id("cash-out")));
            StringAssert.IsMatch(@"\$95,001 - \$100,000", cashout0.SelectedOption.Text);
            StringAssert.IsMatch(@"\$25,001 - \$30,000", cashout0.Options.First().Text);
            StringAssert.IsMatch(@"\$130,001 - \$140,000", cashout0.Options.Last().Text);
            mortgage.SelectByValue("second-mortgage-balance", "10000");
            mortgage.WaitForAjaxToComplete(5);
            StringAssert.IsMatch(@"\$85,001 - \$90,000", cashout0.SelectedOption.Text);
            StringAssert.IsMatch(@"\$15,001 - \$20,000", cashout0.Options.First().Text);
            StringAssert.IsMatch(@"\$125,001 - \$130,000", cashout0.Options.Last().Text);

            // Test minLoanAmount = 1
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-3");
            mortgage.WaitForElementDisplayed(By.Id("step-3"), 5);
            mortgage.SelectByValue("estproperty-value", "200000");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-4"), 5);
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            SelectElement firstBalance1 = new SelectElement(driver.FindElement(By.Id("est-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", firstBalance1.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", firstBalance1.Options[1].Text);
            StringAssert.IsMatch(@"\$180,001 - \$190,000", firstBalance1.Options.Last().Text);
            mortgage.SelectByValue("est-mortgage-balance", "50001");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-6"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-balance"), 5);
            SelectElement secondBalance1 = new SelectElement(driver.FindElement(By.Id("second-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", secondBalance1.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", secondBalance1.Options[1].Text);
            StringAssert.IsMatch(@"\$100,001 - \$110,000", secondBalance1.Options.Last().Text);
            SelectElement cashout1 = new SelectElement(driver.FindElement(By.Id("cash-out")));
            StringAssert.IsMatch(@"\$95,001 - \$100,000", cashout1.SelectedOption.Text);
            StringAssert.IsMatch(@"\$45,001 - \$50,000", cashout1.Options.First().Text);
            StringAssert.IsMatch(@"\$105,001 - \$110,000", cashout1.Options.Last().Text);
            mortgage.SelectByValue("second-mortgage-balance", "10000");
            mortgage.WaitForAjaxToComplete(5);
            StringAssert.IsMatch(@"\$85,001 - \$90,000", cashout1.SelectedOption.Text);
            StringAssert.IsMatch(@"\$35,001 - \$40,000", cashout1.Options.First().Text);
            StringAssert.IsMatch(@"\$95,001 - \$100,000", cashout1.Options.Last().Text);

            // Test minLoanAmount = 2
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2#_aspnetform=step-3");
            mortgage.WaitForElementDisplayed(By.Id("step-3"), 5);
            mortgage.SelectByValue("estproperty-value", "200000");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-4"), 5);
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            SelectElement firstBalance2 = new SelectElement(driver.FindElement(By.Id("est-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", firstBalance2.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", firstBalance2.Options[1].Text);
            StringAssert.IsMatch(@"\$180,001 - \$190,000", firstBalance2.Options.Last().Text);
            mortgage.SelectByValue("est-mortgage-balance", "50001");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-6"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-balance"), 5);
            SelectElement secondBalance2 = new SelectElement(driver.FindElement(By.Id("second-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", secondBalance2.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", secondBalance2.Options[1].Text);
            StringAssert.IsMatch(@"\$100,001 - \$110,000", secondBalance2.Options.Last().Text);
            SelectElement cashout2 = new SelectElement(driver.FindElement(By.Id("cash-out")));
            StringAssert.IsMatch(@"\$95,001 - \$100,000", cashout2.SelectedOption.Text);
            StringAssert.IsMatch(@"\$70,001 - \$75,000", cashout2.Options.First().Text);
            StringAssert.IsMatch(@"\$105,001 - \$110,000", cashout2.Options.Last().Text);
            mortgage.SelectByValue("second-mortgage-balance", "10000");
            mortgage.WaitForAjaxToComplete(5);
            StringAssert.IsMatch(@"\$85,001 - \$90,000", cashout2.SelectedOption.Text);
            StringAssert.IsMatch(@"\$60,001 - \$65,000", cashout2.Options.First().Text);
            StringAssert.IsMatch(@"\$95,001 - \$100,000", cashout2.Options.Last().Text);

            // Test minLoanAmount = 3
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-3");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-3#_aspnetform=step-3");
            mortgage.WaitForElementDisplayed(By.Id("step-3"), 5);
            mortgage.SelectByValue("estproperty-value", "200000");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-4"), 5);
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            SelectElement firstBalance3 = new SelectElement(driver.FindElement(By.Id("est-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", firstBalance3.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", firstBalance3.Options[1].Text);
            StringAssert.IsMatch(@"\$190,001 - \$200,000", firstBalance3.Options.Last().Text);
            mortgage.SelectByValue("est-mortgage-balance", "50001");
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-6"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-balance"), 5);
            SelectElement secondBalance3 = new SelectElement(driver.FindElement(By.Id("second-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", secondBalance3.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", secondBalance3.Options[1].Text);
            StringAssert.IsMatch(@"\$140,001 - \$150,000", secondBalance3.Options.Last().Text);
            SelectElement cashout3 = new SelectElement(driver.FindElement(By.Id("cash-out")));
            StringAssert.IsMatch(@"\$95,001 - \$100,000", cashout3.SelectedOption.Text);
            StringAssert.IsMatch(@"\$0 No Cash", cashout3.Options.First().Text);
            StringAssert.IsMatch(@"\$140,001 - \$150,000", cashout3.Options.Last().Text);
            mortgage.SelectByValue("second-mortgage-balance", "10000");
            mortgage.WaitForAjaxToComplete(5);
            StringAssert.IsMatch(@"\$85,001 - \$90,000", cashout3.SelectedOption.Text);
            StringAssert.IsMatch(@"\$0 No Cash", cashout3.Options.First().Text);
            StringAssert.IsMatch(@"\$130,001 - \$140,000", cashout3.Options.Last().Text);

            // Test minLoanAmount = <default>
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-3", "");
            mortgage.WaitForElementDisplayed(By.Id("step-3"), 5);
            // the following wait is necessary for the auto-advance to work
            System.Threading.Thread.Sleep(1000);
            mortgage.SelectByValue("estproperty-value", "200000");
            // auto advance to step-4
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-4"), 5);
            mortgage.ContinueToNextStep();
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            mortgage.WaitForElementDisplayed(By.Id("est-mortgage-balance"), 5);
            SelectElement firstBalance = new SelectElement(driver.FindElement(By.Id("est-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", firstBalance.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", firstBalance.Options[1].Text);
            StringAssert.IsMatch(@"\$190,001 - \$200,000", firstBalance.Options.Last().Text);
            // the following wait is necessary for the auto-advance to work
            System.Threading.Thread.Sleep(1000);
            mortgage.SelectByValue("est-mortgage-balance", "50001");
            // auto advance to step-6
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-6"), 5);
            mortgage.ClickRadio("second-mortgage-yes");
            mortgage.WaitForElementDisplayed(By.Id("second-mortgage-balance"), 5);
            SelectElement secondBalance = new SelectElement(driver.FindElement(By.Id("second-mortgage-balance")));
            StringAssert.IsMatch("Select mortgage balance", secondBalance.SelectedOption.Text);
            StringAssert.IsMatch(@"\$10,000 or less", secondBalance.Options[1].Text);
            StringAssert.IsMatch(@"\$140,001 - \$150,000", secondBalance.Options.Last().Text);
            SelectElement cashout = new SelectElement(driver.FindElement(By.Id("cash-out")));
            StringAssert.IsMatch(@"\$95,001 - \$100,000", cashout.SelectedOption.Text);
            StringAssert.IsMatch(@"\$0 No Cash", cashout.Options.First().Text);
            StringAssert.IsMatch(@"\$140,001 - \$150,000", cashout.Options.Last().Text);
            mortgage.SelectByValue("second-mortgage-balance", "10000");
            mortgage.WaitForAjaxToComplete(5);
            StringAssert.IsMatch(@"\$85,001 - \$90,000", cashout.SelectedOption.Text);
            StringAssert.IsMatch(@"\$0 No Cash", cashout.Options.First().Text);
            StringAssert.IsMatch(@"\$130,001 - \$140,000", cashout.Options.Last().Text);
        }

        [Test]
        public void mortgage_21_stepOneImage()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test stepOneImage = 0 (not shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            var images = driver.FindElement(By.Id("step-1")).FindElements(By.TagName("img"));
            Assert.IsEmpty(images);

            // Test stepOneImage = 1 (orange arrow shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            var stepOneImage = driver.FindElement(By.Id("step-1")).FindElement(By.TagName("img"));
            // Change introduced 7/11/2012
            //StringAssert.EndsWith("arrow-orange-swoop.png", stepOneImage.GetAttribute("src"));
            StringAssert.EndsWith("arrow-orange-swoop", stepOneImage.GetAttribute("class"));

            // Test stepOneImage = <default> (orange arrow shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            var image = driver.FindElement(By.Id("step-1")).FindElement(By.TagName("img"));
            // Change introduced 7/11/2012
            //StringAssert.EndsWith("arrow-orange-swoop.png", image.GetAttribute("src"));
            StringAssert.EndsWith("arrow-orange-swoop", image.GetAttribute("class"));
        }

        [Test]
        public void mortgage_22_ssnLabel()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test ssnLabel = 0 ("Optional" text not shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("social-security-one"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("optionalSSN")));
            Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("(Optional)"));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("detailSSN")));
            Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("(NOT REQUIRED, but providing this final detail could help you save!)"));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ssnTip")));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Your information is protected."));

            // Test ssnLabel = 1 ("Optional" text shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("social-security-one"), 5);
            System.Threading.Thread.Sleep(250);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("optionalSSN")));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("(Optional)"));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("detailSSN")));
            Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("(NOT REQUIRED, but providing this final detail could help you save!)"));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ssnTip")));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Your information is protected."));

            // Test ssnLabel = 2 ("Not Required..." text shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("social-security-one"), 5);
            System.Threading.Thread.Sleep(250);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("optionalSSN")));
            Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("(Optional)"));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("detailSSN")));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("(NOT REQUIRED, but providing this final detail could help you save!)"));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ssnTip")));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Your information is protected."));

            // Test ssnLabel = 3 ("Find out how..." text shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-3");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "1-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-3#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("social-security-one"), 5);
            System.Threading.Thread.Sleep(250);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("optionalSSN")));
            Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("(Optional)"));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("detailSSN")));
            Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("(NOT REQUIRED, but providing this final detail could help you save!)"));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ssnTip")));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Find out how LendingTree requests your credit report and how it doesn't count toward your credit score"));

            // Test ssnLabel = <default> ("optional" text not shown)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-12", "");
            mortgage.WaitForElementDisplayed(By.Id("social-security-one"), 5);
            System.Threading.Thread.Sleep(250);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("optionalSSN")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("detailSSN")));
            Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("(NOT REQUIRED, but providing this final detail could help you save!)"));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ssnTip")));
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Your information is protected."));
        }

        [Test]
        public void mortgage_23_InputStyle()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test InputStyle = 0 (system inputs)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            Assert.IsFalse(driver.PageSource.Contains("/assets/css/uniform.default.css"));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("uniform-homeloan-product-type")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("uniform-property-type")));

            // Test InputStyle = 1 (uniformed inputs)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            Assert.IsTrue(driver.PageSource.Contains("/assets/css/uniform.default.css"));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("uniform-homeloan-product-type")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("uniform-property-type")));

            // Test InputStyle = <default> (system inputs)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("step-1"), 5);
            Assert.IsFalse(driver.PageSource.Contains("/assets/css/uniform.default.css"));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("uniform-homeloan-product-type")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("uniform-property-type")));
        }

        [Test]
        public void mortgage_24_currentREAgent()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test currentREAGent = 0 (not shown, required, purchase only)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            mortgage.SelectByValue("homeloan-product-type", "PURCHASE");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-2"), 5);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2-0-0-0-0-0-0-0-0-0#_aspnetform=step-4");
            mortgage.WaitForElementDisplayed(By.Id("property-use"), 5);
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ig-current-realestate-agent")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_yes")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_no")));
            //mortgage.ContinueToNextStep();
            //mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            //Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-5")));
            //Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("purchase-price")));
            //Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ig-current-realestate-agent")));
            //Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ig-current-realestate-agent")));
            //Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_yes")));
            //Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_no")));
            //mortgage.ContinueToNextStep();
            //System.Threading.Thread.Sleep(1000);
            //Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-4")));
            //Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ig-current-realestate-agent")));
            //StringAssert.IsMatch("true", driver.FindElement(By.Id("ig-current-realestate-agent")).FindElements(By.TagName("label"))[3].GetAttribute("generated"));
            //StringAssert.IsMatch("current-realestate-agent", driver.FindElement(By.Id("ig-current-realestate-agent")).FindElements(By.TagName("label"))[3].GetAttribute("for"));
            //StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-current-realestate-agent")).FindElements(By.TagName("label"))[3].Text);

            // Test currentREAGent = 1 (shown, required, purchase only)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            mortgage.SelectByValue("homeloan-product-type", "PURCHASE");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-2"), 5);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-2-0-0-0-0-0-0-0-0-1#_aspnetform=step-4");
            mortgage.WaitForElementDisplayed(By.Id("property-use"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ig-current-realestate-agent")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_yes")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_no")));
            mortgage.ContinueToNextStep();
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-4")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ig-current-realestate-agent")));
            StringAssert.IsMatch("true", driver.FindElement(By.Id("ig-current-realestate-agent")).FindElements(By.TagName("label"))[3].GetAttribute("generated"));
            StringAssert.IsMatch("current-realestate-agent", driver.FindElement(By.Id("ig-current-realestate-agent")).FindElements(By.TagName("label"))[3].GetAttribute("for"));
            StringAssert.IsMatch("This field is required.", driver.FindElement(By.Id("ig-current-realestate-agent")).FindElements(By.TagName("label"))[3].Text);

            // Test currentREAGent = <default> (shown, required, purchase only)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            mortgage.SelectByValue("homeloan-product-type", "PURCHASE");
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-2"), 5);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-4", "");
            mortgage.WaitForElementDisplayed(By.Id("property-use"), 5);
            //Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ig-current-realestate-agent")));
            //Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_yes")));
            //Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_no")));
            //mortgage.ContinueToNextStep();
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("ig-current-realestate-agent")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_yes")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("current-realestate-agent_no")));
            mortgage.ContinueToNextStep();
            mortgage.WaitForElementDisplayed(By.Id("step-5"), 5);
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("step-5")));
            Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("purchase-price")));
            Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("ig-current-realestate-agent")));
        }

        [Test]
        public void mortgage_25_loanTypeDropdown()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test loanTypeDropdown = 0 (all products)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            SelectElement productType0 = new SelectElement(driver.FindElement(By.Id("homeloan-product-type")));
            Assert.IsTrue(productType0.Options.Count.Equals(7), "The number of options in the 'homeloan-product-type' dropdown did not match the expected result (7)!");
            StringAssert.IsMatch("REFINANCE", productType0.Options[0].GetAttribute("value"));
            StringAssert.IsMatch("PURCHASE", productType0.Options[1].GetAttribute("value"));
            StringAssert.IsMatch("HOMEEQUITY", productType0.Options[2].GetAttribute("value"));
            StringAssert.IsMatch("REVERSEMORTGAGE", productType0.Options[3].GetAttribute("value"));
            StringAssert.IsMatch("AUTOPURCHASE", productType0.Options[4].GetAttribute("value"));
            StringAssert.IsMatch("AUTOREFINANCE", productType0.Options[5].GetAttribute("value"));
            StringAssert.IsMatch("PERSONAL", productType0.Options[6].GetAttribute("value"));
            StringAssert.IsMatch("Refinance", productType0.SelectedOption.Text);

            // Test loanTypeDropdown = 1 (only Purchase and Refinance)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            SelectElement productType1 = new SelectElement(driver.FindElement(By.Id("homeloan-product-type")));
            Assert.IsTrue(productType1.Options.Count.Equals(2), "The number of options in the 'homeloan-product-type' dropdown did not match the expected result (2)!");
            StringAssert.IsMatch("REFINANCE", productType1.Options[0].GetAttribute("value"));
            StringAssert.IsMatch("PURCHASE", productType1.Options[1].GetAttribute("value"));
            StringAssert.IsMatch("Refinance", productType1.SelectedOption.Text);

            // Test loanTypeDropdown = <default> (all products)
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
            SelectElement productType = new SelectElement(driver.FindElement(By.Id("homeloan-product-type")));
            Assert.IsTrue(productType.Options.Count.Equals(7), "The number of options in the 'homeloan-product-type' dropdown did not match the expected result (7)!");
            StringAssert.IsMatch("REFINANCE", productType.Options[0].GetAttribute("value"));
            StringAssert.IsMatch("PURCHASE", productType.Options[1].GetAttribute("value"));
            StringAssert.IsMatch("HOMEEQUITY", productType.Options[2].GetAttribute("value"));
            StringAssert.IsMatch("REVERSEMORTGAGE", productType.Options[3].GetAttribute("value"));
            StringAssert.IsMatch("AUTOPURCHASE", productType.Options[4].GetAttribute("value"));
            StringAssert.IsMatch("AUTOREFINANCE", productType.Options[5].GetAttribute("value"));
            StringAssert.IsMatch("PERSONAL", productType.Options[6].GetAttribute("value"));
            StringAssert.IsMatch("Refinance", productType.SelectedOption.Text);
        }

        [Test]
        public void mortgage_Validation_PhoneNumber()
        {
            driver = StartBrowser(strBrowser);
            mortgagePage mortgage = new mortgagePage(driver);

            // Test validation of PhoneNumber with BriteVerify OFF
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-1-1-1-0-0");
            mortgage.WaitForAjaxToComplete(10);
            mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "2-1-1-1-0-0#_aspnetform=step-12");
            mortgage.WaitForElementDisplayed(By.Id("home-phone-one"), 5);

            mortgage.FillHomePhone("012", "541", "5351");
            mortgage.ClickElement(By.Id("social-security-one"));
            //TODO: need to figure this out...the error message is not immediate when BV is OFF
            //StringAssert.IsMatch("Please enter a valid phone number.", driver.FindElement(By.Id("ig-home-phone")).FindElement(By.ClassName("error")).Text);
        }



        //[Test]
        //public void mortgage_26_ltEdu()
        //{
        //    //question removed
        //    return;

        //    driver = StartBrowser(strBrowser);
        //    mortgagePage mortgage = new mortgagePage(driver);

        //    // Test ltEdu = 0 (not shown)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-9");
        //    mortgage.WaitForElementDisplayed(By.Id("step-9"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("declared-bankruptcy-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-no")));
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0#_aspnetform=step-7");
        //    mortgage.WaitForElementDisplayed(By.Id("stated-credit-history"), 5);
        //    mortgage.SelectByValue("stated-credit-history", "MAJORCREDITPROBLEMS");
        //    mortgage.ContinueToNextStep();
        //    mortgage.WaitForElementDisplayed(By.Id("step-8"), 5);
        //    mortgage.ContinueToNextStep();
        //    mortgage.WaitForElementDisplayed(By.Id("step-9"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("declared-bankruptcy-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-no")));

        //    // Test ltEdu = 1 (shown)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-9");
        //    mortgage.WaitForElementDisplayed(By.Id("step-9"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("declared-bankruptcy-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-no")));
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-1#_aspnetform=step-7");
        //    mortgage.WaitForElementDisplayed(By.Id("stated-credit-history"), 5);
        //    mortgage.SelectByValue("stated-credit-history", "MAJORCREDITPROBLEMS");
        //    mortgage.ContinueToNextStep();
        //    mortgage.WaitForElementDisplayed(By.Id("step-8"), 5);
        //    mortgage.ContinueToNextStep();
        //    mortgage.WaitForElementDisplayed(By.Id("step-9"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("declared-bankruptcy-yes")));
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("edu-optin-yes")));
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("edu-optin-no")));

        //    // Test ltEdu = <default> (not shown)
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid, "");
        //    mortgage.WaitForAjaxToComplete(10);
        //    mortgage.WaitForElementDisplayed(By.Id("homeloan-product-type"), 5);
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-9", "");
        //    mortgage.WaitForElementDisplayed(By.Id("step-9"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("declared-bankruptcy-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-no")));
        //    mortgage.NavigateToFossaForm(strEnv, "tl.aspx", strTid + "#_aspnetform=step-7", "");
        //    mortgage.WaitForElementDisplayed(By.Id("stated-credit-history"), 5);
        //    mortgage.SelectByValue("stated-credit-history", "MAJORCREDITPROBLEMS");
        //    mortgage.ContinueToNextStep();
        //    mortgage.WaitForElementDisplayed(By.Id("step-8"), 5);
        //    mortgage.ContinueToNextStep();
        //    mortgage.WaitForElementDisplayed(By.Id("step-9"), 5);
        //    Assert.IsTrue(mortgage.IsElementDisplayed(By.Id("declared-bankruptcy-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-yes")));
        //    Assert.IsFalse(mortgage.IsElementDisplayed(By.Id("edu-optin-no")));
        //}

        public void WaitForNextButtonValueUpdate(String strValue, Int32 intSeconds)
        {
            // Debug code
            //for (int i = 1; i <= 20; i++)
            //{
            //    Common.ReportEvent("DEBUG", String.Format("Loop #{0}", i));
            //    Common.ReportEvent("DEBUG", String.Format("next button value = {0}", driver.FindElement(By.Id("next")).GetAttribute("value")));
            //    Common.ReportEvent("DEBUG", String.Format("jQuery.active = {0}", (driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active;").ToString()));
            //    Common.ReportEvent("DEBUG", String.Format("document.readystate = {0}", (driver as IJavaScriptExecutor).ExecuteScript("return document.readyState;").ToString()));
            //    System.Threading.Thread.Sleep(250);
            //}
            
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(intSeconds));
            Boolean blnConditionMet = objWait.Until<Boolean>((d) =>
            {
                return (driver.FindElement(By.Id("next")).GetAttribute("value").Equals(strValue));
            });
        }

    }
}
