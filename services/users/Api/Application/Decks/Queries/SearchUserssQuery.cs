using MediatR;
using Api.Data.Models;

namespace Api.Application.Users.Queries
{
  public record SearchUsersQuery(string? Name, string UserID) : IRequest<List<User>>;
}