using MassTransit;

namespace Api.Events
{
  [EntityName(nameof(WelcomeEmailSent))]
  [MessageUrn(nameof(WelcomeEmailSent))]
  public class WelcomeEmailSent
  {
    public Guid SubscriberId { get; set; }
    public required string Email { get; set; }
  }


}