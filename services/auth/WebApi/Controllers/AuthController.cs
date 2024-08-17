using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features.Auths.Commands;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using WebApi.Services;
using Api.Application.Common;
using Api.Application.Roles.Queries;
using Api.Application.Roles.Publishers;

namespace WebApi.Controllers;

[Route("auth")]
public class AuthController(ILogger<AuthController> logger, ISender sender, IAuthService authService, IRolePublisher rolePublisher, IConfiguration configuration) : ControllerBase
{

  [HttpPost("login")]
  [AllowAnonymous]
  public async Task<IActionResult> Login([FromBody] LoginCommand command)
  {
    var auth = await sender.Send(command);
    if (auth == null)
    {
      return NotFound();
    }

    logger.LogDebug("User {userID} logged in", auth.UserID);

    Response.Headers.Append("Authorization", $"Bearer {authService.GenerateJwtToken(auth.UserID, auth.Role.Name.ToLowerInvariant())}");

    return Ok(new { auth.UserID, auth.Role });
  }

  [HttpPost("signup")]
  [AllowAnonymous]
  public async Task<IActionResult> Signup([FromBody] CreateAuthCommand command)
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

    return Ok(new { auth.UserID, auth.Role });
  }

  [HttpGet("roles")]
  [AllowAnonymous]
  public async Task<IActionResult> GetRoles()
  {
    var roles = await sender.Send(new SearchRolesQuery());

    return Ok(roles);
  }

  [HttpPost("roles")]
  [AllowAnonymous]
  public async Task<IActionResult> PostRole()
  {
    var role = new Api.Data.Models.Role(4, "Test");

    await rolePublisher.Publish(role);

    return Ok(role);
  }

  [HttpPost("admin")]
  [AllowAnonymous]
  public async Task<IActionResult> CreateAdmin()
  {
    // If is development environment, create admin user
    if (configuration["ASPNETCORE_ENVIRONMENT"] != "Development")
    {
      return NotFound();
    }

    var auth = await sender.Send(new CreateAuthCommand("elia.lombardi@outlook.it", "Test.123", Convert.ToByte(Roles.Admin)));
    if (auth == null)
    {
      return NotFound();
    }

    Response.Headers.Append("Authorization", $"Bearer {authService.GenerateJwtToken(auth.UserID, auth.Role.Name.ToLowerInvariant())}");

    return Ok(auth);
  }
}