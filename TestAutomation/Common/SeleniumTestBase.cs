﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace TestAutomation
{
    /// <summary>
    /// The SeleniumTestBase class is used to setup and deliver the Selenium 2 driver to the test
    /// </summary>
    public class SeleniumTestBase
    {
        private IWebDriver objDriver;
        private FirefoxProfile ffProfile;
        public String strRandom;
        public Dictionary<String, String> testData;
        // passed to Common.ReportFinalResultsAuditLog called by individual test teardowns
        public string varQFormUIDTest;
        DesiredCapabilities caps = new DesiredCapabilities();

        public IWebDriver StartBrowser(String strBrowser)
        {
            switch (strBrowser.ToUpper())
            {
                case "FIREFOX":
                    
                    ffProfile = new FirefoxProfile();
                    ffProfile.SetPreference("network.automatic-ntlm-auth.trusted-uris", "http://ccavmdevweb007:610, https://qanewlenderportal.lendingtree.com");  // Bypass Canopy authentication dialog
                    ffProfile.AcceptUntrustedCertificates = true;
                    objDriver = new FirefoxDriver(ffProfile);
                    break;
                case "FIREBUG": // Firefox with Firebug add-on
                    //File file = new File("firebug-1.9.1.xpi");
                    ffProfile = new FirefoxProfile();
                    ffProfile.AddExtension("c:\\Tree\\firebug-1.9.1-fx.xpi");
                    ffProfile.AddExtension("c:\\Tree\\netExport-0.7.xpi");
                    ffProfile.SetPreference("extensions.firebug.currentVersion", "1.9.1"); // Avoid startup screen
                    ffProfile.SetPreference("extensions.firebug.console.enableSites", "true"); //enable console
                    ffProfile.SetPreference("extensions.firebug.net.enableSites", "true"); //enable net
                    ffProfile.SetPreference("extensions.firebug.defaultPanelName", "net");
                    ffProfile.SetPreference("extensions.firebug.netexport.defaultLogDir", "c:\\Logs\\Firebug");
                    ffProfile.SetPreference("extensions.firebug.netexport.alwaysEnableAutoExport", "true");
                    ffProfile.SetPreference("network.automatic-ntlm-auth.trusted-uris", "http://ccavmdevweb007:610, https://qanewlenderportal.lendingtree.com");  // Bypass Canopy authentication dialog
                    ffProfile.AcceptUntrustedCertificates = true;
                    objDriver = new FirefoxDriver(ffProfile);
                    break;
                case "IEXPLORE":
                case "IE":
                    objDriver = new InternetExplorerDriver();
                    objDriver.Manage().Cookies.DeleteAllCookies();
                    break;
                case "CHROME":
                    objDriver = new ChromeDriver();
                    break;
                case "REMOTE":
                    objDriver = new RemoteWebDriver(new Uri("http://10.29.150.153:4444/wd/hub"), caps, TimeSpan.FromSeconds(840));
                    break;
                case "BROWSERSTACK":
                    caps.SetCapability("browser", "IE");
                    caps.SetCapability("browser_version", "11.0");
                    caps.SetCapability("os", "Windows");
                    caps.SetCapability("os_version", "7");
                    caps.SetCapability("resolution", "1024x768");
                    caps.SetCapability("browserstack.user", "mikeschupp1");
                    caps.SetCapability("browserstack.key", "ydejoaozpt7zhzCz6yM9");
                    caps.SetCapability("public", "public");
                    objDriver = new RemoteWebDriver(new Uri("http://hub.browserstack.com/wd/hub/"), caps, TimeSpan.FromSeconds(840));
                    break;
                case "CBT":             // CrossBrowserTesting.com
                    // IE 11, Win 10
                    caps.SetCapability("name", "Selenium Test Example");
                    caps.SetCapability("build", "1.0");
                    caps.SetCapability("browser_api_name", "Chrome45x64");
                    caps.SetCapability("os_api_name", "Win10");
                    caps.SetCapability("screen_resolution", "1024x768");
                    //caps.SetCapability("browser_api_name", "IE11");
                    //caps.SetCapability("os_api_name", "Win10");
                    //caps.SetCapability("screen_resolution", "1024x768");
                    // iPhone 6
                    //caps.SetCapability("browser_api_name", "MblSafari8.0");
                    //caps.SetCapability("os_api_name", "iPhone6-iOS8sim");
                    //caps.SetCapability("screen_resolution", "750x1334");
                    // Galaxy S5
                    //caps.SetCapability("browser_api_name", "MblChrome35");
                    //caps.SetCapability("os_api_name", "GalaxyS5-And44");
                    //caps.SetCapability("screen_resolution", "1080x1920");
                    //
                    caps.SetCapability("record_video", "true");
                    caps.SetCapability("record_network", "true");
                    caps.SetCapability("record_snapshot", "false");
                    caps.SetCapability("username", "Lendingtree");
                    caps.SetCapability("password", "u3bfb1efb3b23305");
                    //caps.SetCapability("name", NUnit.Framework.TestContext.CurrentContext.Test.Name);
                    //caps.SetCapability("build", "1.0");
                    objDriver = new RemoteWebDriver(new Uri("http://hub.crossbrowsertesting.com:80/wd/hub"), caps);
                    objDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(300));

                    // _Driver = new RemoteWebDriver(commandExecutorUri, caps);
                    // _Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(300));
                    break;
                case "SAUCELABS":
                    // IE 11
                    caps.SetCapability("browserName", "Internet Explorer");
                    caps.SetCapability("platform", "Windows 7");
                    caps.SetCapability("version", "11.0");
                    // iPhone
                    //caps.SetCapability("browserName", "iPhone");
                    //caps.SetCapability("platform", "OS X 10.10");
                    //caps.SetCapability("version", "9.2");
                    //caps.SetCapability("deviceName", "iPhone 6");
                    //caps.SetCapability("deviceOrientation", "portrait");
                    // Android
                    //caps.SetCapability("browserName", "Android");
                    //caps.SetCapability("platform", "Linux");
                    //caps.SetCapability("version", "4.4");
                    //caps.SetCapability("deviceName", "Samsung Galaxy S4 Emulator");
                    //caps.SetCapability("deviceOrientation", "portrait");
                    //
                    caps.SetCapability("username", "mpschupp");
                    caps.SetCapability("accessKey", "ca34f7d2-89d2-46be-bc4b-8aad12f2fedb");
                    caps.SetCapability("name", NUnit.Framework.TestContext.CurrentContext.Test.Name);
                    objDriver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), caps, TimeSpan.FromSeconds(600));
                    break;
                default:    // Firefox
                    ffProfile = new FirefoxProfile();
                    ffProfile.AcceptUntrustedCertificates = true;
                    objDriver = new FirefoxDriver(ffProfile);
                    break;
            }

            IJavaScriptExecutor js = objDriver as IJavaScriptExecutor;
            String strAgent = (String)js.ExecuteScript("return navigator.userAgent");
            Common.ReportEvent(Common.INFO, String.Format("The userAgent for this test is: {0}.", strAgent));

            return objDriver;
        }


        public void GetTestData(String table, String testcase)
        {
            DataSheet testSheet = new DataSheet(table, testcase);
            testData = testSheet.GetDataDictionary();
        }


        public void InitializeTestData()
        {
            // Note: This is very Lending/Fossa centric.  Probably does not belong here...

            // In the data sheet, we allow for some fields to be defaulted to a randomly generated value.
            // That logic is handled here using a 6-digit random number.
            strRandom = Common.RandomNumber(100000, 999999).ToString();

            // Email Address
            if (testData.ContainsKey("EmailAddress"))
            {
                if (testData["EmailAddress"].Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    testData["EmailAddress"] = "otto" + strRandom + "@lendingtree.com";
                }
                if (testData["EmailAddress"].Equals("asdf", StringComparison.OrdinalIgnoreCase) ||
                    testData["EmailAddress"].Equals("mailinator", StringComparison.OrdinalIgnoreCase) ||
                    testData["EmailAddress"].Equals("example", StringComparison.OrdinalIgnoreCase))
                {
                    testData["EmailAddress"] = "otto" + strRandom + "@" + testData["EmailAddress"] + ".com";
                }
                if (testData["EmailAddress"].Equals("qa+random", StringComparison.OrdinalIgnoreCase))
                {
                    testData["EmailAddress"] = "qa+" + strRandom + "@lendingtree.com";
                }
                Common.ReportEvent(Common.INFO, String.Format("The Email Address for this test is: {0}",
                    testData["EmailAddress"]));
            }

            //Password
            if (testData.ContainsKey("Password"))
            {
                if (testData["Password"].Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    testData["Password"] = "otto" + strRandom;
                }
                //06.04.2015 --MSweat--Adding a dictionary key check for MC Sign Up Automated Regression
                if (testData.ContainsKey("ConfirmPassword"))
                {
                    if (testData["ConfirmPassword"].Equals("default", StringComparison.OrdinalIgnoreCase))
                    {
                        testData["ConfirmPassword"] = "otto" + strRandom;
                    }
                }
                Common.ReportEvent(Common.INFO, String.Format("The Password for this test is: {0}", testData["Password"]));
            }

            //SSN
            //06.04.2015 -- MSweat -- Adding a dictionary key check for MC Sign Up Automated Regression
            if (testData.ContainsKey("BorrowerSsn1") && testData.ContainsKey("BorrowerSsn2") && testData.ContainsKey("BorrowerSsn3"))
            {
                if (testData["BorrowerSsn1"].Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    testData["BorrowerSsn1"] = "980";
                }
                if (testData["BorrowerSsn2"].Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    testData["BorrowerSsn2"] = strRandom.Substring(0, 2);
                }
                if (testData["BorrowerSsn3"].Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    testData["BorrowerSsn3"] = strRandom.Substring(2, 4);
                }
                Common.ReportEvent(Common.INFO, String.Format("The SSN for this test is: {0}-{1}-{2}", testData["BorrowerSsn1"],
                    testData["BorrowerSsn2"], testData["BorrowerSsn3"]));
            }

            //Generate a random Date of Birth (DOB)
            if (testData.ContainsKey("DateOfBirthMonth") && testData.ContainsKey("DateOfBirthDay") && testData.ContainsKey("DateOfBirthYear"))
            {
                if (testData["DateOfBirthMonth"].Equals("random", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime randomDOB;
                    DateTime start = DateTime.Today.AddYears(-70);
                    randomDOB = start.AddDays(Common.RandomNumber(1, 18250));
                    testData["DateOfBirthMonth"] = randomDOB.Month.ToString().PadLeft(2, '0');
                    testData["DateOfBirthDay"] = randomDOB.Day.ToString().PadLeft(2, '0');
                    testData["DateOfBirthYear"] = randomDOB.Year.ToString();
                }
                Common.ReportEvent(Common.INFO, String.Format("The DOB for this test (MM/DD/YYYY) is: {0}/{1}/{2}", testData["DateOfBirthMonth"],
                    testData["DateOfBirthDay"], testData["DateOfBirthYear"]));
            }

            //Browser
            if (testData["BrowserType"].Equals("random", StringComparison.OrdinalIgnoreCase))
            {
                testData["BrowserType"] = Common.RandomSelectBrowser();
            }
            //testData["BrowserType"] = "REMOTE";

            // The Auto QF has coborrower fields which can also be defaulted
            if (testData.ContainsKey("CoEmailAddress"))
            {
                if (testData["CoEmailAddress"].Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    testData["CoEmailAddress"] = "cobo" + strRandom + "@lendingtree.com";
                }

                if (testData["CoborrowerSsn1"].Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    testData["CoborrowerSsn1"] = "979";
                }

                if (testData["CoborrowerSsn2"].Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    testData["CoborrowerSsn2"] = strRandom.Substring(0, 2);
                }

                if (testData["CoborrowerSsn3"].Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    testData["CoborrowerSsn3"] = strRandom.Substring(2, 4);
                }
            }

            if (testData.ContainsKey("DateOfBirthYear") && testData.ContainsKey("DateOfBirthMonth") && testData.ContainsKey("DateOfBirthDay"))
            {
                
                if (testData["DateOfBirthYear"].Length > 0 && testData["DateOfBirthMonth"].Length > 0 && testData["DateOfBirthDay"].Length > 0)
                {
                    DateTime borrowerDOB = new DateTime(Int32.Parse(testData["DateOfBirthYear"]), Int32.Parse(testData["DateOfBirthMonth"]), Int32.Parse(testData["DateOfBirthDay"]));
                    DateTime now = DateTime.Now;
                    int age = now.Year - borrowerDOB.Year;
                    if (now < borrowerDOB.AddYears(age)) age--;

                    testData["Age"] = age.ToString();
                }
            }
        }

        public Boolean VerifytQFormRecord(string strQform)
        {
            Boolean blnFound = false;
            String strConnectionString;
            String strQuery;

            strQuery = "SELECT * FROM LendX..tQForm with(nolock) WHERE QFormUID = '" + strQform + "'";
            strConnectionString = Common.DBBuildLendXConnectionString(testData["TestEnvironment"]);

            // Sometimes there is a delay getting the record written to the DB...so loop and check for ~60 seconds
            int i = 1;
            do
            {
                Common.ReportEvent(Common.INFO, String.Format("Executing SQL to check for tQForm record, attempt #{0}", 
                    i.ToString()));

                if (Common.DBVerifyRecordWritten(strConnectionString, strQuery, 1))
                {
                    blnFound = true;
                    varQFormUIDTest = strQform;
                }
                else
                {
                    i++;
                    System.Threading.Thread.Sleep(5000);
                }
            } while ((!blnFound) && (i <= 12));

            if (blnFound)
            {
                Common.ReportEvent(Common.PASS, String.Format("Record written to DB for GUID: {0}", strQform));
            }
            else
            {
                Common.ReportEvent(Common.FAIL, String.Format("Record not found in DB for GUID: {0}", strQform));
            }

            return blnFound;
        }
    }
}
