namespace AccountAggregator.ModelLayer
{
    public class RequestConsentStatusNotification
    {
        public string ver { get; set; }
        public string timestamp { get; set; }
        public string txnid { get; set; }
        public string clienttxnid { get; set; }
        public string purpose { get; set; }
        public ClsNotifier Notifier { get; set; }
        public ClsConsentStatusNotification ConsentStatusNotification { get; set; }
    }

    public class ClsNotifier
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public class ClsConsentStatusNotification
    {
        public string consentId { get; set; }
        public string consentHandle { get; set; }
        public string consentStatus { get; set; }
    }
}
