﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class CashModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Փասթաթղթի համարը")]
        public string OrdersNumber { get; set; }

        [Display(Name = "Կազմման ամսաթիվը")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Թղթակցող հաշիվը")]
        public string Account { get; set; }

        [Display(Name = "Վերլուծական հաշվառման ծածկագիրը")]
        public string AccountingPass { get; set; }

        [Display(Name = "Գումարը")]
        public double Amount { get; set; }

        [Display(Name = "Նպատակը")]
        public string Destination { get; set; }

        [Display(Name = "Անուն Ազգանուն")]
        public string FullName { get; set; }

        [Display(Name = "Գործարքի տեսակ")]
        public BargainType Type { get; set; }


        public int MembershipFeeId { get; set; }
        public MembershipFee Fee { get; set; }

    }

    public enum BargainType
    {
        [Display(Name = "Մուտք")]
        CashIn,
        [Display(Name = "Ելք")]
        CashOut
    }
}
