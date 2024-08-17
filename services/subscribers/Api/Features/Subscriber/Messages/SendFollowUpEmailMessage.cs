using MassTransit;

namespace Api.Features.Subscriber.Messages
{
  [EntityName(nameof(SendFollowUpEmailMessage))]
  [MessageUrn(nameof(SendFollowUpEmailMessage))]
  public record SendFollowUpEmailMessage(Guid SubscriberId, string Email);
}