using Api.Features.Subscriber.Events;
using Api.Features.Subscriber.Messages;
using ConsoleApp.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Consumers;

public class SendWelcomeEmailConsumer(ILogger<SendWelcomeEmailConsumer> logger, IEmailService emailService) : IConsumer<SendWelcomeEmailMessage>
{
  public async Task Consume(ConsumeContext<SendWelcomeEmailMessage> context)
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
