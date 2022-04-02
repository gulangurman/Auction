using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineAuction.WebUI.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
