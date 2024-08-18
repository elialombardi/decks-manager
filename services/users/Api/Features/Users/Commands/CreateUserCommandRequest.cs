namespace Api.Features.Users.Commands
{
  public record CreateUserCommandRequest(string Email, string? Username = null);
}