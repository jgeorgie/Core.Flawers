using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models
{
    public class Privilege
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Description { get; set; }

        public int Discount { get; set; }



        public string MembershipFeeForeignKey { get; set; }
        public MembershipFee Fee { get; set; }
    }
}
