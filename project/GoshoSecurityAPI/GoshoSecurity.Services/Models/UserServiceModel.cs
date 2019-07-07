namespace GoshoSecurity.Services.Models
{
    using GoshoSecurity.Infrastructure.Mapping.Interfaces;
    using GoshoSecurity.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserServiceModel : IdentityUser, IMapWith<GoshoSecurityUser>
    {
        [Required]
        public string Name { get; set; }

        public ICollection<PhotoServiceModel> Photos { get; set; }
    }
}
