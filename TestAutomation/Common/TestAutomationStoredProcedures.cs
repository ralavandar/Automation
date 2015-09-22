using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

//TODO add Try Catch to each proc to handle exeptions
//TODO figure out why TestAutomation namespace would not work for this 

namespace TestAutomationSP
{
    public static class InsertTestResultToLog
    {

        public static void addTestResult(int varTestCaseId, string varTestCaseName, string varTestEnvironment, string varLastQformUid, string varEmailAddress, string varPassword, string varCategory, string varSeverity, string varResult, string varMessageDetail)
        {

            SqlConnection sqlConnStr =
            new SqlConnection("Data Source=CCAVMBZTSB01;Initial Catalog=AutomatedTesting;Integrated Security=True");

            SqlCommand sqlCmd = sqlConnStr.CreateCommand();

            sqlCmd.CommandText = "EXEC   pInsertTestAutomationResult " + varTestCaseId + ",'" + varTestCaseName + "','" + varTestEnvironment + "','" + varLastQformUid + "','" + varEmailAddress + "','" + varPassword + "','" + varCategory + "','" + varSeverity + "','" + varResult + "','" + varMessageDetail + "'";

            sqlConnStr.Open();
            sqlCmd.ExecuteNonQuery();
            sqlConnStr.Close();
        }
    }

    public static class AddManualReferral
    {

        public static void addManualReferral(int varipQformID, int varipFilterID)
        {

            SqlConnection sqlConnStr =
            new SqlConnection("Data Source=LendxQA\\LXprod01;Initial Catalog=Lendx;Integrated Security=True");
            //TODO pull connection string to cofig 

            SqlCommand sqlCmd = sqlConnStr.CreateCommand();

            sqlCmd.CommandText = "EXEC   [Config].[paddmanualreferral] " + varipQformID + "," + varipFilterID;

            sqlConnStr.Open();
            sqlCmd.ExecuteNonQuery();
            sqlConnStr.Close();
        }
    }


}
