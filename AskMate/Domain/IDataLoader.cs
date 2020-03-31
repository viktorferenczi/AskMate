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
        int AddQuestion(string title, string text,string image);
        int CountAnswers(int questionId);
        int AddAnswer(int questionId, string message,string image);
        public void DeleteQuestion(int ID);
        public void DeleteAnswer(int ID);
        public void EditQuestion(int qid, string title, string text);
        public void Like(int qid);
        public void Dislike(int qid);
        public void LikeAnswer(int aid, int qid);
        public void DislikeAnswer(int aid, int qid);
        public void EditAnswer(int qid, int aid, string text);
        public Answer GetAnswerToQuestion(int qid, int aid);
        public int AddCommentToQuestion(int questionID, string message);
        public void DeleteCommentFromQuestion(int ID, int commentID);
        public int AddCommentToAnswer(int questionID, int answerID, string message);
        public void DeleteCommentFromAnswer(int questionID, int answerID, int commentID);
        public void EditCommentForQuestion(int qid, int commentID, string text);
        public void EditCommentForAnswer(int qid, int commentID, int answerID, string text);



    }
}
