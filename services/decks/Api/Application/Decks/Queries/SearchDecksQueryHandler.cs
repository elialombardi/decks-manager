using MediatR;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Data.Models;

namespace Api.Application.Decks.Queries
{
    public class SearchDecksQueryHandler(DecksDbContext context) : IRequestHandler<SearchDecksQuery, List<Deck>>
    {
        public async Task<List<Deck>> Handle(SearchDecksQuery request, CancellationToken cancellationToken)
        {
            var query = context.Decks.AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(p => p.Name.Contains(request.Name));

            return await query.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}