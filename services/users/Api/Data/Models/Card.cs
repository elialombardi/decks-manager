namespace Api.Data.Models;

public record Card(Guid CardID, string UserID, string Name, int Color, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);
