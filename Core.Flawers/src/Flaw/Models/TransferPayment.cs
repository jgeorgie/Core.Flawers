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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Ամսաթիվ")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Գումարի չափ")]
        public double Amount { get; set; }

        [Display(Name = "Անուն Ազգանուն")]
        public string FullName { get; set; }

        [Display(Name ="Հանձնարարագրի համար")]
        public string PaymentNo { get; set; }

        [Display(Name = "Նպատակ")]
        public string Destination { get; set; }

        public int MembershipFeeId { get; set; }
        public MembershipFee Fee { get; set; }
    }
}
