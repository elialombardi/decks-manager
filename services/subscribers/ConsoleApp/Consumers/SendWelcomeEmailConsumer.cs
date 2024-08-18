using Api.Events;
using ConsoleApp.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Consumers;

public class SendWelcomeEmailConsumer(ILogger<SendWelcomeEmailConsumer> logger, IEmailService emailService) : IConsumer<SendWelcomeEmail>
{
  public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
  {
    logger.LogInformation("SendWelcomeEmailMessage for {Email}", context.Message.Email);

    await emailService.SendWelcomeEmailAsync(context.Message.Email);

    await context.Publish(new WelcomeEmailSent()
    {
      SubscriberId = context.Message.SubscriberId,
      Email = context.Message.Email
    });

  }
}
