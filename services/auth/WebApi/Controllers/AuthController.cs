using Microsoft.AspNetCore.Mvc;
using Api.Features.Auths.Commands;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using Api.Features.Common;
using Api.Features.Roles.Queries;
using Api.Features.Roles.Publishers;
using MassTransit;

namespace WebApi.Controllers;

[Route("auth")]
public class AuthController(ILogger<AuthController> logger, IScopedClientFactory clientFactory, IAuthService authService, IRolePublisher rolePublisher, IConfiguration configuration) : ControllerBase
{

  [HttpPost("login")]
  [AllowAnonymous]
  public async Task<IActionResult> Login([FromBody] LoginCommandRequest command)
  {

    var client = clientFactory.CreateRequestClient<LoginCommandRequest>();

    var response = await client.GetResponse<LoginCommandResponse>(command);
    var auth = response.Message.Auth;

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
  public async Task<IActionResult> Signup([FromBody] CreateAuthCommandRequest command)
  {
    // If is external user signin up, set role to user
    if (HttpContext.User?.Identity == null || !HttpContext.User.Identity.IsAuthenticated || !HttpContext.User.IsInRole(Roles.Admin.ToString().ToLowerInvariant()))
    {
      command = command with { RoleID = Convert.ToByte(Roles.User) };
    }

    var client = clientFactory.CreateRequestClient<CreateAuthCommandRequest>();

    var response = await client.GetResponse<CreateAuthCommandResponse>(command);
    var auth = response.Message.Auth;

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
    var client = clientFactory.CreateRequestClient<SearchRolesQueryRequest>();

    var response = await client.GetResponse<SearchRolesQueryResponse>(new SearchRolesQueryRequest());

    return Ok(response.Message.Roles);
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

    var command = new CreateAuthCommandRequest("elia.lombardi@outlook.it", "Test.123", Convert.ToByte(Roles.Admin));

    var client = clientFactory.CreateRequestClient<CreateAuthCommandRequest>();

    var response = await client.GetResponse<CreateAuthCommandResponse>(command);
    var auth = response.Message.Auth;

    if (auth == null)
    {
      return NotFound();
    }

    Response.Headers.Append("Authorization", $"Bearer {authService.GenerateJwtToken(auth.UserID, auth.Role.Name.ToLowerInvariant())}");

    return Ok(auth);
  }
}