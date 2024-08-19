using Microsoft.EntityFrameworkCore;
using Api.Data;
using MassTransit;
using Api.Features.Users.Queries;

namespace Api.Features.Users.Queries
{
    public class GetUserByIdQueryConsumer(UsersDbContext dbContext) : IConsumer<GetUserByIdQueryRequest>
    {
        public async Task Consume(ConsumeContext<GetUserByIdQueryRequest> context)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(p => p.UserID == context.Message.Id);

            await context.RespondAsync(new GetUserByIdQueryResponse(user));
        }
    }
}