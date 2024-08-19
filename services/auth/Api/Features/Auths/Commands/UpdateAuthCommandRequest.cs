namespace Api.Features.Auths.Commands
{
  public record UpdateAuthCommandRequest(string AuthID, string? Email = null, string? Password = null, bool? IsBlocked = false, bool? IsEmailConfirmed = false, string? PasswordChangeRequestCode = null, DateTime? PasswordChangeRequestDate = null);
}