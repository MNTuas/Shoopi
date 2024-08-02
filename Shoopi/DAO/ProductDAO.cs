using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shoopi.Data;
using Shoopi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
	public class ProductDAO
	{
		private readonly ShoopiContext _context;
		private static ProductDAO instance = null;
		private ProductDAO()
		{
			_context = new ShoopiContext();
		}

		public static ProductDAO Instance 
		{
			get
			{
				if(instance == null)
				{
					instance = new ProductDAO();
				}
				return instance;
			}
		}

		public async Task<List<ProductVM>> GetList(int? type)
		{
			var products = _context.Products.AsQueryable();
			if (type.HasValue)
			{
				products = products.Where(p => p.TypeId == type.Value);
			}
			var result = await products.Select(p => new ProductVM
			{
				ProductId = p.ProductId,
				ProductName = p.ProductName,
				AliasName = p.AliasName,
				Detail = p.Detail,
				Price = p.Price,
				Quantity = p.Quantity,
				Picture = p.Picture,
				ViewNumber = p.ViewNumber,
			}).ToListAsync();

			return result;
		}

		

		public async Task<List<ProductVM>> GetProducts(int? type, string query)
		{
			var products = _context.Products.AsQueryable();

			if (!string.IsNullOrEmpty(query))
			{
				products = products.Where(p => p.ProductName.Contains(query));
			}

			if (type.HasValue)
			{
				products = products.Where(p => p.TypeId == type.Value);
			}


			var result = await products.Select(p => new ProductVM
			{
				ProductId = p.ProductId,
				ProductName = p.ProductName,
				AliasName = p.AliasName,
				Detail = p.Detail,
				Price = p.Price,
				Quantity = p.Quantity,
				Picture = p.Picture,
				ViewNumber = p.ViewNumber,
			}).ToListAsync();

			return result;
		}


	}
}
