using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AskMate.Domain;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using AskMate.Models;
using Microsoft.AspNetCore.Authorization;

namespace AskMate.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly DataBaseLoader _DBloader;
        private readonly IUserService _userService;
        private readonly InDataBaseService _userDatabaseService;

        public AccountController(ILogger<AccountController> logger, DataBaseLoader DBloader, IUserService userService, InDataBaseService _userDatabaseService)
        {
            _userService = userService;
            this._userDatabaseService = _userDatabaseService;
        }

        public IActionResult Login()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromForm] string email, [FromForm] string password)
        {
            UserModel user = _userDatabaseService.Login(email, password);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
               


            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Profile");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] string email, [FromForm] string password)
        {
            _userDatabaseService.RegisterIntoDatabase(email, password);
            _userService.AddUser(_userService.Register(email, password));
            return RedirectToAction("Index", "Profile");
        }

    }
}
