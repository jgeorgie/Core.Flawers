using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class MembershipFee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string Id { get; set; }

        [Display(Name = "Անուն Ազգանուն")]
        public string FullName { get; set; }

        [Display(Name = "Սկիզբ")]
        public DateTime Start { get; set; }

        [Display(Name = "Ավարտ")]
        public DateTime End { get; set; }

        [Display(Name = "Անդամավճարի չափ")]
        public double RealAmount { get; set; }

        [Display(Name = "Զեղչված Անդամավճարի չափ")]
        public double AmountWithDiscount { get; set; }

        [Display(Name = "Ընթացիկ վիճակ")]
        public FeeState CurrentState { get; set; }

        [Display(Name = "Պարբերականություն")]
        public FeePeriodicity Periodicity { get; set; }

        public DateTime? Paused { get; set; }

        public DateTime? Reactiveted { get; set; }

        public int? TotalDaysPaused { get; set; }

        [Display(Name = "Ընթացիկ գումարի չափ")]
        public double LeftOver { get; set; }

        [Display(Name = "Պարտքը օրվա դրությամբ")]
        public double currentDebt { get; set; }


        public Privilege Privilege { get; set; }

        public List<FeeAmountChangeModel> AmountChanges { get; set; }
        public List<PendingPaymentModel> Payments { get; set; }

    }


    public enum FeeState
    {
        Active,
        Pause,
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
