using Auction.Auction.Models;
using Auction.Auction.Repositories.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auction.Auction.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        public BidController(IBidRepository bidRepository, IMapper mapper)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SendBid([FromBody] CreateBidDTO dto)
        {
            var bid = _mapper.Map<Bid>(dto);
            await _bidRepository.SendBid(bid);
            return Ok();
        }

        [HttpGet("GetBidByAuctionId")]
        [ProducesResponseType(typeof(IEnumerable<Bid>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidByAuctionId(string id)
        {
            IEnumerable<Bid> bids = await _bidRepository.GetBidsByAuctionId(id);

            return Ok(bids);
        }

        [HttpGet("GetAllBidsByAuctionId")]
        [ProducesResponseType(typeof(List<Bid>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetAllBidsByAuctionId(string id)
        {
            var bids = await _bidRepository.GetAllBidsByAuctionId(id);
            return Ok(bids);
        }

        [HttpGet("GetWinnerBid")]
        [ProducesResponseType(typeof(Bid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bid>> GetWinnerBid(string id)
        {
            Bid bid = await _bidRepository.GetWinnerBid(id);
            return Ok(bid);
        }
    }
}
