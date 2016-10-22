using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class TransferPayment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Display(Name = "Ամսաթիվ")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Գումարի չափ")]
        public double Amount { get; set; }

        [Display(Name ="Հանձնարարագրի համար")]
        public string PaymentNo { get; set; }

        [Display(Name = "Նպատակ")]
        public string Destination { get; set; }


        public MembershipFee Fee { get; set; }
        public string MembershipFeeId { get; set; }

    }
}
