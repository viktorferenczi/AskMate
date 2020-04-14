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

namespace AskMate.Controllers
{
   [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly DataBaseLoader _DBloader;

        public ProfileController(ILogger<ProfileController> logger, DataBaseLoader DBloader)
        {
            _logger = logger;
            _DBloader = DBloader;
        }
    
       
        public IActionResult Index()
        {
            
            return View(new UserModel
            {
                Email = "asd@asd.com",
                Password = "valami2",
            });
        }

    }
}
