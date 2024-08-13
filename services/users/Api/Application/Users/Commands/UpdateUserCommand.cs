using Api.Data.Models;
using MediatR;

namespace Api.Features.Users.Commands
{
  public record UpdateUserCommand(string UserID, string UserName, string Email) : IRequest<User?>;
}