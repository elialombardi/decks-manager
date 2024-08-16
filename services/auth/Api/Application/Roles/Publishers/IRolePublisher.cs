using Api.Data.Models;

namespace Api.Application.Roles.Publishers
{
  public interface IRolePublisher
  {
    public Task Publish(Role role);
  }
}