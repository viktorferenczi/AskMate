using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class DataBaseLoader : IDataLoader
    {

        private List<Question> ListOfQuestions = new List<Question>();
        private static readonly string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string dbPass = Environment.GetEnvironmentVariable("DB_PASS");
        private static readonly string dbName = Environment.GetEnvironmentVariable("DB_NAME");
        public static readonly string connectingString = $"Host={dbHost};Username={dbUser};Password={dbPass};Database={dbName}";

       
        // --------------------------------------------------------------------------------- 1

        public void AddQuestion(string title, string text, string image)
        {
            //int nextID;
            //if (ListOfQuestions.Count == 0)
            //{
            //    nextID = 1;
            //}
            //else
            //{
            //    nextID = ListOfQuestions.Select(q => q.ID).Max() + 1;
            //}
            
        

            using (var conn = new NpgsqlConnection(connectingString))

            {
                conn.Open();
                
                var command = new NpgsqlCommand($"INSERT INTO question (question_title,question_text,question_image,submission_time) VALUES ({title},{text},{image},{DateTime.Now})", conn);
                
                command.ExecuteNonQuery();
            }

            //ListOfQuestions.Add(new Question(nextID, title, text, image, DateTime.Now));
        }

        public int CountAnswers(int questionId)
        {
            int counter = 0;

            using (var conn = new NpgsqlConnection(connectingString))

            {
                conn.Open();

                using (var command = new NpgsqlCommand($"SELECT COUNT(answer_id) as f FROM answer WHERE question_id = {questionId}", conn)) 
                {
                    var reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        var count = reader["f"];
                        counter = Convert.ToInt32(count);
                        return counter;
                    }
                }
            }
            return 0;
        }

        public Question GetQuestion(int questionId)
        {
            Question question = new Question();
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand($"SELECT * FROM question WHERE question_id = {questionId}", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var question_id = Convert.ToInt32(reader["question_id"]);
                        var submission_time = Convert.ToDateTime(reader["submission_time"]);
                        var view_number = Convert.ToInt32(reader["view_number"]);
                        var vote_number = Convert.ToInt32(reader["vote_number"]);
                        var downvote_number = Convert.ToInt32(reader["downvote_number"]);
                        var question_title = Convert.ToString(reader["question_title"]);
                        var question_text = Convert.ToString(reader["question_text"]);
                        var question_image = Convert.ToString(reader["question_image"]);
                        question = new Question(question_id, question_title, question_text, question_image, vote_number,downvote_number,view_number, submission_time);
                    }
                }
            }
            return question;
        }

        public List<Question> GetQuestions()
        {
            return ListOfQuestions;
        }

        public void AddAnswer(int questionID, string message, string image)
        {
            using (var conn = new NpgsqlConnection(connectingString))

            {
                conn.Open();

                var command = new NpgsqlCommand($"INSERT INTO answer (question_id,answer_text,answer_image,submission_time) VALUES ({questionID},{message},{image},{DateTime.Now})", conn);

                command.ExecuteNonQuery();
            }

        }

        public void DeleteQuestion(int ID)
        {
            using (var conn = new NpgsqlConnection(connectingString))

            {
                conn.Open();

                var command = new NpgsqlCommand($"DELETE FROM question WHERE question_id = {ID}");

                command.ExecuteNonQuery();
            }
        }


        public void DeleteAnswer(int ID)
        {

            using (var conn = new NpgsqlConnection(connectingString))

            {
                conn.Open();

                var command = new NpgsqlCommand($"DELETE FROM answer WHERE answer_id = {ID}");

                command.ExecuteNonQuery();
            }
        }

        // --------------------------------------------------------------------------------- 2




        public void EditQuestion(int qid, string title, string text)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question " +
                                                $"SET question_title = {title}, question_text = {text}" +
                                                $"WHERE question_id = {qid}");

                command.ExecuteNonQuery();
            }
        }


        public void Like(int qid)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question SET vote_number = vote_number + 1 WHERE question_id = {qid}");

                command.ExecuteNonQuery();
            }
        }


        public void Dislike(int qid)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question SET downvote_number = downvote_number + 1 WHERE question_id = {qid}");

                command.ExecuteNonQuery();
            }
        }

        public void LikeAnswer(int aid, int qid = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE answer SET vote_number = vote_number + 1 WHERE answer_id = {aid}");

                command.ExecuteNonQuery();
            }

        }

        public void DislikeAnswer(int aid, int qid = 0)
        {

            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE answer SET downvote_number = downvote_number + 1 WHERE answer_id = {aid}");

                command.ExecuteNonQuery();
            }
        }

        public void EditAnswer(int aid, string text, int qid = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE answer " +
                                                $"SET answer_text = {text}" +
                                                $"WHERE answer_id = {aid}");

                command.ExecuteNonQuery();
            }
        }


        public Answer GetAnswerToQuestion(int aid, int qid = 0)
        {
            Answer answer = new Answer();

            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand($"SELECT * FROM answer WHERE answer_id = {aid}", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var answer_id = Convert.ToInt32(reader["answer_id"]);
                        var downvote_number = Convert.ToInt32(reader["downvote_number"]);
                        var question_id = Convert.ToInt32(reader["question_id"]);
                        var submission_time = Convert.ToDateTime(reader["submission_time"]);
                        var vote_number = Convert.ToInt32(reader["vote_number"]);
                        var answer_text = Convert.ToString(reader["answer_text"]);
                        var answer_image = Convert.ToString(reader["answer_image"]);
                        answer = new Answer(answer_id, question_id, answer_text, answer_image, vote_number, downvote_number, submission_time);
                    }
                }
            }
            return answer;

        }


        // --------------------------------------------------------------------------------- 3




        public void AddCommentToQuestion(int questionID, string message)
        {
           
            // xdddd
            

        }

        public void DeleteCommentFromQuestion(int ID, int commentID)
        {

            foreach (var q in ListOfQuestions)
            {
                if (q.ID.Equals(ID))
                {
                    foreach (var comment in q.ListOfComments)
                    {
                        if (comment.ID == commentID)
                        {
                            q.ListOfComments.Remove(comment);
                            return;
                        }
                    }
                }
            }
        }


        public int AddCommentToAnswer(int questionID, int answerID, string message)
        {
            int nextID = 0;
            foreach (var q in ListOfQuestions)
            {
                if (q.ListOfComments.Count == 0)
                {
                    nextID = 1;
                }
                else
                {
                    nextID = q.ListOfComments.Select(aq => q.ID).Max() + 1;
                }
            }

            foreach (var q in ListOfQuestions)
            {
                if (q.ID.Equals(questionID))
                {
                    foreach (var answer in q.ListOfAnswers)
                    {
                        if (answer.ID.Equals(answerID))
                        {
                            answer.ListOfComments.Add(new Comment(nextID, message, questionID));
                            return nextID;
                        }
                    }
                }
            }
            return nextID;
        }


        public void DeleteCommentFromAnswer(int questionID, int answerID, int commentID)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID.Equals(questionID))
                {
                    foreach (var answer in q.ListOfAnswers)
                    {
                        if (answer.ID.Equals(answerID))
                        {
                            foreach (var comment in answer.ListOfComments)
                            {
                                if (comment.ID.Equals(commentID))
                                {
                                    answer.ListOfComments.Remove(comment);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

        }


        public void EditCommentForQuestion(int qid, int commentID, string text)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID == qid)
                {
                    foreach (var comment in q.ListOfComments)
                    {
                        if (comment.ID.Equals(commentID))
                        {
                            comment.Text = text;
                        }
                    }
                }
            }
        }

        public void EditCommentForAnswer(int qid, int commentID, int answerID, string text)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID == qid)
                {
                    foreach (var answer in q.ListOfAnswers)
                    {
                        if (answer.ID.Equals(answerID))
                        {
                            foreach (var comment in answer.ListOfComments)
                            {
                                if (comment.ID.Equals(commentID))
                                {
                                    comment.Text = text;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
