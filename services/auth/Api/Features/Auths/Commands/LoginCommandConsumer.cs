using Api.Data;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Api.Features.Common;
using Microsoft.Extensions.Configuration;
using MassTransit;

namespace Api.Features.Auths.Commands
{
  public class LoginCommandConsumer(AuthDbContext dbContext, IConfiguration configuration) : IConsumer<LoginCommandRequest>
  {
    public async Task Consume(ConsumeContext<LoginCommandRequest> context)
    {
      var auth = await dbContext.Auths
              .Include(d => d.Role)
              .SingleOrDefaultAsync(d => d.Email == context.Message.Email);

      if (auth != null)
      {
        var result = HashPasswordHelper.VerifyHashedPassword(auth.Password, context.Message.Password);

        if (result == HashPasswordHelper.PasswordVerificationResult.Failed)
        {
          if (auth.LoginAttempts >= configuration.GetValue<int>("auth:MaxLoginAttempts"))
          {
            auth.IsBlocked = true;
          }

          auth.LoginAttempts++;
        }
        else
        {
          if (result == HashPasswordHelper.PasswordVerificationResult.SuccessRehashNeeded)
          {
            auth.Password = HashPasswordHelper.HashPassword(context.Message.Password);
          }

          auth.LoginAttempts = 0;
          auth.LastLoginDate = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();

        auth = result == HashPasswordHelper.PasswordVerificationResult.Failed ? null : auth;
      }

      await context.RespondAsync(new LoginCommandResponse(auth));
    }
  }
}