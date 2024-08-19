
using Api.Features.Common;
using Api.Features.Users.Commands;
using Api.Features.Users.Publishers;
using Api.Features.Users.Queries;
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
    brc.AddRequestClient<GetUserByIdQueryRequest>(new Uri($"queue:{nameof(GetUserByIdQueryRequest)}"));
    brc.AddRequestClient<SearchUsersQueryRequest>(new Uri($"queue:{nameof(SearchUsersQueryRequest)}"));
    brc.AddRequestClient<CreateUserCommandRequest>(new Uri($"queue:{nameof(CreateUserCommandRequest)}"));
    brc.AddRequestClient<UpdateUserCommandRequest>(new Uri($"queue:{nameof(UpdateUserCommandRequest)}"));
    brc.AddRequestClient<DeleteUserCommandRequest>(new Uri($"queue:{nameof(DeleteUserCommandRequest)}"));

    return brc;
  }

  public static IBusRegistrationConfigurator RegisterRequestsConsumers(
    this IBusRegistrationConfigurator brc)
  {
    brc.AddConsumer<GetUserByIdQueryConsumer>()
        .Endpoint(e => e.Name = nameof(GetUserByIdQueryRequest));
    brc.AddConsumer<SearchUsersQueryConsumer>()
        .Endpoint(e => e.Name = nameof(SearchUsersQueryRequest));
    brc.AddConsumer<CreateUserCommandConsumer>()
        .Endpoint(e => e.Name = nameof(CreateUserCommandRequest));
    brc.AddConsumer<UpdateUserCommandConsumer>()
        .Endpoint(e => e.Name = nameof(UpdateUserCommandRequest));
    brc.AddConsumer<DeleteUserCommandConsumer>()
        .Endpoint(e => e.Name = nameof(DeleteUserCommandRequest));

    return brc;
  }
}
