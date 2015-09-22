using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.zSandbox
{
    
    class CJWidgetTests : SeleniumTestBase
    {
        public IWebDriver driver;

        [Test]
        public void CJWidgetLoad_StagingEachBrowser_LoadsIFrame([Values("FIREFOX", "IEXPLORE", "CHROME")] string browser)
        {
            driver = StartBrowser(browser);
            driver.Navigate().GoToUrl("http://widgets.staging.lendingtree.com/Content/loader/loader-staging.html");
            VerifyIframe(driver.FindElement(By.Id("lendingtreeadvancedwrapper")), "http://widgets.staging.lendingtree.com/FormStart/Advanced/");
            VerifyIframe(driver.FindElement(By.Id("lendingtreeadvancedhwrapper")), "http://widgets.staging.lendingtree.com/FormStart/AdvancedH/");
            driver.Close();
        }

        private void VerifyIframe(IWebElement wrapper, string src)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(ArgumentOutOfRangeException));
            var iframe = wait.Until<IWebElement>(d => 
                wrapper.FindElements(By.TagName("iframe"))[0]);
            Console.WriteLine("Wrapper " + wrapper.GetAttribute("id") + " has iframe with src " + iframe.GetAttribute("src"));
            Assert.That(iframe.GetAttribute("src"), Is.EqualTo(src));
        }
    }
}
