using MediatR;

namespace Api.Features.Decks.Commands
{
  public record UpdateDeckCommand(Guid DeckID, string UserID, string Name, string Description, string ImageUrl, IEnumerable<UpdateDeckCommandCard> Cards) : IRequest<Guid?>;
  public record UpdateDeckCommandCard(Guid CardID, Guid DeckID, string Name, int Color);
}