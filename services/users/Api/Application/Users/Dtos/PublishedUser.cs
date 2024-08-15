namespace Api.Application.Users.Dtos
{
  public record PublishedUser(Guid UserID, string? Username, string Email, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);
}