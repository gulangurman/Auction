using Auction.Auction.Models;
using AutoMapper;
using EventBusRabbitMQ.Events;

namespace Auction.Auction.Mapping
{
    public class AuctionMapping : Profile
    {
        public AuctionMapping()
        {
            CreateMap<OrderCreateEvent, Bid>().ReverseMap();
            CreateMap<CreateAuctionDTO, EAuction>();
        }
    }
}
