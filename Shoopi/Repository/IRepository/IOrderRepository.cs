﻿using DAO.Data;
using DAO.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IOrderRepository
	{
		Task<OrderViewModel> GetOrderById(int id);
		Task<OrderResponse> GetOrders(int? type, string query, int pageIndex, int pageSize);
		Task<OrderResponse> GetOrderByUserLogin(int userId, int? type, string query, int pageIndex, int pageSize);

    }
}
