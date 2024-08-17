using MassTransit;

namespace Api.Features.Subscriber.Messages
{
  [EntityName(nameof(SubscribeToNewsletterMessage))]
  [MessageUrn(nameof(SubscribeToNewsletterMessage))]
  public record SubscribeToNewsletterMessage(string Email);
}