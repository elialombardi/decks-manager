using MassTransit;

namespace Api.Events
{
  [EntityName(nameof(FollowUpEmailSent))]
  [MessageUrn(nameof(FollowUpEmailSent))]
  public class FollowUpEmailSent
  {
    public Guid SubscriberId { get; set; }
    public required string Email { get; set; }
  }
}