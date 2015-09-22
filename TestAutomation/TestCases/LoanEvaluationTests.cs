using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.zSandbox
{
    [TestFixture]
    class LoanEvaluationTests : SeleniumTestBase
    {
        private IWebDriver driver;

        [Test]
        public void LoanEvaluation_Purchase_GoesToPurchase([Values("CHROME", "IEXPLORE", "FIREFOX")] string browser)
        {
            driver = StartBrowser(browser);

            driver.Navigate().GoToUrl("http://staging.lendingtree.com/loan-evaluation");

            var select = new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until<IWebElement>((d) => d.FindElement(By.Id("LoanPurpose")));

            new SelectElement(select).SelectByValue("PURCHASE");

            System.Threading.Thread.Sleep(5000);

            Assert.That(driver.Url, Is.StringContaining("loan-evaluation/purchase"));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
