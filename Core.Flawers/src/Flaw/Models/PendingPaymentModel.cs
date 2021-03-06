﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class PendingPaymentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Վերջնաժամկետ")]
        [DataType(DataType.Date)]
        public DateTime PaymentDeadline { get; set; }

        public DateTime PayedOn { get; set; }

        [Display(Name = "Վճարի չափ")]
        public double Amount { get; set; }

        [Display(Name = "Պարտք")]
        public double? DepositOrDebt { get; set; }

        [Display(Name = "Կարգավիճակ")]
        public PaymentStatus Status { get; set; }

        public int MembershipFeeForeignKey { get; set; }
        public MembershipFee Fee { get; set; }


        public int TransferPaymentForeignKey { get; set; }
        public TransferPayment TransferPayment { get; set; }

        public int CashPaymentForeignKey { get; set; }
        public CashModel CashPayment { get; set; }

    }

    public enum PaymentStatus
    {
        Pending,
        Payed,
        Cancelled,
        Paused
    }

}
