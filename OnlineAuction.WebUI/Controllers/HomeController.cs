using Microsoft.AspNetCore.Mvc;

namespace OnlineAuction.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
