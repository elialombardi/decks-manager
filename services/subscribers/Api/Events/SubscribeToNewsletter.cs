using MassTransit;

namespace Api.Events
{
  [EntityName(nameof(SubscribeToNewsletter))]
  [MessageUrn(nameof(SubscribeToNewsletter))]
  public record SubscribeToNewsletter(string Email);
}