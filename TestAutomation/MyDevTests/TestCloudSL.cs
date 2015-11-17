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
    public class TestCloudSL : SeleniumTestBase
    {
        private IWebDriver driver;

        [SetUp]
        public void Init()
        {
            DesiredCapabilities caps = new DesiredCapabilities();
            caps.SetCapability(CapabilityType.BrowserName, "iPhone");
            caps.SetCapability(CapabilityType.Version, "9.1");
            caps.SetCapability(CapabilityType.Platform, "OS X 10.10");
            caps.SetCapability("deviceName", "iPhone 6");
            caps.SetCapability("deviceOrientation", "");
            caps.SetCapability("username", "tdakhla"); // supply sauce labs username
            caps.SetCapability("accessKey", "41f99617-74dd-4c9d-a4af-e126becb6182");  // supply sauce labs account key
            caps.SetCapability("name", TestContext.CurrentContext.Test.Name);
            caps.SetCapability("public", "public");
            driver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), caps, TimeSpan.FromSeconds(840));
        }

        [Test]
        public void googleTest()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            StringAssert.Contains("Google", driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("Sauce Labs");
            query.Submit();
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }
    }
}
