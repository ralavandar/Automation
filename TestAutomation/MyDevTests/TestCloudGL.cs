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
    public class TestCloudGL : SeleniumTestBase
    {
        private IWebDriver driver;

        private BrowserTestHelper browserTestHelper;

        [TestFixtureSetUp]
        public void SetupCombos()
        {
            List<List<string>> OSes = new List<List<string>>();
            OSes.Add(new List<string>() { "Windows", "XP" });
            OSes.Add(new List<string>() { "Windows", "7" });
            OSes.Add(new List<string>() { "Windows", "8" });
            OSes.Add(new List<string>() { "Windows", "8.1" });
            OSes.Add(new List<string>() { "Windows", "10" });
            OSes.Add(new List<string>() { "OS X", "10.11" });

            List<List<string>> Browsers = new List<List<string>>();
            Browsers.Add(new List<string> () { "Chrome", "" });
            Browsers.Add(new List<string> () { "Chrome", "beta" });
            Browsers.Add(new List<string> () { "Firefox", "" });
            Browsers.Add(new List<string> () { "Firefox", "beta" });

            browserTestHelper = new BrowserTestHelper(OSes, Browsers);
        }

        [SetUp]
        public void SetupDriver()
        {
            DesiredCapabilities caps = DesiredCapabilities.Firefox();

            SetBrowserCaps(caps);
            
            caps.SetCapability("gridlasticUser", "DZGgqSSRWBv4uczsU4GuOj2exWzZILgb");
            caps.SetCapability("gridlasticKey", "BNiRv1zitUcNXB6IwC2bLVf9HFZfh5hI");
            caps.SetCapability("video", "True");
            driver = new RemoteWebDriver(new Uri("http://testgridtamer.gridlastic.com:80/wd/hub/"), caps, TimeSpan.FromSeconds(840));
        }

        private void RunTest()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            StringAssert.Contains("Google", driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("Testing");
            query.Submit();

            string sessionId = (string)((RemoteWebDriver)driver).Capabilities.GetCapability("webdriver.remote.sessionid");
            Console.WriteLine("Video: https://s3.amazonaws.com/4ad4a405-ef2a-b3d3-a629-1ab0a2d338b1/eb9c1b2c-2dae-2ff4-b467-ae06efffd68c/play.html?" + sessionId);
            driver.Quit();
        }

        private void SetBrowserCaps(DesiredCapabilities caps)
        {
            caps.SetCapability("platform", "VISTA");
            caps.SetCapability("version", "41");

            browserTestHelper.GoToNextBrowser();
        }

        [Test]
        public void Test1()
        {
            RunTest();
        }

        /*[Test]
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

        [Test]
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
