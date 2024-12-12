using DAO.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
	public class OrderDAO
	{
		private readonly Shoopi1Context _context;

		public OrderDAO(Shoopi1Context context)
		{
			_context = context;
		}

		public async Task<List<Order>> GetAllOrdersAsync()
		{
			return await _context.Orders
				.Include(x => x.OrderDetails)
					.ThenInclude(o => o.Product)
				.Include(u => u.OrderStatus)
				.OrderByDescending(o => o.OrderId)
				.ToListAsync();
		}

		public async Task<Order?> GetOrderById(int id)
		{
			return await _context.Orders
				.Include(x => x.OrderDetails)
                    .ThenInclude(o => o.Product)
                .Include(u => u.OrderStatus)
                .OrderByDescending(o => o.OrderId)

                .FirstOrDefaultAsync(p => p.OrderId == id);
		}
			
		public async Task<List<Order?>> GetOrderByUserLogin(int userId)
		{
			return await _context.Orders
				.Include(x => x.OrderDetails)
                    .ThenInclude(o => o.Product)
                .Include(u => u.OrderStatus)
                 .Where(p => p.UserId == userId)
                .OrderByDescending(o => o.OrderId)
				.ToListAsync();

        }
	}
}
