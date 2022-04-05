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
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(a => a.AuctionId, id);

            List<Bid> bids = await _context.Bids.Find(filter)
                                                .ToListAsync();

            bids = bids.OrderByDescending(a => a.CreatedAt)
                       .GroupBy(a => a.SellerUserName)
                       .Select(a => new Bid
                       {
                           AuctionId = a.FirstOrDefault().AuctionId,
                           Price = a.FirstOrDefault().Price,
                           CreatedAt = a.FirstOrDefault().CreatedAt,
                           SellerUserName = a.FirstOrDefault().SellerUserName,
                           ProductId = a.FirstOrDefault().ProductId,
                           Id = a.FirstOrDefault().Id
                       })
                       .ToList();

            return bids;
        }

        public async Task<List<Bid>> GetAllBidsByAuctionId(string id)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(p => p.AuctionId, id);

            List<Bid> bids = await _context
                          .Bids
                          .Find(filter)
                          .ToListAsync();

            bids = bids.OrderByDescending(a => a.CreatedAt)
                                   .Select(a => new Bid
                                   {
                                       AuctionId = a.AuctionId,
                                       Price = a.Price,
                                       CreatedAt = a.CreatedAt,
                                       SellerUserName = a.SellerUserName,
                                       ProductId = a.ProductId,
                                       Id = a.Id
                                   })
                                   .ToList();

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
