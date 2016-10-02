using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class TransferPayment : IPayments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Destination { get; set; }


        public MembershipFee Fee { get; set; }
        public string MembershipFeeId { get; set; }

    }
}
