using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    [Table("OrderPakages")]
    public class OrderPakages
    {
        [Key]
        public int Id { get; set; }
        public int IdAccount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PakageId { get; set; }
        public decimal RateNow { get; set; }
        public DateTime CompleteDate { get; set; }
        public int Status { get; set; }
        public decimal Prices { get; set; }
        public decimal FinalPrice { get; set; }
        public string TransactionId { get; set; }
    }
    public class OrderPakagesList: OrderPakages
    {
        public int TotalRow { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PakageName { get; set; }
    }
    public class AccountRef
    {
        public int Id { get; set; }
        public int Ref { get; set; }
        public int Level { get; set; }
    }
}
