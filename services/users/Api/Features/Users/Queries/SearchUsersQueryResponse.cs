using Api.Data.Models;

namespace Api.Application.Users.Queries
{
  public record SearchUsersQueryResponse(List<User> Users);
}