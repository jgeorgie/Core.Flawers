using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class PendingPaymentModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public double Amount { get; set; }
        public PaymentStatus Status { get; set; }

        public string MembershipFeeForeignKey { get; set; }
        public MembershipFee Fee { get; set; }

        public List<PaymentModel> Payments { get; set; }

    }

    public enum PaymentStatus
    {
        Pending,
        Payed,
        Cancelled,
        Paused
    }

}
