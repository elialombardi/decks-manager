using MediatR;
using Api.Data;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Api.Application.Common;

namespace Api.Features.Auths.Commands
{
  public class LoginCommandHandler(AuthDbContext context) : IRequestHandler<UpdateAuthCommand, Auth?>
  {
    public async Task<Auth?> Handle(UpdateAuthCommand request, CancellationToken cancellationToken)
    {
      var auth = await context.Auths
        .SingleOrDefaultAsync(d => d.AuthID == Guid.Parse(request.AuthID), cancellationToken);

      if (auth == null)
      {
        return null;
      }

      if (!string.IsNullOrEmpty(request.Email))
      {
        auth.Email = request.Email;
      }

      if (!string.IsNullOrEmpty(request.Password))
      {
        auth.Password = HashPasswordHelper.HashPassword(request.Password);
      }

      auth.IsBlocked = request.IsBlocked ?? auth.IsBlocked;

      auth.IsEmailConfirmed = request.IsEmailConfirmed ?? auth.IsEmailConfirmed;

      auth.PasswordChangeRequestCode = request.PasswordChangeRequestCode ?? auth.PasswordChangeRequestCode;
      if (request.PasswordChangeRequestCode != null)
      {
        auth.PasswordChangeRequestDate = DateTime.UtcNow;
      }

      auth.UpdatedAt = DateTime.UtcNow;

      await context.SaveChangesAsync(cancellationToken);

      return auth;
    }
  }

  public class UpdateAuthCommandHandler(AuthDbContext context) : IRequestHandler<UpdateAuthCommand, Auth?>
  {
    public async Task<Auth?> Handle(UpdateAuthCommand request, CancellationToken cancellationToken)
    {
      var auth = await context.Auths
        .SingleOrDefaultAsync(d => d.AuthID == Guid.Parse(request.AuthID), cancellationToken);

      if (auth == null)
      {
        return null;
      }

      if (!string.IsNullOrEmpty(request.Email))
      {
        auth.Email = request.Email;
      }

      if (!string.IsNullOrEmpty(request.Password))
      {
        auth.Password = HashPasswordHelper.HashPassword(request.Password);
      }

      auth.IsBlocked = request.IsBlocked ?? auth.IsBlocked;
      auth.PasswordChangeRequestCode = request.PasswordChangeRequestCode ?? auth.PasswordChangeRequestCode;
      if (request.PasswordChangeRequestCode != null)
      {
        auth.PasswordChangeRequestDate = DateTime.UtcNow;
      }

      auth.UpdatedAt = DateTime.UtcNow;

      await context.SaveChangesAsync(cancellationToken);

      return auth;
    }
  }
}