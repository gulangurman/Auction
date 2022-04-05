using Auction.Auction.Data;
using Auction.Auction.Repositories;
using Auction.Auction.Repositories.Abstract;
using Auction.Auction.Settings;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using AutoMapper;
using Auction.Auction.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<AuctionDatabaseSettings>(builder.Configuration
    .GetSection(nameof(AuctionDatabaseSettings)));
builder.Services.AddSingleton<IAuctionDatabaseSettings>(sp =>
  sp.GetRequiredService<IOptions<AuctionDatabaseSettings>>().Value);

builder.Services.AddTransient<IAuctionContext, AuctionContext>();
builder.Services.AddTransient<IAuctionRepository, AuctionRepository>();
builder.Services.AddTransient<IBidRepository, BidRepository>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Auction.Auction",
        Version = "v1"
    });
});

builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration.GetSection("EventBus").GetSection("HostName").Value
    };

    if (!string.IsNullOrWhiteSpace(builder.Configuration.GetSection("EventBus").GetSection("UserName").Value))
    {
        factory.UserName = builder.Configuration.GetSection("EventBus").GetSection("UserName").Value;
    }

    if (!string.IsNullOrWhiteSpace(builder.Configuration.GetSection("EventBus").GetSection("Password").Value))
    {
        factory.UserName = builder.Configuration.GetSection("EventBus").GetSection("Password").Value;
    }

    var retryCount = 5;
    if (!string.IsNullOrWhiteSpace(builder.Configuration.GetSection("EventBus").GetSection("RetryCount").Value))
    {
        retryCount = int.Parse(builder.Configuration.GetSection("EventBus").GetSection("RetryCount").Value);
    }

    return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
});

builder.Services.AddSingleton<EventBusRabbitMQProducer>();

builder.Services.AddCors(x => x.AddPolicy("CorsPolicy", builder =>
  {
      builder.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials()
      .WithOrigins("http://localhost:5048"); // web ui
  }));

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<AuctionHub>("/auctionhub");

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Auction API v1");
});

app.Run();
