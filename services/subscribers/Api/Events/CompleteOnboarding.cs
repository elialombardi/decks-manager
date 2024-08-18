using MassTransit;

namespace Api.Events
{

  [EntityName(nameof(CompleteOnboarding))]
  [MessageUrn(nameof(CompleteOnboarding))]
  public record CompleteOnboarding(Guid SubscriberId, string Email);
}