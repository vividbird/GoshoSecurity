namespace GoshoSecurity.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class GoshoSecurityUser : IdentityUser
    {
        public string Name { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}
