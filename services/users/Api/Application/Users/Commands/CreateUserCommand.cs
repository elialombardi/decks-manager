using System.Security.Claims;
using MediatR;

namespace Api.Features.Users.Commands
{
  public record CreateUserCommand(string Email, string? Username) : IRequest<Guid>;
}