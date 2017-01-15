using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class PaymentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public PaymentType Type { get; set; }

        public int MembershipFeeForeignKey { get; set; }
        public MembershipFee Fee { get; set; }

        public int CashPaymentId { get; set; }
        public CashModel CashPayment { get; set; }


    }

    public enum PaymentType
    {
        Cash,
        Тransfer
    }

}
