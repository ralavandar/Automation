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
    public class TestCloud : SeleniumTestBase
    {
        private IWebDriver driver;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            driver = StartBrowser("FIREFOX");
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }


        [Test]
        public void LinkTest_01_Disclosures()
        {
            // Navigate to the form
            driver.Navigate().GoToUrl("https://offers.staging.lendingtree.com/tlm.aspx?tid=m2&esourceid=14349");
            System.Threading.Thread.Sleep(2000);

            // Click Disclosures link
            driver.FindElement(By.LinkText("Disclosures")).Click();
            System.Threading.Thread.Sleep(5000);

            // Verify 
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            Validation.StringCompare("https://www.lendingtree.com/legal/advertising-disclosures", driver.Url);
           
        }

    }
}
