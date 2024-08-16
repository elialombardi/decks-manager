using Microsoft.EntityFrameworkCore;

using Api.Data.Models;

namespace Api.Data;
public class DecksDbContext : DbContext
{
  public DecksDbContext(DbContextOptions<DecksDbContext> options)
      : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<CardColor>()
      .HasKey(c => new { c.CardID, c.Color });

    modelBuilder.Entity<DeckCard>()
      .HasKey(c => new { c.DeckID, c.CardID });

  }


  public DbSet<Deck> Decks { get; set; }
  public DbSet<Card> Cards { get; set; }
  public DbSet<DeckCard> DeckCards { get; set; }
  public DbSet<CardColor> CardColors { get; set; }
}