using Api.Features.Subscriber.Sagas;
using Microsoft.EntityFrameworkCore;

namespace Api.Features
{
  public class EmailDbContext : DbContext
  {
    public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
    {
    }

    public override void OnModelCreating(ModelBuilder modelBuilder)
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