using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    [System.ComponentModel.DataAnnotations.Schema.Table("BankCustomer")]
    public class BankCustomer
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public int IdAccount { get; set; }
        public int BankId { get; set; }
        public string BankAccount { get; set; }
        public string BankNumber { get; set; }
    }
    public class BankCustomerExtend : BankCustomer
    {
        public string BankName { get; set; }
    }
}
