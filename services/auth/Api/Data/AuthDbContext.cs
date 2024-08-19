using Microsoft.EntityFrameworkCore;

using Api.Data.Models;

namespace Api.Data;
public class AuthDbContext : DbContext
{
  public AuthDbContext(DbContextOptions<AuthDbContext> options)
      : base(options)
  {
  }

  public DbSet<Role> Roles { get; set; }
  public DbSet<Auth> Auths { get; set; }

  public static void Initialize(AuthDbContext context)
  {
    // Ensure the database is created
    context.Database.EnsureCreated();

    // Check if the database is already seeded
    if (!context.Roles.Any())
    {
      // Seed data
      context.Roles.AddRangeAsync(Enum.GetValues(typeof(Features.Common.Roles))
              .Cast<Features.Common.Roles>()
              .Select(e => new Role(Convert.ToByte(e), e.ToString())));

      context.SaveChanges();
    }
  }
}