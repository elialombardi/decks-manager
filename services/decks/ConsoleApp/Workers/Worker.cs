using System.Text.Json;
using Api.Application.Cards.Commands;
using ConsoleApp.ImportData;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Workers
{
  public class Worker(IBus bus, ILogger<Worker> logger, ISender sender) : BackgroundService
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

        logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);

        // Load json database
        logger.LogInformation("Loading json database");
        var json = await File.ReadAllTextAsync("/app/files/oracle-cards-20240816090159.json", stoppingToken);

        // Deserialize json
        logger.LogInformation("Deserializing json");
        var cards = JsonSerializer.Deserialize<List<ScryfallCard>>(json);

        if (cards == null)
        {
          logger.LogError("Failed to deserialize json");
          return;
        }

        // Import data
        logger.LogInformation("Importing data");
        foreach (var card in cards)
        {
          var importedCard = await sender.Send(new ImportCardCommand(card.Id, card.Name), stoppingToken);
          logger.LogInformation("Imported card {CardID}", importedCard?.CardID);
        }

        executed = true;

        logger.LogInformation("Waiting");
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}