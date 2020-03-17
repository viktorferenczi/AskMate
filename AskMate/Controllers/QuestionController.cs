using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskMate.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AskMate.Controllers
{
    public class QuestionController : Controller
    {

        private readonly ILogger<QuestionController> _logger;
        private readonly FakeDataLoader _loader;

        public QuestionController(ILogger<QuestionController> logger, FakeDataLoader loader)
        {
            _logger = logger;
            _loader = loader;
        }

        public IActionResult Question(int id, [FromForm(Name = "comment")] string comment = "lol")
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            _loader.AddComment(id, comment);
            return View(question);
        }

    }

}