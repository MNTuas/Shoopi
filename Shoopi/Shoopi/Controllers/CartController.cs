using DAO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using DAO.Data;
using Shoopi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Repository.PaymentService;
namespace Shoopi.wwwroot
{
    public class CartController : Controller
    {
        private readonly IProductRepository _product;
        private readonly Shoopi1Context _context;
		private readonly IVnPayService _vnPayservice;


		public CartController(IProductRepository product, Shoopi1Context context, IVnPayService vnPayservice)
        {
            _context = context;
            _product = product;
            _vnPayservice = vnPayservice;
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
		public IActionResult CheckOut(CheckoutVM model, string payment = "VnPay")
		{
			if (!ModelState.IsValid)
			{
				return View(Cart);
			}

			// Lấy thông tin từ claim
			var customerId = HttpContext.User.Claims
				.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID)?.Value;

			if (customerId == null)
			{
				TempData["Message"] = "Không thể xác định khách hàng.";
				return View(Cart);
			}

			var user = new User();
			if (model.IsSameInfo)
			{
				user = _context.Users.SingleOrDefault(u => u.UserId.ToString().Equals(customerId));
				if (user == null || user.Address == null || user.PhoneNumber == null)
				{
					TempData["Message"] = "Địa chỉ và số điện thoại của bạn bị thiếu. Vui lòng cập nhật thông tin.";
					return View(Cart);
				}
			}

			// Tạo đơn hàng
			var order = new Order
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

			using var transaction = _context.Database.BeginTransaction();
			try
			{
				// Thêm đơn hàng vào DB
				_context.Add(order);
				_context.SaveChanges();

				// Cập nhật số lượng sản phẩm
				foreach (var pr in Cart)
				{
					var product = _context.Products.SingleOrDefault(p => p.ProductId == pr.ProductID);
					if (product == null || product.Quantity < pr.Quantity)
					{
						throw new Exception($"Sản phẩm {pr.ProductID} không đủ số lượng.");
					}
					product.Quantity -= pr.Quantity;
					_context.Update(product);
				}
				_context.SaveChanges();

				// Thêm thông tin chi tiết đơn hàng
				var orderDetails = Cart.Select(item => new OrderDetail
				{
					OrderId = order.OrderId,
					ProductId = item.ProductID,
					Quantity = item.Quantity,
					Discount = 0,
					TotalPrice = item.Price
				}).ToList();

				_context.AddRange(orderDetails);
				_context.SaveChanges();

				// Xử lý thanh toán VNPay
				if (payment.Trim() == "VnPay")
				{
					var vnPayModel = new VnPaymentRequestModel
					{
						Amount = (double)Cart.Sum(p => p.Price * p.Quantity),
						CreatedDate = DateTime.Now,
						Description = $"{model.FullName} {model.PhoneNumber}",
						FullName = model.FullName,
						OrderId = order.OrderId
					};

					transaction.Commit(); // Commit trước khi chuyển hướng
					return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
				}

				// Xóa giỏ hàng và commit giao dịch
				HttpContext.Session.Set<List<CartVM>>(MySetting.CART_KEY, new List<CartVM>());
				transaction.Commit();
				return RedirectToAction("Index", "Product");
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				TempData["Message"] = "Đã xảy ra lỗi trong quá trình thanh toán. Vui lòng thử lại.";
				return View(Cart);
			}
		}


		#endregion

		[Authorize]
		public IActionResult PaymentFail()
		{
			return View();
		}

		[Authorize]
		public IActionResult PaymentSuccess()
		{
			return View();
		}

		[Authorize]
		public IActionResult PaymentCallBack()
		{
			var response = _vnPayservice.PaymentExecute(Request.Query);

			//tìm order để sửa lại status order
			var orderId = int.Parse(response.OrderDescription);
			var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);


			if (response == null || response.VnPayResponseCode != "00" || order == null)
			{
				TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";				
				return RedirectToAction("PaymentFail");
			}

			order.OrderStatusId = 7;
			_context.Update(order);
			_context.SaveChanges();	

			// Lưu đơn hàng vô database
			TempData["Message"] = $"Thanh toán VNPay thành công";
			return RedirectToAction("PaymentSuccess");
		}


	}

}

