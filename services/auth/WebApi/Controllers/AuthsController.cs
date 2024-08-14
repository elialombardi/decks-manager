using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features.Auths.Commands;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using WebApi.Services;
using Api.Application.Common;
using Api.Application.Roles.Queries;

namespace WebApi.Controllers;

[Route("auth")]
public class AuthsController(ILogger<AuthsController> logger, ISender sender, IAuthService authService) : ControllerBase
{

  [HttpPost("login")]
  [AllowAnonymous]
  public async Task<IActionResult> Login([FromBody] LoginCommand command)
  {
    try
    {
      var auth = await sender.Send(command);
      if (auth == null)
      {
        return NotFound();
      }
      Response.Headers.Append("Authorization", $"Bearer {authService.GenerateJwtToken(auth.UserID, auth.Role.Name.ToLowerInvariant())}");

      return Ok(new { auth.UserID, auth.Role.Name });
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Login error");
      return StatusCode(500);
    }
  }

  [HttpPost("signup")]
  [AllowAnonymous]
  public async Task<IActionResult> Signup([FromBody] CreateAuthCommand command)
  {
    try
    {
      // If is external user signin up, set role to user
      if (HttpContext.User?.Identity == null || !HttpContext.User.Identity.IsAuthenticated || !HttpContext.User.IsInRole(Roles.Admin.ToString().ToLowerInvariant()))
      {
        command = command with { RoleID = Convert.ToByte(Roles.User) };
      }

      var auth = await sender.Send(command);
      if (auth == null)
      {
        return NotFound();
      }

      Response.Headers.Append("Authorization", $"Bearer {authService.GenerateJwtToken(auth.UserID, auth.Role.Name.ToLowerInvariant())}");

      return Ok(new { auth.UserID, auth.Role.Name });
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Login error");
      return StatusCode(500);
    }
  }

  [HttpGet("roles")]
  [AllowAnonymous]
  public async Task<IActionResult> GetRoles()
  {
    try
    {
      var roles = await sender.Send(new SearchRolesQuery());

      return Ok(roles);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting roles");
      return StatusCode(500);
    }
  }
}