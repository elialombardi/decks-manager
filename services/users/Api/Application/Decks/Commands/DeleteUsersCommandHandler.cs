using MediatR;
using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Commands
{
  public class DeleteUserCommandHandler(UsersDbContext context) : IRequestHandler<DeleteUserCommand, Guid?>
  {
    public async Task<Guid?> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {

      var user = await context.Users
        .Include(d => d.Cards)
        .SingleOrDefaultAsync(d => d.UserID == Guid.Parse(request.UserID), cancellationToken);

      if (user == null || user.UserID != request.UserID)
      {
        return null;
      }

      var updatedUser = user with
      {
        DeletedAt = DateTime.UtcNow
      };

      // Soft delete cards
      foreach (var card in updatedUser.Cards)
      {
        var updatedCard = card with { DeletedAt = DateTime.UtcNow };
        context.Entry(updatedCard).State = EntityState.Modified;
      }

      context.Entry(updatedUser).State = EntityState.Modified;

      await context.SaveChangesAsync(cancellationToken);

      return user.UserID;
    }
  }
}