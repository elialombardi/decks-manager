using Microsoft.EntityFrameworkCore;
using Api.Data;
using MassTransit;
using Api.Features.Users.Queries;

namespace Api.Features.Users.Queries
{
    public class SearchUsersQueryConsumer(UsersDbContext dbContext) : IConsumer<SearchUsersQueryRequest>
    {
        public async Task Consume(ConsumeContext<SearchUsersQueryRequest> context)
        {
            var query = dbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(context.Message.Username))
                query = query.Where(p => p.Username.Contains(context.Message.Username));

            var users = await query.ToListAsync();

            await context.RespondAsync(new SearchUsersQueryResponse(users));
        }
    }
}