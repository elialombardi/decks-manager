using Microsoft.EntityFrameworkCore;

using Api.Data.Models;

namespace Api.Data;
public class UsersDbContext : DbContext
{
  public UsersDbContext(DbContextOptions<UsersDbContext> options)
      : base(options)
  {
  }

  public DbSet<User> Users { get; set; }
}