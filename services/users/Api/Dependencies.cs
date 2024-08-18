
using Api.Application.Common;
using Api.Application.Users.Publishers;
using Api.Application.Users.Queries;
using Api.Features.Users.Commands;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Dependencies
{
  public static IServiceCollection RegisterRequestHandlers(
      this IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IUserPublisher, UserPublisher>();

    // Add automapper profile
    services.AddAutoMapper(typeof(Dependencies).Assembly);

    return services;
  }

  public static IBusRegistrationConfigurator RegisterRequestsClients(
      this IBusRegistrationConfigurator brc)
  {
    brc.AddRequestClient<GetUserByIdQueryRequest>();
    brc.AddRequestClient<SearchUsersQueryRequest>();
    brc.AddRequestClient<CreateUserCommandRequest>();
    brc.AddRequestClient<DeleteUserCommandRequest>();

    return brc;
  }
}
