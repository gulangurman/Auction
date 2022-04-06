
using Auction.Ordering.Models;

using AutoMapper;
using EventBusRabbitMQ.Events;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Ordering.Mapping
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {                              
            CreateMap<Order, CreateOrderDTO>().ReverseMap();
            CreateMap<OrderCreateEvent, CreateOrderDTO>().ReverseMap();
        }
    }
}
