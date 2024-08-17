
using System.Reflection;
using Api.Features;
using Api.Features.Subscriber.Sagas;
using ConsoleApp.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmailDbContext>(options =>
{
  var connectionString = builder.Configuration.GetConnectionString("decks");
  options.UseNpgsql(connectionString, m =>
  {
    m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
    m.MigrationsHistoryTable($"__{nameof(EmailDbContext)}");
  });
});

builder.Host.UseSerilog();

builder.Services.RegisterRequestHandlers(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddMassTransit(x =>
{
  x.AddDelayedMessageScheduler();

  x.AddConsumers(typeof(Program).Assembly);

  x.AddSagaStateMachine<NewsletterOnboardingSaga, NewsletterOnboardingSagaData>()
      .EntityFrameworkRepository(r =>
      {
        r.ExistingDbContext<EmailDbContext>();
        r.UsePostgres();
      });


  x.SetKebabCaseEndpointNameFormatter();

  x.UsingRabbitMq((context, cfg) =>
  {
    var configuration = context.GetRequiredService<IConfiguration>();
    cfg.Host(configuration.GetValue<string>("rabbitMQ:host"), configuration.GetValue<string>("rabbitMQ:virtualHost"), h =>
         {
           h.Username(configuration.GetValue<string?>("rabbitMQ:username") ?? string.Empty);
           h.Password(configuration.GetValue<string?>("rabbitMQ:password") ?? string.Empty);
         });

    cfg.ConfigureEndpoints(context);
  });
});

var app = builder.Build();


static void ApplyMigrations(IHost host)
{
  Log.Logger.Information("Applying migrations");
  using var scope = host.Services.CreateScope();
  var services = scope.ServiceProvider;
  var context = services.GetRequiredService<EmailDbContext>();
  context.Database.Migrate();
  Log.Logger.Information("Migrations applied");
}

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  ApplyMigrations(app);
}

app.UseOpenApi();
app.UseSwaggerUi();

app.UseRouting();
app.UseAuthorization();

static Task HealthCheckResponseWriter(HttpContext context, HealthReport result)
{
  context.Response.ContentType = "application/json";

  return context.Response.WriteAsync(result.ToJsonString());
}

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
  Predicate = check => check.Tags.Contains("ready"),
  ResponseWriter = HealthCheckResponseWriter
});

app.MapHealthChecks("/health/live", new HealthCheckOptions { ResponseWriter = HealthCheckResponseWriter });

app.MapControllers();

await app.RunAsync();


