using MediatR;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Data.Models;

namespace Api.Application.Users.Queries
{
    public class SearchUsersQueryHandler(UsersDbContext context) : IRequestHandler<SearchUsersQuery, List<User>>
    {
        public async Task<List<User>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var query = context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(p => p.Name.Contains(request.Name));

            return await query.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}