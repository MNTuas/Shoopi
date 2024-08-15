using DAO;
using Repository.IRepository;
using DAO.Data;
using DAO.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly ProductDAO _productDAO;

		public ProductRepository(ProductDAO productDAO)
		{
			_productDAO = productDAO;
		}
		//tra ve null
		public async Task<Product?> GetProductById(int id)
		{
			return await _productDAO.GetProductById(id);			
		}

		//type lay tu viewcomponent
		public async Task<ProductResponse> GetProducts(int? type, string query, int pageIndex, int pageSize)
		{
			try
			{
				var products = await _productDAO.GetAllProductsAsync();

				if (!string.IsNullOrEmpty(query))
				{
					products = products.Where(p => p.ProductName.Contains(query)).ToList();
				}

				if (type.HasValue)
				{
					products = products.Where(p => p.TypeId == type.Value).ToList();
				}

				//paging
				var count = products.Count;
				var totalPages = (int)Math.Ceiling(count / (double)pageSize);

				var pagedProducts = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

				return new ProductResponse
				{
					Products = pagedProducts,
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
