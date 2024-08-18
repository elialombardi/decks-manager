using Api.Events;
using ConsoleApp.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Consumers;

public class SendFollowUpEmailConsumer(ILogger<SendFollowUpEmailConsumer> logger, IEmailService emailService) : IConsumer<SendFollowUpEmail>
{
  public async Task Consume(ConsumeContext<SendFollowUpEmail> context)
  {
    logger.LogInformation("SendFollowUpEmailMessage for {Email}", context.Message.Email);

    await emailService.SendFollowUpEmailAsync(context.Message.Email);

    await context.Publish(new FollowUpEmailSent()
    {
      SubscriberId = context.Message.SubscriberId,
      Email = context.Message.Email
    });

  }
}
