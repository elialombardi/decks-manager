using MassTransit;

namespace Api.Features.Users.Queries
{
  public record GetUserByIdQueryRequest(Guid Id);
}