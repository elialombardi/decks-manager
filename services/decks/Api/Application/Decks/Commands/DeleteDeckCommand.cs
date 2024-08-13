using MediatR;

namespace Api.Features.Decks.Commands
{
  public record DeleteDeckCommand(string DeckID, string UserID) : IRequest<Guid?>;
}