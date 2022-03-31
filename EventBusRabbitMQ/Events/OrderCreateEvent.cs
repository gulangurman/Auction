using EventBusRabbitMQ.Events.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Events
{
    public class OrderCreateEvent : IEvent
    {
        // from Bid model
        public string Id { get; set; }
        public string AuctionId { get; set; }
        public string ProductId { get; set; }
        public string SellerUserName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }

        // from Auction model
        public int Quantity { get; set; }
    }
}
