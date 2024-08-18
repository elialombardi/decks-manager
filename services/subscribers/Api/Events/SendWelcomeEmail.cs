using MassTransit;

namespace Api.Events
{
  [EntityName(nameof(SendWelcomeEmail))]
  [MessageUrn(nameof(SendWelcomeEmail))]
  public record SendWelcomeEmail(Guid SubscriberId, string Email);
}