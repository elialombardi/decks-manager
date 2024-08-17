namespace Api.Features.Subscriber.Events
{
  public class OnboardingCompleted
  {
    public Guid SubscriberId { get; set; }
    public required string Email { get; set; }
  }
}