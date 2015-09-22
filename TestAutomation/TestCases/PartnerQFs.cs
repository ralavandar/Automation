using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;

namespace TestAutomation.LendingTree.zSandbox
{
    [TestFixture]
    public class morttreeTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_Mortgage";
        private tl.morttreePage morttree;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            morttree = new tl.morttreePage(driver, testData);
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        private void FinishTest()
        {
            // Handle the cross-sells
            // morttree.BypassTlCrossSells();

            // Check for the QForm in the DB
            Validation.IsTrue(VerifytQFormRecord(morttree.strQFormUID));

            // Verify redirect to My LendingTree
            morttree.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void morttree_LF_P_Aru_80LTV()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_P_Aru_85LTV()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_P_Aru_90LTV_Townhome()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_P_Aru_Condo()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_P_Aru_Military()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_R_Aru_Primary()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_R_Aru_PrimaryFirstAndSecond()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_R_Aru_PrimaryFirstPlusCash()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_R_Aru_Secondary()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_R_Aru_Investment()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_R_Aru_MilitaryWithVA()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void morttree_LF_R_Aru_MilitaryWithoutVA()
        {
            // Fill out and submit a QF
            morttree.FillOutValidQF();

            FinishTest();
        }
    }

    [TestFixture]
    public class prequalTests : SeleniumTestBase
    {
        public IWebDriver driver;
        private const String strTableName = "tTestData_prequal";
        private tla.prequalPage prequal;

        [SetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            GetTestData(strTableName, TestContext.CurrentContext.Test.Name);
            InitializeTestData();
            driver = StartBrowser(testData["BrowserType"]);
            prequal = new tla.prequalPage(driver, testData);
        }

        [TearDown]
        // This is the last step in every test, even if the test fails TearDown will still run. This step needs QFormUid
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
            /*
            int varTestCaseIdTest = int.Parse(testData["TestCaseID"]);
            string varTestCaseNameTest = testData["TestCaseName"];
            string varTestEnvironmentTest = testData["TestEnvironment"];
            string varEmailAddressTest = testData["EmailAddress"];
            string varPasswordTest = testData["Password"];
            Common.ReportFinalResultsAuditLog(varTestCaseIdTest, varTestCaseNameTest, varTestEnvironmentTest, varQFormUIDTest, varEmailAddressTest, varPasswordTest);
            varQFormUIDTest = null;
            */
        }

        private void FinishTest()
        {
            Validation.IsTrue(VerifytQFormRecord(prequal.strQFormUID));

            prequal.VerifyRedirectToMyLendingTree(testData);
        }

        [Test]
        public void prequal_Aru_80LTV()
        {
            prequal.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void prequal_Aru_85LTV()
        {
            prequal.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void prequal_Aru_90LTV_Townhome()
        {
            prequal.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void prequal_Aru_Condo()
        {
            prequal.FillOutValidQF();

            FinishTest();
        }

        [Test]
        public void prequal_Aru_Military()
        {
            prequal.FillOutValidQF();

            FinishTest();
        }
    }
}
