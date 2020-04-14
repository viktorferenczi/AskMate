using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AskMate.Domain;
using AskMate.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace AskMate.Controllers
{
  
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly DataBaseLoader _DBloader;

        public AccountController(ILogger<AccountController> logger, DataBaseLoader DBloader)
        {
            _logger = logger;
            _DBloader = DBloader;
        }
    
       [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromForm] string email, [FromForm] string password)
        {

            if ("test@test.com" != email || "test" != password)
            {
                return RedirectToAction("Index", "Home");
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
               
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


    }
}
