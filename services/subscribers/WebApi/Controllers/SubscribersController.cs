using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MassTransit;
using Api.Features.Subscriber.Events;

namespace WebApi.Controllers;

[Route("[controller]")]
public class SubscribersControler(ILogger<SubscribersControler> logger, IPublishEndpoint bus) : ControllerBase
{
  [HttpPost("")]
  [AllowAnonymous]
  public async Task<ActionResult> Post([FromBody] string email)
  {
    logger.LogInformation("Creating subscriber with email {Email}", email);

    await bus.Publish(new SubscriberCreated() { Email = email });

    return Ok(email);
  }
}