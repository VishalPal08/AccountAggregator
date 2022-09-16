using System.ComponentModel.DataAnnotations;


namespace AccountAggregator.ModelLayer
{
    public class ResponseAuth
    {
        
        public string txnID { get; set; }
        [Required]
        public string fiuId { get; set; }
        [Required]
        public string statusCode { get; set; }
        [Required]
        public string message { get; set; }
        [Required]
        public string sessionId { get; set; }
        [Required]
        public string token { get; set; }

    }
}
