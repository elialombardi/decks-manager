using Api.Data.Models;

namespace Api.Features.Roles.Publishers
{
  public interface IRolePublisher
  {
    public Task Publish(Role role);
  }
}