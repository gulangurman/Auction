namespace Auction.Auction.Settings
{
    public interface IAuctionDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
