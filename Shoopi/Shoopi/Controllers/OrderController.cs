using DAO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;
using Shoopi.Helper;

namespace Shoopi.Controllers
{
	public class OrderController : Controller
	{
		[BindProperty(SupportsGet = true)]
		public int PageIndex { get; set; } = 1;
		public int PageSize { get; set; } = 3;
		public int TotalPages { get; set; }

		private readonly IOrderRepository _orderRepository;
		public OrderController(IOrderRepository Order)
		{
            _orderRepository = Order;
		}
		
		public IList<Order> Orders { get; set; } = default!;

		[Authorize]
		public async Task<IActionResult> Index( int? type, string query, int PageIndex = 1)
		{
            // Lấy thông tin từ claim
            var userId = int.Parse(HttpContext.User.Claims
                .SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID)?.Value);

            var result = await _orderRepository.GetOrderByUserLogin(userId, type, query, PageIndex, 6);
			if (!result.Orders.Any())
			{
                TempData["NoOrdersMessage"] = "You don't have any orders.";
            }
			PageIndex = result.PageIndex;
			TotalPages = result.TotalPages;

			return View(result);
		}
        
		[Authorize]
        public async Task<IActionResult> OrderDetail(int id)
		{
			if (id == null)
			{
				return Redirect("/404");
			}
			var Orders = await _orderRepository.GetOrderById(id);
			if (Orders == null)
			{
				return Redirect("/404");
			}
			return View(Orders);
		}

        [ShoopiAuthorizedAddtribute("Admin", "Allowed")]
        public async Task<IActionResult> GetUserOrder(int id, int? type, string query, int PageIndex = 1)
        {
            if (id == 0)
            {
                TempDataHelper.AddNotification(TempData, "Error", "ID not found");
				return View("/404");
            }

            var result = await _orderRepository.GetOrderByUserLogin(id, type, query, PageIndex, 6);
            if (!result.Orders.Any())
            {
                TempDataHelper.AddNotification(TempData, "Warning", "User doesn't have any orders.");
            }

            PageIndex = result.PageIndex;
            TotalPages = result.TotalPages;

            return View(result);
        }


    }
}
