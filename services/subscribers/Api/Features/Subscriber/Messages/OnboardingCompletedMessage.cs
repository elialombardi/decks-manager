using MassTransit;

namespace Api.Features.Subscriber.Messages
{

  // [EntityName(nameof(OnboardingCompletedMessage))]
  [MessageUrn(nameof(OnboardingCompletedMessage))]
  public record OnboardingCompletedMessage(Guid SubscriberId, string Email);
}