using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    [Table("Pakages")]
    public class Pakages
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Prices { get; set; }
        public decimal Bonus { get; set; }
        public decimal MinRate { get; set; }
        public decimal MaxRate { get; set; }
        public decimal RateNow { get; set; }
        public int Type { get; set; }
    }
    public class PakagesList : Pakages
    {
        public int TotalRow { get; set; }
    }
}
