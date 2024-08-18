using MassTransit;

namespace Api.Events
{
  [EntityName(nameof(SendFollowUpEmail))]
  [MessageUrn(nameof(SendFollowUpEmail))]
  public record SendFollowUpEmail(Guid SubscriberId, string Email);
}