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
    public class TestCloud : SeleniumTestBase
    {
        private IWebDriver _Driver;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();

            // construct the url to sauce labs
            Uri commandExecutorUri = new Uri("http://ondemand.saucelabs.com/wd/hub");
            // set up the desired capabilities
            DesiredCapabilities desiredCapabilites = new DesiredCapabilities("chrome", "35", Platform.CurrentPlatform); // set the desired browser
            desiredCapabilites.SetCapability("platform", "linux"); // operating system to use
            desiredCapabilites.SetCapability("username", "tdakhla"); // supply sauce labs username
            desiredCapabilites.SetCapability("accessKey", "41f99617-74dd-4c9d-a4af-e126becb6182");  // supply sauce labs account key
            desiredCapabilites.SetCapability("name", TestContext.CurrentContext.Test.Name); // give the test a name

            // start a new remote web driver session on sauce labs
            _Driver = new RemoteWebDriver(commandExecutorUri, desiredCapabilites);
            _Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));

            // navigate to the page under test
            _Driver.Navigate().GoToUrl("https://saucelabs.com/test/guinea-pig");
        }

        [TearDown]
        public void TeardownTest()
        {
            _Driver.Quit();
            Common.ReportFinalResults();
        }


        [Test]
        public void LinkTest_01_Disclosures()
        {
            // Navigate to the form
            _Driver.Navigate().GoToUrl("https://offers.staging.lendingtree.com/tlm.aspx?tid=m2&esourceid=14349");
            System.Threading.Thread.Sleep(2000);

            // Click Disclosures link
            _Driver.FindElement(By.LinkText("Disclosures")).Click();
            System.Threading.Thread.Sleep(5000);

            // Verify 
            _Driver.SwitchTo().Window(_Driver.WindowHandles.Last());
            Validation.StringCompare("https://www.lendingtree.com/legal/advertising-disclosures", _Driver.Url);

        }

    }
}
