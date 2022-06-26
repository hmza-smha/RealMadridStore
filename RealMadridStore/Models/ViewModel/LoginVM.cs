using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Models.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username is required!")]
        [Display(Name = "User Name")]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
