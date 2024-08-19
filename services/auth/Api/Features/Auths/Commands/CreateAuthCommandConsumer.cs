using Api.Data;
using Api.Data.Models;
using Api.Proxies;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Api.Features.Common;

namespace Api.Features.Auths.Commands
{
  public class CreateAuthCommandConsumer(AuthDbContext dbContext, IUserProxy userProxy) : IConsumer<CreateAuthCommandRequest>
  {
    public async Task Consume(ConsumeContext<CreateAuthCommandRequest> context)
    {
      var userID = await userProxy.CreateUser(context.Message.Email);

      if (string.IsNullOrEmpty(userID))
        throw new Exception("Failed to create user");

      var auth = new Auth()
      {
        AuthID = Guid.NewGuid(),
        UserID = userID,
        RoleID = context.Message.RoleID ?? Convert.ToByte(Common.Roles.User),
        Email = context.Message.Email,
        Password = HashPasswordHelper.HashPassword(context.Message.Password),
        IsBlocked = false,
        IsEmailConfirmed = false,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      dbContext.Auths.Add(auth);

      await dbContext.SaveChangesAsync();

      auth.Role = await dbContext.Roles.FirstAsync(r => r.RoleID == auth.RoleID);

      await context.RespondAsync(new CreateAuthCommandResponse(auth));
    }
  }
}