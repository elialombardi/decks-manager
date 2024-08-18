namespace Api.Features.Users.Commands
{
  public record UpdateUserCommandRequest(string UserID, string UserName, string Email);
}