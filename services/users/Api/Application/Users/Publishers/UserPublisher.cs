using System.Reflection;
using Api.Application.Users.Dtos;
using Api.Data.Models;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Api.Application.Users.Publishers
{
  public class UserPublisher(ILogger<UserPublisher> logger, IPublishEndpoint bus, IMapper mapper, IConfiguration configuration) : IUserPublisher
  {
    public async Task Publish(User user)
    {
      logger.LogDebug("Publishing user {UserID}", user.UserID);

      var publishedUser = mapper.Map<PublishedUser>(user);

      await bus.Publish(publishedUser);
    }
  }
}