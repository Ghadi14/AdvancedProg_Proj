using HouseMates.Areas.Identity.Data;
using HouseMates.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseMates.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<House> Houses { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<House>(entity =>
        {
            entity.HasKey(h => h.Id);

            entity.HasOne(h => h.User)
            .WithMany(u => u.Houses)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
