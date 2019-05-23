using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BandAid.Models
{
    public partial class User
    {
        public User()
        {
            Event = new HashSet<Event>();
            Review = new HashSet<Review>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Display(Name = "E-mail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail je obavezan")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Ime je obavezno")]
        public string Name { get; set; }

        [Display(Name = "Kontakt broj")]
        public string PhoneNumber { get; set; }


        public string Adress { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage ="Zaporka je obavezna")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Zaporka mora sadržavati minimalno 6 znakova")]
        public string PassHash { get; set; }

        //[Display(Name = "Potvrdite zaporku")]
        //[DataType(DataType.Password)]
        //[Compare("PassHash", ErrorMessage = "Zaporke se ne popudaraju")]
        //public string ConfirmPassword { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ProfileImg { get; set; }

        [Required(ErrorMessage = "Odaberite ulogu")]
        public int RoleId { get; set; }
        
        public bool IsEmailVerified { get; set; }
        public Guid ActivationCode { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public UserRole Role { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Event> Event { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Review> Review { get; set; }
    }
}
