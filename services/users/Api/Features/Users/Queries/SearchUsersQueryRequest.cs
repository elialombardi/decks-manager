namespace Api.Application.Users.Queries
{
  public record SearchUsersQueryRequest(string? Username, string? Email);
}