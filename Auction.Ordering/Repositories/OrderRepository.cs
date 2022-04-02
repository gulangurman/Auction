using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using Ordering.Domain.Repositories;
using Auction.Ordering.Data;
using Auction.Ordering.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Ordering.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                      .Where(o => o.SellerUserName == userName)
                      .ToListAsync();

            return orderList;
        }
    }
}
