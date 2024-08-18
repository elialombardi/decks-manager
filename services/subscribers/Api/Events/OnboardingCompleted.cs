using MassTransit;

namespace Api.Events
{
  [EntityName(nameof(OnboardingCompleted))]
  [MessageUrn(nameof(OnboardingCompleted))]
  public class OnboardingCompleted
  {
    public Guid SubscriberId { get; set; }
    public required string Email { get; set; }
  }
}