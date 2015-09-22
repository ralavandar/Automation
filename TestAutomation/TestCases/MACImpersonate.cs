using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using TestAutomationSP;
using OpenQA.Selenium.Firefox;

namespace TestAutomation.LendingTree.tl
{
    [TestFixture]
    public class MACImpersonate : SeleniumTestBase
    {
       public IWebDriver driver;
       public string prodbaseUrl = "https://lendingtree.com/";
       public string baseUrl = "https://staging.lendingtree.com/";
       public string preprodbaseUrl = "https://int.lendingtree.com/"; //preprod
        public string devbaseUrl = "dev.lendingtree.com/";
       //TODO These variables below will come from the data dictionary
        public string varLender1 = "WCS"; //lender name from first referral.
       // public string varTestCaseNameTest = "MyLendingTreeTest_01_HC";
        public string varHipster = "Boom";
        public string varPurchasesPrice = "$425,000";
        public string varLTV = "80%";
        public string varDownPayment = "$85,000";
        public string varLoanAmount = "$340,000";
        public string varRequestDate = "10/31/2013";
        public int varTestCaseIdTest = 1;
        //public string varTestEnvironmentTest = "STAGE";
        //TODO uncomment this out as a work around to hard code this while getting real value passed
        public string varQFormUIDTest2 = "72F4E183-BD45-4067-9489-0FA841C1A90C";
       public string varEmailAddressTest = "MACTest1@asdf.com";
       public string varProdEmailAddressTest = "brianpbeam@hotmail.com";
               public string varDevEmailAddressTest = "devmac@asdf.com";
        public string varPasswordTest = "Test1234";  //testData["password"];
        public string varURLParam1 = "account/logon?returnurl=%2foffers%2fdashboard";
        public string varURLParam2 = "account/impersonate";
        public string varURLParam3 = "user/request-";
        //public string varConsumerUID = "F75B10F0-92DE-4638-87FF-84D668AE59DC";
        //public string varQformId = "46396002";
        //public string varOfferCount = "4 loan offers";

//TODO offer details will come from a modification of the following query, this one returns Offer count.
//Use
//Lendx
//Go
//Declare @QformId as varchar(20)
//Declare @QformName  as varchar(20)
//Declare @OfferCount as int
//Set @QformId = '46396002'
//select  count(rpo.ReferralProductID) as count
//from tqform q with (nolock)
//join treferral r with (nolock) on  q.qformid = r.qformid
//join tpartner p with (nolock) on p.partnerid = r.lenderid
//left join treferralproduct rp with (nolock) on rp.referralid = r.referralid
//left join treferralproductoffer rpo with (nolock) on rpo.ReferralProductID = rp.ReferralProductID
//where q.qformid = @QformId
//group by q.qformname
      

        [SetUp]
        public void SetupTest()
        {
            //ReturnOfferData.returnOfferData(varQformId);

        }
        
        [TearDown]

        public void TeardownTest()
        {
            driver.Quit();
                     
            // Writes to Audit log, some values are sent to stored procedure inside ReportFinalResultsAuditLog based on PASS, FAIL, INCONCLUSIVE, PASS with warnings
           // Common.ReportFinalResultsAuditLog(varTestCaseIdTest, varTestCaseNameTest, varTestEnvironmentTest, varQFormUIDTest2, varEmailAddressTest, varPasswordTest);
            //Common.ReportFinalResults();
        }

        [Test]
        public void MAC_Impersonate_WESTSVARefOnly()
        {
            string varConsumerUID = "10AD0862-7607-4D0A-A424-E64911ADED64";
            string varQformId = "46400356";
            //string varOfferCount = "4 loan offers";

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varEmailAddressTest);
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains("Bummer", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);

        }

        [Test]
        public void MAC_Impersonate_WESTSMI()
        {
            string varConsumerUID = "09330563-3060-4623-9A05-F7E1D6E072F6";
            string varQformId = "46400250";
            //string varOfferCount = "4 loan offers";

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varEmailAddressTest);
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains("Bummer", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);

        }

        [Test]
        public void MAC_Impersonate_WESTSAZ()
        {
            string varConsumerUID = "AA2AA88F-5EE8-40C6-BBEB-C2294128ABC4";
            string varQformId = "46400244";
            //string varOfferCount = "4 loan offers";

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varEmailAddressTest);
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains("Bummer", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);

        }

        [Test]
        public void MAC_Impersonate_WESTSTX()
        {
            string varConsumerUID = "E00E80B4-A550-4132-A57E-A8CFEDF3592A";
            string varQformId = "46400246";
            //string varOfferCount = "4 loan offers";

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varEmailAddressTest);
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains("Bummer", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);

        }

        [Test]
        public void MAC_Impersonate_WESTSVA()
        {
            string varConsumerUID = "22E73344-CB69-4179-B5AC-601C9252857E";
            string varQformId = "46400242";
            //string varOfferCount = "4 loan offers";

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varEmailAddressTest);
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains("Bummer", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);

        }


        [Test]
        public void MAC_Impersonate_WESTSFL()
        {
            string varConsumerUID = "CBACA078-CAAD-47A3-B4E1-C441B2518680";
            string varQformId = "46400245";
            //string varOfferCount = "4 loan offers";

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varEmailAddressTest);
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains("Bummer", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);

        }
        [Test]
        public void MAC_Impersonate_ProdNoref()
        {
            string varConsumerUID = "CA62086F-3E89-46D2-91F1-E873585A5CBE";

            string varQformId = "49455424";
            //string varOfferCount = "4 loan offers";

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(prodbaseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varProdEmailAddressTest);
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("brianpbeam@hotmail.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(prodbaseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(prodbaseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains("Bummer", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);

            //TODO add this query to find qfs with no referrals
            //use lendx
            //go
            //SELECT a.description, * FROM Filter.vFactQFormExchange with (nolock) JOIN dbo.tExchangeLeadTypeLookup a on ExchangeLeadTypeLookupID = a.ID
            //WHERE HomeLoanProductTypeLookupID=1 AND MatchCountAll<1 
            //ORDER BY QFormID DESC

        }
        [Test]
        public void MAC_Impersonate_QAnonref()
        {
            string varConsumerUID = "CEC827C0-D24A-4E52-9CE1-2F01DB09E161";
            string varQformId = "46396678";
            //string varOfferCount = "4 loan offers";

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varEmailAddressTest);
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains("Bummer", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);
            
        }

        [Test]
        public void MAC_Impersonate_QA_Test1()
        {
            string varConsumerUID = "4CE75B19-7476-4B23-A177-9CC0626370F7";
            string varQformId = "46398404";
            //string varOfferCount = "4 loan offers";

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varEmailAddressTest);
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains(varHipster, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);

        }
               
        //[Test]
        //public void MAC_Impersonate_DevTest1()
        //{
        //    string varConsumerUID = "15C0BE2F-3234-4BCB-B886-2181802F9402";

        //    string varQformId = "47156544";
        //    //string varOfferCount = "4 loan offers";

        //    driver = new FirefoxDriver();
        //    driver.Navigate().GoToUrl(devbaseUrl + varURLParam1);

        //    driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
        //    System.Threading.Thread.Sleep(500);
        //    // Enters good email as username        
        //    driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
        //    System.Threading.Thread.Sleep(500);
        //    driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys(varDevEmailAddressTest);
        //    System.Threading.Thread.Sleep(500);
        //    Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
        //    System.Threading.Thread.Sleep(500);

        //    // Enters password
        //    driver.FindElement(By.XPath("//*[@id='Password']")).Click();
        //    System.Threading.Thread.Sleep(500);
        //    driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
        //    Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

        //    driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
        //    System.Threading.Thread.Sleep(500);

        //    driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
        //    System.Threading.Thread.Sleep(4000);

        //    // Go to impersonate
        //    driver.Navigate().GoToUrl(devbaseUrl + varURLParam2);

        //    driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
        //    System.Threading.Thread.Sleep(500);
        //    // Enters good email as username        
        //    driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
        //    System.Threading.Thread.Sleep(500);
        //    driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

        //    driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
        //    System.Threading.Thread.Sleep(10000);

        //    driver.Navigate().GoToUrl(devbaseUrl + varURLParam3 + varQformId);
        //    System.Threading.Thread.Sleep(15000);
        //    ////hipster referral message
        //    Validation.StringContains(varHipster, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

        //    //System.Threading.Thread.Sleep(6000);
        //    //Validate offer summary text
        //    //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
        //    System.Threading.Thread.Sleep(100);



        //}
        [Test]
        public void MAC_Impersonate_AutoTest1()
        {
            string varConsumerUID = "303DFAFB-06DA-4B42-AA0B-7E9CB681F3DF";
            string varQformId = "46398580";
            

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys("MACTest1@asdf.com");
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains(varHipster, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains('Boom', driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);



        }
        [Test]
        public void MAC_Impersonate_PersonalTest1()
        {
            string varConsumerUID = "F372006F-DB07-4035-8453-58DEEC1F39A2";
            string varQformId = "46398201";
            

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys("MACTest1@asdf.com");
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
           //Validation.StringContains("Cong", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(100);
            
        }
       
        [Test]
        public void MAC_Impersonate_Prototype()
        {
            string varConsumerUID = "3107DC97-6D82-4FCA-9A08-E91EC422AA63";
            string varQformId = "46398371";
            

            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl + varURLParam1);

            driver.FindElement(By.XPath("//*[@id='UserName']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='UserName']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='UserName']")).SendKeys("MACTest1@asdf.com");
            System.Threading.Thread.Sleep(500);
            Validation.StringContains("MACTest1@asdf.com", driver.FindElement(By.XPath("//*[@id='UserName']")).GetAttribute("value"));
            System.Threading.Thread.Sleep(500);

            // Enters password
            driver.FindElement(By.XPath("//*[@id='Password']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='Password']")).SendKeys("Test1234");
            Validation.StringContains("Test1234", driver.FindElement(By.XPath("//*[@id='Password']")).GetAttribute("value"));

            driver.FindElement(By.XPath("//*[@id='RememberMe']")).Click();
            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("//*[@id='layout_body']/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(4000);

            // Go to impersonate
            driver.Navigate().GoToUrl(baseUrl + varURLParam2);

            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Clear();
            System.Threading.Thread.Sleep(500);
            // Enters good email as username        
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id='ConsumerUid']")).SendKeys(varConsumerUID);

            driver.FindElement(By.XPath("//*[@id='page-content']/div[2]/div/div/div/form/fieldset/button")).Click();
            System.Threading.Thread.Sleep(10000);

            driver.Navigate().GoToUrl(baseUrl + varURLParam3 + varQformId);
            System.Threading.Thread.Sleep(15000);
            ////hipster referral message
            Validation.StringContains("Boom", driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dt")).GetAttribute("innerHTML"));

            //System.Threading.Thread.Sleep(6000);
            //Validate offer summary text
            //ValidationAudit.StringContains(varOfferCount, driver.FindElement(By.XPath("//*[@id='layout_body']/div[1]/table/tbody/tr/td[1]/dl/dd/p")).GetAttribute("innerHTML"));
            System.Threading.Thread.Sleep(6000);
                    }
          }
}