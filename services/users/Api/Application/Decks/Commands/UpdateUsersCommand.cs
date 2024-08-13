using MediatR;

namespace Api.Features.Users.Commands
{
  public record UpdateUserCommand(string UserID, string UserID, string Name, string Description, string ImageUrl, IEnumerable<UpdateUserCommandCard> Cards) : IRequest<Guid?>;
  public record UpdateUserCommandCard(string CardID, string UserID, string Name, int Color);
}