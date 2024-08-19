using System.Security.Claims;
using Api.Data.Models;

namespace Api.Features.Auths.Commands
{
  public record DeleteAuthCommandResponse(Auth? Auth);
}