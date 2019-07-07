namespace GoshoSecurity.Data
{
    using GoshoSecurity.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class GoshoSecurityDbContext : IdentityDbContext<GoshoSecurityUser>
    {
        public GoshoSecurityDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Photo>()
               .HasOne(p => p.User)
               .WithMany(u => u.Photos)
               .HasForeignKey(p => p.UserId);
        }
    }
}
