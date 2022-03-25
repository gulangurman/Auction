using Auction.Auction.Models;

namespace Auction.Auction.Repositories.Abstract
{
    public interface IAuctionRepository
    {
        Task<IEnumerable<EAuction>> GetAuctions();
        Task<EAuction> GetAuction(string id);
        Task<EAuction> GetAuctionByName(string name);
        Task Create(EAuction auction);
        Task<bool> Update(EAuction auction);
        Task<bool> Delete(string id);
    }
}
