using Api.Data.Models;

namespace Api.Features.Users.Queries
{
  public record SearchUsersQueryResponse(List<User> Users);
}