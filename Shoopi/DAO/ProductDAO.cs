using DAO.Data;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
	public class ProductDAO
	{
		private readonly Shoopi1Context _context;

		public ProductDAO(Shoopi1Context context) 
		{
			_context = context;
		}

		public async Task<List<Product>> GetAllProductsAsync()
		{
			return await _context.Products.ToListAsync();
		}

		public async Task<Product?> GetProductById(int id)
		{
			return await _context.Products.Include(x => x.Type).FirstOrDefaultAsync(p => p.ProductId == id);
		}
	}
}
