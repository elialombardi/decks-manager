using System.Security.Claims;
using Api.Data.Models;
using MediatR;

namespace Api.Features.Users.Commands
{
  public record DeleteUserCommand(string UserID) : IRequest<User?>;
}