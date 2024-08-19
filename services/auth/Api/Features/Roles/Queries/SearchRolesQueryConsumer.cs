using Api.Data;
using Api.Data.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Roles.Queries
{
  public class SearchRolesQueryConsumer(AuthDbContext dbContext) : IConsumer<SearchRolesQueryRequest>
  {
    public async Task Consume(ConsumeContext<SearchRolesQueryRequest> context)
    {
      var roles = await dbContext.Roles.ToListAsync();

      await context.RespondAsync(new SearchRolesQueryResponse(roles));
    }

  }
}