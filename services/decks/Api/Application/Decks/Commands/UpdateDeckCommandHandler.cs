using MediatR;
using Api.Data;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Decks.Commands
{
  public class UpdateDeckCommandHandler(DecksDbContext context) : IRequestHandler<UpdateDeckCommand, Guid?>
  {
    public async Task<Guid?> Handle(UpdateDeckCommand request, CancellationToken cancellationToken)
    {
      var deck = await context.Decks
        .Include(d => d.Cards)
        .SingleOrDefaultAsync(d => d.DeckID == Guid.Parse(request.DeckID), cancellationToken);

      if (deck == null || deck.UserID != request.UserID)
      {
        return null;
      }

      // Add or update cards
      var updatedCards = request.Cards.Select(async c =>
      {
        var card = deck.Cards.SingleOrDefault(deckCard => deckCard.CardID == Guid.Parse(c.CardID));
        if (card == null)
        {
          card = new Card(Guid.NewGuid(), c.DeckID, c.Name, c.Color, DateTime.UtcNow, DateTime.UtcNow, null);
          await context.Cards.AddAsync(card, cancellationToken);

          return card;
        }

        return card with
        {
          Name = c.Name,
          Color = c.Color,
          UpdatedAt = DateTime.UtcNow
        };
      });


      var updatedDeck = deck with
      {
        Name = request.Name,
        Description = request.Description,
        ImageUrl = request.ImageUrl,
        Cards = await Task.WhenAll(updatedCards),
        UpdatedAt = DateTime.UtcNow
      };

      context.Entry(updatedDeck).State = EntityState.Modified;


      await context.SaveChangesAsync(cancellationToken);

      return updatedDeck.DeckID;
    }
  }
}