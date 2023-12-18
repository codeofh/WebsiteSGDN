﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    [Table("Withdrawal")]
    public class Withdrawal
    {
        [Key]
        public int Id { get; set; }
        public int IdAccount { get; set; }
        public int BankId { get; set; }
        public string BankAccount { get; set; }
        public string BankNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string TransactionId { get; set; }
        public string Note { get; set; }
        public int Usd2Vnd { get; set; }
        public int AccountConfirm { get; set; }
    }
    public class WithdrawalExtend : Withdrawal
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string BankName { get; set; }
        public string UserConfirm { get; set; }
        public int TotalRow { get; set; }
        public decimal TotalRef { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
