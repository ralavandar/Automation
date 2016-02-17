using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.IO;

namespace TestAutomation.Lending
{
    class BackDoorFormSubmit
    {

        public static void FormSubmit(Dictionary<string, string> testData)
        {
            JObject o1 = new JObject(new JProperty("new"));

            switch (testData["LoanType"].ToUpper())
            {
                case "PURCHASE":
                    o1 = JObject.Parse(File.ReadAllText(@"..\\.\\..\\Lending\\formDataAsJson\\purchase.json"));
                    break;
                case "REFINANCE":
                    o1 = JObject.Parse(File.ReadAllText(@"..\\.\\..\\Lending\\formDataAsJson\\refinance.json"));
                    break;
                default:    //Default, unknown loan type provided.
                    Common.ReportEvent(Common.FAIL, String.Format("INVALID PRODUCT TYPE PROVIDED {0}", testData["LoanType"]));
                    Assert.Fail();
                    break;
            }

            Guid newQFGuid = Guid.NewGuid();
            o1["FieldValues"]["email"] = testData["EmailAddress"];
            o1["FieldValues"]["password"] = testData["Password"];
            o1["FieldValues"]["ssn"] = testData["BorrowerSsn1"] + "-" + testData["BorrowerSsn2"] + "-" + testData["BorrowerSsn3"];
            o1["FieldValues"]["dob"] = testData["DateOfBirthMonth"] + "/" + testData["DateOfBirthDay"] + "/" + testData["DateOfBirthYear"];
            o1["QFormUID"] = newQFGuid.ToString();
            o1["id"]= newQFGuid.ToString();
            o1["FieldValues"]["guid"]= newQFGuid.ToString();
            o1["FieldValues"]["targuscheck"]["QFormUID"] = newQFGuid.ToString();
            
            Common.ReportEvent(Common.INFO, String.Format("Email for this test is {0}", o1["FieldValues"]["email"].ToString()));
            
            Common.ReportEvent(Common.INFO, String.Format("New QFGUID for this test is {0}", newQFGuid.ToString()));

            //string formattedTest = string.Format("abc", newQFGuid.ToString());
            Common.ReportEvent(Common.INFO, String.Format("New QF Post for this test is {0}", o1.ToString()));

            string strUrl;

            switch (testData["TestEnvironment"].ToUpper())
            {
                case "PROD":
                    strUrl = "https://offers.lendingtree.com/formstore/submit-lead.ashx";
                    break;
                case "QA":
                case "STAGING":
                case "STAGE":
                    strUrl = "https://offers.staging.lendingtree.com/formstore/submit-lead.ashx";
                    break;
                case "DEV":
                    strUrl = "https://offers.dev.lendingtree.com/formstore/submit-lead.ashx";
                    break;
                default:    //QA
                    strUrl = "https://offers.staging.lendingtree.com/formstore/submit-lead.ashx";
                    break;
            }
            Post(strUrl, o1.ToString());        
        }

        public static void Post(string urlToPostTo,string valueToPost)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(urlToPostTo);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";


            string result = "";
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = client.UploadString(urlToPostTo, "POST", valueToPost);
                Common.ReportEvent(Common.INFO, String.Format("Post Result String = {0}", result));
            }

        }
    }
}
