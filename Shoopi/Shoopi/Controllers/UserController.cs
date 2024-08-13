using AutoMapper;
using DAO.Data;
using DAO.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Shoopi.Helper;
using System.Security.Claims;

namespace Shoopi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _user;

        public UserController(IUser user)
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

        [HttpGet]
        public IActionResult Login(string? ReturnUrl) //tra ve url neu  
        {
            ViewBag.ReturnUrl = ReturnUrl; 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var user = await _user.Login(model);
                var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, model.Email),
                                new Claim(ClaimTypes.Name, model.FullName),
                                new Claim("CustomerID", model.UserId.ToString()),

								//claim - role động
								new Claim(ClaimTypes.Role, "User")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return Redirect("/");
                }
            }
            return View();
        }
      
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
    }
}
