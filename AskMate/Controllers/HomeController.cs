using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AskMate.Domain;
using AskMate.Models;

namespace AskMate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FakeDataLoader _loader;

        public HomeController(ILogger<HomeController> logger, FakeDataLoader loader)
        {
            _logger = logger;
            _loader = loader;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        public IActionResult QuestionList()
        {

            return View(_loader.GetQuestions());
        }



        public IActionResult QuestionAdd([FromForm(Name ="Title")] string title, [FromForm(Name = "Text")] string text)
        {
            _loader.AddQuestion(title, text);
                
            return View("QuestionList",_loader.GetQuestions());

        }



        public IActionResult QuestionAsking()
        {
            return View();
        }

        public IActionResult Question ()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
