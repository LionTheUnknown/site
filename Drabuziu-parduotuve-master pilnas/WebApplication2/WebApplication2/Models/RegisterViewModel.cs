using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Vardas")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Pavarde")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Telefono numeris")]
        [Phone(ErrorMessage = "netinkamas telefono numeris")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "slaptazodis")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "patvirtinkite slaptazodi")]
        [Compare("Password", ErrorMessage = "slaptazodziai nesutampa")]
        public string ConfirmPassword { get; set; }
    }
}
