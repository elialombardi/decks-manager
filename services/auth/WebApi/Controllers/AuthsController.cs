using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features.Auths.Commands;
using WebApi.Services;
using Api.Application.Auths.Queries;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;
public class AuthsController(ILogger<AuthsController> logger, ISender sender) : ControllerBase
{

  [HttpGet("{id}")]
  [Authorize]
  public async Task<IActionResult> Get(string id)
  {
    try
    {
      var auth = await sender.Send(new GetAuthByIdQuery(Guid.Parse(id)));
      if (auth == null)
      {
        return NotFound();
      }
      return Ok(auth);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting auth");
      return StatusCode(500);
    }
  }

  [HttpPost("search")]
  [Authorize]
  public async Task<IActionResult> Search(SearchAuthsQuery command)
  {
    try
    {
      var auths = await sender.Send(command);
      return Ok(auths);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting auth");
      return StatusCode(500);
    }
  }

  [HttpPost("")]
  [Authorize]
  public async Task<ActionResult> Post(CreateAuthCommand command)
  {
    try
    {
      var id = await sender.Send(command);
      return Ok(id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating auth");
      return StatusCode(500);
    }
  }
  [HttpPut("")]
  [Authorize]
  public async Task<ActionResult> Put(UpdateAuthCommand command)
  {
    try
    {
      var id = await sender.Send(command);
      return Ok(id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating auth");
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
      var deletedAuthID = await sender.Send(new DeleteAuthCommand(id));
      return Ok(deletedAuthID);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating auth");
      return StatusCode(500);
    }
  }
}