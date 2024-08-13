using MediatR;
using Api.Data.Models;

namespace Api.Application.Users.Queries
{
  public record SearchUsersQuery(string? Username, string? Email) : IRequest<List<User>>;
}