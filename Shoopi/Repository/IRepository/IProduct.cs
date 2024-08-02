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
		Task<List<ProductVM>> GetList(int? type);
		Task<List<ProductVM>> GetProducts(int? type, string query);

	}
}
