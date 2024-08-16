using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features.Decks.Commands;
using WebApi.Services;
using Api.Application.Decks.Queries;
using Api.Application.Decks.Commands;

namespace WebApi.Controllers;

public class DecksController(ILogger<DecksController> logger, ISender sender, IAuthService authService) : ControllerBase
{

  [HttpGet("{id}")]
  public async Task<IActionResult> Get(string id)
  {
    try
    {

      var deck = await sender.Send(new GetDeckByIdQuery(Guid.Parse(id), authService.GetUserId()));
      if (deck == null)
      {
        return NotFound();
      }
      return Ok(deck);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting deck");
      return StatusCode(500);
    }
  }

  [HttpPost("search")]
  public async Task<IActionResult> Search(SearchDecksQuery command)
  {
    try
    {
      var decks = await sender.Send(command with { UserID = authService.GetUserId() });
      return Ok(decks);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting deck");
      return StatusCode(500);
    }
  }

  [HttpPost("")]
  public async Task<ActionResult> Post(CreateDeckCommand command)
  {
    try
    {
      var id = await sender.Send(command with { UserID = authService.GetUserId() });
      return Ok(id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating deck");
      return StatusCode(500);
    }
  }
  [HttpPut("")]
  public async Task<ActionResult> Put(UpdateDeckCommand command)
  {
    try
    {
      var id = await sender.Send(command with { UserID = authService.GetUserId() });
      return Ok(id);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating deck");
      return StatusCode(500);
    }
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Post(string id)
  {
    try
    {
      var deletedDeckID = await sender.Send(new DeleteDeckCommand(id, authService.GetUserId()));
      return Ok(deletedDeckID);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating deck");
      return StatusCode(500);
    }
  }
}