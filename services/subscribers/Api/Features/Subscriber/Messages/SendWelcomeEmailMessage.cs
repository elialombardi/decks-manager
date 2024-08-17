using MassTransit;

namespace Api.Features.Subscriber.Messages
{
  [EntityName(nameof(SendWelcomeEmailMessage))]
  [MessageUrn(nameof(SendWelcomeEmailMessage))]
  public record SendWelcomeEmailMessage(Guid SubscriberId, string Email);
}