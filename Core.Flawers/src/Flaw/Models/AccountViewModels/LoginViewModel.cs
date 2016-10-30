using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flaw.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Էլ. փոստ")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Գաղտնաբառ")]
        public string Password { get; set; }

        [Display(Name = "Հիշե՞լ")]
        public bool RememberMe { get; set; }
    }
}
