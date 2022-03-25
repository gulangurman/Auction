using Auction.Products.Models;
using MongoDB.Driver;

namespace Auction.Products.Data
{
    public class SeedProducts
    {
        public static void Seed(IMongoCollection<Product> collection)
        {
            bool productExists = collection.Find(x => true).Any();
            if (!productExists)
            {
                collection.InsertManyAsync(GetSeedProducts());
            }
        }

        private static IEnumerable<Product> GetSeedProducts()
        {
            return new List<Product>()
            {
                new Product{ Name="Product A", Summary="Summary A", Description="Description A",
                    ImageFile="A.png", Price=5M, Category="Category 1"
                },
                new Product{ Name="Product B", Summary="Summary B", Description="Description B",
                    ImageFile="B.png", Price=6.05M, Category="Category 2"
                }
            };
        }
    }
}
