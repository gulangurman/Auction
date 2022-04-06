using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAuction.WebUI.Clients;
using OnlineAuction.WebUI.Models;

namespace OnlineAuction.WebUI.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ProductClient _productClient;
        private readonly AuctionClient _auctionClient;
        private readonly BidClient _bidClient;

        public AuctionController(UserManager<AppUser> userManager, ProductClient productClient, AuctionClient auctionClient, BidClient bidClient)
        {
            _userManager = userManager;
            _productClient = productClient;
            _auctionClient = auctionClient;
            _bidClient = bidClient;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _auctionClient.GetAuctions();
            if (model != null)
            {
                return View(model);
            }
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var products = await _productClient.GetProducts();
            if (products != null)
            {
                ViewBag.Products = products;
            }
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuctionViewModel model)
        {
            model.Status = default(int);
            model.CreatedAt = DateTime.Now;
            model.IncludedSellers.Add(model.SellerId);
            var result = await _auctionClient.CreateAuction(model);
            if (result != null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            AuctionBidsViewModel model = new AuctionBidsViewModel();

            var auction = await _auctionClient.GetAuctionById(id);
            var bids = await _bidClient.GelAllBidsByAuctionId(id);

            model.SellerUserName = HttpContext.User?.Identity.Name;
            model.AuctionId = auction.Id;
            model.ProductId = auction.ProductId;
            model.Bids = bids;
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            model.IsAdmin = Convert.ToBoolean(isAdmin);

            return View(model);
        }

        [HttpPost]
        public async Task<string> SendBid(BidViewModel model)
        {
            model.CreatedAt = DateTime.Now;
            return await _bidClient.SendBid(model);
        }

        [HttpPost]
        public async Task<string> CompleteBid(string id)
        {
            return await _auctionClient.CompleteBid(id);
        }
    }
}
