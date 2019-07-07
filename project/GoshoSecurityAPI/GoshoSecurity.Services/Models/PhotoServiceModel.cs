namespace GoshoSecurity.Services.Models
{
    using GoshoSecurity.Infrastructure.Mapping.Interfaces;
    using GoshoSecurity.Models;
    using System.ComponentModel.DataAnnotations;

    public class PhotoServiceModel : IMapWith<Photo>
    {
        public string Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string UserId { get; set; }

        public UserServiceModel User { get; set; }
    }
}
