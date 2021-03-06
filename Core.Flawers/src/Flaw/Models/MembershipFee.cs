﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class MembershipFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Անուն")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Ազգանուն")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Հայրանուն")]
        [Required]
        public string MiddleName { get; set; }

        [Display(Name = "Սկիզբ")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Start { get; set; }

        [Display(Name = "Ավարտ")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        [Display(Name = "Անդամավճարի ամսական վճ.")]
        public double MonthlyPay { get; set; }

        [Display(Name = "Անդամավճարի չափ")]
        [Required]
        public double RealAmount { get; set; }

        [Display(Name = "Զեղչված Անդամավճարի չափ")]
        public double AmountWithDiscount { get; set; }

        [Display(Name = "Ընթացիկ վիճակ")]
        public FeeState CurrentState { get; set; }

        [Display(Name = "Պարբ.-թյուն")]
        public FeePeriodicity Periodicity { get; set; }
        [Display(Name = "Կասեցված")]
        public DateTime? Paused { get; set; }
        [Display(Name = "Վերականգնված")]
        public DateTime? Reactiveted { get; set; }

        public int? TotalDaysPaused { get; set; }

        [Display(Name = "Պարտք")]
        public double LeftOver { get; set; }

        [Display(Name = "Կանխավճար")]
        public double? Deposit { get; set; }


        public double currentDebt { get; set; }


        [Display(Name = "Արտոնագրի համար")]
        [Required]
        public int LicenseNumber { get; set; }

        [Display(Name = "Արտոնության տեսակ")]
        public string PrivilegeType { get; set; }

        [Display(Name = "Արտոնության սկիզբ")]
        [DataType(DataType.Date)]
        public DateTime? ActivePrivilegeStart { get; set; }

        [Display(Name = "Արտոնության ավարտ")]
        [DataType(DataType.Date)]
        public DateTime? ActivePrivilegeEnd { get; set; }


        public List<PrivilegeModel> PrivilegeModels { get; set; }
        public List<FeeAmountChangeModel> AmountChanges { get; set; }
        public List<CashModel> CashPayments { get; set; }
        public List<TransferPayment> TransferPayments { get; set; }
        public List<FeeStateChangeModel> FeeStateChanges { get; set; }

    }


    public enum FeeState
    {
        [Display(Name = "Ակտիվ")]
        Active,
        [Display(Name = "Սառեցված")]
        Pause,
        [Display(Name = "Դադարեցված")]
        Finish
    }

    public enum FeePeriodicity
    {
        [Display(Name = "Ամիս")]
        Month,
        [Display(Name = "Տարի")]
        Year
    }

}
