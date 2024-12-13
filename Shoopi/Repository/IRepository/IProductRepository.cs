using DAO.Data;
using DAO.ViewModels.Response;
using Shoopi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAO.ProductDAO;

namespace Repository.IRepository
{
    public interface IProductRepository
	{

		Task<Product> GetProductById(int id);
		Task<ProductResponse> GetProducts(int? type, string query, int pageIndex, int pageSize);

	}
}
