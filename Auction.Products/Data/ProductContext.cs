using Auction.Products.Data.Abstract;
using Auction.Products.Models;
using Auction.Products.Settings;
using MongoDB.Driver;

namespace Auction.Products.Data
{
    public class ProductContext : IProductContext
    {
        public ProductContext(IProductDatabaseSettings settings)
        {
            var client = new MongoClient(connectionString: settings.ConnectionString);
            var database = client.GetDatabase(name: settings.DatabaseName);
            Products = database.GetCollection<Product>(name: settings.CollectionName);
            SeedProducts.Seed(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
