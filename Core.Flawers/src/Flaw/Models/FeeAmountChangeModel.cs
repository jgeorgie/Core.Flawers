using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class FeeAmountChangeModel
    {
        public string id { get; set; }
        public DateTime ChangeDate { get; set; }
        public double OldAmount { get; set; }
        public double NewAmount { get; set; }


        public string MembershipFeeForeignKey { get; set; }
        public MembershipFee Fee { get; set; }
    }
}
