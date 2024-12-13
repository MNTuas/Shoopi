using AutoMapper;
using DAO;
using DAO.Data;
using DAO.ViewModels;
using Microsoft.AspNetCore.Http;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class OrderRepository : IOrderRepository
	{
		private readonly OrderDAO _orderDAO;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderRepository(OrderDAO orderDAO, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
			_orderDAO = orderDAO;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

		public async Task<Order?> GetOrderById(int id)
		{
			return await _orderDAO.GetOrderById(id);
		}

        public async Task<OrderResponse> GetOrderByUserLogin(int userId, int? type, string query, int pageIndex, int pageSize)
        {
            try
            {
                var orders = await _orderDAO.GetOrderByUserLogin(userId);
				
				if(orders == null)
				{
					return null;
				}

                //paging
                var count = orders.Count;
                var totalPages = (int)Math.Ceiling(count / (double)pageSize);

                var pagedOrders = orders.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var orderViewModel = _mapper.Map<List<OrderViewModel>>(pagedOrders);

                return new OrderResponse
                {
                    Orders = orderViewModel,
                    TotalPages = totalPages,
                    PageIndex = pageIndex
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OrderResponse> GetOrders(int? type, string query, int pageIndex, int pageSize)
		{
			try
			{
				var orders = await _orderDAO.GetAllOrdersAsync();

				//paging
				var count = orders.Count;
				var totalPages = (int)Math.Ceiling(count / (double)pageSize);

				var pagedOrders = orders.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

				var orderViewModel = _mapper.Map<List<OrderViewModel>>(pagedOrders);

				return new OrderResponse
				{
					Orders = orderViewModel,
					TotalPages = totalPages,
					PageIndex = pageIndex
				};

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
