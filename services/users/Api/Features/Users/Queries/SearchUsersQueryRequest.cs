namespace Api.Features.Users.Queries
{
  public record SearchUsersQueryRequest(string? Username, string? Email);
}