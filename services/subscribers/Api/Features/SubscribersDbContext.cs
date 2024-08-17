using Api.Features.Subscriber.Sagas;
using Microsoft.EntityFrameworkCore;

namespace Api.Features
{
  public class SubscribersDbContext(DbContextOptions<SubscribersDbContext> options) : DbContext(options)
  {
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<NewsletterOnboardingSagaData>(e =>
       {
         e.HasKey(x => x.CorrelationId);
       });
    }

    public DbSet<Subscriber.Models.Subscriber> Subscribers { get; set; }
    public DbSet<NewsletterOnboardingSagaData> NewsletterOnboardingSagaDatas { get; set; }
  }
}