using Api.Features.Roles.Dtos;
using Api.Data.Models;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Api.Features.Roles.Publishers
{
  public class RolePublisher(ILogger<RolePublisher> logger, IPublishEndpoint bus, IMapper mapper) : IRolePublisher
  {
    public async Task Publish(Role role)
    {
      logger.LogDebug("Publishing role {RoleID}", role.RoleID);

      var publishedRole = mapper.Map<PublishedRole>(role);

      await bus.Publish(publishedRole);
    }
  }
}