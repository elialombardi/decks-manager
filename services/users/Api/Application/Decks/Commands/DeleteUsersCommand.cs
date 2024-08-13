using MediatR;

namespace Api.Features.Users.Commands
{
  public record DeleteUserCommand(string UserID, string UserID) : IRequest<Guid?>;
}