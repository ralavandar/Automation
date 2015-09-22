using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace TestAutomation
{
    public class DataSheet
    {
        private const String strConnectionString = 
            "Data Source=CCAVMBZTSB01;Initial Catalog=AutomatedTesting;Integrated Security=True";
            //"Data Source=JCLARK2-W2K8R2;Initial Catalog=AutomatedTesting;Integrated Security=True";
        private Dictionary<string, string> testDataDictionary = new Dictionary<string, string>();
        
        // Constructor
        public DataSheet(String strTableName, String strTestCaseName)
        {
            // Go connect to the database and populate the test data dictionary
            var strQuery = "SELECT * FROM AutomatedTesting.dbo." + strTableName + " with(nolock) WHERE TestCaseName = '" 
                + strTestCaseName + "'";

            // Establish connection to DB
            SqlConnection objConn = new SqlConnection(strConnectionString);
            try
            {
                objConn.Open();
            }
            catch (Exception e)
            {
                Common.ReportEvent(Common.ERROR, String.Format("Unable to open a connection to the database "
                    + "with the provided connection string.  Check the connection string and try again.  "
                    + "Connection String: {0}", strConnectionString));
                Common.ReportEvent(Common.ERROR, e.ToString());
                return;
            }

            //Execute Query
            SqlCommand objSqlCommand = new SqlCommand(strQuery, objConn);
            SqlDataReader objReader = null;
            DataTable objDataTable = new DataTable();
            try
            {
                objReader = objSqlCommand.ExecuteReader();
                objDataTable.Load(objReader);
            }
            catch (Exception e)
            {
                Common.ReportEvent(Common.ERROR, String.Format("There was a problem executing the provided SQL.  "
                    + "Check the SQL and try again.  SQL: {0}", strQuery));
                Common.ReportEvent(Common.ERROR, e.ToString());
                return;
            }

            if (objDataTable.Rows.Count == 1)
            {
                // Found a matching strTestCaseName - good!
                DataRowCollection objCollection = objDataTable.Rows;
                DataRow objRow = objDataTable.Rows[0];
                Common.ReportEvent(Common.INFO, String.Format("TestCaseName = '{0}'. Test case data is from table '{1}'.",
                    strTestCaseName, strTableName));

                // Populate the test data dictionary
                foreach (DataColumn column in objDataTable.Columns)
                {
                    testDataDictionary.Add(column.ColumnName, objRow[column].ToString());
                }
            }
            else if (objDataTable.Rows.Count == 0)
            {
                // Did not find a matching strTestCaseName - not good.
                Common.ReportEvent(Common.ERROR, String.Format("Test case data was NOT FOUND in table '{0}', " 
                    + "TestCaseName = '{1}'.  Check the test case data and try again.", strTableName, strTestCaseName));
            }
            else if (objDataTable.Rows.Count > 1)
            {
                // Found more than 1 strTestCaseName record - not good.
                Common.ReportEvent(Common.ERROR, String.Format("There are {0} rows in {1} with the TestCaseName: '{2}'.  "
                    + "There should only be 1 matching row.  Please check the test case data and try again.", 
                    objDataTable.Rows.Count.ToString(), strTableName, strTestCaseName));
            }
            else
            {
                // We should never hit this condition, but just in case...
                Common.ReportEvent(Common.ERROR, String.Format("There are {0} rows in {1} with the TestCaseName: '{2}'.  "
                    + "There should only be 1 matching row.  Please check the test case data and try again.",
                    objDataTable.Rows.Count.ToString(), strTableName, strTestCaseName));
            }

            // Close the connection
            try
            {
                objConn.Close();
            }
            catch (Exception e)
            {
                Common.ReportEvent(Common.ERROR, e.ToString());
            }

        }

        public Dictionary<string, string> GetDataDictionary()
        {
            return this.testDataDictionary;
        }
    }
}
