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
    public class CanopyTests : SeleniumTestBase
    {
        private IWebDriver driver;
        private String baseURL;
        //private const String strTableName = "tTestData_Canopy";

        [SetUp]
        public void SetupTest()
        {
            baseURL = "http://ccavmdevweb007:610/";
            Common.InitializeTestResults();
            //GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            //InitializeTestData();
            //driver = StartBrowser("FIREFOX");
            driver = StartBrowser("FIREBUG");
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }


        [Test]
        public void Test01_ViewCampaignListQuickenLoans()
        {
            // Navigate to Canopy URL
            driver.Navigate().GoToUrl(baseURL + "/Dashboard.aspx");
            System.Threading.Thread.Sleep(3000);

            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).Click();
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).Clear();
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys("Quicken");
            System.Threading.Thread.Sleep(3000);
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys(Keys.ArrowDown);

            // In firefox, seems I have to click to select the lender
            driver.FindElement(By.Id("ui-active-menuitem")).Click();
            // In IE, seems I have to hit <enter> key to select the lender
            //driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys(Keys.Enter);

            System.Threading.Thread.Sleep(5000);

            // Select date range here - bypassing calendar controls and just entering the date range brute force
            driver.FindElement(By.Id("addcamp_date")).Clear();
            driver.FindElement(By.Id("addcamp_date")).SendKeys("03/06/2012 - 03/13/2012");
            driver.FindElement(By.Id("addcamp_date")).SendKeys(Keys.Tab);

            System.Threading.Thread.Sleep(5000);

            // Click Campaigns from top nav bar
            driver.FindElement(By.LinkText("Campaigns")).Click();

            System.Threading.Thread.Sleep(3000);

            // Verify we are on campaign list page
            Validation.StringContains("/Campaigns/CampaignsList.aspx", driver.Url);

            // Verify the Add New Campaign button displays
            IWebElement button = driver.FindElement(By.CssSelector("button"));
            if (Validation.IsTrue(button.Text.Equals("Add New Campaign")))
            {
                Common.ReportEvent(Common.PASS, "The 'Add New Campaign' button was displayed on the page as expected.");
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The 'Add New Campaign' button was not found on the page!");
            }
        }


        [Test]
        public void Test02_ViewCampaignEditFixed()
        {
            // Navigate to Canopy URL
            driver.Navigate().GoToUrl(baseURL + "/Dashboard.aspx");
            System.Threading.Thread.Sleep(3000);

            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).Click();
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).Clear();
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys("WCS Lending");
            System.Threading.Thread.Sleep(3000);
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys(Keys.ArrowDown);
            
            // In firefox, seems I have to click to select the lender
            driver.FindElement(By.Id("ui-active-menuitem")).Click();
            // In IE, seems I have to hit <enter> key to select the lender
            //driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys(Keys.Enter);

            System.Threading.Thread.Sleep(3000);

            // Select date range here - bypassing calendar controls and just entering the date range brute force
            driver.FindElement(By.Id("addcamp_date")).Clear();
            driver.FindElement(By.Id("addcamp_date")).SendKeys("03/06/2012 - 03/13/2012");
            driver.FindElement(By.Id("addcamp_date")).SendKeys(Keys.Tab);

            System.Threading.Thread.Sleep(3000);

            // Click Campaigns from top nav bar
            driver.FindElement(By.LinkText("Campaigns")).Click();

            System.Threading.Thread.Sleep(3000);

            // Click on the Campaign link to go to edit page
            driver.FindElement(By.LinkText("Conforming Prime Refinance")).Click();

            System.Threading.Thread.Sleep(3000);

            // Verify we are on campaign edit page
            Validation.StringContains("/Campaigns/EditCampaign.aspx?filterid=", driver.Url);
            
            // Verify the Save Campaign button displays
            if (Validation.IsTrue(driver.FindElement(By.Id("ContentBody_ContentBody_btnSaveCampaign")).Displayed))
            {
                Common.ReportEvent(Common.PASS, "The Save button was displayed on the page as expected.");
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The Save Campaign button was not found on the page!");
            }
        }

        [Test]
        public void Test03_AddNewCustomCampaignLF()
        {
            // Navigate to Canopy URL
            driver.Navigate().GoToUrl(baseURL + "/Dashboard.aspx");
            System.Threading.Thread.Sleep(3000);

            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).Click();
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).Clear();
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys("WCS");
            System.Threading.Thread.Sleep(3000);
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys(Keys.ArrowDown);

            // In firefox, seems I have to click to select the lender
            driver.FindElement(By.Id("ui-active-menuitem")).Click();
            // In IE, seems I have to hit <enter> key to select the lender
            //driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys(Keys.Enter);

            System.Threading.Thread.Sleep(5000);

            //// Select date range here - bypassing calendar controls and just entering the date range brute force
            //driver.FindElement(By.Id("addcamp_date")).Clear();
            //driver.FindElement(By.Id("addcamp_date")).SendKeys("03/06/2012 - 03/13/2012");
            //driver.FindElement(By.Id("addcamp_date")).SendKeys(Keys.Tab);

            //System.Threading.Thread.Sleep(5000);

            // Click Campaigns from top nav bar
            driver.FindElement(By.LinkText("Campaigns")).Click();

            System.Threading.Thread.Sleep(3000);

            // Verify we are on campaign list page
            Validation.StringContains("/Campaigns/CampaignsList.aspx", driver.Url);

            // Verify the Add New Campaign button displays and click it
            IWebElement button = driver.FindElement(By.CssSelector("button"));
            if (button.Text.Equals("Add New Campaign"))
            {
                button.Click();
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The 'Add New Campaign' button was not found on the page!");
            }

            // Verify we are on campaign Add page
            Validation.StringContains("/Campaigns/EditCampaign.aspx", driver.Url);

            //driver.FindElement(By.Id("camp_name")).Set;
            String strPattern = @"MDyyhhmmss";
            String campaignName = "Otto Test Campaign " + DateTime.Now.ToString(strPattern);
            //Console.WriteLine(String.Format("{0} : {1} : {2}", DateTime.Now.ToString(strPattern), strEvent, strMsg));

            driver.FindElement(By.Id("camp_name")).Clear();
            System.Threading.Thread.Sleep(100);
            driver.FindElement(By.Id("camp_name")).SendKeys(campaignName);

            driver.FindElement(By.Id("producttype_purchase")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.Id("filterclass_custom")).Click();

            driver.FindElement(By.Id("PropertyType_SingleFamily")).Click();
            driver.FindElement(By.Id("PropertyType_TownHouse")).Click();
            driver.FindElement(By.Id("PropertyUse_PrimaryHome")).Click();
            driver.FindElement(By.Id("Bankruptcy_9")).Click();
            driver.FindElement(By.Id("Foreclosure_9")).Click();

            driver.FindElement(By.ClassName("StateNC")).Click();
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.XPath("//input[@value='']")).Clear();
            driver.FindElement(By.XPath("//input[@value='']")).SendKeys("1");

            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.Id("ContentBody_ContentBody_btnAddCampaign")).Click();

            System.Threading.Thread.Sleep(10000);

            driver.FindElement(By.XPath("//button[@type='button']")).Click();

            System.Threading.Thread.Sleep(1000);

            // Verify we are still on campaign edit page
            Validation.StringContains("/Campaigns/EditCampaign.aspx", driver.Url);

            // Verify the Save Campaign button displays
            if (Validation.IsTrue(driver.FindElement(By.Id("lblCampID")).Displayed))
            {
                Common.ReportEvent(Common.PASS, String.Format("Campaign saved successfully. The new Campaign ID is {0}",
                    driver.FindElement(By.Id("lblCampID")).Text));
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The Campaign may not have saved successfully.  "
                    + "Cannot locate the new Campaign ID on CampaignEdit.aspx.");
            }

        }


        [Test]
        public void Test05_ViewLeadsWon()
        {
            // Navigate to Canopy URL
            driver.Navigate().GoToUrl(baseURL + "/Dashboard.aspx");
            System.Threading.Thread.Sleep(3000);

            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).Click();
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).Clear();
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys("Quicken");
            System.Threading.Thread.Sleep(3000);
            driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys(Keys.ArrowDown);

            // In firefox, seems I have to click to select the lender
            driver.FindElement(By.Id("ui-active-menuitem")).Click();
            // In IE, seems I have to hit <enter> key to select the lender
            //driver.FindElement(By.Id("ContentHeader_PageTopHeader_ucLender_txtLender")).SendKeys(Keys.Enter);
            System.Threading.Thread.Sleep(5000);

            // Click Leads from top nav bar
            driver.FindElement(By.LinkText("Leads")).Click();
            System.Threading.Thread.Sleep(5000);
            Validation.StringContains("/Leads/Leads.aspx", driver.Url);

            // Set the date
            driver.FindElement(By.Id("addcamp_date")).Clear();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.Id("addcamp_date")).SendKeys("03/07/2012 - 03/07/2012");
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.Id("addcamp_date")).SendKeys(Keys.Tab);

            // Wait for the table to display and then check for the expected # of rows (11)\
            // TODO: This doesn't work - the table is alyways displayed.
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement table = objWait.Until<IWebElement>((d) =>
            {
                //Common.ReportEvent("DEBUG", String.Format("WaitForElementID for {0} is '{1}'", strID, IsElementPresent(By.Id(strID))));
                //Common.ReportEvent("DEBUG", String.Format("The Displayed property for {0} is '{1}'", strID, d.FindElement(By.Id(strID)).Displayed.ToString()));
                return d.FindElement(By.Id("ContentBody_ContentBody_MyLeads"));
            });

            //IWebElement table = driver.FindElement(By.Id("ContentBody_ContentBody_MyLeads"));
            System.Threading.Thread.Sleep(1000);
            if (Validation.IsTrue(table.FindElements(By.TagName("tr")).Count.Equals(11)))
            {
                Common.ReportEvent(Common.PASS, "The MyLeads table displayed the expected number of rows (11).");
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("The MyLeads table did not display the expected number "
                    + "of rows. Expected rows = 11; Actual rows = {0}", table.FindElements(By.TagName("tr")).Count));
            }
        }


        private IWebElement GetElement(String strId)
        {
            try
            {
                return driver.FindElement(By.Id(strId));
            }
            catch (NoSuchElementException)
            {
                Common.ReportEvent(Common.ERROR, String.Format("FindElement failed to find an object with id '{0}'. "
                    + "See following 'FindElementException' screenshot for more details.", strId));
                return null;
            }
        }


        private void Fill(String strId, String strValue)
        {
            var objElement = GetElement(strId);
            objElement.Clear();
            System.Threading.Thread.Sleep(100);
            objElement.SendKeys(strValue);
        }


    }
}
