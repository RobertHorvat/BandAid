using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BandAid.Models.PomocneKlase
{
    public class ContactUser
    {
        [Required(ErrorMessage = "Upišite Vaše ime")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Upišite Vaše prezime")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Unesite email adresu")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Upišite temu")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Unesite poruku")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
