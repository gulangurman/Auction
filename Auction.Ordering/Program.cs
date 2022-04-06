using Auction.Ordering.Consumers;
using Auction.Ordering.Extensions;
using EventBusRabbitMQ;
using RabbitMQ.Client;
using Ordering.Domain.Repositories.Base;
using Auction.Ordering.Repositories.Base;
using Ordering.Domain.Repositories;
using Auction.Ordering.Repositories;
using Auction.Ordering.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using AutoMapper;
using Auction.Ordering.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderContext>(options =>
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString("OrderConnection"),
                        b => b.MigrationsAssembly(typeof(OrderContext).Assembly.FullName)), ServiceLifetime.Singleton);

//Add Repositories
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

//builder.Services.AddApplication();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

var config = new MapperConfiguration(cfg =>
{
    cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
    cfg.AddProfile<OrderMappingProfile>();
});
var mapper = config.CreateMapper();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
