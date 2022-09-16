using System.ComponentModel.DataAnnotations;

namespace AccountAggregator.ModelLayer
{
    public class ResponseRedirectUrl
    {
        [Required]
        public string txnid { get; set; }
        [Required]
        public string clienttxnid { get; set; }
        [Required]
        public string sessionid { get; set; }
        [Required]
        public string redirectionurl { get; set; }
        [Required]
        public string timestamp { get; set; }
        public string statusCode { get; set; }
        public string message { get; set; }
    }
}
