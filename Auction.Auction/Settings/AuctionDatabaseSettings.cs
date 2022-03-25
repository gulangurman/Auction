namespace Auction.Auction.Settings
{
    public class AuctionDatabaseSettings : IAuctionDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
