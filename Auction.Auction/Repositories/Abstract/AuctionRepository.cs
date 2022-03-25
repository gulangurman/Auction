using Auction.Auction.Data;
using Auction.Auction.Models;
using MongoDB.Driver;

namespace Auction.Auction.Repositories.Abstract
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly IAuctionContext _context;
        public AuctionRepository(IAuctionContext context)
        {
            _context = context;
        }
        public async Task Create(EAuction auction)
        {
            await _context.Auctions.InsertOneAsync(auction);
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _context.Auctions.DeleteOneAsync(id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<EAuction> GetAuction(string id)
        {
            return await _context.Auctions.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<EAuction> GetAuctionByName(string name)
        {
            return await _context.Auctions.Find(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EAuction>> GetAuctions()
        {
            return await _context.Auctions.Find(x => true).ToListAsync();
        }

        public async Task<bool> Update(EAuction auction)
        {
            var result = await _context.Auctions.ReplaceOneAsync(x => x.Id == auction.Id, auction);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
