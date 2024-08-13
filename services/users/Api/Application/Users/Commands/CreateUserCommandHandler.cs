using MediatR;
using Api.Data;
using Api.Data.Models;

namespace Api.Features.Users.Commands
{
  public class CreateUserCommandHandler(UsersDbContext context) : IRequestHandler<CreateUserCommand, Guid>
  {
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
      var user = new User(Guid.NewGuid(),
        request.UserName,
        request.Email,
       DateTime.UtcNow,
       DateTime.UtcNow,
       null);

      context.Users.Add(user);

      await context.SaveChangesAsync(cancellationToken);

      return user.UserID;
    }
  }
}