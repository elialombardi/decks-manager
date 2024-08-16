using Api.Data.Models;
using MediatR;

namespace Api.Application.Decks.Commands
{
  public record CreateDeckCommand(string UserID, string Name, string Description, string ImageUrl, IEnumerable<CreateDeckCommandCard> Cards) : IRequest<Deck?>;
  public record CreateDeckCommandCard(Guid CardID);
}