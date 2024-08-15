using Api.Data.Models;

namespace Api.Application.Users.Publishers
{
  public interface IUserPublisher
  {
    public Task Publish(User user);
  }
}