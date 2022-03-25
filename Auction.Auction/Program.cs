using Auction.Auction.Data;
using Auction.Auction.Repositories.Abstract;
using Auction.Auction.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<AuctionDatabaseSettings>(builder.Configuration
    .GetSection(nameof(AuctionDatabaseSettings)));
builder.Services.AddSingleton<IAuctionDatabaseSettings>(sp =>
  sp.GetRequiredService<IOptions<AuctionDatabaseSettings>>().Value);
builder.Services.AddTransient<IAuctionContext, AuctionContext>();
builder.Services.AddTransient<IAuctionRepository, AuctionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
