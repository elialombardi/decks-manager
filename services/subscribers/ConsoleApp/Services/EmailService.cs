using Microsoft.Extensions.Logging;

namespace ConsoleApp.Services
{
  public class EmailService(ILogger<EmailService> logger) : IEmailService
  {
    public async Task SendFollowUpEmailAsync(string email)
    {
      logger.LogDebug($"Sending follow up email to {email}");

      await Task.Delay(1000);

      logger.LogDebug($"follow up email sent to {email}");
    }

    public async Task SendWelcomeEmailAsync(string email)
    {
      logger.LogDebug($"Sending welcome email to {email}");

      await Task.Delay(1000);

      logger.LogDebug($"Welcome email sent to {email}");
    }
  }
}