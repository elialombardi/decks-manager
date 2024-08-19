namespace Api.Features.Auths.Commands
{
  public record CreateAuthCommandRequest(string Email, string Password, byte? RoleID = null);
}