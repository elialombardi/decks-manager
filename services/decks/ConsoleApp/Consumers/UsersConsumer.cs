using Api.Application.Cards.Queries;
using Api.Application.Decks.Commands;
using Api.Data.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Consumers
{
  public class UsersConsumer(ILogger<UsersConsumer> logger, ISender sender) : IConsumer<UserMessage>
  {

    public async Task Consume(ConsumeContext<UserMessage> context)
    {
      logger.LogInformation("Received message for user: {UserID}", context.Message.UserID);

      if (context.Message.DeletedAt.HasValue)
      {
        logger.LogInformation("User {UserID} was deleted at {DeletedAt}", context.Message.UserID, context.Message.DeletedAt);

        var deletedDecks = await sender.Send(new DeleteDecksByUserIdCommand(context.Message.UserID));
        logger.LogInformation("Deleted {Count} decks for user {UserID}", deletedDecks, context.Message.UserID);
      }
      else
      {
        logger.LogInformation("User {UserID} was not deleted", context.Message.UserID);

        // ONLY FOR TESTING
        // TODO: Remove
        var random = new Random();

        for (var i = 0; i < 5; i++)
        {
          // generete a random number between 1 and 3
          var cardCount = random.Next(1, 4);

          // get randomically cards from context
          var cards = await sender.Send(new SearchCardsQuery(null, true, cardCount));

          var createdDeckId = await sender.Send(new CreateDeckCommand(context.Message.UserID, $"Deck {i}", "Description", "https://example.com/image.jpg", cards.Select(c => new CreateDeckCommandCard(c.CardID))));
          logger.LogInformation("Created deck {DeckID} for user {UserID}", createdDeckId, context.Message.UserID);
        }
      }
      await Task.CompletedTask;
    }
  }
}