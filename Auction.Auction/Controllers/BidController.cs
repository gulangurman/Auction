using Auction.Auction.Models;
using Auction.Auction.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auction.Auction.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;
        public BidController(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SendBid([FromBody] Bid bid)
        {
            await _bidRepository.SendBid(bid);
            return Ok();
        }
        [HttpGet("GetBidByAuctionId")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidByAuctionId(string id)
        {
            var bids = await _bidRepository.GetBidsByAuctionId(id);
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
