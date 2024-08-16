using MediatR;
using Api.Data;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Cards.Commands
{
  public class ImportCardCommandHandler(DecksDbContext context) : IRequestHandler<ImportCardCommand, Card?>
  {
    public async Task<Card?> Handle(ImportCardCommand request, CancellationToken cancellationToken)
    {
      var card = await context.Cards.SingleOrDefaultAsync(c => c.ExternalCardID == request.ExternalCardID, cancellationToken);

      if (card != null)
      {
        return card;
      }
      else
      {
        card = new Card(Guid.NewGuid(), request.ExternalCardID, request.Name, [], [], null, DateTime.UtcNow, DateTime.UtcNow, null);

        await context.Cards.AddAsync(card, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
      }


      return card;
    }
  }
}