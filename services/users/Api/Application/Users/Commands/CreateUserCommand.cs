using System.Security.Claims;
using MediatR;

namespace Api.Features.Users.Commands
{
  public record CreateUserCommand(string UserName, string Email) : IRequest<Guid>;
}