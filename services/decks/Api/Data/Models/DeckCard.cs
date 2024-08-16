namespace Api.Data.Models;

public record DeckCard(Guid CardID, Guid DeckID, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);
