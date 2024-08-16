using Microsoft.EntityFrameworkCore;

using Api.Data.Models;

namespace Api.Data;
public class DecksDbContext : DbContext
{
  public DecksDbContext(DbContextOptions<DecksDbContext> options)
      : base(options)
  {
  }

  public DbSet<Deck> Decks { get; set; }
  public DbSet<Card> Cards { get; set; }
  public DbSet<DeckCard> DeckCards { get; set; }
  public DbSet<CardColor> CardColors { get; set; }
}