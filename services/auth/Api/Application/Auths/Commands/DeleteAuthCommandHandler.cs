using MediatR;
using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auths.Commands
{
  public class DeleteAuthCommandHandler(AuthDbContext context) : IRequestHandler<DeleteAuthCommand, Guid?>
  {
    public async Task<Guid?> Handle(DeleteAuthCommand request, CancellationToken cancellationToken)
    {

      var auth = await context.Auths
        .SingleOrDefaultAsync(d => d.AuthID == Guid.Parse(request.AuthID), cancellationToken);

      if (auth == null)
      {
        return null;
      }

      auth.DeletedAt = DateTime.UtcNow;

      await context.SaveChangesAsync(cancellationToken);

      return auth.AuthID;
    }
  }
}