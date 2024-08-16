namespace Api.Data.Models;

public record Card(Guid CardID, string? ExternalCardID, string Name, ICollection<DeckCard> Decks, ICollection<CardColor> Colors, string? ImageUrl, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);
