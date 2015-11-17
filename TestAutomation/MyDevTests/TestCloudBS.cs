using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace TestAutomation.LendingTree.zSandbox
{
    [TestFixture]
    public class TestCloudBS : SeleniumTestBase
    {
        private IWebDriver _Driver;

        [SetUp]
        public void SetupTest()
        {
            Uri commandExecutorUri = new Uri("http://hub.browserstack.com/wd/hub/");

            DesiredCapabilities desiredCap = new DesiredCapabilities();

            /*desiredCap.SetCapability("browserName", "iPhone");
            desiredCap.SetCapability("platform", "MAC");
            desiredCap.SetCapability("device", "iPhone 6 Plus");*/

            desiredCap.SetCapability("browser", "Chrome");
            desiredCap.SetCapability("browser_version", "45.0");
            desiredCap.SetCapability("os", "Windows");
            desiredCap.SetCapability("os_version", "10");
            desiredCap.SetCapability("resolution", "1024x768");

            desiredCap.SetCapability("browserstack.user", "tamerdakhlallah1");
            desiredCap.SetCapability("browserstack.key", "T6S2eGaXrEBx8fvLRQcB");


            _Driver = new RemoteWebDriver(commandExecutorUri, desiredCap);

            _Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(300));
        }

        [Test]
        public void GoToUrl()
        {
            // Navigate to the form
            _Driver.Navigate().GoToUrl("https://www.google.com");

            StringAssert.Contains("Google", _Driver.Title);
            IWebElement query = _Driver.FindElement(By.Name("q"));
            query.SendKeys("Sauce Labs");
            query.Submit();
        }

        [TearDown]
        public void Cleanup()
        {
            _Driver.Quit();
            Common.ReportFinalResults();
        }
    }
}
