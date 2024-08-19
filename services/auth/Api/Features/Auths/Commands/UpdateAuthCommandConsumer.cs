using Api.Data;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Api.Features.Common;
using MassTransit;

namespace Api.Features.Auths.Commands
{
  public class UpdateAuthCommandConsumer(AuthDbContext dbContext) : IConsumer<UpdateAuthCommandRequest>
  {
    public async Task Consume(ConsumeContext<UpdateAuthCommandRequest> context)
    {
      var auth = await dbContext.Auths
              .SingleOrDefaultAsync(d => d.AuthID == Guid.Parse(context.Message.AuthID));

      if (auth != null)
      {
        if (!string.IsNullOrEmpty(context.Message.Email))
        {
          auth.Email = context.Message.Email;
        }

        if (!string.IsNullOrEmpty(context.Message.Password))
        {
          auth.Password = HashPasswordHelper.HashPassword(context.Message.Password);
        }

        auth.IsBlocked = context.Message.IsBlocked ?? auth.IsBlocked;
        auth.PasswordChangeRequestCode = context.Message.PasswordChangeRequestCode ?? auth.PasswordChangeRequestCode;
        if (context.Message.PasswordChangeRequestCode != null)
        {
          auth.PasswordChangeRequestDate = DateTime.UtcNow;
        }

        auth.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
      }

      await context.RespondAsync(new UpdateAuthCommandResponse(auth));
    }

  }
}