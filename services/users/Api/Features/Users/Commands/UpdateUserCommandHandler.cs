using Api.Data;
using Api.Data.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Commands
{
  public class UpdateUserCommandHandler(UsersDbContext dbContext) : IConsumer<UpdateUserCommandRequest>
  {
    public async Task Consume(ConsumeContext<UpdateUserCommandRequest> context)
    {
      var user = await dbContext.Users
          .SingleOrDefaultAsync(d => d.UserID == Guid.Parse(context.Message.UserID));

      if (user != null)
      {
        user.Username = context.Message.UserName;
        user.Email = context.Message.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
      }

      await context.RespondAsync(new UpdateUserCommandResponse(user));
    }
  }
}