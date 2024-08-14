using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features.Users.Commands;
using WebApi.Services;
using Api.Application.Users.Queries;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

[Route("[controller]")]
public class UsersController(ILogger<UsersController> logger, ISender sender) : ControllerBase
{

  [HttpGet("{id}")]
  [Authorize]
  public async Task<IActionResult> Get(string id)
  {
    try
    {
      var user = await sender.Send(new GetUserByIdQuery(Guid.Parse(id)));
      if (user == null)
      {
        return NotFound();
      }
      return Ok(user);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting user");
      return StatusCode(500);
    }
  }

  [HttpPost("search")]
  [Authorize]
  public async Task<IActionResult> Search([FromBody] SearchUsersQuery command)
  {
    try
    {
      var users = await sender.Send(command);
      return Ok(users);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting user");
      return StatusCode(500);
    }
  }

  [HttpPost("")]
  [Authorize]
  // [Authorize(Roles = "microservice,admin")]
  public async Task<ActionResult> Post([FromBody] CreateUserCommand command)
  {
    try
    {
      var user = await sender.Send(command);
      return Ok(user);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating user");
      return StatusCode(500);
    }
  }
  [HttpPut("")]
  [Authorize]
  public async Task<ActionResult> Put([FromBody] UpdateUserCommand command)
  {
    try
    {
      var id = await sender.Send(command);
      return Ok(id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating user");
      return StatusCode(500);
    }
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "admin")]
  // [Authorize(Roles = "admin,manager")]
  public async Task<ActionResult> Post(string id)
  {
    try
    {
      var deletedUserID = await sender.Send(new DeleteUserCommand(id));
      return Ok(deletedUserID);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating user");
      return StatusCode(500);
    }
  }
}