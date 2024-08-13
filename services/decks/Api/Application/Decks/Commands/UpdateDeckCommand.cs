using MediatR;

namespace Api.Features.Decks.Commands
{
  public record UpdateDeckCommand(string DeckID, string UserID, string Name, string Description, string ImageUrl, IEnumerable<UpdateDeckCommandCard> Cards) : IRequest<Guid?>;
  public record UpdateDeckCommandCard(string CardID, string DeckID, string Name, int Color);
}