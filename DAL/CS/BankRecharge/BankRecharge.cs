using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    [Table("BankRecharge")]
    public class BankRecharge
    {
        [Key]
        public int Id { get; set; }
        public int IdAccount { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime ApproveDate { get; set; }
        public string TransactionId { get; set; }
        public int Status { get; set; }
        public int Usd2Vnd { get; set; }
        public int AccountConfirm { get; set; }
    }
    public class BankRechargeList : BankRecharge {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserConfirm { get; set; }
        public int TotalRow { get; set; }
    }
}
