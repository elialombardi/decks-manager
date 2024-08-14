namespace Api.Data.Models;

public record User(Guid UserID, string? Username, string Email, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);