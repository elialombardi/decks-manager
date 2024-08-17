using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MassTransit;
using Api.Features.Subscriber.Events;
using WebApi.Dto;
using Api.Features.Subscriber.Messages;

namespace WebApi.Controllers;

[Route("[controller]")]
public class SubscribersController(ILogger<SubscribersController> logger, IPublishEndpoint bus) : ControllerBase
{
  [HttpPost("")]
  [AllowAnonymous]
  public async Task<ActionResult> Post([FromBody] PostSubscriberData data)
  {
    logger.LogInformation("Creating subscriber with email {Email}", data.Email);

    await bus.Publish(new SubscribeToNewsletterMessage(data.Email));

    return Ok();
  }
}
