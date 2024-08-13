using MediatR;
using Api.Data;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Commands
{
  public class UpdateUserCommandHandler(UsersDbContext context) : IRequestHandler<UpdateUserCommand, User?>
  {
    public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
      var user = await context.Users
        .SingleOrDefaultAsync(d => d.UserID == Guid.Parse(request.UserID), cancellationToken);

      if (user == null)
      {
        return null;
      }


      var updatedUser = user with
      {
        Username = request.UserName,
        Email = request.Email,
        UpdatedAt = DateTime.UtcNow
      };

      context.Entry(updatedUser).State = EntityState.Modified;


      await context.SaveChangesAsync(cancellationToken);

      return updatedUser;
    }
  }
}