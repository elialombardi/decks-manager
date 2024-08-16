using MediatR;
using Api.Data;
using Api.Data.Models;
using Api.Application.Decks.Commands;

namespace Api.Features.Decks.Commands
{
  public class CreateDeckCommandHandler(DecksDbContext context) : IRequestHandler<CreateDeckCommand, Deck?>
  {
    public async Task<Deck?> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
    {
      var deckID = Guid.NewGuid();
      Deck deck = new()
      {
        DeckID = deckID,
        UserID = request.UserID,
        Name = request.Name,
        Description = request.Description,
        ImageUrl = request.ImageUrl,
        Cards = request.Cards.Select(c => new DeckCard(c.CardID, deckID, DateTime.UtcNow, DateTime.UtcNow, null)).ToList(),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        DeletedAt = null
      };

      await context.AddAsync(deck);

      await context.SaveChangesAsync(cancellationToken);

      return deck;
    }
  }
}