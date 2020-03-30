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
        private readonly DataLoader _loader;

        public HomeController(ILogger<HomeController> logger, DataLoader loader)
        {
            _logger = logger;
            _loader = loader;
        }
    

        public IActionResult Index()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.PostedDate).ToList();
            List.Reverse();
            return View(List);
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
     
        public IActionResult Question(int id, [FromForm(Name = "comment")] string comment, string image, [FromForm(Name = "question_comment")] string message, [FromForm(Name = "anid")] string anid, [FromForm(Name = "answer_message")] string answermessage)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            if (comment != null)
            {
               _loader.AddAnswer(id, comment,image);
            }

            if (message != null)
            {
                _loader.AddCommentToQuestion(id, message);
            }

            if (answermessage != null)
            {
                _loader.AddCommentToAnswer(id, Convert.ToInt32(anid), answermessage);
            }
            return View(question);
        }

        public RedirectResult View(int id)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            question.NumOfViews++;
            return Redirect($"/Home/Question/{id}");
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
        public IActionResult QuestionAddTag(int id)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            return View(question);
        }

        public IActionResult AnswerEdit([FromQuery]int aid, [FromQuery] int qid, [FromForm(Name = "Text")] string text)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _loader.EditAnswer(aid, qid,text);
            return View(_loader.GetAnswerToQuestion(qid, aid));
        }
        public IActionResult DeleteAnswer(int id,int qid)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _loader.DeleteAnswer(id);
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

        public ActionResult SortingByDateDesc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.PostedDate).ToList();
            return View("QuestionList", List);
        }

        public ActionResult SortingByDateAsc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.PostedDate).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }

        public ActionResult SortingByViewsDesc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.NumOfViews).ToList();
            return View("QuestionList", List);
        }

        public ActionResult SortingByViewsAsc()
        {
            List<Question> List = _loader.GetQuestions();
            List = List.OrderBy(q => q.NumOfViews).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }


        public IActionResult DeleteCommentFromQuestion(int commentid, int questionid)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == questionid);
            _loader.DeleteCommentFromQuestion(questionid, commentid);
            return Redirect($"/Home/Question/{questionid}");
        }


        public IActionResult DeleteCommentFromAnswer(int commentid, int questionid, int answerid)
        {
            var questionModel = _loader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == questionid);
            _loader.DeleteCommentFromAnswer(questionid, answerid, commentid);
            return Redirect($"/Home/Question/{questionid}");
        }

    }
}
