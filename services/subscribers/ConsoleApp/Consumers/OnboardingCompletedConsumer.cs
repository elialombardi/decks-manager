using Api.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Consumers;

public class OnboardingCompletedConsumer(ILogger<OnboardingCompletedConsumer> logger) : IConsumer<CompleteOnboarding>
{
  public Task Consume(ConsumeContext<CompleteOnboarding> context)
  {
    logger.LogInformation("OnboardingCompletedMessage for {Email}", context.Message.Email);

    return Task.CompletedTask;

  }
}