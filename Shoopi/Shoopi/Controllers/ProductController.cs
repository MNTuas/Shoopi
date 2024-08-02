using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Shoopi.Data;
using Shoopi.ViewModels;

namespace Shoopi.Controllers
{
	public class ProductController : Controller
	{
		
		private readonly IProduct _product;
		public ProductController(IProduct product) 
		{
            _product = product;
		}
		public IList<Product> Products { get; set; } = default!;
		public async Task<IActionResult> Index(int? type, string query)
		{
			Products = await _product.GetProducts(type, query);
			return View(Products);
			
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
