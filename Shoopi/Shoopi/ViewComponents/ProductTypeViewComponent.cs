﻿using Microsoft.AspNetCore.Mvc;
using DAO.Data;
using Shoopi.ViewModels;

namespace Shoopi.ViewComponents
{
    public class ProductTypeViewComponent : ViewComponent
    {
        private readonly Shoopi1Context db;

        public ProductTypeViewComponent(Shoopi1Context context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Types.Select(type => new ProductTypeVM
            {
				TypeID = type.TypeId,
                TypeName = type.TypeName,
                Quantity = type.Products.Count,
            });
            return View(data); //defaut.html
            //return view("default.html", data);
        }
    }
}
