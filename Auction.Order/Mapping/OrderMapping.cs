using AutoMapper;
using Ordering.Application.Commands.OrderCreate;

namespace Auction.Order.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<OrderCreateEvent, OrderCreateCommand>().ReverseMap();
        }
    }
}
