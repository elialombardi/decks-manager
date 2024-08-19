using Api.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auths.Commands
{
  public class DeleteAuthCommandConsumer(AuthDbContext dbContext) : IConsumer<DeleteAuthCommandRequest>
  {
    public async Task Consume(ConsumeContext<DeleteAuthCommandRequest> context)
    {
      var auth = await dbContext.Auths
          .SingleOrDefaultAsync(d => d.AuthID == Guid.Parse(context.Message.AuthID));

      if (auth != null)
      {
        auth.DeletedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
      }

      await context.RespondAsync(new DeleteAuthCommandResponse(auth));
    }
  }
}