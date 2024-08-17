namespace Api.Features.Subscriber.Events
{
  public class WelcomeEmailSent
  {
    public Guid SubscriberId { get; set; }
    public required string Email { get; set; }
  }


}