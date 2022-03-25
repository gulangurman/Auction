using Auction.Auction.Models;
using Auction.Auction.Settings;
using MongoDB.Driver;

namespace Auction.Auction.Data
{
    public class AuctionContext : IAuctionContext
    {
        public AuctionContext(IAuctionDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Auctions = database.GetCollection<EAuction>(nameof(EAuction));
            Bids = database.GetCollection<Bid>(nameof(Bid));

            AuctionContextSeed.Seed(Auctions);

        }
        public IMongoCollection<EAuction> Auctions { get; }

        public IMongoCollection<Bid> Bids { get; }
    }
}
