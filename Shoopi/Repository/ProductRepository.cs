using DAO;
using Repository.IRepository;
using DAO.Data;
using Shoopi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAO.ProductDAO;

namespace Repository
{
	public class ProductRepository : IProduct
	{
		public Task<Product> GetProductById(int id)
		{
			return ProductDAO.Instance.GetProductById(id);
		}

		public async Task<ProductResponse> GetProducts(int? type, string query, int pageIndex, int pageSize)
		{
			return await ProductDAO.Instance.GetProducts(type, query, pageIndex, pageSize);
		}

		//public async Task<List<Product>> GetProducts(int? type, string query)
		//{
		//	return await ProductDAO.Instance.GetProducts(type, query);
		//}
	}
}
