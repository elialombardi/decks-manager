namespace Api.Data.Models;

public class Card
{
  public Guid CardID { get; set; }
  public string? ExternalCardID { get; set; }
  public required string Name { get; set; }
  public required ICollection<DeckCard> Decks { get; set; }
  public required ICollection<CardColor> Colors { get; set; }
  public string? ImageUrl { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public DateTime? DeletedAt { get; set; }
}
