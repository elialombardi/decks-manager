using Api.Data;
using Api.Data.Models;
using MassTransit;

namespace Api.Features.Users.Commands
{
  public class CreateUserCommandConsumer(UsersDbContext dbContext) : IConsumer<CreateUserCommandRequest>
  {
    public async Task Consume(ConsumeContext<CreateUserCommandRequest> context)
    {
      var user = new User()
      {
        UserID = Guid.NewGuid(),
        Username = context.Message.Username,
        Email = context.Message.Email,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        DeletedAt = null
      };
      await dbContext.Users.AddAsync(user);

      await dbContext.SaveChangesAsync();

      await context.RespondAsync(new CreateUserCommandResponse(user));
    }
  }
}