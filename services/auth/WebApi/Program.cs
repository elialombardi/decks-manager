using System.Text;
using Api.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Filters;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthDbContext>(options =>
{
  options.UseInMemoryDatabase("Auth");
});

builder.Services.AddMassTransit(x =>
   {
     x.UsingRabbitMq((context, cfg) =>
      {
        var configuration = context.GetRequiredService<IConfiguration>();
        cfg.Host(configuration.GetValue<string>("rabbitMQ:host"), configuration.GetValue<string>("rabbitMQ:virtualHost"), h =>
           {
             h.Username(configuration.GetValue<string?>("rabbitMQ:username") ?? string.Empty);
             h.Password(configuration.GetValue<string?>("rabbitMQ:password") ?? string.Empty);
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

builder.Services.AddControllers(options =>
{
  options.Filters.Add<ValidationExceptionFilter>();
});

builder.Services.RegisterRequestHandlers(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();

  using var scope = app.Services.CreateScope();
  var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
  AuthDbContext.Initialize(context);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();