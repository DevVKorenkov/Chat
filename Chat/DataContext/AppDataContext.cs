using Chat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataContext;

public class AppDataContext : IdentityDbContext<IdentityUser>
{
	public DbSet<AppIdentityUser> Users { get; set; }
	public DbSet<Clan> Clans { get; set; }

    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {
    }
}
