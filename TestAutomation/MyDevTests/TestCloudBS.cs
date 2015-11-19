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
using TestAutomation.MyDevTests;

namespace TestAutomation.LendingTree.zSandbox
{
    [TestFixture]
    public class TestCloudBS : SeleniumTestBase
    {
        private IWebDriver driver;

        private BrowserTestHelper browserTestHelper;

        [TestFixtureSetUp]
        public void SetupCombos()
        {
            List<List<string>> desktopOSes = new List<List<string>>();
            desktopOSes.Add(new List<string>() { "Windows", "XP" });
            desktopOSes.Add(new List<string>() { "Windows", "7" });
            desktopOSes.Add(new List<string>() { "Windows", "8" });
            desktopOSes.Add(new List<string>() { "Windows", "8.1" });
            desktopOSes.Add(new List<string>() { "Windows", "10" });
            desktopOSes.Add(new List<string>() { "OS X", "El Capitan" });


            List<List<string>> desktopBrowsers = new List<List<string>>();
            desktopBrowsers.Add(new List<string>() { "Chrome", "45.0" });
            desktopBrowsers.Add(new List<string>() { "Chrome", "46.0" });
            desktopBrowsers.Add(new List<string>() { "Firefox", "42.0" });
            /*desktopBrowsers.Add(new List<string> () { "Internet Explorer", "11.0"});
            desktopBrowsers.Add(new List<string> () { "Internet Explorer", "10.0"});
            desktopBrowsers.Add(new List<string> () { "Internet Explorer", "9.0"});
            desktopBrowsers.Add(new List<string> () { "Internet Explorer", "8.0"});
            desktopBrowsers.Add(new List<string> () { "MicrosoftEdge", "20.10240" });
            desktopBrowsers.Add(new List<string> () { "Safari", "9.0" });*/

            browserTestHelper = new BrowserTestHelper(desktopOSes, desktopBrowsers);
        }

        [SetUp]
        public void SetupDriver()
        {
            DesiredCapabilities caps = new DesiredCapabilities();

            SetBrowserCaps(caps);

            caps.SetCapability("browserstack.user", "tamerdakhlallah1");
            caps.SetCapability("browserstack.key", "T6S2eGaXrEBx8fvLRQcB");
            caps.SetCapability("name", TestContext.CurrentContext.Test.Name);
            caps.SetCapability("public", "public");
            driver = new RemoteWebDriver(new Uri("http://hub.browserstack.com/wd/hub/"), caps, TimeSpan.FromSeconds(840));
        }

        private void RunTest()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            StringAssert.Contains("Google", driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("Testing");
            query.Submit();

            driver.Quit();
        }

        private void SetBrowserCaps(DesiredCapabilities caps)
        {
            caps.SetCapability("os", browserTestHelper.GetOS()[0]);
            caps.SetCapability("os_version", browserTestHelper.GetOS()[1]);
            caps.SetCapability("browser", browserTestHelper.GetBrowser()[0]);
            caps.SetCapability("browser_version", browserTestHelper.GetBrowser()[1]);

            browserTestHelper.GoToNextBrowser();
        }

        [Test]
        public void Test1()
        {
            RunTest();
        }

        [Test]
        public void Test2()
        {
            RunTest();
        }

        [Test]
        public void Test3()
        {
            RunTest();
        }

        [Test]
        public void Test4()
        {
            RunTest();
        }

        [Test]
        public void Test5()
        {
            RunTest();
        }

        [Test]
        public void Test6()
        {
            RunTest();
        }

        [Test]
        public void Test7()
        {
            RunTest();
        }

        [Test]
        public void Test8()
        {
            RunTest();
        }

        [Test]
        public void Test9()
        {
            RunTest();
        }

        [Test]
        public void Test10()
        {
            RunTest();
        }

        /*[Test]
        public void Test11()
        {
            RunTest();
        }

        [Test]
        public void Test12()
        {
            RunTest();
        }

        [Test]
        public void Test13()
        {
            RunTest();
        }

        [Test]
        public void Test14()
        {
            RunTest();
        }

        [Test]
        public void Test15()
        {
            RunTest();
        }

        [Test]
        public void Test16()
        {
            RunTest();
        }

        [Test]
        public void Test17()
        {
            RunTest();
        }

        [Test]
        public void Test18()
        {
            RunTest();
        }

        [Test]
        public void Test19()
        {
            RunTest();
        }

        [Test]
        public void Test20()
        {
            RunTest();
        }

        [Test]
        public void Test21()
        {
            RunTest();
        }

        [Test]
        public void Test22()
        {
            RunTest();
        }

        [Test]
        public void Test23()
        {
            RunTest();
        }

        [Test]
        public void Test24()
        {
            RunTest();
        }

        [Test]
        public void Test25()
        {
            RunTest();
        }

        [Test]
        public void Test26()
        {
            RunTest();
        }

        [Test]
        public void Test27()
        {
            RunTest();
        }

        [Test]
        public void Test28()
        {
            RunTest();
        }

        [Test]
        public void Test29()
        {
            RunTest();
        }

        [Test]
        public void Test30()
        {
            RunTest();
        }

        [Test]
        public void Test31()
        {
            RunTest();
        }

        [Test]
        public void Test32()
        {
            RunTest();
        }

        [Test]
        public void Test33()
        {
            RunTest();
        }

        [Test]
        public void Test34()
        {
            RunTest();
        }

        [Test]
        public void Test35()
        {
            RunTest();
        }

        [Test]
        public void Test36()
        {
            RunTest();
        }

        [Test]
        public void Test37()
        {
            RunTest();
        }

        [Test]
        public void Test38()
        {
            RunTest();
        }

        [Test]
        public void Test39()
        {
            RunTest();
        }

        [Test]
        public void Test40()
        {
            RunTest();
        }

        [Test]
        public void Test41()
        {
            RunTest();
        }

        [Test]
        public void Test42()
        {
            RunTest();
        }

        [Test]
        public void Test43()
        {
            RunTest();
        }

        [Test]
        public void Test44()
        {
            RunTest();
        }

        [Test]
        public void Test45()
        {
            RunTest();
        }

        [Test]
        public void Test46()
        {
            RunTest();
        }

        [Test]
        public void Test47()
        {
            RunTest();
        }

        [Test]
        public void Test48()
        {
            RunTest();
        }

        [Test]
        public void Test49()
        {
            RunTest();
        }

        [Test]
        public void Test50()
        {
            RunTest();
        }

        [Test]
        public void Test51()
        {
            RunTest();
        }

        [Test]
        public void Test52()
        {
            RunTest();
        }

        [Test]
        public void Test53()
        {
            RunTest();
        }

        [Test]
        public void Test54()
        {
            RunTest();
        }

        [Test]
        public void Test55()
        {
            RunTest();
        }

        [Test]
        public void Test56()
        {
            RunTest();
        }

        [Test]
        public void Test57()
        {
            RunTest();
        }

        [Test]
        public void Test58()
        {
            RunTest();
        }

        [Test]
        public void Test59()
        {
            RunTest();
        }

        [Test]
        public void Test60()
        {
            RunTest();
        }*/

        [TearDown]
        public void CleanUp()
        {

        }
    }
}
