using System;
using System.Collections.Generic;

namespace AccountAggregator.ModelLayer
{
    public class RequestDownloadStatement
    {
        public string purpose { get; set; }
        public string ver { get; set; }
        public string timestamp { get; set; }
        public string txnid { get; set; }
        public string clienttxnid { get; set; }
        public string customerId { get; set; }
        public string fipid { get; set; }
        public string fipname { get; set; }
        public string pan { get; set; }
        public ClsPdfDetail pdfDetail { get; set; }
    }

    public class ClsPdfDetail
    {
        public string pdfbase64 { get; set; }
        public string pdfbinary { get; set; }
        public string jsonData { get; set; }
        //public ClsJsonData jsonData { get; set; }
        public string xmlData { get; set; }
        public string consentid { get; set; }
        public string maskedAccountNumber { get; set; }
        public string accRefNumber { get; set; }
    }

    public class ClsJsonData
    {
        public ClsAccountDetails Account { get; set; }
    }

    public class ClsAccountDetails
    {
        public ClsTransactions Transactions { get; set; }
        public string xmlns { get; set; }
        public string linkedAccRef { get; set; }
        public string maskedAccNumber { get; set; }
        public string version { get; set; }
        public string type { get; set; }
        public ClsProfileDetails Profile { get; set; }
        public ClsSummaryDetails Summary { get; set; }
    }

    public class ClsTransactions
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<ClsTransactionDetails> Transaction { get; set; }
    }

    public class ClsTransactionDetails
    {
        public double? amount { get; set; }
        public double? currentBalance { get; set; }
        public string mode { get; set; }
        public string narration { get; set; }
        public string reference { get; set; }
        public DateTime transactionTimestamp { get; set; }
        public string txnId { get; set; }
        public string type { get; set; }
        public DateTime valueDate { get; set; }
    }

    public class ClsProfileDetails
    {
        public ClsHoldersDetails Holders { get; set; }
    }

    public class ClsHoldersDetails
    {
        public string type { get; set; }
        public ClsHolder Holder { get; set; }
    }

    public class ClsHolder
    {
        public string address { get; set; }
        public bool ckycCompliance { get; set; }
        public DateTime dob { get; set; }
        public string email { get; set; }
        public string landline { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
        public string nominee { get; set; }
        public string pan { get; set; }
    }

    public class ClsSummaryDetails
    {
        public DateTime balanceDateTime { get; set; }
        public string branch { get; set; }
        public string currency { get; set; }
        public double? currentBalance { get; set; }
        public double? drawingLimit { get; set; }
        public int? exchgeRate { get; set; }
        public string ifscCode { get; set; }
        public string micrCode { get; set; }
        public DateTime openingDate { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string facility { get; set; }
        public ClsPendingAmount Pending { get; set; }
    }
    public class ClsPendingAmount
    {
        public string amount { get; set; }
    }
}
