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

namespace AskMate.Controllers
{
   [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly DataBaseLoader _DBloader;
        private readonly IUserService _userService;
        private readonly InDataBaseService _DBService;

        public ProfileController(ILogger<ProfileController> logger, DataBaseLoader DBloader, IUserService userService, InDataBaseService _DBService )
        {
            _logger = logger;
            _DBloader = DBloader;
            _userService = userService;
            this._DBService = _DBService; 
        }
    
       
        public IActionResult Index()
        {
            var email = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
     
            UserModel user = _DBService.GetOne(email);
            if (user != null)
            {
                return View(new UserModel(user.Id, user.Email, user.Password));
            }
            return RedirectToAction("Login", "Account");
        }

    }
}
