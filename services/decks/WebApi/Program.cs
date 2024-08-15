using System.Text;
using Api.Application.Decks.Queries;
using Api.Data;
using Api.Features.Decks.Commands;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Services;
using WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DecksDbContext>(options =>
{
  options.UseInMemoryDatabase("Decks");
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
             h.Username("rabbitMQ:username");
             h.Password("rabbitMQ:password");
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

builder.Service.AddAuthorization();

builder.Services.AddControllers(options =>
{
  options.Filters.Add<ValidationExceptionFilter>();
});

builder.Services.RegisterRequestHandlers(builder.Configuration);


builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();