using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MassTransit;
using WebApi.Dto;
using Api.Events;

namespace WebApi.Controllers;

[Route("[controller]")]
public class SubscribersController(ILogger<SubscribersController> logger, IPublishEndpoint bus) : ControllerBase
{
  [HttpPost("")]
  [AllowAnonymous]
  public async Task<ActionResult> Post([FromBody] PostSubscriberData data)
  {
    logger.LogInformation("Creating subscriber with email {Email}", data.Email);

    await bus.Publish(new SubscribeToNewsletter(data.Email));

    return Ok();
  }
}
