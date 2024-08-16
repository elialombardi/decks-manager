using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NSwag;
using Serilog;
using Serilog.Events;
using ConsoleApp.Consumers;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ConsoleApp.Workers;
using System.Reflection;

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

builder.Services.AddDbContext<DecksDbContext>(options =>
{
  // options.UseInMemoryDatabase("Decks");
  // Use postgresql
  var connectionString = builder.Configuration.GetConnectionString("decks");
  options.UseNpgsql(connectionString, m =>
  {
    m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
    m.MigrationsHistoryTable($"__{nameof(DecksDbContext)}");
  });
});



builder.Host.UseSerilog();

builder.Services.RegisterRequestHandlers(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(cfg => cfg.PostProcess = d =>
{
  d.Info.Title = "Job Consumer Sample";
  d.Info.Contact = new OpenApiContact
  {
    Name = "Job Consumer Sample using MassTransit",
    Email = "support@masstransit.io"
  };
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// builder.Services.AddDbContext<JobServiceSagaDbContext>(optionsBuilder =>
// {
//     var connectionString = builder.Configuration.GetConnectionString("JobService");

//     optionsBuilder.UseNpgsql(connectionString, m =>
//     {
//         m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
//         m.MigrationsHistoryTable($"__{nameof(JobServiceSagaDbContext)}");
//     });
// });

// builder.Services.AddHostedService<MigrationHostedService<JobServiceSagaDbContext>>();

builder.Services.AddMassTransit(x =>
{
  x.AddDelayedMessageScheduler();

  x.AddConsumer<UsersConsumer>();
  // x.SetJobConsumerOptions();
  // x.AddJobSagaStateMachines(options => options.FinalizeCompleted = false)
  //     .EntityFrameworkRepository(r =>
  //     {
  //         r.ExistingDbContext<JobServiceSagaDbContext>();
  //         r.UsePostgres();
  //     });

  x.SetKebabCaseEndpointNameFormatter();

  x.UsingRabbitMq((context, cfg) =>
  {
    var configuration = context.GetRequiredService<IConfiguration>();
    cfg.Host(configuration.GetValue<string>("rabbitMQ:host"), configuration.GetValue<string>("rabbitMQ:virtualHost"), h =>
         {
           h.Username(configuration.GetValue<string?>("rabbitMQ:username") ?? string.Empty);
           h.Password(configuration.GetValue<string?>("rabbitMQ:password") ?? string.Empty);
         });

    // cfg.UseDelayedMessageScheduler();

    var usersQueueName = configuration.GetValue<string>("massTransit:usersQueueName") ?? string.Empty;
    // cfg.ReceiveEndpoint($"decks-{usersQueueName}", e =>
    // {
    //   e.ConfigureConsumeTopology = false;
    //   e.Bind<UserMessage>(x =>
    //   {
    //     x.RoutingKey = usersQueueName;
    //     x.ExchangeType = "direct";
    //   });

    //   e.ConfigureConsumer<UsersConsumer>(context);
    // });

    cfg.ConfigureEndpoints(context);
  });
});

// builder.Services.AddOptions<MassTransitHostOptions>()
//     .Configure(options =>
//     {
//         options.WaitUntilStarted = true;
//         options.StartTimeout = TimeSpan.FromMinutes(1);
//         options.StopTimeout = TimeSpan.FromMinutes(1);
//     });

// builder.Services.AddOptions<HostOptions>()
//     .Configure(options => options.ShutdownTimeout = TimeSpan.FromMinutes(1));

builder.Services.AddHostedService<Worker>();



var app = builder.Build();


static void ApplyMigrations(IHost host)
{
  Log.Logger.Information("Applying migrations");
  using var scope = host.Services.CreateScope();
  var services = scope.ServiceProvider;
  var context = services.GetRequiredService<DecksDbContext>();
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


