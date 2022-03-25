using Auction.Auction.Models;
using MongoDB.Driver;

namespace Auction.Auction.Data
{
    public interface IAuctionContext
    {
        IMongoCollection<EAuction> Auctions { get; }
        IMongoCollection<Bid> Bids { get; }
    }
}
