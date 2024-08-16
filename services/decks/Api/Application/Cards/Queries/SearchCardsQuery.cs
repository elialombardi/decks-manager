using Api.Data.Models;
using MediatR;

namespace Api.Application.Cards.Queries
{
  public record SearchCardsQuery(string? Name, bool Random = false, int? Take = null) : IRequest<List<Card>>;
}