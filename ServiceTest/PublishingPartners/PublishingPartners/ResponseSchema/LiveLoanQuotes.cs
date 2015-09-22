using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ConfigurationSettings;
using ServiceTestEngine;
//using PublishingPartners;

namespace PublishingPartners
{
    
    [Serializable]
    class LiveLoanQuotes : DataConverter, IDataConverter
    {
        public string QuotesId { get; set; }
        public bool OffersPending { get; set; }
        public double AverageAPR { get; set; }
        public int OfferSetVersion { get; set; }
        public List<Offer> Offers { get; set; }
        public List<Lender> Lenders { get; set; }
        public List<Link3> Links { get; set; }
         
    }

    [Serializable]
    public class Link
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }

     [Serializable]
    public class Offer
    {
        public string OfferId { get; set; }
        public string ShortOfferId { get; set; }
        public int LenderId { get; set; }
        public double APRPercentage { get; set; }
        public string AmortizationType { get; set; }
        public int FixedRatePeriodMonths { get; set; }
        public bool HasPrepaymentPenalty { get; set; }
        public bool IsFHALoan { get; set; }
        public bool IsJumboLoan { get; set; }
        public bool IsVALoan { get; set; }
        public double LoanAmount { get; set; }
        public string LoanProductName { get; set; }
        public int LoanProgramId { get; set; }
        public int LoanTermMonths { get; set; }
        public int LockTermDays { get; set; }
        public double MIPayment { get; set; }
        public double PIPayment { get; set; }
        public double Points { get; set; }
        public int LoanProgramPercentileRank { get; set; }
        public double RatePercentage { get; set; }
        public double RelevanceSortScore { get; set; }
        public bool ShowTelephoneNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public double TotalCredits { get; set; }
        public double TotalFees { get; set; }
        public double TotalMonthlyPayment { get; set; }
        public List<Link> Links { get; set; }
    }

     [Serializable]
    public class Link2
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }

     [Serializable]
    public class Lender
    {
        public int LenderId { get; set; }
        public int LendXLenderId { get; set; }
        public double AverageCustomerServiceRating { get; set; }
        public double AverageFeesAndClosingCostRating { get; set; }
        public double AverageOverallRating { get; set; }
        public double AverageRateRating { get; set; }
        public double AverageResponsivenessRating { get; set; }
        public string ContentKeyForReviews { get; set; }
        public bool IsCPCLeadEnabled { get; set; }
        public bool IsDataLeadEnabled { get; set; }
        public bool IsTelephoneLeadEnabled { get; set; }
        public string LogoUrl { get; set; }
        public string NMLSID { get; set; }
        public string Name { get; set; }
        public int TotalRatingsAndReviews { get; set; }
        public List<Link2> Links { get; set; }
    }

     [Serializable]
    public class Link3
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }

}
