using Microsoft.EntityFrameworkCore;

using Api.Data.Models;

namespace Api.Data;
public class AuthDbContext : DbContext
{
  public AuthDbContext(DbContextOptions<AuthDbContext> options)
      : base(options)
  {
  }

  public DbSet<Auth> Auths { get; set; }
}