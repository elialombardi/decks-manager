namespace Api.Data.Models;

public record Deck(Guid DeckID, string UserID, string Name, string Description, string ImageUrl, IEnumerable<Card> Cards, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);