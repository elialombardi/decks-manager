
using Api.Features.Users.Commands;
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
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    services.AddTransient<AbstractValidator<CreateUserCommand>, CreateUserCommandValidator>();
    return services;
  }
}
