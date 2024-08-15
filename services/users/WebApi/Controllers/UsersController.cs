using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features.Users.Commands;
using WebApi.Services;
using Api.Application.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Api.Application.Users.Publishers;

namespace WebApi.Controllers;

[Route("[controller]")]
public class UsersController(ILogger<UsersController> logger, ISender sender, IAuthService authService, IUserPublisher userPublisher) : ControllerBase
{

  [HttpGet("{id}")]
  [Authorize]
  public async Task<IActionResult> Get(string id)
  {
    logger.LogDebug("Getting user {UserID}", id);

    var user = await sender.Send(new GetUserByIdQuery(Guid.Parse(id)));
    if (user == null)
    {
      return NotFound();
    }
    return Ok(user);
  }

  [HttpGet("current-user")]
  [Authorize]
  public async Task<IActionResult> GetCurrentUser(string id)
  {
    var user = await sender.Send(new GetUserByIdQuery(Guid.Parse(authService.GetUserId())));
    if (user == null)
    {
      return NotFound();
    }
    return Ok(user);
  }

  [HttpPost("search")]
  [Authorize]
  public async Task<IActionResult> Search([FromBody] SearchUsersQuery command)
  {
    var users = await sender.Send(command);
    return Ok(users);
  }

  [HttpPost("")]
  [Authorize(Roles = "microservice,admin")]
  public async Task<ActionResult> Post([FromBody] CreateUserCommand command)
  {
    var user = await sender.Send(command);

    await userPublisher.Publish(user);

    return Ok(user);
  }
  [HttpPut("")]
  [Authorize]
  public async Task<ActionResult> Put([FromBody] UpdateUserCommand command)
  {
    var user = await sender.Send(command);

    if (user == null)
    {
      return NotFound();
    }

    await userPublisher.Publish(user);

    return Ok(user);
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "admin")]
  public async Task<ActionResult> Delete(string id)
  {
    var user = await sender.Send(new DeleteUserCommand(id));

    if (user == null)
    {
      return NotFound();
    }

    await userPublisher.Publish(user);

    return Ok(user);
  }
}