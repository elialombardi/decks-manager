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

            if (!string.IsNullOrEmpty(request.UserID))
                query = query.Where(p => p.UserID == request.UserID);

            if (request.Random)
                query = query.OrderBy(_ => Guid.NewGuid());

            if (request.Take.HasValue)
                query = query.Take(request.Take.Value);

            return await query.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}