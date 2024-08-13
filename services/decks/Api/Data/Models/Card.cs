namespace Api.Data.Models;

public record Card(Guid CardID, string DeckID, string Name, int Color, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);
