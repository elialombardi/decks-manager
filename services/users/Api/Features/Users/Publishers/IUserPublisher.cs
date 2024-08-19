using Api.Data.Models;

namespace Api.Features.Users.Publishers
{
  public interface IUserPublisher
  {
    public Task Publish(User user);
  }
}