using MediatR;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Data.Models;

namespace Api.Application.Decks.Queries
{
    public class GetDeckByIdQueryHandler(DecksDbContext context) : IRequestHandler<GetDeckByIdQuery, Deck?>
    {
        public async Task<Deck?> Handle(GetDeckByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Decks.FirstOrDefaultAsync(p => p.DeckID == request.Id, cancellationToken: cancellationToken);
        }
    }
}