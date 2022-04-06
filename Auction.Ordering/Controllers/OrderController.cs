using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ordering.Domain.Repositories;
using Ordering.Domain.Models;
using Auction.Ordering.Models;
using AutoMapper;
using Auction.Ordering.Validators;
using FluentValidation.Results;
using FluentValidation;

namespace Auction.Ordering.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
              private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderController( ILogger<OrderController> logger, IOrderRepository orderRepository, IMapper mapper)
        {
            
            _logger = logger;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet("GetOrdersByUserName/{userName}")]
        [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserName(string userName)
        {
            // var query = new GetOrdersBySellerUsernameQuery(userName);

            var orders = await _orderRepository.GetOrdersBySellerUserName(userName);

            // var orders = await _mediator.Send(query);

            if (orders.Count() == decimal.Zero)
                return NotFound();

            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Order>> OrderCreate([FromBody] CreateOrderDTO dto)
        {
            ValidationResult validationResult = new CreateOrderValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                var failures = new List<ValidationFailure>();
                foreach (var failure in validationResult.Errors)
                {
                    failures.Add(failure);
                }
                throw new ValidationException(failures);
            }

            var order = _mapper.Map<Order>(dto);
            if (order == null)
            {
                throw new ApplicationException("Entity could not be mapped!");
            } 
            var result = await _orderRepository.AddAsync(order);
            _logger.LogInformation("Order created for auction: {auction}", order.AuctionId);
            return Ok(result);
        }
    }
}
