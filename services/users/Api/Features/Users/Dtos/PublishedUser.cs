using MassTransit;

namespace Api.Features.Users.Dtos
{
  [EntityName("user")]
  [MessageUrn("user")]
  public record PublishedUser(Guid UserID, string? Username, string Email, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);
}