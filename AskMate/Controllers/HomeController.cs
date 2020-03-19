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

        public IActionResult QuestionAsking()
        {

            return View();
        }

        public IActionResult AskQuestion([FromForm(Name = "Title")] string title, [FromForm(Name = "Text")] string text, [FromForm(Name = "Image")] string image)
        {
            _loader.AddQuestion(title, text, image);
            return View("QuestionList",_loader.GetQuestions());
        }
     
        public IActionResult Question(int id, [FromForm(Name = "comment")] string comment, string image )
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            if (comment != null)
            {
               _loader.AddComment(id, comment,image);
            }
            return View(question);
        }

        public IActionResult Delete(int id)
        {
            _loader.DeleteQuestion(id);
            return Redirect("/Home/QuestionList");
        }

        public IActionResult QuestionEdit(int id, [FromForm(Name = "Title")] string title, [FromForm(Name = "Text")] string text)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            _loader.EditQuestion(id, title, text);
            return View(question);
        }

        public IActionResult DeleteAnswer(int id,int qid)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _loader.DeleteComment(id);
            return Redirect($"/Home/Question/{qid}");
        }

        public IActionResult Like(int qid)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _loader.Like(qid);
            return Redirect($"/Home/Question/{qid}");
        }
        public IActionResult Dislike(int qid)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _loader.Dislike(qid);
            return Redirect($"/Home/Question/{qid}");
        }


        public IActionResult LikeAnswer(int qid, int aid)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _loader.LikeAnswer(aid,qid);
            return Redirect($"/Home/Question/{qid}");
        }
        public IActionResult DislikeAnswer(int qid, int aid)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _loader.DislikeAnswer(aid,qid);
            return Redirect($"/Home/Question/{qid}");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult SortingByTitleDesc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.Title).ToList();
            return View("QuestionList", List);
        }

        public ActionResult SortingByLikesDesc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.Like).ToList();
            
            return View("QuestionList", List);
        }

        public ActionResult SortingByCommentsDesc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.NumOfMessages).ToList();
           
            return View("QuestionList", List);
        }
        public ActionResult SortingByTitleAsc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.Title).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }

        public ActionResult SortingByLikesAsc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.Like).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }

        public ActionResult SortingByCommentsAsc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.NumOfMessages).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }
    }
}
