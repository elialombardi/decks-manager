using Api.Features.Subscriber.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Consumers;

public class OnboardingCompletedConsumer(ILogger<OnboardingCompletedConsumer> logger) : IConsumer<OnboardingCompletedMessage>
{
  public Task Consume(ConsumeContext<OnboardingCompletedMessage> context)
  {
    logger.LogInformation("OnboardingCompletedMessage for {Email}", context.Message.Email);

    return Task.CompletedTask;

  }
}