using MediatR;

namespace Api.Features.Subscriber.Commands
{
  public record SubscribeToNewsletter(string Email) : IRequest<Models.Subscriber?>;
}