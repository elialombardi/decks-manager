
using Api.Features.Auths.Commands;
using Api.Features.Roles.Publishers;
using Api.Proxies;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

public static class Dependencies
{
  public static IServiceCollection RegisterRequestHandlers(
      this IServiceCollection services, IConfiguration configuration)
  {

    services.AddValidatorsFromAssembly(typeof(Dependencies).Assembly);

    services.AddAutoMapper(typeof(Dependencies).Assembly);

    services.AddScoped<IRolePublisher, RolePublisher>();

    services.AddHttpClient<IUserProxy, UserProxy>(client =>
    {
      client.BaseAddress = new Uri(configuration.GetValue<string>("UserService:BaseAddress") ?? string.Empty);
    })
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

    return services;
  }
  static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
  {
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
  }

  static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
  {
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
  }

  public static IBusRegistrationConfigurator RegisterRequestsClients(
     this IBusRegistrationConfigurator brc)
  {
    brc.AddRequestClient<CreateAuthCommandRequest>(new Uri($"queue:{nameof(CreateAuthCommandRequest)}"));
    brc.AddRequestClient<DeleteAuthCommandRequest>(new Uri($"queue:{nameof(DeleteAuthCommandRequest)}"));
    brc.AddRequestClient<LoginCommandRequest>(new Uri($"queue:{nameof(LoginCommandRequest)}"));
    brc.AddRequestClient<UpdateAuthCommandRequest>(new Uri($"queue:{nameof(UpdateAuthCommandRequest)}"));

    return brc;
  }

  public static IBusRegistrationConfigurator RegisterRequestsConsumers(
    this IBusRegistrationConfigurator brc)
  {
    brc.AddConsumer<CreateAuthCommandConsumer>()
        .Endpoint(e => e.Name = nameof(CreateAuthCommandRequest));
    brc.AddConsumer<DeleteAuthCommandConsumer>()
        .Endpoint(e => e.Name = nameof(DeleteAuthCommandRequest));
    brc.AddConsumer<LoginCommandConsumer>()
        .Endpoint(e => e.Name = nameof(LoginCommandRequest));
    brc.AddConsumer<UpdateAuthCommandConsumer>()
        .Endpoint(e => e.Name = nameof(UpdateAuthCommandRequest));

    return brc;
  }
}
