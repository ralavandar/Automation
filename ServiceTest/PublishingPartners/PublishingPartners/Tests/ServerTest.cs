using NUnit.Framework;
using ServiceTestEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ConfigurationSettings;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace PublishingPartners
{
    [TestFixture]
    class ServerTest : ApplicationClass
    {
        //Initialize the constructors for the response class LiveLoanQuotes
        LiveLoanQuotes responseClass = new LiveLoanQuotes();

        [Test]
        public void PurchaseConventional()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCLoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCLoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCLoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCFixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCLoanAmount"]);
                int intMIPayment = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCMIPayment"]);

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PurchaseConventional"];
                
                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);
                //for (int j = 1; j < 30; j++)
                //{
                //    if (!responseClass.Offers.Count.Equals(0))
                //    {
                //        break;
                //    }
                //    System.Threading.Thread.Sleep(1000);
                //    //Execute the rest service for the created request
                //    httpContext.executeRESTService();
                //    responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                //}
                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod, false, false, false, intLoanAmount,"purchase conventional" ,intMIPayment, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }
            
            //Report 
            Reporter.ReportFinalResults();
        }

        [Test]
        public void RefinanceConventional()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RCLoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RCLoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RCAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RCLoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RCFixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RCLoanAmount"]);
                int intMIPayment = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RCMIPayment"]);

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RefinanceConventional"];

                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);
                
                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod,false, false, false, intLoanAmount,"refinance conventional" ,intMIPayment, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }

            //Report 
            Reporter.ReportFinalResults();
        }

        [Test]
        public void PurchaseConventionalWithPMI()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCPMILoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCPMILoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCPMIAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCPMILoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCPMIFixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PCPMILoanAmount"]);

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PurchaseConventionalPMI"];

                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);

                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod, false, false, false, intLoanAmount, "purchase pmi", 0, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }

            //Report 
            Reporter.ReportFinalResults();
        }

        [Test]
        public void RefinanceSecondPlusCashout()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RSPLoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RSPLoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RSPAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RSPLoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RSPixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RSPLoanAmount"]);
                int intMIPayment = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RSPMIPayment"]);

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RefinanceSecondPlusCashout"];

                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);

                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod, false, false, false, intLoanAmount, "refinance with second plus", intMIPayment, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
               
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }
            //Report 
            Reporter.ReportFinalResults();
         }

        [Test]
        public void PurchaseJumbo()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PJLoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PJLoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PJAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PJLoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PJFixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PJLoanAmount"]);
                int intMIPayment = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PJMIPayment"]);

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PurchaseJumbo"];

                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);

                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod, false, true, false, intLoanAmount, "purchase jumbo", intMIPayment, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }

            //Report 
            Reporter.ReportFinalResults();
        }

        [Test]
        public void RefinanceJumbo()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RJLoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RJLoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RJAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RJLoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RJFixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RJLoanAmount"]);

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RefinanceJumbo"];

                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);

                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod, false, true, false, intLoanAmount, "refinance jumbo", 0, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }

            //Report 
            Reporter.ReportFinalResults();
        }

        [Test]
        public void PurchaseFHAEligible()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PFHALoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PFHALoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PFHAAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PFHALoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PFHAFixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PFHALoanAmount"]);
                

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PurchaseFHAEligible"];

                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);

                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod, true, false, false, intLoanAmount, "purchase fha", 0, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }

            //Report 
            Reporter.ReportFinalResults();
        }

        [Test]
        public void RefinanceFHAEligible()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RFHALoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RFHALoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RFHAAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RFHALoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RFHAFixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RFHALoanAmount"]);

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RefinanceFHAEligible"];

                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);

                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod, true, false, false, intLoanAmount, "refinance fha", 0, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }

            //Report 
            Reporter.ReportFinalResults();
        }

        [Test]
        public void PurchaseVAEligible()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PVALoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PVALoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PVAAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PVALoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PVAFixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PVALoanAmount"]);
                int intMIPayment = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PVAMIPayment"]);

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["PurchaseVAEligible"];

                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);

                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod, false, false, true, intLoanAmount, "purchase va", intMIPayment, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }

            //Report 
            Reporter.ReportFinalResults();
        }

        [Test]
        public void RefinanceVAEligible()
        {
            try
            {
                //Initialize the test data
                string strQFormID = "";
                bool blnStepRC = false;
                int intLoanProgramId = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RVALoanProgramId"]);
                string strLoanProductName = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RVALoanProductName"];
                string strAmortizationType = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RVAAmortizationType"];
                int intLoanTermMonths = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RVALoanTermMonths"]);
                int intFixedRatePeriod = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RVAFixedRatePeriod"]);
                int intLoanAmount = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RVALoanAmount"]);

                //Initialize the parameters for creating the desired request
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")["RefinanceVAEligible"];

                //Initialize the HTTP Method to be used
                httpContext.requestMethod = HTTPContext.HTTPMethod.GET;
                //Execute the rest service for the created request
                httpContext.executeRESTService();

                //Get the response from the JSON
                responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);
                System.Threading.Thread.Sleep(1000);

                if (!responseClass.QuotesId.Equals(null))
                {
                    strQFormID = responseClass.QuotesId;
                    Reporter.ReportEvent("pass", "Successfully fetched the QFormID : " + strQFormID);
                }
                else
                {
                    Reporter.ReportEvent("fail", "No QFormID is displayed in the response. Please check the Request URL.");
                }

                //Check all the expected assertions in the response
                blnStepRC = CheckAssertions(intLoanProgramId, strLoanProductName, strAmortizationType, intLoanTermMonths, intFixedRatePeriod, false, false, true, intLoanAmount, "refinance va", 0, strQFormID);
                if (blnStepRC)
                {
                    Reporter.ReportEvent("pass", "Successfully validated the required assertions from the JSON response.");
                }
                else
                {
                    Reporter.ReportEvent("fail", "Assertions Check failed for the scenario.");
                }
            }
            catch (Exception assertionException)
            {
                Reporter.ReportEvent("fail", "The test case failed due to the error: " + assertionException);
            }

            //Report 
            Reporter.ReportFinalResults();
        }
    }
}
