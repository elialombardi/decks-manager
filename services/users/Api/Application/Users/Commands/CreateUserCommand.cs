using System.Security.Claims;
using Api.Data.Models;
using MediatR;

namespace Api.Features.Users.Commands
{
  public record CreateUserCommand(string Email, string? Username = null) : IRequest<User>;
}