namespace Api.Data.Models;

public record User(Guid UserID, string UserID, string Name, string Description, string ImageUrl, IEnumerable<Card> Cards, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);