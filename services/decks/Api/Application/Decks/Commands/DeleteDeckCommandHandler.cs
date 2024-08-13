using MediatR;
using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Decks.Commands
{
  public class DeleteDeckCommandHandler(DecksDbContext context) : IRequestHandler<DeleteDeckCommand, Guid?>
  {
    public async Task<Guid?> Handle(DeleteDeckCommand request, CancellationToken cancellationToken)
    {

      var deck = await context.Decks
        .Include(d => d.Cards)
        .SingleOrDefaultAsync(d => d.DeckID == Guid.Parse(request.DeckID), cancellationToken);

      if (deck == null || deck.UserID != request.UserID)
      {
        return null;
      }

      var updatedDeck = deck with
      {
        DeletedAt = DateTime.UtcNow
      };

      // Soft delete cards
      foreach (var card in updatedDeck.Cards)
      {
        var updatedCard = card with { DeletedAt = DateTime.UtcNow };
        context.Entry(updatedCard).State = EntityState.Modified;
      }

      context.Entry(updatedDeck).State = EntityState.Modified;

      await context.SaveChangesAsync(cancellationToken);

      return deck.DeckID;
    }
  }
}