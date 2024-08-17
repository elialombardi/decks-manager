namespace Api.Features.Subscriber.Events
{
  public class SubscriberCreated
  {
    public Guid SubscriberId { get; set; }
    public required string Email { get; set; }
  }


}