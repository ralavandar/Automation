using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace TestAutomation.Lending
{
    class BackDoorFormSubmit
    {  
        string test1 = @"{{
    ""FieldValues"": {{
        ""agreed-terms-flag"": ""Y"",
        ""IsComplete"": ""Y"",
        ""form-complete-date"": ""10/21/2015 09:11:04 AM"",
        ""environment"": ""stage"",
        ""client-ip-address"": ""12.152.10.63"",
        ""web-server-ip-address"": ""IP-AC110468"",
        ""webpage-name"": ""https://offers.staging.lendingtree.com/tlm.aspx?tid=m2&vid=0-0-4-1-1-0-0-0-1-0-0-0-0-0-0-0-2-0-0-0-0-0-0-0-0-0-0-1-1-0"",
        ""dns-host-name"": ""offers.staging.lendingtree.com"",
        ""http-user-agent"": ""Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36"",
        ""form-start-date"": ""10/21/2015 09:07:43 AM"",
        ""shortform-version"": ""0-0-4-1-1-0-0-0-1-0-0-0-0-0-0-0-2-0-0-0-0-0-0-0-0-0-0-1-1-0"",
        ""telNum"": ""888-246-4181"",
        ""template"": ""m2"",
        ""template-id"": ""m2"",
        ""marketplace-uid"": ""10000"",
        ""esource-id"": ""6131666"",
        ""icode"": ""30"",
        ""exchange-lead-type"": ""SHORTFORM"",
        ""form-name"": ""MortgageQuickMatchForm"",
        ""qform-type"": ""QFORMCONSUMERGETSMART"",
        ""qform-sub-type"": ""QUICKMATCH"",
        ""email-optin"": ""Y"",
        ""source-application"": ""mortgage"",
        ""version"": ""1.0"",
        ""promoNum"": ""00001"",
        ""product-type"": ""MORTGAGE"",
        ""product"": ""PURCHASE"",
        ""guid"": ""{0}"",
        ""cchannel"": ""seo"",
        ""tid"": ""m2"",
        ""vid"": ""0-0-4-1-1-0-0-0-1-0-0-0-0-0-0-0-2-0-0-0-0-0-0-0-0-0-0-1-1-0"",
        ""trackingStepGroups"": ""ig-ssn"",
        ""monthlyPayment"": null,
        ""totalMortgageBalance"": 160000,
        ""ltv"": 0.8,
        ""refiLtvWithoutCashOut"": 0.8,
        ""secondMortgageBalanceAvailable"": true,
        ""downPaymentAmtDollars"": 45000,
        ""homeloan-product-type"": ""PURCHASE"",
        ""optimized-path"": 1,
        ""LastUpdatedField"": ""monthlyPayment"",
        ""external-leadid-token"": ""1DE28F4C-565C-46B0-A6CA-E250E9659A0F"",
        ""allLoanTypes"": ""HOME"",
        ""token"": ""97AB32FB756A49B8D6471440042EABB656C5E666229BBBAA347F8BE30529EE10"",
        ""propertyCityStateTypeAhead"": {{
            ""city"": ""Charlotte"",
            ""displayText"": ""Charlotte, NC"",
            ""state"": ""NC"",
            ""stateName"": ""North Carolina""
        }},
        ""dob"": ""05/09/1984"",
        ""city"": ""Pineville"",
        ""cityTypeAhead"": {{
            ""name"": ""Pineville"",
            ""value"": ""Pineville""
        }},
        ""email"": ""msqa102815.216@lendingtree.com"",
        ""password"": ""dru1dess"",
        ""ssn"": ""980-10-2801"",
        ""bank"": [
            ""0""
        ],
        ""banks"": ""0"",
        ""targuscheck"": {{
            ""QFormUID"": ""{0}"",
            ""Result"": ""PASS"",
            ""Grade"": ""D"",
            ""Score"": 26,
            ""IsSuccess"": true,
            ""ErrorDesc"": """",
            ""IsNewTargusCheck"": true
        }},
        ""property-type"": ""SINGLEFAMDET"",
        ""property-use"": ""OWNEROCCUPIED"",
        ""current-realestate-agent"": ""N"",
        ""estproperty-value"": 200000,
        ""purchase-price"": 225000,
        ""est-mortgage-balance"": 160000,
        ""down-payment-amt"": 0.2,
        ""new-home"": ""NO"",
        ""have-multiple-mortgages"": ""NO"",
        ""property-state"": ""NC"",
        ""property-state-name"": ""North Carolina"",
        ""property-city"": ""Charlotte"",
        ""inline_realtor_optin"": ""NO"",
        ""cash-out"": 0,
        ""homeservice-optin"": ""NO"",
        ""home-service-schedule"": """",
        ""bankruptcy-or-foreclosure"": ""NO"",
        ""declared-bankruptcy"": ""NO"",
        ""bankruptcy-discharged"": ""NEVER"",
        ""foreclosure-text"": ""NEVER"",
        ""stated-credit-history"": ""EXCELLENT"",
        ""is-veteran"": ""NO"",
        ""first-name"": ""Maryanne"",
        ""last-name"": ""Sweat"",
        ""zip-code"": ""28134"",
        ""street1"": ""1110 Main St"",
        ""state"": ""NC"",
        ""state-name"": ""NORTH CAROLINA"",
        ""home-phone"": ""7045411824"",
        ""current-monthly-payment"": ""0"",
        ""2nd-mortgage-monthly-payment"": ""0"",
        ""hear-about-us"": """"
    }},
    ""Product"": ""PURCHASE"",
    ""QFormUID"": ""{0}"",
    ""id"": ""{0}"",
    ""Token"": ""97AB32FB756A49B8D6471440042EABB656C5E666229BBBAA347F8BE30529EE10"",
    ""status"": ""submit""
     }}";

        public void FormSubmit()
        {
            Guid newQFGuid = Guid.NewGuid();
            Common.ReportEvent(Common.INFO, String.Format("New QFGUID for this test is {0}", newQFGuid.ToString()));

            string formattedTest = string.Format(test1, newQFGuid.ToString());
            Common.ReportEvent(Common.INFO, String.Format("New QF Post for this test is {0}", formattedTest));

            Post("https://offers.staging.lendingtree.com/formstore/submit-lead.ashx", formattedTest);

                       
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
