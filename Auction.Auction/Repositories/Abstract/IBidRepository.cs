using Auction.Auction.Data;
using Auction.Auction.Models;

namespace Auction.Auction.Repositories.Abstract
{
    public interface IBidRepository
    {
        Task<List<Bid>> GetBidsByAuctionId(string id);
        Task<Bid> GetWinnerBid(string id);
        Task SendBid(Bid bid);
    }
}
