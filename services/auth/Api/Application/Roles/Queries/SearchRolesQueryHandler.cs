using Api.Data;
using Api.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Roles.Queries
{
  public class SearchRolesQueryHandler(AuthDbContext context) : IRequestHandler<SearchRolesQuery, List<Role>>
  {
    public async Task<List<Role>> Handle(SearchRolesQuery request, CancellationToken cancellationToken)
    {
      return await context.Roles.ToListAsync(cancellationToken: cancellationToken);
    }
  }
}