using MediatR;

namespace Api.Features.Users.Commands
{
  public record CreateUserCommand(string UserID, string Name, string Description, string ImageUrl, IEnumerable<CreateUserCommandCard> Cards) : IRequest<Guid>;
  public record CreateUserCommandCard(string UserID, string Name, int Color) : IRequest<Guid>;
}