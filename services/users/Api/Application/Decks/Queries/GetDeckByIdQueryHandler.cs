using MediatR;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Data.Models;

namespace Api.Application.Users.Queries
{
    public class GetUserByIdQueryHandler(UsersDbContext context) : IRequestHandler<GetUserByIdQuery, User?>
    {
        public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.UserID == request.Id, cancellationToken: cancellationToken);
        }
    }
}