using MediatR;
using Api.Data;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Api.Application.Common;
using Microsoft.Extensions.Configuration;

namespace Api.Features.Auths.Commands
{
  public class LoginCommandHandler(AuthDbContext context, IConfiguration configuration) : IRequestHandler<LoginCommand, Auth?>
  {
    public async Task<Auth?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
      var auth = await context.Auths
        .SingleOrDefaultAsync(d => d.Email == request.Email, cancellationToken);

      if (auth == null)
      {
        return null;
      }

      var result = HashPasswordHelper.VerifyHashedPassword(auth.Password, request.Password);

      if (result == HashPasswordHelper.PasswordVerificationResult.SuccessRehashNeeded)
      {
        auth.Password = HashPasswordHelper.HashPassword(request.Password);
      }
      else if (result == HashPasswordHelper.PasswordVerificationResult.Failed)
      {
        if (auth.LoginAttempts >= configuration.GetValue<int>("auth:MaxLoginAttempts"))
        {
          auth.IsBlocked = true;
        }

        auth.LoginAttempts++;
      }
      else
      {
        auth.LoginAttempts = 0;
        auth.LastLoginDate = DateTime.UtcNow;
      }

      await context.SaveChangesAsync(cancellationToken);

      return result == HashPasswordHelper.PasswordVerificationResult.Failed ? null : auth;
    }
  }
}