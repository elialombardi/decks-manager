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
       request.UserID,
       request.Name,
       request.Description,
       request.ImageUrl,
       request.Cards.Select(c => new Card(Guid.NewGuid(), c.UserID, c.Name, c.Color, DateTime.UtcNow, DateTime.UtcNow, null)).ToList(),
       DateTime.UtcNow,
       DateTime.UtcNow,
       null);

      context.Users.Add(user);

      await context.SaveChangesAsync(cancellationToken);

      return user.UserID;
    }
  }
}