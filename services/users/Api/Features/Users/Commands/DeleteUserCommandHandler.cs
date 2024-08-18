using Api.Data;
using Microsoft.EntityFrameworkCore;
using Api.Data.Models;
using MassTransit;

namespace Api.Features.Users.Commands
{
  public class DeleteUserCommandConsumer(UsersDbContext dbContext) : IConsumer<DeleteUserCommandRequest>
  {
    public async Task Consume(ConsumeContext<DeleteUserCommandRequest> context)
    {
      var user = await dbContext.Users
              .SingleOrDefaultAsync(d => d.UserID == Guid.Parse(context.Message.UserID));

      if (user != null)
      {
        user.DeletedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
      }

      await context.RespondAsync(new DeleteUserCommandResponse(user));
    }

  }
}