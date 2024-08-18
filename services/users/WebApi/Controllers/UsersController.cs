using Microsoft.AspNetCore.Mvc;
using Api.Features.Users.Commands;
using WebApi.Services;
using Api.Application.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Api.Application.Users.Publishers;
using MassTransit;

namespace WebApi.Controllers;

[Route("[controller]")]
public class UsersController(ILogger<UsersController> logger, IScopedClientFactory clientFactory, IAuthService authService) : ControllerBase
{

  [HttpGet("{id}")]
  [Authorize]
  public async Task<IActionResult> Get(string id)
  {
    logger.LogDebug("Getting user {UserID}", id);

    // var user = await sender.Send(new GetUserByIdQuery(Guid.Parse(id)));

    var serviceAddress = new Uri("queue:get-order-status"); // Replace with your actual queue address
    var client = clientFactory.CreateRequestClient<GetUserByIdQueryRequest>(serviceAddress);

    var request = new GetUserByIdQueryRequest(Guid.Parse(id));

    var response = await client.GetResponse<GetUserByIdQueryResponse>(request);
    return response.Message.User == null ? NotFound() : Ok(response.Message.User);


  }

  [HttpGet("current-user")]
  [Authorize]
  public async Task<IActionResult> GetCurrentUser(string id)
  {
    var serviceAddress = new Uri("queue:get-order-status"); // Replace with your actual queue address
    var client = clientFactory.CreateRequestClient<GetUserByIdQueryRequest>(serviceAddress);

    var request = new GetUserByIdQueryRequest(Guid.Parse(authService.GetUserId()));

    var response = await client.GetResponse<GetUserByIdQueryResponse>(request);
    return response.Message.User == null ? NotFound() : Ok(response.Message.User);
  }

  [HttpPost("search")]
  [Authorize]
  public async Task<IActionResult> Search([FromBody] SearchUsersQueryRequest command)
  {
    var serviceAddress = new Uri("queue:get-order-status"); // Replace with your actual queue address
    var client = clientFactory.CreateRequestClient<SearchUsersQueryRequest>(serviceAddress);

    var response = await client.GetResponse<SearchUsersQueryResponse>(command);
    return Ok(response.Message.Users);
  }

  [HttpPost("")]
  [Authorize(Roles = "microservice,admin")]
  public async Task<ActionResult> Post([FromBody] CreateUserCommandRequest command)
  {
    var serviceAddress = new Uri("queue:get-order-status"); // Replace with your actual queue address
    var client = clientFactory.CreateRequestClient<CreateUserCommandRequest>(serviceAddress);


    var response = await client.GetResponse<CreateUserCommandResponse>(command);
    return Ok(response.Message.User);
  }
  [HttpPut("")]
  [Authorize]
  public async Task<ActionResult> Put([FromBody] UpdateUserCommandRequest command)
  {
    var serviceAddress = new Uri("queue:get-order-status"); // Replace with your actual queue address
    var client = clientFactory.CreateRequestClient<UpdateUserCommandRequest>(serviceAddress);

    var response = await client.GetResponse<GetUserByIdQueryResponse>(command);
    return response.Message.User == null ? NotFound() : Ok(response.Message.User);
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "admin")]
  public async Task<ActionResult> Delete(string id)
  {
    var serviceAddress = new Uri("queue:get-order-status"); // Replace with your actual queue address
    var client = clientFactory.CreateRequestClient<UpdateUserCommandRequest>(serviceAddress);
    var request = new DeleteUserCommandRequest(id);

    var response = await client.GetResponse<GetUserByIdQueryResponse>(request);
    return response.Message.User == null ? NotFound() : Ok(response.Message.User);
  }
}