using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestAutomation.LendingTree.zSandbox
{
    [TestFixture]
    public class splitterTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strBrowser = "firefox";
        private const String strEnv = "STAGE-BETA";

        [SetUp]
        public void SetupTest()
        {
            driver = StartBrowser(strBrowser);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
        }

        [Test]
        public void splitter_01_TestBullseyeToRefiQF()
        {
            ltHomepage homepage = new ltHomepage(driver);

            homepage.NavigateToHomepage(strEnv);

            // Check if offers area is expanded - if not, expand it
            if (!(homepage.IsElementDisplayed(By.Id("splitterid"))))
            {
                Common.ReportEvent("DEBUG", "Offers area not expanded...expanding it now.");

                // Expand the Offers area by clicking the 1st 'Get Started' button
                IWebElement expandButton = driver.FindElement(By.Id("step_list")).FindElements(By.ClassName("step-item"))[0]
                    .FindElements(By.TagName("button"))[0];
                if (expandButton.Displayed)
                {
                    Common.ReportEvent("DEBUG", "Clicking Expand button.");
                    expandButton.Click();
                }
                else
                {
                    Common.ReportEvent("DEBUG", "Expand button was not displayed.  Check following screenshot to see what is going on.");
                    homepage.RecordScreenshot("HomepageExpandButtonClickFail");
                }
            }
            else
            {
                Common.ReportEvent("DEBUG", "Offers area already expanded...proceed with test case.");
            }

            // Select Refi Loan Type, Home Type, and click Get Started
            homepage.SelectByValue("splitterid", "ns-hp-refinance");
            System.Threading.Thread.Sleep(500);
            homepage.SelectByValue("property-type", "SINGLEFAMDET");
            IWebElement submitButton = driver.FindElement(By.Id("step_list")).FindElements(By.ClassName("step-item"))[0]
                .FindElements(By.TagName("button"))[1];
            if (submitButton.Displayed)
            {
                Common.ReportEvent("DEBUG", "Clicking Submit button.");
                submitButton.Click();
            }
            else
            {
                Common.ReportEvent("DEBUG", "Submit button was not displayed.  Check following screenshot to see what is going on");
                homepage.RecordScreenshot("HomepageSubmitButtonClickFail");
            }

            // Verify QF displays through the following Assert statments
            homepage.WaitForAjaxToComplete(10);

            // TODO: need to have a flexible wait here...sometimes 2 seconds is not long enough...but waiting > 2 seconds is painful.
            System.Threading.Thread.Sleep(2000);
            Common.ReportEvent(Common.INFO, String.Format("Redirected to URL: {0}", driver.Url));

            Assert.IsTrue((driver.Url.Contains("/tl.aspx")) || (driver.Url.Contains("/quickmatchformloader.aspx")), 
                "The redirect through splitter did not land on one of the expected URLs");
            Assert.IsTrue((homepage.IsElementPresent(By.Id("step-1")) || (homepage.IsElementPresent(By.Id("step1")))), 
                "The tl or quickmatchformloader 'step 1' element was not present on the page (either 'step-1' or 'step1')");
            Assert.IsTrue(driver.Url.Contains("&LOAN_TYPE=REFINANCE"), 
                "The loan type was not in the query string as expected: '&LOAN_TYPE=REFINANCE'.");
            Assert.IsTrue(driver.Url.Contains("&property-type=SINGLEFAMDET"),
                "The property type was not in the query string as expected: '&property-type=SINGLEFAMDET'.");
        }

        [Test]
        public void splitter_02_TestBullseyeToPurchaseQF()
        {
            ltHomepage homepage = new ltHomepage(driver);

            homepage.NavigateToHomepage(strEnv);

            // Check if offers area is expanded - if not, expand it
            if (!(homepage.IsElementDisplayed(By.Id("splitterid"))))
            {
                // Expand the Offers area by clicking the 1st 'Get Started' button
                IWebElement expandButton = driver.FindElement(By.Id("step_list")).FindElements(By.ClassName("step-item"))[0]
                    .FindElements(By.TagName("button"))[0];
                expandButton.Click();
            }

            // Select Purchase Loan Type, Home Type, and click Get Started
            homepage.SelectByValue("splitterid", "ns-hp-purchase");
            System.Threading.Thread.Sleep(500);
            homepage.SelectByValue("property-type", "SINGLEFAMATT");
            IWebElement submitButton = driver.FindElement(By.Id("step_list")).FindElements(By.ClassName("step-item"))[0]
                .FindElements(By.TagName("button"))[1];
            submitButton.Click();

            // Verify QF displays through the following Assert statments
            homepage.WaitForAjaxToComplete(10);
            System.Threading.Thread.Sleep(2000);
            Common.ReportEvent(Common.INFO, String.Format("Redirected to URL: {0}", driver.Url));

            Assert.IsTrue((driver.Url.Contains("/tl.aspx")) || (driver.Url.Contains("/quickmatchformloader.aspx")),
                "The redirect through splitter did not land on one of the expected URLs");
            Assert.IsTrue((homepage.IsElementPresent(By.Id("step-1")) || (homepage.IsElementPresent(By.Id("step1")))),
                "The tl or quickmatchformloader 'step 1' element was not present on the page (either 'step-1' or 'step1')");
            Assert.IsTrue(driver.Url.Contains("&LOAN_TYPE=PURCHASE"),
                "The loan type was not in the query string as expected: '&LOAN_TYPE=PURCHASE'.");
            Assert.IsTrue(driver.Url.Contains("&property-type=SINGLEFAMATT"),
                "The property type was not in the query string as expected: '&property-type=SINGLEFAMATT'.");
        }

        [Test]
        public void splitter_03_TestProductPageToRefiQF()
        {
            ltHomepage homepage = new ltHomepage(driver);

            homepage.NavigateToHomepage(strEnv);

            System.Threading.Thread.Sleep(2000);

            // Navigate to menu option "Home Loans | Refinance"
            Common.ReportEvent("DEBUG", String.Format("site-navigation innerHtml is: {0}", driver.FindElement(By.Id("site-navigation")).GetAttribute("innerHTML")));
            driver.FindElement(By.Id("site-navigation")).FindElement(By.LinkText("HOME LOANS")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.Id("site-navigation")).FindElement(By.LinkText("Refinance")).Click();
            
            // Wait and verify the Refi product page displays
            System.Threading.Thread.Sleep(2000);
            Assert.IsTrue(driver.Url.Contains("/mortgage-refinance/home"),
                "The navigation to the Refi product page failed.  Did not land on the expected URL.");
            homepage.WaitForElementDisplayed(By.Id("property-type"), 10);

            // Select Purchase Loan Type, Home Type, and click Get Started
            homepage.SelectByValue("property-type", "LOWRISECONDO");
            IWebElement submitButton = driver.FindElements(By.ClassName("form-start"))[0]
                .FindElements(By.TagName("button"))[0];
            submitButton.Click();

            // Verify QF displays through the following Assert statments
            homepage.WaitForAjaxToComplete(10);
            System.Threading.Thread.Sleep(2000);
            Common.ReportEvent(Common.INFO, String.Format("Redirected to URL: {0}", driver.Url));

            Assert.IsTrue((driver.Url.Contains("/tl.aspx")) || (driver.Url.Contains("/quickmatchformloader.aspx")),
                "The redirect through splitter did not land on one of the expected URLs");
            Assert.IsTrue((homepage.IsElementPresent(By.Id("step-1")) || (homepage.IsElementPresent(By.Id("step1")))),
                "The tl or quickmatchformloader 'step 1' element was not present on the page (either 'step-1' or 'step1')");
            //Assert.IsTrue(driver.Url.Contains("&LOAN_TYPE=REFINANCE"),
            //    "The loan type was not in the query string as expected: '&LOAN_TYPE=REFINANCE'.");
            Assert.IsTrue(driver.Url.Contains("&property-type=LOWRISECONDO"),
                "The property type was not in the query string as expected: '&property-type=LOWRISECONDO'.");
        }
    
    }
}
