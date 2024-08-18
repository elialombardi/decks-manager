using Microsoft.EntityFrameworkCore;
using Api.Data;
using MassTransit;

namespace Api.Application.Users.Queries
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