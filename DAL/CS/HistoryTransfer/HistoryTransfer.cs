using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    [Table("HistoryTransfer")]
    public class HistoryTransfer
    {
        [Key]
        public long Id { get; set; }

        public int IdAccount { get; set; }
        public int IdAccountAdmin { get; set; }

        public int Type { get; set; }

        public decimal AmountBefore { get; set; }

        public decimal AmountModified { get; set; }

        public decimal AmountAfter { get; set; }

        public string Note { get; set; }

        public DateTime CreatedDate { get; set; }

    }
    public class HistoryTransferExtend
    {
        public long Id { get; set; }

        public int IdAccount { get; set; }
        public string FullName { get; set; }
        public string FullNameAdmin { get; set; }
        public string Email { get; set; }
        public string PakageName { get; set; }

        public int Type { get; set; }

        public decimal AmountBefore { get; set; }

        public decimal AmountModified { get; set; }
        public decimal AmountStart { get; set; }

        public decimal AmountAfter { get; set; }

        public string Note { get; set; }

        public DateTime CreatedDate { get; set; }

        public int DateNumber { get; set; }
        public int TotalRow { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPakage { get; set; }

    }
    public class HistoryReport
    {
        public int Total { get; set; }
        public decimal Prices { get; set; }
    }
}
