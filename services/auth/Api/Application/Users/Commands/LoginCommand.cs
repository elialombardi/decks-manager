using MediatR;

namespace Api.Features.Auths.Commands
{
  public record LoginCommand(string Email, string Password) : IRequest<bool>;
}