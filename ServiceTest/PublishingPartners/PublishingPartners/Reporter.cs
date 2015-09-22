using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishingPartners
{
    class Reporter
    {
        public const String PASS = "PASS";
        public const String FAIL = "FAIL";
        public const String ERROR = "ERROR";
        public const String WARNING = "WARNING";
        public const String INFO = "INFO";
        public static Dictionary<String, Int32> testResults
            = new Dictionary<string, int>
            {
                {PASS, 0},
                {FAIL, 0},
                {ERROR, 0},
                {WARNING, 0}
            };

        public static void InitializeTestResults()
        {
            testResults[PASS] = 0;
            testResults[FAIL] = 0;
            testResults[ERROR] = 0;
            testResults[WARNING] = 0;
        }

        public static void ReportEvent(String strEvent, String strMsg)
        {
            strEvent = strEvent.ToUpper();
            String strPattern = @"M/d/yyyy hh:mm:ss tt";
            Console.WriteLine(String.Format("{0} : {1} : {2}", DateTime.Now.ToString(strPattern), strEvent, strMsg));

            // Increment the counts of PASS, FAIL, ERROR and WARNING

            switch (strEvent)
            {

                case PASS:
                    testResults[PASS]++;

                    break;
                case FAIL:
                    testResults[FAIL]++;

                    break;
                case ERROR:
                    testResults[ERROR]++;

                    break;
                case WARNING:
                    testResults[WARNING]++;

                    break;
            }
        }

        public static void ReportFinalResults()
        {
            // Evaluate the counts of individual PASS, FAIL, ERROR and WARNING events - and determine a final PASS/FAIL
            // for the entire test case.
            // RULES:
            // If there are any FAILs, then the overall test is a fail
            // If there are any ERRORs, then the overall test is a fail
            // If there are any WARNINGs, but no FAILs or ERRORs, then the overall test is a PASS

            if ((testResults[FAIL] > 0) || (testResults[ERROR] > 0))
            {
                // Report the overall test case status as FAIL
                ReportEvent(FAIL, String.Format("The test case FAILED.  PASS count = {0}.  "
                    + "FAIL count = {1}.  ERROR count = {2}.  WARNING count = {3}.", testResults[PASS],
                    testResults[FAIL], testResults[ERROR], testResults[WARNING]));
                //Setting the test parameters to 0 
                InitializeTestResults();
                // TODO: Try forcing an Assert expception, so the test case status in NUnit will turn red
                NUnit.Framework.Assert.Fail("The test case execution Failed!");
            }
            else if (testResults[WARNING] > 0)
            {
                // Report the test case as a PASS with warnings
                ReportEvent(WARNING, String.Format("The test case PASSED, but with warnings.  Please investigate the "
                    + "warnings.  PASS count = {0}.  FAIL count = {1}.  ERROR count = {2}.  WARNING count = {3}.",
                    testResults[PASS], testResults[FAIL], testResults[ERROR], testResults[WARNING]));
            }
            else if ((testResults[PASS].Equals(0)) && (testResults[FAIL].Equals(0)) && (testResults[ERROR].Equals(0)))
            {
                // Report the test case as an ERROR
                ReportEvent(ERROR, String.Format("The test case results are inconclusive.  There were no PASS, FAIL, "
                    + "or ERROR events logged.  Please check the NUnit Console/Logs for exceptions. "
                    + "PASS count = {0}.  FAIL count = {1}.  ERROR count = {2}.  WARNING count = {3}.",
                    testResults[PASS], testResults[FAIL], testResults[ERROR], testResults[WARNING]));
            }
            else
            {
                // Report the test case as a PASS with no warnings
                ReportEvent(PASS, String.Format("The test case PASSED with no warnings!  "
                    + "PASS count = {0}.  FAIL count = {1}.  ERROR count = {2}.  WARNING count = {3}.",
                    testResults[PASS], testResults[FAIL], testResults[ERROR], testResults[WARNING]));
            }

            //Setting the test parameters to 0 
            InitializeTestResults();
        }
    }
}
