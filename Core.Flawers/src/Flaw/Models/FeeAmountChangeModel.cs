using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class FeeAmountChangeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Փոփոխման ամսաթիվ")]
        public DateTime ChangeDate { get; set; }

        [Display(Name = "Նախկին չափ")]
        public double OldAmount { get; set; }

        [Display(Name = "Նոր չափ")]
        public double NewAmount { get; set; }


        public int MembershipFeeForeignKey { get; set; }
        public MembershipFee Fee { get; set; }
    }
}
