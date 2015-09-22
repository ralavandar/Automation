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
    public class LoanExplorerTests : SeleniumTestBase
    {
        private IWebDriver driver;
        private String baseURL;
        //private const String strTableName = "tTestData_Canopy";

        [SetUp]
        public void SetupTest()
        {
            baseURL = "http://dev.lendingtree.com/loan-explorer?lending.istest&esourceid=14349";
            Common.InitializeTestResults();
            //GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            //InitializeTestData();
            driver = StartBrowser("FIREFOX");
            //driver = StartBrowser("FIREBUG");
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }


        [Test]
        public void LoanExplorer_01_Purchase()
        {
            // Navigate to LoanExplorer
            driver.Navigate().GoToUrl(baseURL);
            System.Threading.Thread.Sleep(10000);

            SelectElement productSelect = new SelectElement(driver.FindElement(By.CssSelector("[ng-model='request.RequestedLoanTypeId']")));
            productSelect.SelectByText("Purchase");
            System.Threading.Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[ng-model='request.EstimatedPurchasePrice']")).Clear();
            driver.FindElement(By.CssSelector("[ng-model='request.EstimatedPurchasePrice']")).SendKeys("225000");
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.CssSelector("[ng-model='request.EstimatedDownPayment']")).Clear();
            driver.FindElement(By.CssSelector("[ng-model='request.EstimatedDownPayment']")).SendKeys("25000");
            System.Threading.Thread.Sleep(500);

            SelectElement creditSelect = new SelectElement(driver.FindElement(By.CssSelector("[ng-model='request.EstimatedCreditScore']")));
            creditSelect.SelectByValue("1");
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.CssSelector("[ng-model='request.PropertyZipCode']")).Clear();
            driver.FindElement(By.CssSelector("[ng-model='request.PropertyZipCode']")).SendKeys("95066");
            System.Threading.Thread.Sleep(1000);

            Common.ReportEvent(Common.INFO, "Clicking on the 'Submit' button");
            driver.FindElement(By.CssSelector("[ng-click='search()']")).Click();

            // TODO: try to 'detect' the Searching screen


            // Wait for 10 seconds
            System.Threading.Thread.Sleep(10000);

            // Get the Search Request GUID from the URL
            Common.ReportEvent(Common.INFO, String.Format("The URL is: {0}", driver.Url));
            Common.ReportEvent(Common.INFO, String.Format("The Request GUID is: {0}", driver.Url.Substring(driver.Url.IndexOf("/offers/") + 8)));

            // Count how many offers we have - but we might have 0.
            // document.getElementById("offers").children
            // document.getElementById("offers").children[1].getElementsByTagName("a")[1].getAttribute("href")
            var offerCount = driver.FindElement(By.Id("offers")).GetAttribute("childElementCount");
            Common.ReportEvent(Common.INFO, String.Format("The count of offers on the page is: {0}", offerCount.ToString()));

            // Click on the 'Details' button for one of the offers
            var offerlink = driver.FindElement(By.Id("offers")).FindElements(By.ClassName("detail-link"))[3];
            var offerhref = offerlink.GetAttribute("ng-href");
            Common.ReportEvent(Common.INFO, String.Format("The ng-href of the link we are about to click on is: {0}", offerhref));
            //offerlink.Click();
            driver.FindElement(By.CssSelector("a[ng-href*='" + offerhref + "']")).Click();

            System.Threading.Thread.Sleep(2000);

            // Verify data in table
            var rows = driver.FindElement(By.ClassName("detail-table")).FindElements(By.TagName("tr"));
            Validation.StringCompare(rows[1].FindElement(By.ClassName("row-label")).Text, "Purpose");
            Validation.StringCompare(rows[1].FindElement(By.CssSelector("[on='request.RequestedLoanTypeId']")).Text, "Purchase");
            Validation.StringCompare(rows[9].FindElement(By.ClassName("row-label")).Text, "Home Value");
            Validation.StringCompare(rows[9].FindElement(By.CssSelector("[ng-show=\"request.RequestedLoanTypeId=='1'\"]")).Text, "$225,000");
            Validation.StringCompare(rows[10].FindElement(By.ClassName("row-label")).Text, "Down Payment");
            Validation.StringCompare(rows[10].FindElement(By.CssSelector("[class='ng-binding']")).Text, "$25,000");
            Validation.StringCompare(rows[14].FindElement(By.ClassName("row-label")).Text, "Requested Loan Amount");
            Validation.StringCompare(rows[14].FindElement(By.CssSelector("[class='ng-binding']")).Text, "$200,000");

            System.Threading.Thread.Sleep(2000);
        }


        [Test]
        public void LoanExplorer_02_RefCashout()
        {
            // Navigate to LoanExplorer
            driver.Navigate().GoToUrl(baseURL);
            System.Threading.Thread.Sleep(10000);

            SelectElement select = new SelectElement(driver.FindElement(By.CssSelector("[ng-model='request.RequestedLoanTypeId']")));
            select.SelectByText("Refinance");
            System.Threading.Thread.Sleep(1000);

            driver.FindElement(By.CssSelector("[ng-model='request.EstimatedPropertyValue']")).Clear();
            driver.FindElement(By.CssSelector("[ng-model='request.EstimatedPropertyValue']")).SendKeys("345000");
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.CssSelector("[ng-model='request.CurrentMortgageBalance']")).Clear();
            driver.FindElement(By.CssSelector("[ng-model='request.CurrentMortgageBalance']")).SendKeys("151000");
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.CssSelector("[ng-model='request.PropertyZipCode']")).Clear();
            driver.FindElement(By.CssSelector("[ng-model='request.PropertyZipCode']")).SendKeys("28173");
            System.Threading.Thread.Sleep(1000);

            driver.FindElement(By.Id("layout-sidebar")).FindElement(By.LinkText("More Options")).Click();
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[ng-model='request.RequestedCashoutAmount']")).Clear();
            driver.FindElement(By.CssSelector("[ng-model='request.RequestedCashoutAmount']")).SendKeys("9000");

            Common.ReportEvent(Common.INFO, "Clicking on the 'Submit' button");
            driver.FindElement(By.Id("layout-sidebar")).FindElement(By.CssSelector("[ng-click='search()']")).Click();

            // Wait for 10 seconds
            System.Threading.Thread.Sleep(10000);

            // Let's try to get the Search Request GUID from the URL
            Common.ReportEvent(Common.INFO, String.Format("The URL is: {0}", driver.Url));
            Common.ReportEvent(Common.INFO, String.Format("The Request GUID is: {0}", driver.Url.Substring(driver.Url.IndexOf("/offers/") + 8)));

            // Let's try to count how many offers we have - but we might have 0.
            // document.getElementById("offers").children
            // document.getElementById("offers").children[1].getElementsByTagName("a")[1].getAttribute("href")
            var offerCount = driver.FindElement(By.Id("offers")).GetAttribute("childElementCount");
            Common.ReportEvent(Common.INFO, String.Format("The count of offers on the page is: {0}", offerCount.ToString()));

            // Randomly click on the 'Details' button for one of the offers
            var offerlink = driver.FindElement(By.Id("offers")).FindElements(By.ClassName("detail-link"))[3];
            var offerhref = offerlink.GetAttribute("ng-href");
            Common.ReportEvent(Common.INFO, String.Format("The ng-href of the link we are about to click on is: {0}", offerhref));
            //offerlink.Click();
            driver.FindElement(By.CssSelector("a[ng-href*='" + offerhref + "']")).Click();

            System.Threading.Thread.Sleep(2000);

            // Verify data in table
            var rows = driver.FindElement(By.ClassName("detail-table")).FindElements(By.TagName("tr"));
            Validation.StringCompare(rows[1].FindElement(By.ClassName("row-label")).Text, "Purpose");
            Validation.StringCompare(rows[1].FindElement(By.CssSelector("[on='request.RequestedLoanTypeId']")).Text, "Refinance");
            Validation.StringCompare(rows[9].FindElement(By.ClassName("row-label")).Text, "Home Value");
            Validation.StringCompare(rows[9].FindElement(By.CssSelector("[ng-show=\"request.RequestedLoanTypeId=='2'\"]")).Text, "$345,000");
            Validation.StringCompare(rows[11].FindElement(By.ClassName("row-label")).Text, "First Mortgage Balance");
            Validation.StringCompare(rows[11].FindElement(By.CssSelector("[class='ng-binding']")).Text, "$151,000");
            Validation.StringCompare(rows[12].FindElement(By.ClassName("row-label")).Text, "Second Mortgage Balance");
            Validation.StringCompare(rows[12].FindElement(By.CssSelector("[class='ng-binding']")).Text, "$0");
            Validation.StringCompare(rows[13].FindElement(By.ClassName("row-label")).Text, "Cash Out Amount");
            Validation.StringCompare(rows[13].FindElement(By.CssSelector("[class='ng-binding']")).Text, "$9,000");
            Validation.StringCompare(rows[14].FindElement(By.ClassName("row-label")).Text, "Requested Loan Amount");
            Validation.StringCompare(rows[14].FindElement(By.CssSelector("[class='ng-binding']")).Text, "$160,000");
            
            System.Threading.Thread.Sleep(2000);
        }

    }
}
