using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using NUnit.Framework;

namespace TestAutomation.LendingTree.zzArchive
{
    [TestFixture]
    public class wizardTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        [Test]
        public void wizard_01_RefiPrimaryFirstAndSecond()
        {
            wizardPage wizard = new wizardPage(driver);

            // Fill out and submit a QF
            wizard.FillOutValidWizardQF(testData);

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(wizard.strQFormUID));

            wizard.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void wizard_02_RefiSecondaryFirstPlusCash()
        {
            wizardPage wizard = new wizardPage(driver);

            // Fill out and submit a QF
            wizard.FillOutValidWizardQF(testData);

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(wizard.strQFormUID));

            wizard.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void wizard_03_RefiInvestmentFirstOnlyNoCash()
        {
            wizardPage wizard = new wizardPage(driver);

            // Fill out and submit a QF
            wizard.FillOutValidWizardQF(testData);

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(wizard.strQFormUID));

            wizard.VerifyRedirectToMyLendingTree(testData);
        }

        //[Test]
        //public void wizard_04_RefiSsnValidationTest()
        //{
        //    wizardPage wizard = new wizardPage(driver);

        //    // Fill out and submit a QF
        //    wizard.FillOutValidWizardQF(testData);

        //    // Check for the QForm in the DB
        //    Assert.IsTrue(VerifytQFormRecord(wizard.strQFormUID));

        //    VerifyRedirectToMyLendingTree();
        //}

        [Test]
        public void wizard_05_PurchasePrimary()
        {
            wizardPage wizard = new wizardPage(driver);

            // Fill out and submit a QF
            wizard.FillOutValidWizardQF(testData);

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(wizard.strQFormUID));

            wizard.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void wizard_06_PurchaseSecondary()
        {
            wizardPage wizard = new wizardPage(driver);

            // Fill out and submit a QF
            wizard.FillOutValidWizardQF(testData);

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(wizard.strQFormUID));

            wizard.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void wizard_07_PurchaseInvestmentProperty()
        {
            wizardPage wizard = new wizardPage(driver);

            // Fill out and submit a QF
            wizard.FillOutValidWizardQF(testData);

            // Check for the QForm in the DB
            Assert.IsTrue(VerifytQFormRecord(wizard.strQFormUID));

            wizard.VerifyRedirectToMyLendingTree(testData);
        }


        //public void VerifyRedirectToMyLendingTree()
        //{
        //    // Check for redirect to My LendingTree
        //    // Validate Url Contains "/user/dashboard"
        //    Validation.StringContains("/user/dashboard", driver.Url);

        //    // Validate nav-main element contains className "logged-in"
        //    Validation.StringContains("logged-in", driver.FindElement(By.Id("nav-main")).GetAttribute("className"));

        //    // Validate page contains text "Hello " + <firstname> + " " + <lastname>
        //    // TODO

        //    // Validate title = "Welcome to My LendingTree, <firstname>"
        //    Validation.StringContains("Welcome to My LendingTree, " + testData["BorrowerFirstName"],
        //        driver.FindElements(By.ClassName("title"))[0].GetAttribute("textContent"));
        //}
    }
}
