using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class PrivilegeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Սկիզբ")]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }


        [Display(Name = "Ավարտ")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        [Display(Name = "Արտոնության տեսակ")]
        public string Type { get; set; }

        [Display(Name = "Արտոնության համար")]
        public long PrivilegeNumber { get; set; }


        public int MembershipFeeFoeignKey { get; set; }
        public MembershipFee Fee { get; set; }


        //public string PrivilegeFoeignKey { get; set; }
        //public Privilege Privilege { get; set; }
    }
}
