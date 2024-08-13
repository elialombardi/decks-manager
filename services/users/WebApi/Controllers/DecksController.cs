using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features.Users.Commands;
using WebApi.Services;
using Api.Application.Users.Queries;

namespace WebApi.Controllers;

public class UsersController(ILogger<UsersController> logger, ISender sender, IAuthService authService) : ControllerBase
{

  [HttpGet("{id}")]
  public async Task<IActionResult> Get(string id)
  {
    try
    {

      var user = await sender.Send(new GetUserByIdQuery(Guid.Parse(id), authService.GetUserId()));
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
  public async Task<IActionResult> Search(SearchUsersQuery command)
  {
    try
    {
      var users = await sender.Send(command with { UserID = authService.GetUserId() });
      return Ok(users);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting user");
      return StatusCode(500);
    }
  }

  [HttpPost("")]
  public async Task<ActionResult> Post(CreateUserCommand command)
  {
    try
    {
      var id = await sender.Send(command with { UserID = authService.GetUserId() });
      return Ok(id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating user");
      return StatusCode(500);
    }
  }
  [HttpPut("")]
  public async Task<ActionResult> Put(UpdateUserCommand command)
  {
    try
    {
      var id = await sender.Send(command with { UserID = authService.GetUserId() });
      return Ok(id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating user");
      return StatusCode(500);
    }
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Post(string id)
  {
    try
    {
      var deletedUserID = await sender.Send(new DeleteUserCommand(id, authService.GetUserId()));
      return Ok(deletedUserID);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating user");
      return StatusCode(500);
    }
  }
}