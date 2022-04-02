using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
