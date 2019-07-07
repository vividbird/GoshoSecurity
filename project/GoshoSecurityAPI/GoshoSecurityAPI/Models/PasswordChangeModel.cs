namespace GoshoSecurityAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PasswordChangeModel
    {
        public string UserId { get; set; }

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
