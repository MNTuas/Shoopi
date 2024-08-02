using DAO;
using Repository.IRepository;
using Shoopi.Data;
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

		public async Task<List<Product>> GetProducts(int? type, string query)
		{
			return await ProductDAO.Instance.GetProducts(type, query);
		}
	}
}
