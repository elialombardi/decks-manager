using MediatR;
using Api.Data;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Commands
{
  public class UpdateUserCommandHandler(UsersDbContext context) : IRequestHandler<UpdateUserCommand, Guid?>
  {
    public async Task<Guid?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
      var user = await context.Users
        .Include(d => d.Cards)
        .SingleOrDefaultAsync(d => d.UserID == Guid.Parse(request.UserID), cancellationToken);

      if (user == null || user.UserID != request.UserID)
      {
        return null;
      }

      // Add or update cards
      var updatedCards = request.Cards.Select(async c =>
      {
        var card = user.Cards.SingleOrDefault(userCard => userCard.CardID == Guid.Parse(c.CardID));
        if (card == null)
        {
          card = new Card(Guid.NewGuid(), c.UserID, c.Name, c.Color, DateTime.UtcNow, DateTime.UtcNow, null);
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


      var updatedUser = user with
      {
        Name = request.Name,
        Description = request.Description,
        ImageUrl = request.ImageUrl,
        Cards = await Task.WhenAll(updatedCards),
        UpdatedAt = DateTime.UtcNow
      };

      context.Entry(updatedUser).State = EntityState.Modified;


      await context.SaveChangesAsync(cancellationToken);

      return updatedUser.UserID;
    }
  }
}