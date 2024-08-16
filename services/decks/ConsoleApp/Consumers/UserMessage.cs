using MassTransit;

namespace ConsoleApp.Consumers
{
  [EntityName("user")]
  [MessageUrn("user")]
  public record UserMessage(string UserID, DateTime? DeletedAt);
}