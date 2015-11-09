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
    public class TestCloudCBT : SeleniumTestBase
    {
        private IWebDriver _Driver;

        [SetUp]
        public void SetupTest()
        {
            Uri commandExecutorUri = new Uri("http://hub.crossbrowsertesting.com:80/wd/hub");

            DesiredCapabilities caps = new DesiredCapabilities();

            caps.SetCapability("name", "Selenium Test Example");
            caps.SetCapability("build", "1.0");
            caps.SetCapability("browser_api_name", "Chrome45x64");
            caps.SetCapability("os_api_name", "Win10");
            caps.SetCapability("screen_resolution", "1024x768");
            caps.SetCapability("record_video", "true");
            caps.SetCapability("record_network", "true");
            caps.SetCapability("record_snapshot", "false");

            caps.SetCapability("username", "Lendingtree");
            caps.SetCapability("password", "u3bfb1efb3b23305");


            _Driver = new RemoteWebDriver(commandExecutorUri, caps);

            _Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(300));
        }

        [Test]
        public void GoToUrl()
        {
            // Navigate to the form
            _Driver.Navigate().GoToUrl("https://www.google.com");
        }

        [TearDown]
        public void Cleanup()
        {
            _Driver.Quit();
        }
    }
}
