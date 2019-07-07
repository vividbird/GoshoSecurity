namespace GoshoSecurityAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserEditModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

    }
}
