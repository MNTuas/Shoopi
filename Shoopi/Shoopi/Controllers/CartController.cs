using Microsoft.AspNetCore.Mvc;

namespace Shoopi.wwwroot
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            return View();
        }
    }
}
