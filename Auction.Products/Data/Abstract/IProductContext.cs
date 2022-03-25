using Auction.Products.Models;
using MongoDB.Driver;

namespace Auction.Products.Data.Abstract
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
