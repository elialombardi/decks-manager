using MediatR;
using Api.Data.Models;

namespace Api.Application.Decks.Queries
{
  public record SearchDecksQuery(string? Name, string UserID) : IRequest<List<Deck>>;
}