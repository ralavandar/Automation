using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation
{
    class Validation
    {

        public static Boolean AreEqual(Object expected, Object actual, String comparisonName)
        {
            try
            {
                Assert.AreEqual(expected, actual);
                Common.ReportEvent(Common.PASS, String.Format
                    ("The {0} comparison passed.  Expected: \"{1}\".  Actual: \"{2}\".", comparisonName, expected, actual));
                return true;

            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("The {0} comparison failed.  Expected: \"{1}\".  Actual: \"{2}\".", comparisonName, expected, actual));
                return false;

            }
        }


        public static Boolean StringCompare(String strExpected, String strActual)
        {
            try
            {
                Assert.AreEqual(strExpected, strActual);
                Common.ReportEvent(Common.PASS, String.Format
                    ("The string comparison passed.  Expected: \"{0}\".  Actual: \"{1}\".", strExpected, strActual));

                return true;

            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("The string comparison failed.  Expected: \"{0}\".  Actual: \"{1}\".", strExpected, strActual));
                return false;


            }
        }

        
        /// <summary>
        /// Performs a case-insensitive string comparison using an NUnit StringAssert method.  But the difference is
        /// this method will not exit on Fail - the exception is handled and the Failure is logged.
        /// </summary>
        /// <param name="strExpected">The expected value for the test condition</param>
        /// <param name="strActual">The actual value from the application under test</param>
        /// <returns>True if the strings match; False if the strings do not match</returns>
        public static Boolean StringCompareIgnoreCase(String strExpected, String strActual)
        {
            try
            {
                StringAssert.AreEqualIgnoringCase(strExpected, strActual);
                Common.ReportEvent(Common.PASS, String.Format
                    ("The string comparison passed.  Expected: \"{0}\".  Actual: \"{1}\".", strExpected, strActual));
                return true;
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("The string comparison failed.  Expected: \"{0}\".  Actual: \"{1}\".", strExpected, strActual));
                // Maybe capture screenshot or page innerText
                return false;
            }
        }


        /// <summary>
        /// Compares a string to an expected regular expression pattern using NUnit's StringAssert.IsMatch method.  
        /// But the difference is this method will not exit on Fail.  The exception is handled and the Failure is logged.
        /// </summary>
        /// <param name="strPattern">A regular expression pattern representing the expected value
        /// for the test condition</param>
        /// <param name="strActual">The actual value from the application under test</param>
        /// <returns>True if the regular expression pattern is matched; otherwise False</returns>
        public static Boolean StringCompareRegEx(String strPattern, String strActual)
        {
            try
            {
                StringAssert.IsMatch(strPattern, strActual);
                Common.ReportEvent(Common.PASS, String.Format
                    ("The string comparison passed.  Expected: \"{0}\".  Actual: \"{1}\".", strPattern, strActual));
                return true;
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("The string comparison failed.  Expected: \"{0}\".  Actual: \"{1}\".", strPattern, strActual));
                return false;
            }
        }


        /// <summary>
        /// Checks to see if a Test String from the AUT contains an expected string using NUnit's 
        /// StringAssert.Contains method.  But the difference is this method will not exit on 
        /// Fail - the exception is handled and the Failure is logged.
        /// </summary>
        /// <param name="strExpected">An expected value to search for</param>
        /// <param name="strActual">The Test String being searched.</param>
        /// <returns>True if the string is found; otherwise False</returns>
        public static Boolean StringContains(string strExpected, string strTestString)
        {
            try
            {
                StringAssert.Contains(strExpected, strTestString);
                Common.ReportEvent(Common.PASS, String.Format
                    ("The TestString contains the expected value.  Expected: \"{0}\".  TestString: \"{1}\".",
                        strExpected, strTestString));
                return true;
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("The TestString does not contain the expected value.  Expected: \"{0}\".  TestString: \"{1}\".",
                        strExpected, strTestString));
                // Maybe capture screenshot or page innerText
                return false;
            }
        }

        /// <summary>
        /// Validates that the selected item in the dropdown matches the expected string value
        /// </summary>
        /// <param name="objSelect">a SelectElement object representing the dropdown list</param>
        /// <param name="strExpected">the expected selected value</param>
        /// <returns></returns>
        public static Boolean VerifySelectedOption(IWebDriver driver, string strId, string strExpected)
        {
            SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(strId)));
            String strActual = objSelect.SelectedOption.Text;

            try
            {
                //StringAssert.AreEqualIgnoringCase(strExpected, strActual);
                Assert.AreEqual(strExpected, strActual);
                Common.ReportEvent(Common.PASS, String.Format
                    ("The selected item for '{0}' matched the expected value.  Expected: \"{1}\".  Actual: \"{2}\".", 
                        strId, strExpected, strActual));
                return true;
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("The selected item for '{0}' DID NOT MATCH the expected value.  Expected: \"{1}\".  "
                    + "Actual: \"{2}\".", strId, strExpected, strActual));
                return false;
            }
        }


        public static Boolean VerifyDropdownListMinValue(IWebDriver driver, String strId, String strExpected)
        {
            String strActual = "";

            SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(strId)));

            // if the 1st option's value is blank/empty "", then this is the default option, and its the 2nd option 
            // we want to validate 
            if (objSelect.Options.First().GetAttribute("value").Length.Equals(0))
            {
                strActual = objSelect.Options[1].Text;
            }
            else
            {
                strActual = objSelect.Options.First().Text;
            }

            try
            {
                //StringAssert.IsMatch(strExpected, strActual);
                Assert.AreEqual(strExpected, strActual);
                Common.ReportEvent(Common.PASS, String.Format
                    ("The {0} min dropdown option matched the expected value.  Expected: \"{1}\".  Actual: \"{2}\".",
                        strId, strExpected, strActual));
                return true;
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("The {0} min dropdown option DID NOT MATCH the expected value.  Expected: \"{1}\".  "
                    + "Actual: \"{2}\".", strId, strExpected, strActual));
                return false;
            }
        }


        public static Boolean VerifyDropdownListMaxValue(IWebDriver driver, String strId, String strExpected)
        {
            SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(strId)));
            String strActual = objSelect.Options.Last().Text;

            try
            {
                //StringAssert.IsMatch(strExpected, strActual);
                Assert.AreEqual(strExpected, strActual);
                Common.ReportEvent(Common.PASS, String.Format
                    ("The {0} max dropdown option matched the expected value.  Expected: \"{1}\".  Actual: \"{2}\".",
                        strId, strExpected, strActual));
                return true;
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("The {0} max dropdown option DID NOT MATCH the expected value.  Expected: \"{1}\".  "
                    + "Actual: \"{2}\".", strId, strExpected, strActual));
                return false;
            }
        }


        public static Boolean VerifyDropdownListAtPosition(IWebDriver driver, String strId, Int32 index, String strExpected)
        {
            SelectElement objSelect = new SelectElement(driver.FindElement(By.Id(strId)));
            String strActual = objSelect.Options[index].Text;

            try
            {
                //StringAssert.IsMatch(strExpected, strActual);
                Assert.AreEqual(strExpected, strActual);
                Common.ReportEvent(Common.PASS, String.Format
                    ("The {0} dropdown option at index {1} matched the expected value.  Expected: \"{2}\".  "
                    + "Actual: \"{3}\".", strId, index, strExpected, strActual));
                return true;
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("The {0} dropdown option at index {1} DID NOT MATCH the expected value.  Expected: \"{2}\".  "
                    + "Actual: \"{3}\".", strId, index, strExpected, strActual));
                return false;
            }
        }

        public static Boolean IsTrue(Boolean booleanCondition)
        {
            try
            {
                Assert.IsTrue(booleanCondition);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Boolean IsTrue(Boolean booleanCondition, string assertString)
        {
            try
            {
                Assert.IsTrue(booleanCondition);
                Common.ReportEvent(Common.PASS, String.Format
                    ("Assertion \"{0}\" passed.  Expected true, got true", assertString));
                return true;
            }
            catch
            {
                Common.ReportEvent(Common.FAIL, String.Format
                    ("Assertion \"{0}\" failed.  Expected true, got false", assertString));
                return false;
            }
        }
    }
}