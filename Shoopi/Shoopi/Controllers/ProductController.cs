using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Shoopi.Data;
using Shoopi.ViewModels;

namespace Shoopi.Controllers
{
	public class ProductController : Controller
	{
		[BindProperty(SupportsGet = true)]
		public int PageIndex { get; set; } = 1;
		public int PageSize { get; set; } = 3;
		public int TotalPages { get; set; }

		private readonly IProduct _product;
		public ProductController(IProduct product) 
		{
            _product = product;
		}
		public IList<Product> Products { get; set; } = default!;
		public async Task<IActionResult> Index(int? type, string query, int PageIndex = 1)
		{
			var result = await _product.GetProducts(type, query, PageIndex, 6);
			Products = result.Products;
			PageIndex = result.PageIndex;
			TotalPages = result.TotalPages;
			
			return View(result);
		}

		public async Task<IActionResult> ProductDetail(int id)
		{
			if (id == null)
			{
				return Redirect("/404");
			}
			var products = await _product.GetProductById(id);
			if (products == null)
			{
				return Redirect("/404");
			}
			return View(products);
		}
		
    }
}
