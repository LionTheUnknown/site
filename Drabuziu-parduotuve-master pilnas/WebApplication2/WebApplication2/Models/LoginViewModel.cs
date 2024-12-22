using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Vardas")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Slaptazodis")]
        public string Password { get; set; }
    }
}
