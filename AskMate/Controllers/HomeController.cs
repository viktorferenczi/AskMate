﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AskMate.Domain;
using AskMate.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AskMate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseLoader _DBloader;
        private readonly InDataBaseService _DataBaseservice;


        public HomeController(ILogger<HomeController> logger, DataBaseLoader DBloader, InDataBaseService _DataBaseservice)
        {
            _logger = logger;
            _DBloader = DBloader;
            this._DataBaseservice = _DataBaseservice;
        }


        public IActionResult Index()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
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
            return View(_DBloader.GetQuestions());
        }

        public ActionResult Search(string search)
        {
            if (search != null)
            {
                return View("QuestionList", _DBloader.GetQuestions().Where(x => x.Title.Contains(search) || x.Text.Contains(search) || search == null).ToList());
            }
            return View("QuestionList", _DBloader.GetQuestions());


        }

        [Authorize]
        public IActionResult QuestionAsking()
        {
            return View();
        }


        public IActionResult AskQuestion([FromForm(Name = "Title")] string title, [FromForm(Name = "Text")] string text, [FromForm(Name = "Image")] string image)
        {


            var email = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            UserModel user = _DataBaseservice.GetOne(email);

            _DBloader.AddQuestion(title, text, image, user.Id);
            return View("QuestionList", _DBloader.GetQuestions());
        }
     
        [Authorize]
        public IActionResult Question(int id, [FromForm(Name = "comment")] string comment, string image, [FromForm(Name = "question_comment")] string message, [FromForm(Name = "anid")] string anid, [FromForm(Name = "answer_message")] string answermessage)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            var email = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            UserModel user = _DataBaseservice.GetOne(email);
            if (comment != null && email != null)
            {
                _DBloader.AddAnswer(id, comment,image,user.Id);
                _DBloader.PlusNumberOfMessages(id);
            }
          

            if (message != null)
            {
                _DBloader.AddCommentToQuestion(id, message,user.Id);
            }

            if (answermessage != null)
            {
                _DBloader.AddCommentToAnswer(Convert.ToInt32(anid), answermessage,user.Id);
            }

            var newquestionModel = _DBloader.GetQuestions();
            var newquestion = questionModel.FirstOrDefault(q => q.ID == id);
           
            
            return View(newquestion);
        }

       // respond.redirect, actionname, teljes eleresi ut cshtml
     

        public RedirectResult View(int id)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            _DBloader.PlusViewToQuestion(id);
            return Redirect($"/Home/Question/{id}");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            _DBloader.DeleteQuestion(id);
            return Redirect("/Home/QuestionList");
        }


        [Authorize]
        public IActionResult QuestionEdit(int id, [FromForm(Name = "Title")] string title, [FromForm(Name = "Text")] string text)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            _DBloader.EditQuestion(id, title, text);
            return View(question);
        }

        public IActionResult QuestionAddTag(int id)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == id);
            return View(question);
        }

        [Authorize]
        public ActionResult AnswerEditing([FromQuery]int aid, [FromQuery] int qid)
        {
            return View("AnswerEdit", _DBloader.GetAnswerModelToQuestion(aid,qid));
        }

     

        public IActionResult AnswerEdit([FromForm(Name = "aid")]int aid, [FromForm(Name = "qid")] int qid, [FromForm(Name = "Text")] string text)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);

            if (text != null)
            {
                _DBloader.EditAnswer(aid, text);
            }

            var ans = _DBloader.GetAnswerModelToQuestion(aid); 
            return View(ans);
        }
        [Authorize]
        public ActionResult Answer_CommentModelEditing([FromQuery]int aid, [FromQuery] int cid)
        {
            return View("Answer_CommentModelEdit", _DBloader.GetCommentModelToAnswer(cid));
        }



        public IActionResult Answer_CommentModelEdit([FromForm(Name = "cid")]int cid, [FromForm(Name = "qid")] int qid, [FromForm(Name = "Text")] string text)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);

            if (text != null)
            {
                var email = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
              

                _DBloader.EditCommentForAnswer(text, cid);
                _DBloader.PlusEditedForAnswerComment(cid);
            }

            var comment = _DBloader.GetCommentModelToAnswer(cid);
            return View(comment);
        }

        [Authorize]
        public ActionResult Question_CommentModelEditing([FromQuery]int qid, [FromQuery] int cid)
        {
            return View("Question_CommentModelEdit", _DBloader.GetCommentModelToQuestion(cid));
        }



        public IActionResult Question_CommentModelEdit([FromForm(Name = "cid")]int cid, [FromForm(Name = "qid")] int qid, [FromForm(Name = "Text")] string text)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);

            if (text != null)
            {
                _DBloader.EditCommentForQuestion(cid, text);
                _DBloader.PlusEditedForQuestionComment(cid);
            }

            var comment = _DBloader.GetCommentModelToQuestion(cid);
            return View(comment);
        }


        [Authorize]
        public IActionResult DeleteAnswer(int id,int qid)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _DBloader.DeleteCommentFromAnswerWithAnswerID(id);
            _DBloader.DeleteAnswer(id);
            _DBloader.MinusNumberOfMessages(qid);
          
            return Redirect($"/Home/Question/{qid}");
        }

        [Authorize]
        public IActionResult Like(int qid)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _DBloader.Like(qid);
            return Redirect($"/Home/Question/{qid}");
        }

        [Authorize]
        public IActionResult Dislike(int qid)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _DBloader.Dislike(qid);
            return Redirect($"/Home/Question/{qid}");
        }

        [Authorize]
        public IActionResult LikeAnswer(int qid, int aid)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _DBloader.LikeAnswer(aid,qid);
            return Redirect($"/Home/Question/{qid}");
        }

        [Authorize]
        public IActionResult DislikeAnswer(int qid, int aid)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == qid);
            _DBloader.DislikeAnswer(aid,qid);
            return Redirect($"/Home/Question/{qid}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult SortingByTitleDesc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.Title).ToList();
            return View("QuestionList", List);
        }

        public ActionResult SortingByLikesDesc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.Like).ToList();
            return View("QuestionList", List);
        }

        public ActionResult SortingByCommentsDesc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.NumOfMessages).ToList();
            return View("QuestionList", List);
        }
        public ActionResult SortingByTitleAsc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.Title).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }

        public ActionResult SortingByLikesAsc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.Like).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }

        public ActionResult SortingByCommentsAsc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.NumOfMessages).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }

        public ActionResult SortingByDateDesc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.PostedDate).ToList();
            return View("QuestionList", List);
        }

        public ActionResult SortingByDateAsc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.PostedDate).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }

        public ActionResult SortingByViewsDesc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.NumOfViews).ToList();
            return View("QuestionList", List);
        }

        public ActionResult SortingByViewsAsc()
        {
            List<QuestionModel> List = _DBloader.GetQuestions();
            List = List.OrderBy(q => q.NumOfViews).ToList();
            List.Reverse();
            return View("QuestionList", List);
        }

        [Authorize]
        public IActionResult DeleteCommentFromQuestion(int commentid, int questionid)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == questionid);
            _DBloader.DeleteCommentFromQuestion(commentid);
            
            return Redirect($"/Home/Question/{questionid}");
        }

        [Authorize]
        public IActionResult DeleteCommentFromAnswer(int commentid, int questionid, int answerid)
        {
            var questionModel = _DBloader.GetQuestions();
            var question = questionModel.FirstOrDefault(q => q.ID == questionid);
            _DBloader.DeleteCommentFromAnswer(commentid);
            return Redirect($"/Home/Question/{questionid}");
        }

    }
}
