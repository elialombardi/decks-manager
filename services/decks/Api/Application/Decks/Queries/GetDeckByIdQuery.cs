using MediatR;
using Api.Data.Models;

namespace Api.Application.Decks.Queries
{
  public record GetDeckByIdQuery(Guid Id, string UserID) : IRequest<Deck>;
}