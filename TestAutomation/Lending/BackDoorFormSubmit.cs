using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TestAutomation.Lending
{
    class BackDoorFormSubmit
    {  
        public void FormSubmit()
        {
            JObject o1 = JObject.Parse(File.ReadAllText(@"..\\.\\..\\Lending\\formDataAsJson\\purchase.json"));

            Guid newQFGuid = Guid.NewGuid();
            Common.ReportEvent(Common.INFO, String.Format("New QFGUID for this test is {0}", newQFGuid.ToString()));

            string formattedTest = string.Format("abc", newQFGuid.ToString());
            Common.ReportEvent(Common.INFO, String.Format("New QF Post for this test is {0}", o1.ToString()));

            Post("https://offers.staging.lendingtree.com/formstore/submit-lead.ashx", o1.ToString());        
        }

        public void Post(string urlToPostTo,string valueToPost)
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
