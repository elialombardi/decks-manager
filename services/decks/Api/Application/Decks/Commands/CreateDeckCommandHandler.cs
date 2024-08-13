using MediatR;
using Api.Data;
using Api.Data.Models;

namespace Api.Features.Decks.Commands
{
  public class CreateDeckCommandHandler(DecksDbContext context) : IRequestHandler<CreateDeckCommand, Guid>
  {
    public async Task<Guid> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
    {
      var deck = new Deck(Guid.NewGuid(),
       request.UserID,
       request.Name,
       request.Description,
       request.ImageUrl,
       request.Cards.Select(c => new Card(Guid.NewGuid(), c.DeckID, c.Name, c.Color, DateTime.UtcNow, DateTime.UtcNow, null)).ToList(),
       DateTime.UtcNow,
       DateTime.UtcNow,
       null);

      context.Decks.Add(deck);

      await context.SaveChangesAsync(cancellationToken);

      return deck.DeckID;
    }
  }
}