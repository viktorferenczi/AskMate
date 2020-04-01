using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public interface IDataLoader
    {
        List<Question> GetQuestions();
        Question GetQuestion(int questionId);
        public void AddQuestion(string title, string text,string image);
        public int CountAnswers(int questionId);
        public void AddAnswer(int questionId, string message,string image);
        public void DeleteQuestion(int ID);
        public void DeleteAnswer(int ID);
        public void EditQuestion(int qid, string title, string text);
        public void Like(int qid);
        public void Dislike(int qid);
        public void LikeAnswer(int aid, int qid);
        public void DislikeAnswer(int aid, int qid);
        public void EditAnswer(int aid, string text, int qid);
        public Answer GetAnswerToQuestion(int qid, int aid);
        public void AddCommentToQuestion(int questionID, string message);
        public void DeleteCommentFromQuestion(int ID, int commentID);
        public void AddCommentToAnswer(int answerID, string message, int questionID);
        public void DeleteCommentFromAnswer(int questionID, int answerID, int commentID);
        public void EditCommentForQuestion(int commentID, string text, int questionID);
        public void EditCommentForAnswer(string text, int commentID, int answerID , int questionID);



    }
}
