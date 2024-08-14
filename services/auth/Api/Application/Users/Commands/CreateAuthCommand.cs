using System.Security.Claims;
using MediatR;

namespace Api.Features.Auths.Commands
{
  public record CreateAuthCommand(string Email, string Password) : IRequest<Guid>;
}