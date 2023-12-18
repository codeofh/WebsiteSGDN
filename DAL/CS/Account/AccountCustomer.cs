using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    [Table("AccountCustomer")]
    public class AccountCustomer
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public DateTime BirthOfDate { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal AmountAvaiable { get; set; }
        public bool VerifyEmail { get; set; }
        public DateTime OTPEmailDate { get; set; }
        public bool VerifyPhone { get; set; }
        public DateTime OTPPhoneDate { get; set; }
        public bool VerifyKYC { get; set; }
        public string Phone { get; set; }
        public string KYCFront { get; set; }
        public string KYCBack { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
        public int Ref { get; set; }
        public decimal AmountRef { get; set; }

    }
    public class AccountCustomerExtend : AccountCustomer
    {
        public int TotalRow { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalFinalPrice { get; set; }
        public decimal TotalBenefit { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public string FullNameRef { get; set; }
        public string EmailRef { get; set; }
    }
    public class AccountUpdate
    {
        [Required]
        public int BankId { get; set; }
        [Required]
        public string BankAccount { get; set; }
        [Required]
        public string BankNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Gender { get; set; }
    }
    public class AccountCustomerRegister
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int? Ref { get; set; }
    }
}
