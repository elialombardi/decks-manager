
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Dependencies
{
  public static IServiceCollection RegisterRequestHandlers(
      this IServiceCollection services, IConfiguration configuration)
  {
    services
        .AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Dependencies).Assembly));

    // Add automapper profile
    services.AddAutoMapper(typeof(Dependencies).Assembly);

    return services;
  }
}
