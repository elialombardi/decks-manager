using MassTransit;

namespace Api.Events
{
  [EntityName(nameof(SubscriberCreated))]
  [MessageUrn(nameof(SubscriberCreated))]
  public class SubscriberCreated
  {
    public Guid SubscriberId { get; set; }
    public required string Email { get; set; }
  }


}