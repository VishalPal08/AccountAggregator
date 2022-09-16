using System.ComponentModel.DataAnnotations;

namespace AccountAggregator.ModelLayer
{
    public class RequestRedirectUrl
    {
        [Required(ErrorMessage = "clienttxnid is required")]
        public string clienttrnxid { get; set; }
        
        [Required(ErrorMessage = "fiuID is required")]
        public string fiuID { get; set; }
        
        [Required(ErrorMessage = "userId is required")]
        public string userId { get; set; }
        
        [Required(ErrorMessage = "aaCustomerHandleId is required")]
        public string aaCustomerHandleId { get; set; }
        
        public string aaCustomerMobile { get; set; }
        
        [Required(ErrorMessage = "sessionId is required")]
        public string sessionId { get; set; }
        
        [Required(ErrorMessage = "useCaseId is required")]
        public string useCaseId { get; set; }
        
        public bool? Integrated_trigger_sms_email { get; set; }
        
        public string fipid { get; set; }
        
        public string addfip { get; set; }
    }
}
