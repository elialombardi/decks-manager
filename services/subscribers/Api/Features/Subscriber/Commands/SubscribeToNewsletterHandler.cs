using MediatR;

namespace Api.Features.Subscriber.Commands
{
  public class SubscribeToNewsletterHandler(EmailDbContext dbContext) : IRequestHandler<SubscribeToNewsletter, Models.Subscriber?>
  {

    public async Task<Models.Subscriber?> Handle(SubscribeToNewsletter request, CancellationToken cancellationToken)
    {
      var subscriber = new Models.Subscriber
      {
        Email = request.Email
      };

      dbContext.Subscribers.Add(subscriber);
      await dbContext.SaveChangesAsync(cancellationToken);

      return subscriber;
    }
  }
}