using Api.Events;
using Api.Features.Subscriber.Commands;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Consumers;

public class SubcribeToNewsletterConsumer(ILogger<SubcribeToNewsletterConsumer> logger, ISender sender) : IConsumer<SubscribeToNewsletter>
{
  public async Task Consume(ConsumeContext<SubscribeToNewsletter> context)
  {
    var message = context.Message;

    var subscriber = await sender.Send(new SubscriberCreate(message.Email));

    if (subscriber is null)
    {
      logger.LogWarning("Subscriber not created for email {Email}", message.Email);
      return;
    }

    await context.Publish(new SubscriberCreated()
    {
      SubscriberId = subscriber.SubscriberID,
      Email = message.Email
    });
  }
}
