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
		public IList<ProductVM> Products { get; set; } = default!;
		public async Task<IActionResult> Index(int? type, string query)
		{
			Products = await _product.GetProducts(type, query);
			return View(Products);
			
		}       
    }
}
