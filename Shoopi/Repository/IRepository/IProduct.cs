using Shoopi.Data;
using Shoopi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAO.ProductDAO;

namespace Repository.IRepository
{
	public interface IProduct
	{
		Task<List<Product>> GetProducts(int? type, string query);
		Task<Product> GetProductById(int id);

	}
}
