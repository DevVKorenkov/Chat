using Chat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Chat.DataContext;

public class AppDataContext : IdentityDbContext<IdentityUser>
{
	public DbSet<AppIdentityUser> Users { get; set; }
	public DbSet<Clan> Clans { get; set; }

    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppIdentityUser>()
            .Property(u => u.Index)
            .ValueGeneratedOnAdd()
            .Metadata
            .SetAfterSaveBehavior(PropertySaveBehavior.Throw);
    }
}
