using Auction.Order.Consumers;
using Auction.Order.Extensions;
using EventBusRabbitMQ;
using Ordering.Application;
using Ordering.Infrastructure;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

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

builder.Services.AddSingleton<EventBusOrderCreateConsumer>();

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Order API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MigrateDatabase();

app.UseRabbitListener();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
