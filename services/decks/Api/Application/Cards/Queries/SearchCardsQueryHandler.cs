using Api.Data;
using Api.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Cards.Queries
{
  public class SearchCardsQueryHandler(DecksDbContext context) : IRequestHandler<SearchCardsQuery, List<Card>>
  {
    public async Task<List<Card>> Handle(SearchCardsQuery request, CancellationToken cancellationToken)
    {
      var query = context.Cards.AsQueryable();

      if (!string.IsNullOrEmpty(request.Name))
        query = query.Where(p => p.Name.Contains(request.Name));

      if (request.Random)
        query = query.OrderBy(_ => Guid.NewGuid());

      if (request.Take.HasValue)
        query = query.Take(request.Take.Value);

      return await query.ToListAsync(cancellationToken: cancellationToken);
    }
  }
}