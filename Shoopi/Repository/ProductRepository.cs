using DAO;
using Repository.IRepository;
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
		public async Task<List<ProductVM>> GetList(int? type)
		{
			return await ProductDAO.Instance.GetList(type);
		}

		public Task<List<ProductVM>> GetProducts(int? type, string query)
		{
			return ProductDAO.Instance.GetProducts(type, query);
		}
	}
}
