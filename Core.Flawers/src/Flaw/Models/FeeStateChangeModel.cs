using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class FeeStateChangeModel
    {
        public string Id { get; set; }
        [Display(Name = "Նախկին Վիճակ")]
        public FeeState PreviousState { get; set; }
        [Display(Name = "Նոր Վիճակ")]
        public FeeState NewState { get; set; }
        [Display(Name = "Փոփոխման Ամսաթիվ")]
        public DateTime ChangeDate { get; set; }


        public string MembershipFeeForeignKey { get; set; }
        public MembershipFee Fee { get; set; }

    }
}
