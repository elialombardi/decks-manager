using System.Text.Json;
using Api.Application.Cards.Commands;
using Api.Application.Cards.Queries;
using ConsoleApp.ImportData;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Workers
{
  public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
  {
    bool executed = false;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        if (executed)
        {
          break;
        }
        // await bus.Publish(new GettingStarted { Value = $"The time is {DateTimeOffset.Now}" }, stoppingToken);

        // Create new scope
        using var scope = serviceProvider.CreateScope();

        // var bus = scope.ServiceProvider.GetRequiredService<IBus>();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);

        var existingsCards = await sender.Send(new SearchCardsQuery(null, false, 1), stoppingToken);

        if (existingsCards.Count > 0)
        {
          logger.LogInformation("Cards found. Importing disabled");
        }
        else
        {
          // Load json database
          logger.LogInformation("Loading json database");
          var json = await File.ReadAllTextAsync("/app/files/oracle-cards-20240816090159.json", stoppingToken);

          // Deserialize json
          logger.LogInformation("Deserializing json");
          var cards = JsonSerializer.Deserialize<List<ScryfallCard>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

          if (cards == null)
          {
            logger.LogError("Failed to deserialize json");
            return;
          }

          // Import data
          logger.LogInformation("Importing data");
          foreach (var card in cards.Take(10))
          {
            var importedCard = await sender.Send(new ImportCardCommand(card.Id, card.Name), stoppingToken);
            logger.LogInformation("Imported card {CardID}", importedCard?.CardID);
          }
        }

        executed = true;

        logger.LogInformation("Waiting");
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}