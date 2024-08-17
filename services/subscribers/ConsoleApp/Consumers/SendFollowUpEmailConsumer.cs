using Api.Features.Subscriber.Events;
using Api.Features.Subscriber.Messages;
using ConsoleApp.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Consumers;

public class SendFollowUpEmailConsumer(ILogger<SendFollowUpEmailConsumer> logger, IEmailService emailService) : IConsumer<SendFollowUpEmailMessage>
{
  public async Task Consume(ConsumeContext<SendFollowUpEmailMessage> context)
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
