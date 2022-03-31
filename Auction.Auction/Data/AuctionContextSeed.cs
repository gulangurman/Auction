using Auction.Auction.Models;
using MongoDB.Driver;

namespace Auction.Auction.Data
{
    public class AuctionContextSeed
    {
        public static void Seed(IMongoCollection<EAuction> collection)
        {
            bool exists = collection.Find(x => true).Any();
            if (!exists)
            {
                collection.InsertManyAsync(GetSeedAuctions());
            }
        }
        private static IEnumerable<EAuction> GetSeedAuctions()
        {
            return new List<EAuction>()
            {
                new EAuction
                {
                    Name="Auction 1",
                    Description="Auction description",
                    CreatedAt=DateTime.Now,
                    StartedAt=DateTime.Now,
                    FinishedAt=DateTime.Now.AddDays(10),
                    ProductId="623d5384e65fc11fa1b41da1",
                    IncludedSellers=new List<string>
                    {
                        "seller1@example.com",
                        "seller2@example.com"
                    },
                    Quantity=3,
                    Status=(int)Status.Active
                },
                 new EAuction
                {
                    Name="Auction 2",
                    Description="Auction 2 description",
                    CreatedAt=DateTime.Now,
                    StartedAt=DateTime.Now,
                    FinishedAt=DateTime.Now.AddDays(10),
                    ProductId="623d5384e65fc11fa1b41da2",
                    IncludedSellers=new List<string>
                    {
                        "seller2@example.com",
                        "seller3@example.com"
                    },
                    Quantity=1,
                    Status=(int)Status.Active
                }
            };
        }
    }
}
