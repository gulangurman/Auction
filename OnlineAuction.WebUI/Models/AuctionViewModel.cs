using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.WebUI.Models
{
    public class AuctionViewModel
    {

        public string Id { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter product")]
        public string ProductId { get; set; }
        [Required(ErrorMessage = "Please enter quantity")]
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "Please enter start date")]
        public DateTime StartedAt { get; set; }
        [Required(ErrorMessage = "Please enter finish date")]
        public DateTime FinishedAt { get; set; }
        public int Status { get; set; }
        public string SellerId { get; set; }
        public List<string> IncludedSellers { get; set; } = new List<string>();
    }
}
