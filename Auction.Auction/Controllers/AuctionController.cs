using Auction.Auction.Models;
using Auction.Auction.Repositories.Abstract;
using AutoMapper;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auction.Auction.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ILogger<AuctionController> _logger;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBus;

        public AuctionController(IAuctionRepository auctionRepository,
            IBidRepository bidRepository, IMapper mapper,
            EventBusRabbitMQProducer eventBus,
            ILogger<AuctionController> logger)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _mapper = mapper;
            _eventBus = eventBus;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EAuction>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<EAuction>> GetAuctions()
        {
            var auctions = await _auctionRepository.GetAuctions();
            return auctions;
        }

        [HttpGet("{id:length(24)}", Name = "GetAuction")]
        [ProducesResponseType(typeof(EAuction), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<EAuction>> GetAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);
            if (auction == null)
            {
                _logger.LogError($"Auction with id: {id} not found.");
                return NotFound();
            }
            return auction;
        }

        [HttpPost]
        [ProducesResponseType(typeof(EAuction), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<EAuction>> CreateAuction([FromBody] EAuction auction)
        {
            await _auctionRepository.Create(auction);
            return CreatedAtRoute("GetAuction", new { id = auction.Id }, auction);
        }

        [HttpPut]
        [ProducesResponseType(typeof(EAuction), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<EAuction>> UpdateAuction([FromBody] EAuction auction)
        {

            return Ok(await _auctionRepository.Update(auction));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(EAuction), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<EAuction>> DeleteAuctionById(string id)
        {
            return Ok(await _auctionRepository.Delete(id));
        }

        [HttpPost("CompleteAuction")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<ActionResult> CompleteAuction([FromBody] string id)
        {
            EAuction auction = await _auctionRepository.GetAuction(id);
            if (auction == null)
                return NotFound();

            if (auction.Status != (int)Status.Active)
            {
                _logger.LogError("Auction can not be completed");
                return BadRequest();
            }

            Bid bid = await _bidRepository.GetWinnerBid(id);
            if (bid == null) return NotFound();


            OrderCreateEvent eventMessage = _mapper.Map<OrderCreateEvent>(bid);
            eventMessage.Quantity = auction.Quantity;

            auction.Status = (int)Status.Closed;
            bool updateResponse = await _auctionRepository.Update(auction);
            if (!updateResponse)
            {
                _logger.LogError("Auction can not updated");
                return BadRequest();
            }

            // publish auction completed message
            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.Id, "Sourcing");
                throw;
            }

            return Accepted();
        }

        [HttpPost("TestEvent")]
        public ActionResult<OrderCreateEvent> TestEvent()
        {
            OrderCreateEvent eventMessage = new OrderCreateEvent
            {
                AuctionId = "dummy1",
                ProductId = "dummy_product_1",
                Price = 10,
                Quantity = 100,
                SellerUserName = "test@test.com"
            };

            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.Id, "Sourcing");
                throw;
            }

            return Accepted(eventMessage);
        }

    }
}
