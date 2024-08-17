using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Filters;
using System.Reflection;
using Api.Features;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmailDbContext>(options =>
{
    // options.UseInMemoryDatabase("Decks");
    // Use postgresql
    var connectionString = builder.Configuration.GetConnectionString("emails");
    options.UseNpgsql(connectionString, m =>
    {
        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        m.MigrationsHistoryTable($"__{nameof(EmailDbContext)}");
    });
});

builder.Services.AddMassTransit(x =>
   {
       //    x.UsingInMemory((context, cfg) =>
       //     {
       //         cfg.ConfigureEndpoints(context);
       //     });

       x.UsingRabbitMq((context, cfg) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            cfg.Host(configuration.GetValue<string>("rabbitMQ:host"), configuration.GetValue<string>("rabbitMQ:virtualHost"), h =>
             {
                 h.Username(configuration.GetValue<string>("rabbitMQ:username") ?? string.Empty);
                 h.Password(configuration.GetValue<string>("rabbitMQ:password") ?? string.Empty);
             });
        });
   });



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:SecretKey") ?? string.Empty))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationExceptionFilter>();
});


builder.Services.RegisterRequestHandlers(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
static void ApplyMigrations(IHost host)
{
    // Log.Logger.Information("Applying migrations");
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EmailDbContext>();
    context.Database.Migrate();
    // Log.Logger.Information("Migrations applied");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    ApplyMigrations(app);

}

app.UseHttpsRedirection();

app.UseHealthChecks("/users/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();