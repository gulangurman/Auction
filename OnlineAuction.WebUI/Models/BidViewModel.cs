using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAuction.WebUI.Models
{
    public class BidViewModel
    {
        public string Id { get; set; }
        public string AuctionId { get; set; }
        public string ProductId { get; set; }
        public string SellerUserName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
