using DAO.Data;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

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
		public async Task<IActionResult> Index(int? type, string query, int PageIndex = 1)
		{
			var result = await _orderRepository.GetOrders(type, query, PageIndex, 6);		
			PageIndex = result.PageIndex;
			TotalPages = result.TotalPages;

			return View(result);
		}

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

	}
}
