using System.Security.Claims;
using Api.Data.Models;
using MediatR;

namespace Api.Features.Auths.Commands
{
  public record CreateAuthCommand(string Email, string Password, byte? RoleID = null) : IRequest<Auth>;
}