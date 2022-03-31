using Auction.Auction.Data;
using Auction.Auction.Models;
using Auction.Auction.Repositories.Abstract;
using MongoDB.Driver;

namespace Auction.Auction.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly IAuctionContext _context;
        public BidRepository(IAuctionContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetBidsByAuctionId(string id)
        {
            var bids = await _context.Bids.Find(id).ToListAsync();
            bids = bids.OrderByDescending(o => o.CreatedAt)
                .GroupBy(x => x.SellerUserName)
                .Select(x => new Bid
                {
                    AuctionId = x.FirstOrDefault().AuctionId,
                    Price = x.FirstOrDefault().Price,
                    CreatedAt = x.FirstOrDefault().CreatedAt,
                    SellerUserName = x.FirstOrDefault().SellerUserName,
                    ProductId = x.FirstOrDefault().ProductId,
                    Id = x.FirstOrDefault().Id
                }).ToList();
            return bids;
        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            var bids = await GetBidsByAuctionId(id);
            return bids.OrderByDescending(x => x.Price).FirstOrDefault();
        }

        public async Task SendBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }
    }
}
