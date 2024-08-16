namespace Api.Data.Models;

public record Deck
{
  public Guid DeckID { get; set; }
  public required string UserID { get; set; }
  public required string Name { get; set; }
  public required string Description { get; set; }
  public required string ImageUrl { get; set; }
  public required IEnumerable<DeckCard> Cards { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public DateTime? DeletedAt { get; set; }
}