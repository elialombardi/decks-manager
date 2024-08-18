using Api.Sagas;
using MassTransit;

namespace Api.Sagas
{
  public class NewsletterOnboardingSagaDataDefinition : SagaDefinition<NewsletterOnboardingSagaData>
  {
    protected override void ConfigureSaga(IReceiveEndpointConfigurator configurator,
        ISagaConfigurator<NewsletterOnboardingSagaData> sagaConfigurator,
        IRegistrationContext context)
    {
      configurator.UseMessageRetry(r => r.Intervals(100, 1000, 2000, 5000));

      configurator.UseInMemoryOutbox(context);
    }
  }

}