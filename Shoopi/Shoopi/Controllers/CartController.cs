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
        private readonly IProductRepository _product;
        private readonly ShoopiContext _context;
        public CartController(IProductRepository product, ShoopiContext context)
        {
            _context = context;
            _product = product;
        }

        
        //session de luu gio hang 
        public List<CartVM> Cart => HttpContext.Session.Get<List<CartVM>>(MySetting.CART_KEY) ?? new List<CartVM>();
        public List<Product> Product;

        #region CART
        [Authorize]
        public IActionResult Index()
        {
            if (Cart.Count == 0)
            {
                return RedirectToAction("Index", "Product");
            }

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
            HttpContext.Session.Set(MySetting.CART_KEY, cartItem);
            

            //lay du lieu session de load tempdata
            var currentCount = HttpContext.Session.GetInt32("CartCount") ?? 0;
            HttpContext.Session.SetInt32("CartCount", currentCount + quantity);
            TempData["CartCount"] = currentCount + quantity;
            
            return Json(new { success = true, totalQuantity = currentCount + quantity });
        }
        public IActionResult RemoveCart(int id)
        {
            var cartData = HttpContext.Session.Get(MySetting.CART_KEY);
            if (cartData == null)
            {
                TempData["Message"] = "No product(s) in your cart";
                return Content(Url.Action("Index", "Product"));
            }
            var cartItem = Cart;
            var item = cartItem.SingleOrDefault(p => p.ProductID == id);
            if (item != null)
            {
                cartItem.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, cartItem);
            }
            if (cartItem.Count == 0)
            {
                TempData["Message"] = "No product(s) in your cart";
                return RedirectToAction("Index", "Product");
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region CHECKOUT
        [Authorize]
        public IActionResult CheckOut()
        {
            if (Cart.Count == 0)
            {
                return RedirectToAction("Index", "Product");
            }
            return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CheckOut(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                //lay du lieu tu claim / .value la lay gia tri debug roi biet
                var customerId = HttpContext.User.Claims
                    .SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID)?.Value; 
                var user = new User();
                if (model.IsSameInfo)
                {
                     user = _context.Users.SingleOrDefault(u => u.UserId.ToString().Equals(customerId)); 
                }
                var order = new Order()
                {
                    UserId = int.Parse(customerId),
                    FullName = model.FullName ?? user.FullName,
                    Address = model.Address ?? user.Address,
                    PhoneNumber = model.PhoneNumber ?? user.PhoneNumber,
                    Delivery = "GRAB",
                    CreateDate = DateOnly.FromDateTime(DateTime.Now),
                    MethodPayment = model.MethodPayment,
                    Note = model.Note,
                    OrderStatusId = 1
                };
                    // bat dau transaction phai hoan thanh HET thi moi thay doi (change)
                   _context.Database.BeginTransaction();
                try
                {
                    _context.Database.CommitTransaction();
                    _context.Add(order);
                    _context.SaveChanges(); 

                    //xoa quantity trong product
                    var product = new Product();
                    foreach (var pr in Cart)
                    {
                        product = _context.Products.SingleOrDefault(p => p.ProductId == pr.ProductID);
                        product.Quantity = product.Quantity - pr.Quantity;
                    }
                    _context.Update(product);
                    _context.SaveChanges();

                    //add thong tin ben order detail
                    var orderDetail = new List<OrderDetail>();
                    foreach (var item in Cart)
                    {
                        orderDetail.Add(new OrderDetail()
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductID,
                            Quantity = item.Quantity,
                            Discount = 0,
                            TotalPrice = item.Price,
                        });
                    }
                    _context.AddRange(orderDetail);
                    _context.SaveChanges();
                    //xoa session
                    HttpContext.Session.Set<List<CartVM>>(MySetting.CART_KEY, new List<CartVM>()) ;
                    return RedirectToAction("Index", "Product");
                }
                catch (Exception ex) 
                {
                    _context.Database.RollbackTransaction();
                }
               
            }
            return View(Cart);
        }

		#endregion

	}

}

