
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Dependencies
{
  public static IServiceCollection RegisterRequestHandlers(
      this IServiceCollection services, IConfiguration configuration)
  {
    services
        .AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Dependencies).Assembly));

    return services;
  }
}
