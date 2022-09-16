using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountAggregator.ModelLayer
{
    public class BOLPRequest
    {
        [Required(ErrorMessage = "Source Name is required")]
        public string SourceName { get; set; }

        [Required(ErrorMessage = "Application number is required")]
        public string ApplicationNumber { get; set; }

        public string PolicyNumber { get; set; }
        public string TransactionId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }
        public BasicDetails ObjBasicDetail { get; set; }
        public List<BankAccountDetails> ObjBankAccountDetails { get; set; }
    }

    public class BasicDetails
    {
        [Required(ErrorMessage = "First Name is required")]
        public string ProposerFirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required")]
        public string ProposerLastName { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd}")]
        public DateTime? ProposerDateOfBirth { get; set; }

        public string ProposerGender { get; set; }

        [Required(ErrorMessage = "Email Id is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string ProposerEmailId { get; set; }


        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Entered phone format is not valid.")]
        // [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Entered phone format is not valid.")]
        public string ProposerMobileNo { get; set; }

        public string ProposerMaritalStatus { get; set; }

        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]{1}",ErrorMessage = "Invalid pancard number")]
        public string ProposerPANCardNo { get; set; }
    }

    public class BankAccountDetails
    {
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountType { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
    }
}
