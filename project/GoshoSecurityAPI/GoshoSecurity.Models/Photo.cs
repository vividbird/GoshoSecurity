namespace GoshoSecurity.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Photo
    {
        public string Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string UserId { get; set; }

        public GoshoSecurityUser User { get; set; }
    }
}
