using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using TestAutomation.LendingTree.tl;
using System.Collections;
using System.Web.Helpers;

namespace TestAutomation.LendingTree.zSandbox
{
    [TestFixture]
    public class prepopTests : SeleniumTestBase
    {
        public IWebDriver driver;

        private zzArchive.morttreePage morttree;

        private static readonly List<String> variations = new List<String> { "mort-tree", "gs-mort-tree", "mortgage", "gs-mortgage", "lt-m" };

        private static readonly Random random = new Random();

        private static readonly dynamic data = Json.Decode(@"
        {
            ""purchase-price"": [
                { input: ""560"", expected: ""5000"" },
                { input: ""5001"", expected: ""5000"" },
                { input: ""59674"", expected: ""55000"" },
                { input: ""535300"", expected: ""525000"" },
                { input: ""2000000"", expected: ""1750000"" },
                { input: ""2000001"", expected: ""2000001"" },
                { input: ""100000000"", expected: ""2000001"" },
                { input: ""12345678901234567890"", expected: ""2000001"" },
                { input: ""200-000"", expected: ""5000"" },
                { input: ""2000f"", expected: ""5000"" },
                { input: ""200d"", expected: ""5000"" },
                { input: ""-1"", expected: """" },
                { input: ""0"", expected: """" },
                { input: ""$200,000"", expected: """" },
                { input: ""abcdef123"", expected: """" }
            ],
            ""down-payment-amt"": [
                { input: ""0.00"", expected: "".05"" },
                { input: ""0.049"", expected: "".05"" },
                { input: ""0.05"", expected: "".05"" },
                { input: ""0.0501"", expected: "".10"" },
                { input: ""0.17"", expected: "".2"" },
                { input: "".35"", expected: "".40"" },
                { input: ""0.90"", expected: "".90"" },
                { input: ""0.901"", expected: "".91"" },
                { input: ""1"", expected: "".91"" },
                { input: ""-0.05"", expected: """" },
                { input: ""abcd123"", expected: """" },
                { input: ""100000000"", expected: """" },
                { input: ""12345678901234567890"", expected: """" }
            ]
        }
        ");

        [TestFixtureSetUp]
        public void SetupTest()
        {
            Common.InitializeTestResults();
            driver = StartBrowser(Common.RandomSelectBrowser());
            morttree = new zzArchive.morttreePage(driver, null);
        }

        [Test, TestCaseSource(typeof(ParamDataFactory), "TestCases")]
        public void random_prepop_query_string_test(dynamic randomParamData)
        {
            morttree.NavigateToFossaForm("STAGING", "tl.aspx", GetRandomVariation(), String.Empty, BuildRandomQueryString(randomParamData));

            AssertParams(randomParamData);
        }

        [TestFixtureTearDown]
        public void TeardownTest()
        {
            driver.Quit();
            Common.ReportFinalResults();
        }

        private class ParamDataFactory
        {
            private static readonly List<String> paramNames = new List<String> { "purchase-price", "down-payment-amt" };

            public static IEnumerable TestCases
            {
                get
                {
                    for (int i = 0; i < 50; i++)
                        yield return GetRandomParamData();
                }
            }

            private static List<dynamic> GetRandomParamData()
            {
                List<dynamic> randomParamData = new List<dynamic>(paramNames.Count);
                foreach (string paramName in paramNames)
                {
                    dynamic randomTest = GetRandomParamPair(data[paramName]);

                    // Add paramName so we don't lose it
                    randomTest.paramName = paramName;
                    randomParamData.Add(randomTest);
                };

                return randomParamData;
            }

            private static dynamic GetRandomParamPair(dynamic data)
            {
                return data[random.Next(0, data.Length)];
            }
        }

        private static string BuildRandomQueryString(List<dynamic> paramData)
        {
            string randomQueryString = String.Empty;
            foreach (dynamic paramPair in paramData)
            {
                randomQueryString += String.Format("&{0}={1}", paramPair.paramName, paramPair.input);
            }

            return randomQueryString;
        }

        private void AssertParams(List<dynamic> paramData)
        {
            foreach (dynamic paramPair in paramData)
            {
                string name = paramPair.paramName;
                Assert.AreEqual(paramPair.expected, driver.FindElement(By.Id(name)).GetAttribute("value"), ParamPairToString(paramPair));
            }
        }

        private string ParamPairToString(dynamic paramPair)
        {
            return "expected: " + paramPair.expected + ", value: " + paramPair.value;
        }

        private dynamic GetRandomVariation()
        {
            return variations[random.Next(variations.Count)];
        }
    }
}
