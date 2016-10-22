using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class PrivilegeModel
    {
        public string Id { get; set; }

        [Display(Name = "Սկիզբ")]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }


        [Display(Name = "Ավարտ")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        [Display(Name = "Արտոնության տեսակ")]
        public string Type { get; set; }

        [Display(Name = "Արտոնության համար")]
        public int PrivilegeNumber { get; set; }


        public string MembershipFeeFoeignKey { get; set; }
        public MembershipFee Fee { get; set; }


        //public string PrivilegeFoeignKey { get; set; }
        //public Privilege Privilege { get; set; }
    }
}
