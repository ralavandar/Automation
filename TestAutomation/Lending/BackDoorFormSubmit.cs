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

        public static Dictionary<string,string> FormSubmit(Dictionary<string, string> testData)
        {
            JObject newRequest = new JObject(new JProperty("new"));

            switch (testData["LoanType"].ToUpper())
            {
                case "PURCHASE":  //MortgagePurchase Field Set for JSON post
                    newRequest = JObject.Parse(File.ReadAllText(@"..\\.\\..\\Lending\\formDataAsJson\\purchase.json"));
                    newRequest["FieldValues"]["property-type"] = testData["PropertyType"];
                    newRequest["FieldValues"]["property-use"] = testData["PropertyUse"];
                    newRequest["FieldValues"]["current-realestate-agent"] = testData["CurrentRealEstateAgentStatus"];
                    newRequest["FieldValues"]["purchase-price"] = testData["PurchasePrice"];
                    newRequest["FieldValues"]["est-mortgage-balance"] = testData["FirstMortgageBalance"];
                    newRequest["FieldValues"]["down-payment-amt"] = testData["PurchaseDownPayment"];
                    newRequest["FieldValues"]["new-home"] = testData["FoundNewHomeYesNo"];
                    newRequest["FieldValues"]["property-state"] = testData["PropertyState"];
                    newRequest["FieldValues"]["property-state-name"] = testData["PropertyStateName"];
                    newRequest["FieldValues"]["property-city"] = testData["PropertyCity"];
                    newRequest["FieldValues"]["inline_realtor_optin"] = testData["RealtorOptin"];
                    newRequest["FieldValues"]["homeservice-optin"] = testData["HomeServicesOptin"];
                    newRequest["FieldValues"]["bankruptcy-or-foreclosure"] = testData["BankruptcyOrForeclosure"];
                    newRequest["FieldValues"]["declared-bankruptcy"] = testData["BankruptcyDeclaration"];
                    newRequest["FieldValues"]["bankruptcy-discharged"] = testData["BankruptcyDischarge"];
                    newRequest["FieldValues"]["foreclosure-text"] = testData["ForeclosureStatus"];
                    newRequest["FieldValues"]["is-veteran"] = testData["VeteranStatus"];

                    break;
                case "REFINANCE":
                    newRequest = JObject.Parse(File.ReadAllText(@"..\\.\\..\\Lending\\formDataAsJson\\refinance.json"));
                    break;
                default:    //Default, unknown loan type provided.
                    Common.ReportEvent(Common.FAIL, String.Format("INVALID PRODUCT TYPE PROVIDED {0}", testData["LoanType"]));
                    Assert.Fail();
                    break;
            }

            Guid newQFGuid = Guid.NewGuid();
            //Set Consumer Common Data
            newRequest["FieldValues"]["first-name"] = testData["BorrowerFirstName"];
            newRequest["FieldValues"]["last-name"] = testData["BorrowerLastName"];
            newRequest["FieldValues"]["email"] = testData["EmailAddress"];
            newRequest["FieldValues"]["password"] = testData["Password"];
            newRequest["FieldValues"]["ssn"] = testData["BorrowerSsn1"] + "-" + testData["BorrowerSsn2"] + "-" + testData["BorrowerSsn3"];
            newRequest["FieldValues"]["dob"] = testData["DateOfBirthMonth"] + "/" + testData["DateOfBirthDay"] + "/" + testData["DateOfBirthYear"];
            newRequest["FieldValues"]["home-phone"] = testData["BorrowerPhone"];            
            newRequest["FieldValues"]["city"] = testData["BorrowerCity"];
            newRequest["FieldValues"]["state"] = testData["BorrowerState"];
            newRequest["FieldValues"]["state-name"] = testData["BorrowerStateName"];
            newRequest["FieldValues"]["stated-credit-history"] = testData["CreditHistory"];
            newRequest["FieldValues"]["zip-code"] = testData["BorrowerZipCode"];
            //Set QF GUID Parameters
            newRequest["QFormUID"] = newQFGuid.ToString();
            newRequest["id"]= newQFGuid.ToString();
            newRequest["FieldValues"]["guid"]= newQFGuid.ToString();
            newRequest["FieldValues"]["targuscheck"]["QFormUID"] = newQFGuid.ToString();
            
            
            Common.ReportEvent(Common.INFO, String.Format("Email for this test is {0}", newRequest["FieldValues"]["email"].ToString()));
            
            Common.ReportEvent(Common.INFO, String.Format("New QFGUID for this test is {0}", newQFGuid.ToString()));
            testData.Add("guid", newQFGuid.ToString());
            //string formattedTest = string.Format("abc", newQFGuid.ToString());
            Common.ReportEvent(Common.INFO, String.Format("New QF Post for this test is {0}", newRequest.ToString()));

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
            Post(strUrl, newRequest.ToString());
            return testData;        
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
