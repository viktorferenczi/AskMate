using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class DataBaseLoader : IDataLoader
    {

        private List<Question> ListOfQuestionsDB = new List<Question>();

        public void Load()
        {
            var connString = "Host=localhost;Username=postgres;Password=admin;Database=AskMate";
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM question", conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var questiontitle = reader["question_title"];
                        var questiontext = reader["question_text"];
                        Console.WriteLine($"question title = {questiontitle} question text = {questiontext}");
                    }
                }
            }
        }


        public int AddAnswer(int questionId, string message, string image)
        {
            throw new NotImplementedException();
        }

        public int AddCommentToAnswer(int questionID, int answerID, string message)
        {
            throw new NotImplementedException();
        }

        public int AddCommentToQuestion(int questionID, string message)
        {
            throw new NotImplementedException();
        }

        public int AddQuestion(string title, string text, string image)
        {
            throw new NotImplementedException();
        }

        public int CountAnswers(int questionId)
        {
            throw new NotImplementedException();
        }

        public void DeleteAnswer(int ID)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommentFromAnswer(int questionID, int answerID, int commentID)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommentFromQuestion(int ID, int commentID)
        {
            throw new NotImplementedException();
        }

        public void DeleteQuestion(int ID)
        {
            throw new NotImplementedException();
        }

        public void Dislike(int qid)
        {
            throw new NotImplementedException();
        }

        public void DislikeAnswer(int aid, int qid)
        {
            throw new NotImplementedException();
        }

        public void EditAnswer(int qid, int aid, string text)
        {
            throw new NotImplementedException();
        }

        public void EditCommentForAnswer(int qid, int commentID, int answerID, string text)
        {
            throw new NotImplementedException();
        }

        public void EditCommentForQuestion(int qid, int commentID, string text)
        {
            throw new NotImplementedException();
        }

        public void EditQuestion(int qid, string title, string text)
        {
            throw new NotImplementedException();
        }

        public Answer GetAnswerToQuestion(int qid, int aid)
        {
            throw new NotImplementedException();
        }

        public Question GetQuestion(int questionId)
        {
            throw new NotImplementedException();
        }

        public List<Question> GetQuestions()
        {
            throw new NotImplementedException();
        }

        public void Like(int qid)
        {
            throw new NotImplementedException();
        }

        public void LikeAnswer(int aid, int qid)
        {
            throw new NotImplementedException();
        }
    }
}
