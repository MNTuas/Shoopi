using DAO.Data;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
	public class ProductDAO
	{
		private readonly ShoopiContext _context;

		public ProductDAO(ShoopiContext context) 
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
