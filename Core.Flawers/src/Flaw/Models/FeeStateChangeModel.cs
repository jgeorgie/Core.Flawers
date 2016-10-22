using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class FeeStateChangeModel
    {
        public string Id { get; set; }
        public FeeState PreviousState { get; set; }
        public FeeState NewState { get; set; }
        public DateTime ChangeDate { get; set; }


        public string MembershipFeeForeignKey { get; set; }
        public MembershipFee Fee { get; set; }

    }
}
