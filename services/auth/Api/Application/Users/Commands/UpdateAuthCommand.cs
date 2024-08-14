using Api.Data.Models;
using MediatR;

namespace Api.Features.Auths.Commands
{
  public record UpdateAuthCommand(string AuthID, string? Email = null, string? Password = null, bool? IsBlocked = false, bool? IsEmailConfirmed = false, string? PasswordChangeRequestCode = null, DateTime? PasswordChangeRequestDate = null) : IRequest<Auth?>;
}