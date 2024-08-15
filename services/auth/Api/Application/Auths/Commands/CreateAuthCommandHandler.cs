using MediatR;
using Api.Data;
using Api.Data.Models;
using Api.Application.Common;
using Api.Proxies;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auths.Commands
{
  public class CreateAuthCommandHandler(AuthDbContext context, IUserProxy userProxy) : IRequestHandler<CreateAuthCommand, Auth>
  {
    public async Task<Auth> Handle(CreateAuthCommand request, CancellationToken cancellationToken)
    {
      var userID = await userProxy.CreateUser(request.Email);

      if (string.IsNullOrEmpty(userID))
        throw new Exception("Failed to create user");

      var auth = new Auth()
      {
        AuthID = Guid.NewGuid(),
        UserID = userID,
        RoleID = request.RoleID ?? Convert.ToByte(Roles.User),
        Email = request.Email,
        Password = HashPasswordHelper.HashPassword(request.Password),
        IsBlocked = false,
        IsEmailConfirmed = false,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      context.Auths.Add(auth);

      await context.SaveChangesAsync(cancellationToken);

      auth.Role = await context.Roles.FirstAsync(r => r.RoleID == auth.RoleID, cancellationToken);

      return auth;
    }
  }
}