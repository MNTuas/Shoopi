using AutoMapper;
using DAO.Data;
using DAO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Shoopi.Helper;

namespace Shoopi.Controllers
{
    public class UserController : Controller
    {
		private readonly IUser _user;

		public UserController( IUser user) 
        {
            _user = user; 
        }


        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                await _user.SignUp(model);
                var url = Url.Action("Index", "Home");
                return Redirect(url);
            }
            return View(model);
        }

    }
}
