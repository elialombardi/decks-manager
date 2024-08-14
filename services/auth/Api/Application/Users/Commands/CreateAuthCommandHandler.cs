using MediatR;
using Api.Data;
using Api.Data.Models;
using Api.Application.Common;
using Api.Proxies;

namespace Api.Features.Auths.Commands
{
  public class CreateAuthCommandHandler(AuthDbContext context, IUserProxy userProxy) : IRequestHandler<CreateAuthCommand, Guid>
  {
    public async Task<Guid> Handle(CreateAuthCommand request, CancellationToken cancellationToken)
    {
      var userID = await userProxy.CreateUser(request.Email);

      if (string.IsNullOrEmpty(userID))
        throw new Exception("Failed to create user");

      var auth = new Auth()
      {
        AuthID = Guid.NewGuid(),
        UserID = userID,
        Email = request.Email,
        Password = HashPasswordHelper.HashPassword(request.Password),
        IsBlocked = false,
        IsEmailConfirmed = false,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      context.Auths.Add(auth);

      await context.SaveChangesAsync(cancellationToken);

      return auth.AuthID;
    }
  }
}