using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BandAid.Models.PomocneKlase
{
    public class LogUser
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Zaporka je obavezna")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Zaporka mora sadržavati minimalno 6 znakova")]
        public string PassHash { get; set; }

        [Display(Name = "E-mail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail je obavezan")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
