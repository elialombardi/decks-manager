using Api.Data.Models;
using MediatR;

namespace Api.Application.Roles.Queries
{
  public record SearchRolesQuery() : IRequest<List<Role>>;
}