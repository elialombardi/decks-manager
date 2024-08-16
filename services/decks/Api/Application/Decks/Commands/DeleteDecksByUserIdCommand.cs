using Api.Data.Models;
using MediatR;

namespace Api.Application.Decks.Commands
{
  public record DeleteDecksByUserIdCommand(string UserID) : IRequest<List<Deck>>;
}