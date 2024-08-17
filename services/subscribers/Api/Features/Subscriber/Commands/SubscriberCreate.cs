using MediatR;

namespace Api.Features.Subscriber.Commands
{
  public record SubscriberCreate(string Email) : IRequest<Models.Subscriber?>;
}