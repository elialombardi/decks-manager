namespace ConsoleApp.Services
{
  public interface IEmailService
  {
    Task SendFollowUpEmailAsync(string email);
    public Task SendWelcomeEmailAsync(string email);

  }
}