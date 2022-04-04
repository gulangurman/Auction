namespace Auction.Auction.Models
{
    public class CreateAuctionDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
        public List<string> IncludedSellers { get; set; } = new List<string>();
    }
}
