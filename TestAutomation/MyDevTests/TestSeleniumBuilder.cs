using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using NUnit.Framework;

namespace TestAutomation.MyDevTests
{
    [TestFixture()]
    public class blah
    {
        private IWebDriver wd;

        [SetUp]
        public void SetupDriver()
        {
            DesiredCapabilities caps = new DesiredCapabilities();

            SetBrowserCaps(caps);

            caps.SetCapability("username", "tdakhla"); // supply sauce labs username
            caps.SetCapability("accessKey", "41f99617-74dd-4c9d-a4af-e126becb6182");  // supply sauce labs account key
            caps.SetCapability("name", TestContext.CurrentContext.Test.Name);
            caps.SetCapability("public", "public");
            wd = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), caps, TimeSpan.FromSeconds(840));
        }

        private void SetBrowserCaps(DesiredCapabilities caps)
        {
            caps.SetCapability("browserName", "Firefox");
            caps.SetCapability("platform", "Windows 10");
            caps.SetCapability("version", "42.0");
        }

        [Test()]
        public void TestCase()
        {
            try
            {
                wd.Navigate().GoToUrl("https://www.google.com/?gws_rd=ssl");
                wd.FindElement(By.Id("lst-ib")).Click();
                wd.FindElement(By.Id("lst-ib")).Clear();
                wd.FindElement(By.Id("lst-ib")).SendKeys("saucelabs");
                wd.FindElement(By.Name("btnG")).Click();
                wd.FindElement(By.LinkText("Sauce Labs (@saucelabs) | Twitter")).Click();
                wd.FindElement(By.CssSelector("a.SignupForm-submit")).Click();
                if (!(wd.FindElements(By.Id("submit_button")).Count != 0))
                {
                    Console.Error.WriteLine("verifyElementPresent failed");
                }
            }
            finally { wd.Quit(); }
        }
    }
}
