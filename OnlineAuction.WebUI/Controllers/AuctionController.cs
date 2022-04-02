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

        public AuctionController(UserManager<AppUser> userManager, ProductClient productClient)
        {
            _userManager = userManager;
            _productClient = productClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var products = await _productClient.GetProducts();
            if(products != null)
            {
                ViewBag.Products = products;
            }
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        public IActionResult Create(AuctionViewModel model)
        {
            return View(model);
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
