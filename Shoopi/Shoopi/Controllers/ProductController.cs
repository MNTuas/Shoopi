using Microsoft.AspNetCore.Mvc;
using Shoopi.Data;
using Shoopi.ViewModels;

namespace Shoopi.Controllers
{
	public class ProductController : Controller
	{
		private readonly ShoopiContext _context;
		public ProductController(ShoopiContext context) 
		{
            _context = context;
		}
		public IActionResult Index(int? type)
		{
			var products = _context.Products.AsQueryable();
			if (type.HasValue)
			{
				 products = products.Where( p => p.TypeId == type.Value );
			}
			var result = products.Select(p => new ProductVM
			{
				ProductId = p.ProductId,
				ProductName = p.ProductName,
				AliasName = p.AliasName,
				Detail = p.Detail,
				Price = p.Price,
				Quantity = p.Quantity,
				Picture = p.Picture,
				ViewNumber = p.ViewNumber,
			});
			return View(result);
		}

        

        public IActionResult Search(string query)
        {
            var products = _context.Products.AsQueryable();
            if (query != null)
            {
                products = products.Where(p => p.ProductName.Contains(query));
            }
            var result = products.Select(p => new ProductVM
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                AliasName = p.AliasName,
                Detail = p.Detail,
                Price = p.Price,
                Quantity = p.Quantity,
                Picture = p.Picture,
                ViewNumber = p.ViewNumber,
            });
            return View(result);
        }
    }
}
