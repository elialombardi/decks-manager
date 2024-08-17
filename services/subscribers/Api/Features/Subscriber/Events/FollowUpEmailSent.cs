namespace Api.Features.Subscriber.Events
{

  public class FollowUpEmailSent
  {
    public Guid SubscriberId { get; set; }
    public required string Email { get; set; }
  }
}