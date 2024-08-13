using DAO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using DAO.Data;
using Shoopi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
namespace Shoopi.wwwroot
{
    public class CartController : Controller
    {
        private readonly IProduct _product;
        private readonly ShoopiContext _context;
        public CartController(IProduct product, ShoopiContext context)
        {
			_context = context;
            _product = product; 
        }

		const string CART_KEY = "MYCART";
        //session de luu gio hang 
		public List<CartVM> Cart => HttpContext.Session.Get<List<CartVM>>(CART_KEY) ?? new List<CartVM>();
        public IActionResult Index()
        {
            return View(Cart);
        }

        [Authorize]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var cartItem = Cart;
            var item = cartItem.SingleOrDefault(p => p.ProductID == id);
            if (item == null)
            {
                var product = _context.Products.SingleOrDefault(p => p.ProductId == id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }
                item = new CartVM
                {
                    ProductID = product.ProductId,
                    Name = product.ProductName,
                    Price = product.Price ?? 0,
                    Picture = product.Picture ?? string.Empty,
                    Quantity = quantity,
                };
                cartItem.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
            HttpContext.Session.Set(CART_KEY, cartItem);

            //lay du lieu session de load tempdata
			var currentCount = HttpContext.Session.GetInt32("CartCount") ?? 0;
			HttpContext.Session.SetInt32("CartCount", currentCount + quantity);
			TempData["CartCount"] = currentCount + quantity;

			return Json(new { success = true, totalQuantity = currentCount + quantity });
		}


        public IActionResult RemoveCart(int id)
        {
            var cartData = HttpContext.Session.Get(CART_KEY);
            if (cartData == null)
            {
                TempData["Message"] = "No product(s) in your cart";
				return Content(Url.Action("Index", "Product"));
			}
            var cartItem = Cart;
            var item = cartItem.SingleOrDefault( p => p.ProductID == id);
            if (item != null) 
            {
                cartItem.Remove(item);
                HttpContext.Session.Set(CART_KEY, cartItem);
            }
			if (cartItem.Count == 0)
			{
				TempData["Message"] = "No product(s) in your cart";
				return RedirectToAction("Index", "Product");
			}
			return RedirectToAction("Index");
        }
	
	}
}
