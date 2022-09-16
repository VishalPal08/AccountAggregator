using System.ComponentModel.DataAnnotations;

namespace AccountAggregator.ModelLayer
{
    public class RequestAuthenication
    {
        [Required(ErrorMessage = "fiuID is required")]
        public string fiuID { get; set; }

        [Required(ErrorMessage = "redirection_key is required")]
        public string redirection_key { get; set; }

        [Required(ErrorMessage = "userId is required")]
        public string userId { get; set; }
    }
}
