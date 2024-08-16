
using Api.Application.Common;
using Api.Application.Roles.Publishers;
using Api.Features.Auths.Commands;
using Api.Proxies;
using FluentValidation;
using MediatR;
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

    services
        .AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Dependencies).Assembly));

    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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
}
