using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree
{
    public class ltHomepage : PageBase
    {
        private readonly IWebDriver homepageDriver;

        // Constructor
        public ltHomepage(IWebDriver driver)
            : base(driver)
        {
            homepageDriver = driver;
        }

        public void NavigateToHomepage(string strEnvironment)
        {
            String strUrl;

            switch (strEnvironment.ToUpper())
            {
                case "PROD":
                    strUrl = "http://www.lendingtree.com";
                    break;
                case "PROD-BETA":
                    strUrl = "http://beta.lendingtree.com";
                    break;
                case "QA":
                case "STAGING":
                case "STAGE":
                    strUrl = "http://qaborrower.lendingtree.com";
                    break;
                case "STAGING-BETA":
                case "STAGE-BETA":
                    strUrl = "http://beta.staging.lendingtree.com";
                    break;
                case "DEV":
                    strUrl = "http://beta.dev.lendingtree.com";
                    break;
                default:    //QA
                    strUrl = "http://beta.staging.lendingtree.com";
                    break;
            }

            Common.ReportEvent(Common.INFO, String.Format("Navigating to Homepage URL: {0}", strUrl));
            homepageDriver.Navigate().GoToUrl(strUrl);

            // Check that we have landed on expected homepage
            WaitForAjaxToComplete(10);
            WaitForElementDisplayed(By.Id("site-navigation"), 5);
            WaitForElementDisplayed(By.Id("step_list"), 5);
        }

        //public void ClickButton(By objBy)
        //{
        //    var objElement = homepageDriver.FindElement(objBy);
        //    objElement.Click();
        //}
    }
}
