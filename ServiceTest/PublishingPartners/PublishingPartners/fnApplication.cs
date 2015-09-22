using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceTestEngine;

namespace PublishingPartners
{
    class ApplicationClass : TestBase
    {
        LiveLoanQuotes responseClass = new LiveLoanQuotes();

        public bool CheckAssertions(int intLoanProgramId, string strLoanProductName, string strAmortizationType, int intLoanTermMonths, int intFixedRatePeriod, bool IsFHALoan, bool IsJumboLoan, bool IsVALoan, int intLoanAmount, string strLoanType,int intMIPayment, string strURLQualifier = "")
        {            
            bool blnFlag = false;
            int intOfferIndex = 100 ;
            int intFailStepCount = 0;

            httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Host"];
            httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Scheme"];
            httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")["Port"]);
            httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")["LiveLoanResponse"];
            httpContext.requestUrlQualifier = strURLQualifier;
            
            //Execute the rest service for the created request
            httpContext.executeRESTService();

            //Getting the response from the JSON
            responseClass = responseClass.FromJson<LiveLoanQuotes>(httpContext.returnedResponseBodyJSON);

            //Finding the first offer with the expected Loan Program Id 
            for (int i = 0; i <= (responseClass.Offers.Count) - 1; i++)
            {
                if ((responseClass.Offers[i].LoanProgramId).Equals(intLoanProgramId) && responseClass.Offers[i].LoanProductName.Equals(strLoanProductName))
                {
                    intOfferIndex = i;
                    break;
                }
            }

            if (intOfferIndex.Equals(100) && !responseClass.Offers.Count.Equals(100))
            {
                Reporter.ReportEvent("fail", "Cannot find an offer with the expected Loan Program Id: " + intLoanProgramId);
                return blnFlag;
            }

            //Check the Loan Product name
           
            if (!(responseClass.Offers[intOfferIndex].LoanProductName).Equals(strLoanProductName))
            {
                Reporter.ReportEvent("fail", "The Loan Product Name check failed. Expected value was: " + strLoanProductName + ". Fetched value is: " + responseClass.Offers[intOfferIndex].LoanProductName);
                intFailStepCount = 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The Loan Product Name check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].LoanProductName);
                blnFlag = true;
            }
               
            
            //Check the Amortization Type
            if (!(responseClass.Offers[intOfferIndex].AmortizationType).Equals(strAmortizationType))
            {
                Reporter.ReportEvent("fail", "The Amortization Type check failed. Expected value was: " + strAmortizationType + ". Fetched value is: " + responseClass.Offers[intOfferIndex].AmortizationType);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The Amortization Type check is successfull. The value fetched is: " + strAmortizationType);
                blnFlag = true;
            }
            //Check the Loan Term Months
            if (!(responseClass.Offers[intOfferIndex].LoanTermMonths).Equals(intLoanTermMonths))
            {
                Reporter.ReportEvent("fail", "The Loan Term Months check failed. Expected value was: " + intLoanTermMonths + ". Fetched value is: " + responseClass.Offers[intOfferIndex].LoanTermMonths);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The Loan Term Months check is successfull. The value fetched is: " + intLoanTermMonths);
                blnFlag = true;
            }
            //Check the Fixed Rate Period Months
            if (!(responseClass.Offers[intOfferIndex].FixedRatePeriodMonths).Equals(intFixedRatePeriod))
            {
                Reporter.ReportEvent("fail", "The Fixed Rate Period Months check failed. Expected value was: " + intFixedRatePeriod + ". Fetched value is: " + responseClass.Offers[intOfferIndex].FixedRatePeriodMonths);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The Fixed Rate Period Months check is successfull. The value fetched is: " + intFixedRatePeriod);
                blnFlag = true;
            }
            //Check whether the Loan is FHA Loan
            if (strLoanType.ToLower().Contains("fha"))
            {
                if (responseClass.Offers[intOfferIndex].IsFHALoan.Equals(false))
                {
                    Reporter.ReportEvent("fail", "The FHA Loan check failed. Expected value was: true." + " Fetched value is: " + responseClass.Offers[intOfferIndex].IsFHALoan);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The FHA Loan check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].IsFHALoan);
                    blnFlag = true;
                }
            }
            else
            {
                if (responseClass.Offers[intOfferIndex].IsFHALoan.Equals(true))
                {
                    Reporter.ReportEvent("fail", "The FHA Loan check failed. Expected value was: false." + " Fetched value is: " + responseClass.Offers[intOfferIndex].IsFHALoan);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The FHA Loan check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].IsFHALoan);
                    blnFlag = true;
                }
            }
            //Check whether the Loan is Jumbo Loan
            if (!(responseClass.Offers[intOfferIndex].IsJumboLoan).Equals(IsJumboLoan))
            {
                Reporter.ReportEvent("fail", "The Jumbo Loan check failed. Expected value was: " + IsJumboLoan + ". Fetched value is: " + responseClass.Offers[intOfferIndex].IsJumboLoan);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The Jumbo Loan check is successfull. The value fetched is: " + IsJumboLoan);
                blnFlag = true;
            }
            //Check whether the Loan is VA Loan
            if (!(responseClass.Offers[intOfferIndex].IsVALoan).Equals(IsVALoan))
            {
                Reporter.ReportEvent("fail", "The VA Loan check failed. Expected value was: " + IsVALoan + ". Fetched value is: " + responseClass.Offers[intOfferIndex].IsVALoan);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The VA Loan check is successfull. The value fetched is: " + IsVALoan);
                blnFlag = true;
            }
            //Check the Loan Amount
            if (!(responseClass.Offers[intOfferIndex].LoanAmount).Equals(intLoanAmount))
            {
                Reporter.ReportEvent("fail", "The Loan Amount check failed. Expected value was: " + intLoanAmount + ". Fetched value is: " + responseClass.Offers[intOfferIndex].LoanAmount);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The Loan Amount check is successfull. The value fetched is: " + intLoanAmount);
                blnFlag = true;
            }
            //Check the Rate Percentage
            if (!(responseClass.Offers[intOfferIndex].RatePercentage > 2 && (responseClass.Offers[intOfferIndex].RatePercentage < 9.999)))
            {
                Reporter.ReportEvent("fail", "The Rate Percentage check failed. Expected value was between 2 to 9.999." + " Fetched value is: " + responseClass.Offers[intOfferIndex].RatePercentage);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The Rate Percentage check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RatePercentage);
                blnFlag = true;
            }
            //Check the APR Percentage
            if (!(responseClass.Offers[intOfferIndex].APRPercentage > 2 && (responseClass.Offers[intOfferIndex].APRPercentage < 9.999)))
            {
                Reporter.ReportEvent("fail", "The APR Percentage check failed. Expected value was between 2 to 9.999." + " Fetched value is: " + responseClass.Offers[intOfferIndex].APRPercentage);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The APR Percentage check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].APRPercentage);
                blnFlag = true;
            }
            //Check the Points
            if (!(responseClass.Offers[intOfferIndex].Points > 0 && responseClass.Offers[intOfferIndex].Points < 2))
            {
                Reporter.ReportEvent("fail", "The Points check failed. Expected value was between 0 to 2." + " Fetched value is: " + responseClass.Offers[intOfferIndex].Points);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The Points check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].Points);
                blnFlag = true;
            }
            //Check the PIPayment
            switch(strLoanType.ToLower())
            {
                case "purchase conventional":
                    if (!(responseClass.Offers[intOfferIndex].PIPayment > 531 && responseClass.Offers[intOfferIndex].PIPayment < 1265))
                    {
                        Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 531 to 1265." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                        intFailStepCount = intFailStepCount + 1;
                    }
                    else
                    {
                        Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                        blnFlag = true;
                    }
                break;

                case "refinance conventional":
                if (!(responseClass.Offers[intOfferIndex].PIPayment > 1930 && responseClass.Offers[intOfferIndex].PIPayment < 3224))
                {
                    Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 531 to 1265." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    blnFlag = true;
                }
                break;

                case "purchase pmi":
                if (!(responseClass.Offers[intOfferIndex].PIPayment > 665 && responseClass.Offers[intOfferIndex].PIPayment < 1580))
                {
                    Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 531 to 1265." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    blnFlag = true;
                }
                break;

                case "refinance with second plus":
                if (!(responseClass.Offers[intOfferIndex].PIPayment > 1293 && responseClass.Offers[intOfferIndex].PIPayment < 3072))
                {
                    Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 1293 to 3072." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    blnFlag = true;
                }
                break;

                case "purchase jumbo":
                if (!(responseClass.Offers[intOfferIndex].PIPayment > 5058 && responseClass.Offers[intOfferIndex].PIPayment < 9650))
                {
                    Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 5058 to 9650." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    blnFlag = true;
                }
                break;

                case "refinance jumbo":
                if (!(responseClass.Offers[intOfferIndex].PIPayment > 4633 && responseClass.Offers[intOfferIndex].PIPayment < 7737))
                {
                    Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 4633 to 7737." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    blnFlag = true;
                }
                break;

                case "purchase fha":
                if (!(responseClass.Offers[intOfferIndex].PIPayment > 709 && responseClass.Offers[intOfferIndex].PIPayment < 1685))
                {
                    Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 709 to 1685." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    blnFlag = true;
                }
                break;

                case "refinance fha":
                if (!(responseClass.Offers[intOfferIndex].PIPayment > 1698 && responseClass.Offers[intOfferIndex].PIPayment < 2836))
                {
                    Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 1698 to 2836." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    blnFlag = true;
                }
                break;

                case "purchase va":
                if (!(responseClass.Offers[intOfferIndex].PIPayment > 1235 && responseClass.Offers[intOfferIndex].PIPayment < 2064))
                {
                    Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 1235 to 2064." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    blnFlag = true;
                }
                break;

                case "refinance va":
                if (!(responseClass.Offers[intOfferIndex].PIPayment > 831 && responseClass.Offers[intOfferIndex].PIPayment < 1975))
                {
                    Reporter.ReportEvent("fail", "The PIPayment check failed. Expected value was between 831 to 1975." + " Fetched value is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The PIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].PIPayment);
                    blnFlag = true;
                }
                break;

            }
            //Check the MIPayment
            switch(strLoanType.ToLower())
            {
                case "purchase pmi":
                    if (!(responseClass.Offers[intOfferIndex].MIPayment > 25 && responseClass.Offers[intOfferIndex].MIPayment < 250))
                    {
                        Reporter.ReportEvent("fail", "The MIPayment check failed. Expected value was between 25 to 250. Fetched value is: " + responseClass.Offers[intOfferIndex].MIPayment);
                        intFailStepCount = intFailStepCount + 1;
                    }
                    else
                    {
                        Reporter.ReportEvent("pass", "The MIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].MIPayment);
                        blnFlag = true;
                    }
                    break;

                case "refinance jumbo":
                    if (!(responseClass.Offers[intOfferIndex].MIPayment > 25 && responseClass.Offers[intOfferIndex].MIPayment < 999))
                    {
                        Reporter.ReportEvent("fail", "The MIPayment check failed. Expected value was between 25 to 999. Fetched value is: " + responseClass.Offers[intOfferIndex].MIPayment);
                        intFailStepCount = intFailStepCount + 1;
                    }
                    else
                    {
                        Reporter.ReportEvent("pass", "The MIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].MIPayment);
                        blnFlag = true;
                    }
                    break;

                case "purchase fha":
                    if (!(responseClass.Offers[intOfferIndex].MIPayment > 25 && responseClass.Offers[intOfferIndex].MIPayment < 250))
                    {
                        Reporter.ReportEvent("fail", "The MIPayment check failed. Expected value was between 25 to 250. Fetched value is: " + responseClass.Offers[intOfferIndex].MIPayment);
                        intFailStepCount = intFailStepCount + 1;
                    }
                    else
                    {
                        Reporter.ReportEvent("pass", "The MIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].MIPayment);
                        blnFlag = true;
                    }
                    break;

                case "refinance fha":
                    if (!(responseClass.Offers[intOfferIndex].MIPayment > 25 && responseClass.Offers[intOfferIndex].MIPayment < 250))
                    {
                        Reporter.ReportEvent("fail", "The MIPayment check failed. Expected value was between 25 to 250. Fetched value is: " + responseClass.Offers[intOfferIndex].MIPayment);
                        intFailStepCount = intFailStepCount + 1;
                    }
                    else
                    {
                        Reporter.ReportEvent("pass", "The MIPayment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].MIPayment);
                        blnFlag = true;
                    }
                    break;

                default:
                    if (!(responseClass.Offers[intOfferIndex].MIPayment).Equals(intMIPayment))
                    {
                        Reporter.ReportEvent("fail", "The MIPayment check failed. Expected value was: " + intMIPayment + ". Fetched value is: " + responseClass.Offers[intOfferIndex].MIPayment);
                        intFailStepCount = intFailStepCount + 1;
                    }
                    else
                    {
                        Reporter.ReportEvent("pass", "The MIPayment check is successfull. The value fetched is: " + intMIPayment);
                        blnFlag = true;
                    }
                 break;
            }
            //Check the Total Monthly Payment
            if (!(responseClass.Offers[intOfferIndex].TotalMonthlyPayment).Equals(responseClass.Offers[intOfferIndex].MIPayment + responseClass.Offers[intOfferIndex].PIPayment))
            {
                Reporter.ReportEvent("fail", "The Total Monthly Payment check failed. Expected value was: " + (responseClass.Offers[intOfferIndex].MIPayment + responseClass.Offers[intOfferIndex].PIPayment) + ". Fetched value is: " + responseClass.Offers[intOfferIndex].TotalMonthlyPayment);
                intFailStepCount = intFailStepCount + 1;
            }
            else
            {
                Reporter.ReportEvent("pass", "The Total Monthly Payment check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].TotalMonthlyPayment);
                blnFlag = true;
            }
            //Check the Relevance Sort Score
            switch(strLoanType.ToLower())
            {
                case "purchase conventional":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 900 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 1000))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 900 to 1000. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;

                case "refinance conventional":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 700 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 800))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 700 to 800. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;

                case "purchase pmi":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 800 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 900))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 800 to 900. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;

                case "refinance with second plus":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 300 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 400))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 300 to 400. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;

                case "purchase jumbo":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 600 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 700))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 600 to 700. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;

                case "refinance jumbo":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 700 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 800))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 700 to 800. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;

                case "purchase fha":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 900 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 1000))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 900 to 1000. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;

                case "refinance fha":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 700 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 800))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 700 to 800. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;

                case "purchase va":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 700 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 800))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 700 to 800. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;

                case "refinance va":
                if (!(responseClass.Offers[intOfferIndex].RelevanceSortScore > 900 && responseClass.Offers[intOfferIndex].RelevanceSortScore < 1000))
                {
                    Reporter.ReportEvent("fail", "The Relevance Sort Score check failed. Expected value was between 900 to 1000. Fetched value is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    intFailStepCount = intFailStepCount + 1;
                }
                else
                {
                    Reporter.ReportEvent("pass", "The Relevance Sort Score check is successfull. The value fetched is: " + responseClass.Offers[intOfferIndex].RelevanceSortScore);
                    blnFlag = true;
                }
                break;
            }
            //Check the final status
            if (intFailStepCount.Equals(0) && blnFlag.Equals(true))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool GetRequestURL(bool blnUseConfig, string strHostKey, string strSchemeKey, string strPortKey , string strPathKey, string strURLQualifierKey)
        {
            int intFailStepCount = 0;
            bool blnFlag = true;

            if (blnUseConfig)
            {
                httpContext.requestHost = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")[strHostKey];
                if (httpContext.requestHost.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = 1;
                }
                httpContext.requestScheme = configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")[strSchemeKey];
                if (httpContext.requestScheme.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = intFailStepCount + 1;
                }
                httpContext.requestPort = Convert.ToInt32(configuration.GetSection(environmentSettings).GetSection("LLAHostSettings")[strPortKey]);
                if (httpContext.requestPort.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = intFailStepCount + 1;
                }
                httpContext.requestPath = configuration.GetSection(environmentSettings).GetSection("LLARequestPath")[strPathKey];
                if (httpContext.requestPath.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = intFailStepCount + 1;
                }
                httpContext.requestUrlQualifier = configuration.GetSection(environmentSettings).GetSection("AppTestData").GetSection("LiveLoanData")[strURLQualifierKey];
                if (httpContext.requestUrlQualifier.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = intFailStepCount + 1;
                }
            }
            else
            {
                httpContext.requestHost = strHostKey;
                if (httpContext.requestHost.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = 1;
                }
                httpContext.requestScheme = strSchemeKey;
                if (httpContext.requestScheme.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = intFailStepCount + 1;
                }
                httpContext.requestPort = Convert.ToInt32(strPortKey);
                if (httpContext.requestPort.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = intFailStepCount + 1;
                }
                httpContext.requestPath = strPathKey;
                if (httpContext.requestPath.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = intFailStepCount + 1;
                }
                httpContext.requestUrlQualifier = strURLQualifierKey;
                if (httpContext.requestUrlQualifier.Equals(""))
                {
                    blnFlag = false;
                    intFailStepCount = intFailStepCount + 1;
                }
            }
            if (blnFlag = true && intFailStepCount == 0)
                return true;
            else
                return false;
        }
    }
}
