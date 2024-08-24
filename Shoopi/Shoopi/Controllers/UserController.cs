using AutoMapper;
using DAO.Data;
using DAO.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Helpers.Response;
using Repository.IRepository;
using Shoopi.Helper;
using System.Security.Claims;

namespace Shoopi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly Shoopi1Context _context;

        public UserController(IUserRepository user, Shoopi1Context context)
        {
			_userRepository = user;
            _context = context;
        }
        
        [ShoopiAuthorizedAddtribute("Admin","Allowed")] //authorize
        public async Task<IActionResult> GetUser()
        {
            var result = await _userRepository.GetAllUser();
            return  View(result);
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
				string rolename = string.Empty;
				if (model.RoleId == 1)
					rolename = "Admin";
				else rolename = "User";
				if (user.Success)
                {
                    var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, model.Email),
                                new Claim(ClaimTypes.Name, model.FullName),
                                new Claim(MySetting.CLAIM_CUSTOMERID, model.UserId.ToString()),

								//claim - role động
								new Claim(ClaimTypes.Role, rolename)
                };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(1),
                        IsPersistent = true, // cho duoc luu o request hay k
                    };

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

        
        public async Task LoginByGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public async Task<IActionResult> GoogleResponse(LoginVM model)
        {
            // Authenticate with CookieAuthentication
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
            {
                // Handle unsuccessful authentication
                return RedirectToAction("Login", "User");
            }

            // Extract claims from Google
            var googleClaims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims.ToList();
            if (googleClaims == null)
            {
                // Handle missing claims
                return RedirectToAction("Error", "Home");
            }

            // Extract user information from claims
            var name = googleClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = googleClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var provider = googleClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                // Handle missing email
                return RedirectToAction("404", "Home");
            }

            // Check if the user exists
            var existingUser = await _userRepository.getUserByEmailAsync(email);

            //nếu ko có user trong dtb thì tạo 1 user mới 
            if (existingUser == null)
            {
                // Register new user
                var newUser = new RegisterVM
                {
                    Email = email,
                    FullName = name ?? "Default Name" // Default name if not provided
                };

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Đăng ký người dùng mới
                    var news = await _userRepository.SignUp(newUser);

                    if (!news.Success)
                    {
                        return RedirectToAction("404", "Home");
                    }

                    var usid = news.Data.UserId;

                    var updateuser = news.Data;
                    if (updateuser != null)
                    {
                        // Cập nhật isGoogleAccount
                        updateuser.IsGoogleAccount = true;
                        _context.Update(updateuser);
                        await _context.SaveChangesAsync();
                    }

                    // Tạo bản ghi mới trong bảng UserLogin
                    var userLogin = new UserLogin
                    {
                        LoginProvider = "Google",
                        ProviderKey = provider,
                        CreatedDate = DateTime.UtcNow,
                        UserId = usid
                    };

                    await _context.UserLogins.AddAsync(userLogin);
                    await _context.SaveChangesAsync();

                    // Thêm Claim mới cho user ID
                    googleClaims.Add(new Claim(MySetting.CLAIM_CUSTOMERID, usid.ToString()));

                    // Commit transaction
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Handle exception (log error, etc.)
                }
            }

            //nếu có user tồn tại thì lấy userid từ existinguser
            else
            {
                if(existingUser.IsGoogleAccount == false) 
                {
                    existingUser.IsGoogleAccount = true;
                    _context.Update(existingUser);
                    _context.SaveChanges(true);

                    var userLogin = new List<UserLogin>();
                    userLogin.Add(new UserLogin()
                    {
                        LoginProvider = "Google",
                        ProviderKey = provider,
                        CreatedDate = DateTime.UtcNow,
                        UserId = existingUser.UserId

                    });
                    _context.AddRange(userLogin);
                    _context.SaveChanges();
                }
                googleClaims.Add(new Claim(MySetting.CLAIM_CUSTOMERID, existingUser.UserId.ToString()));
            }

            // Tạo ClaimsIdentity và ClaimsPrincipal mới
            var claimsIdentity = new ClaimsIdentity(googleClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Đăng nhập người dùng với Claims mới
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(1),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

            // Redirect to the home page
            return RedirectToAction("Index", "Home");
        }

        
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
    }
}
