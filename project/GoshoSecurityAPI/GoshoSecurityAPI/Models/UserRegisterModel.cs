namespace GoshoSecurityAPI.Models
{
    using GoshoSecurity.Infrastructure.Mapping.Interfaces;
    using GoshoSecurity.Services.Models;
    using System.ComponentModel.DataAnnotations;

    public class UserRegisterModel : IMapWith<UserServiceModel>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
