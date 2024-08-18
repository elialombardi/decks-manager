using MediatR;

namespace Api.Features.Subscriber.Commands
{
  public class SubscriberCreateHandler(SubscribersDbContext dbContext) : IRequestHandler<SubscriberCreate, Models.Subscriber?>
  {
    public async Task<Models.Subscriber?> Handle(SubscriberCreate request, CancellationToken cancellationToken)
    {
      var subscriber = new Models.Subscriber
      {
        Email = request.Email,
        SubscribedOn = DateTime.UtcNow,
      };

      dbContext.Subscribers.Add(subscriber);
      await dbContext.SaveChangesAsync(cancellationToken);

      return subscriber;
    }
  }
}