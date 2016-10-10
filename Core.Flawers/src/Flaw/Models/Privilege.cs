using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class Privilege
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Display(Name ="Արտոնության համար")]
        public int PrivilegeNumber { get; set; }

        [Display(Name = "Արտոնության տեսակ")]
        public string Type { get; set; }

        [Display(Name = "Նկարագրություն")]
        public string Description { get; set; }

        [Display(Name = "Զեղչ")]
        public int Discount { get; set; }


        public List<MembershipFee> Fees { get; set; }
    }


}
