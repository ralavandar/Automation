using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TestAutomation
{
    class Common
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
            testResults[Common.PASS] = 0;
            testResults[Common.FAIL] = 0;
            testResults[Common.ERROR] = 0;
            testResults[Common.WARNING] = 0;
        }

        public static void ReportEvent(String strEvent, String strMsg)
        {
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
                ReportEvent(FAIL, String.Format("The test case FAILED.  Please investigate the cause.  PASS count = {0}.  "
                    + "FAIL count = {1}.  ERROR count = {2}.  WARNING count = {3}.", testResults[PASS],
                    testResults[FAIL], testResults[ERROR], testResults[WARNING]));
                // TODO: Try forcing an Assert expception, so the test case status in NUnit will turn red
                NUnit.Framework.Assert.Fail("The test case Failed!  Please check the log for details.");
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
        }


        public static Int32 RandomNumber(Int32 min, Int32 max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        public static String RandomString(Int32 len)
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            var output = "";

            for (int i = 0; i < len; i++)
            {
                var randomNumber = random.Next(0, 25);
                output += chars[randomNumber];
            }

            return output;
        }

        public static String RandomSelectBrowser()
        {
            // This is actually a 'weighted' random selection.  50% IE, 25% firefox, 25% chrome
            // Update: 5/21/2012 - taking Chrome out of the mix since it exhibits random failures on the test servers :(
            // Update: 7/10/2012 - putting Chrome back in the mix.  Updated WebDriver and ChromeDriver, and appears to be stable :)
            // Update: 8/14/2012 - taking Chrome back out of the mix since it exhibits random failures on the test servers :(
            Random random = new Random();
            Int32 intRandomNum = random.Next(1, 100);

            if (intRandomNum <= 50)         // 01-50 = IE
            {
                return "IE";
            }
            else if (intRandomNum <= 100)    // 51-75 = FIREFOX
            {
                return "FIREFOX";
            }
            else return "CHROME";           // 76-100 = CHROME
        }


        public static Boolean DBVerifyRecordWritten(String strDBConn, String strQuery, Int32 intExpectedRowCount)
        {
            Boolean blnFound = false;
            SqlConnection objConn = new SqlConnection(strDBConn);

            //Establish connection to DB
            try
            {
                objConn.Open();
            }
            catch (Exception e)
            {
                ReportEvent(ERROR, String.Format("Unable to open a connection to the database "
                    + "with the provided connection string.  Check the connection string and try again.  "
                    + "Connection String: {0}", strDBConn));
                ReportEvent(ERROR, e.ToString());
                return blnFound;
            }

            //Execute Query
            SqlCommand objSqlCommand = new SqlCommand(strQuery, objConn);
            SqlDataReader objReader = null;
            DataTable objDataTable = new DataTable();

            ReportEvent(INFO, String.Format("About to execute SQL: {0}", strQuery));
            try
            {
                objReader = objSqlCommand.ExecuteReader();
                objDataTable.Load(objReader);
            }
            catch (Exception e)
            {
                ReportEvent(ERROR, String.Format("There was a problem executing the provided SQL.  "
                    + "Check the SQL and try again.  SQL: {0}", strQuery));
                ReportEvent(ERROR, e.ToString());
                return blnFound;
            }

            //Evaluate results - should get expected row count.
            if (objDataTable.Rows.Count.Equals(1))
            {
                blnFound = true;
            }
            else
            {
                ReportEvent(INFO, String.Format("The number of rows returned by the query ({0}) did not match"
                    + " the expected row count ({1}).", objDataTable.Rows.Count, intExpectedRowCount));
            }

            // Close the connection
            try
            {
                objConn.Close();
            }
            catch (Exception e)
            {
                ReportEvent(ERROR, e.ToString());
            }

            return blnFound;  
        }


        public static DataTable DBGetDataTable(String strDBConn, String strQuery)
        {
            DataTable objDataTable = new DataTable();

            //Establish connection to DB
            SqlConnection objConn = new SqlConnection(strDBConn);
            try
            {
                objConn.Open();
            }
            catch (Exception e)
            {
                ReportEvent(ERROR, String.Format("Unable to open a connection to the database "
                    + "with the provided connection string.  Check the connection string and try again.  "
                    + "Connection String: {0}", strDBConn));
                ReportEvent(ERROR, e.ToString());
                return objDataTable;
            }

            // Execute Query
            SqlCommand objSqlCommand = new SqlCommand(strQuery, objConn);
            SqlDataReader objReader = null;

            ReportEvent(INFO, String.Format("About to execute SQL: {0}", strQuery));
            try
            {
                objReader = objSqlCommand.ExecuteReader();
                objDataTable.Load(objReader);
            }
            catch (Exception e)
            {
                ReportEvent(ERROR, String.Format("There was a problem executing the provided SQL.  "
                    + "Check the SQL and try again.  SQL: {0}", strQuery));
                ReportEvent(ERROR, e.ToString());
                return objDataTable;
            }

            // Close the connection
            try
            {
                objConn.Close();
            }
            catch (Exception e)
            {
                ReportEvent(ERROR, e.ToString());
            }

            return objDataTable;
        }

        public static String DBBuildLendXConnectionString(String environment)
        {
            String connectionString = "";

            switch (environment.ToUpper())
            {
                case "PROD":
                    connectionString = "Data Source=lendxprod\\lxprod01;Initial Catalog=LendX;Integrated Security=True";
                    break;
                case "QA":
                case "STAGE":
                case "STAGING":
                    connectionString = "Data Source=LendxQA\\LXprod01;Initial Catalog=LendX;Integrated Security=True";
                    //connectionString = ConfigurationManager.ConnectionStrings["TestAutomation.Properties.Settings.DB_LX_QA"].ConnectionString;
                    break;
                case "DEV":
                    connectionString = "Data Source=lendxdev\\lxprod01;Initial Catalog=LendX;Integrated Security=True";
                    break;
                default:    //QA
                    connectionString = "Data Source=LendxQA\\LXprod01;Initial Catalog=LendX;Integrated Security=True";
                    break;
            }

            return connectionString;
        }
    }
}
