using MediatR;
using Api.Data;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Decks.Commands
{
  public class DeleteDecksByUserIdCommandHandler(DecksDbContext context) : IRequestHandler<DeleteDecksByUserIdCommand, List<Deck>>
  {
    public async Task<List<Deck>> Handle(DeleteDecksByUserIdCommand request, CancellationToken cancellationToken)
    {
      var decks = await context.Decks
        .Where(d => d.UserID == request.UserID)
        .ToListAsync(cancellationToken);

      foreach (var deck in decks)
        deck.DeletedAt = DateTime.UtcNow;

      await context.SaveChangesAsync(cancellationToken);

      return decks;
    }
  }
}