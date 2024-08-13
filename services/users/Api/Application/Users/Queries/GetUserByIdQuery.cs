using MediatR;
using Api.Data.Models;

namespace Api.Application.Users.Queries
{
  public record GetUserByIdQuery(Guid Id) : IRequest<User>;
}