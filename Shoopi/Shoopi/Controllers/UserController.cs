using AutoMapper;
using DAO.Data;
using DAO.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Helpers.Response;
using Repository.IRepository;
using Shoopi.Helper;
using System.Security.Claims;

namespace Shoopi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository user)
        {
			_userRepository = user;
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
                var result = await _userRepository.SignUp(model);
                if (result.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                    return View(model);
                }              
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
                var user = await _userRepository.Login(model);
                if (user.Success)
                {
                    var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, model.Email),
                                new Claim(ClaimTypes.Name, model.FullName),
                                new Claim(MySetting.CLAIM_CUSTOMERID, model.UserId.ToString()),

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
                else
                {
                    ViewBag.ErrorMessage = user.ErrorMessage;
                    return View(model);  
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
