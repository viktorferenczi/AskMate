using AskMate.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class DataBaseLoader : IDataLoader
    {

        private static readonly string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string dbPass = Environment.GetEnvironmentVariable("DB_PASS");
        private static readonly string dbName = Environment.GetEnvironmentVariable("DB_NAME");
        public static readonly string connectingString = $"Host={dbHost};Username={dbUser};Password={dbPass};Database={dbName}";

       
        // --------------------------------------------------------------------------------- 1

        public List<QuestionModel> GetQuestions()
        {
            List<QuestionModel> questions = new List<QuestionModel>();
            List<AnswerModel> answers = new List<AnswerModel>();
            List<Question_CommentModel> questioncomments = new List<Question_CommentModel>();
            List<Answer_CommentModel> answercomments = new List<Answer_CommentModel>();

            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand($"SELECT * FROM question", conn))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        QuestionModel questionModel = new QuestionModel();
                        var question_id = Convert.ToInt32(reader["question_id"]);
                        var submission_time = Convert.ToDateTime(reader["submission_time"]);
                        var view_number = Convert.ToInt32(reader["view_number"]);
                        var vote_number = Convert.ToInt32(reader["vote_number"]);
                        var downvote_number = Convert.ToInt32(reader["downvote_number"]);
                        var question_title = Convert.ToString(reader["question_title"]);
                        var question_text = Convert.ToString(reader["question_text"]);
                        var question_image = Convert.ToString(reader["question_image"]);
                        var question_messagenumber = Convert.ToInt32(reader["message_number"]);

                        questionModel = new QuestionModel(question_id, question_title, question_text, question_image, vote_number, downvote_number, view_number, submission_time, question_messagenumber);
                        questions.Add(questionModel);
                    }
                }

                conn.Close();

                conn.Open();

                using (var commandanswer = new NpgsqlCommand($"SELECT * FROM answer", conn))
                {
                    var readerans = commandanswer.ExecuteReader();
                    while (readerans.Read())
                    {
                        AnswerModel answer = new AnswerModel();
                        var answer_id = Convert.ToInt32(readerans["answer_id"]);
                        var adownvote_number = Convert.ToInt32(readerans["downvote_number"]);
                        var aquestion_id = Convert.ToInt32(readerans["question_id"]);
                        var asubmission_time = Convert.ToDateTime(readerans["submission_time"]);
                        var avote_number = Convert.ToInt32(readerans["vote_number"]);
                        var answer_text = Convert.ToString(readerans["answer_text"]);
                        var answer_image = Convert.ToString(readerans["answer_image"]);
                        answer = new AnswerModel(answer_id, aquestion_id, answer_text, answer_image, avote_number, adownvote_number, asubmission_time);
                        answers.Add(answer);
                    }
                }

                conn.Close();

                conn.Open();

                using (var commandcomment = new NpgsqlCommand($"SELECT * FROM question_comment", conn))
                {
                    var readercomm = commandcomment.ExecuteReader();
                    while (readercomm.Read())
                    {
                        Question_CommentModel comment = new Question_CommentModel();
                        var comment_id = Convert.ToInt32(readercomm["comment_id"]);
                        var cquestion_id = Convert.ToInt32(readercomm["question_id"]);
                        var csubmission_time = Convert.ToDateTime(readercomm["submission_time"]);
                        var comment_text = Convert.ToString(readercomm["comment_text"]);
                        var comment_edited = Convert.ToInt32(readercomm["edited_number"]);

                        comment = new Question_CommentModel(comment_id, comment_text, cquestion_id,csubmission_time,comment_edited);
                        questioncomments.Add(comment);
                    }
                }

                conn.Close();


                conn.Open();

                using (var commandcomment = new NpgsqlCommand($"SELECT * FROM answer_comment", conn))
                {
                    var readercomm = commandcomment.ExecuteReader();
                    while (readercomm.Read())
                    {
                        Answer_CommentModel comment = new Answer_CommentModel();
                        var comment_id = Convert.ToInt32(readercomm["comment_id"]);
                        var canswer_id = Convert.ToInt32(readercomm["answer_id"]);
                        var csubmission_time = Convert.ToDateTime(readercomm["submission_time"]);
                        var comment_text = Convert.ToString(readercomm["comment_text"]);
                        var comment_edited = Convert.ToInt32(readercomm["edited_number"]);

                        comment = new Answer_CommentModel(comment_id, comment_text, canswer_id, csubmission_time, comment_edited);
                        answercomments.Add(comment);
                    }
                }

                conn.Close();

                foreach (var question in questions)
                {
                    foreach (var answer in answers)
                    {
                        if (question.ID == answer.QuestionID)
                        {
                            question.AnswerModels.Add(answer);
                        }
                    }
                }


                foreach (var question in questions)
                {
                    foreach (var answer in question.AnswerModels)
                    {

                        foreach (var comment in answercomments)
                        {
                            if (comment.AnswerID == answer.ID)
                            {
                                answer.CommentModels.Add(comment);
                            }
                        }
                    }
                }



                foreach (var question in questions)
                {
                    foreach (var comment in questioncomments)
                    {
                        if (question.ID == comment.QuestionID)
                        {
                            question.CommentModels.Add(comment);
                        }
                    }
                }

            }
            return questions;
        }


        public void AddQuestion(string title, string text, string image)
        {
            var conn = new NpgsqlConnection(connectingString);
            
            conn.Open();
                
            var command = new NpgsqlCommand($"INSERT INTO question (question_title,question_text,question_image,submission_time, view_number,vote_number,downvote_number, message_number) VALUES ('{title}','{text}','{image}','{DateTime.Now}',0,0,0,0)", conn);
                
            command.ExecuteNonQuery();
            conn.Close();

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
                        var question_messagenummber = Convert.ToInt32(reader["message_number"]);
                        question = new Question(question_id, question_title, question_text, question_image, vote_number,downvote_number,view_number, submission_time,question_messagenummber);
                    }
                }
            }
            return question;
        }

        public QuestionModel GetQuestionModel(int questionId)
        {
            QuestionModel question = new QuestionModel();
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
                        var question_messagenummber = Convert.ToInt32(reader["message_number"]);
                        question = new QuestionModel(question_id, question_title, question_text, question_image, vote_number, downvote_number, view_number, submission_time, question_messagenummber);
                    }
                }
            }
            return question;
        }





        public int AddAnswer(int questionID, string message, string image)
        {
            int res;
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"INSERT INTO answer (question_id,answer_text,answer_image,submission_time, vote_number,downvote_number) VALUES ({questionID},'{message}','{image}','{DateTime.Now}',0,0) RETURNING answer_id", conn);

               
                res = Convert.ToInt32(command.ExecuteScalar());
            }
            return res;
        }


        public void DeleteQuestion(int ID)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"DELETE FROM question WHERE question_id = {ID}", conn);


                command.ExecuteNonQuery();
            }
        }


        public void DeleteAnswer(int ID)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();
               
                var command = new NpgsqlCommand($"DELETE FROM answer WHERE answer_id = {ID}", conn);

                command.ExecuteNonQuery();
            }
        }

        public void PlusNumberOfMessages(int qid)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question SET message_number = message_number + 1 WHERE question_id = {qid}", conn);

                command.ExecuteNonQuery();
            }

        }

        public void MinusNumberOfMessages(int qid)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question SET message_number = message_number - 1 WHERE question_id = {qid}", conn);

                command.ExecuteNonQuery();
            }

        }


        public void EditQuestion(int qid, string title, string text)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question " +
                                                $"SET question_title = '{title}', question_text = '{text}'" +
                                                $"WHERE question_id = {qid}", conn);

                command.ExecuteNonQuery();
            }
        }


        public void Like(int qid)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question SET vote_number = vote_number + 1 WHERE question_id = {qid}", conn);

                command.ExecuteNonQuery();
            }
        }

        public void Dislike(int qid)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question SET downvote_number = downvote_number + 1 WHERE question_id = {qid}", conn);

                command.ExecuteNonQuery();
            }
        }


        public void LikeAnswer(int aid, int qid = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE answer SET vote_number = vote_number + 1 WHERE answer_id = {aid}", conn);

                command.ExecuteNonQuery();
            }

        }

        public void DislikeAnswer(int aid, int qid = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE answer SET downvote_number = downvote_number + 1 WHERE answer_id = {aid}", conn);

                command.ExecuteNonQuery();
            }
        }

        public void EditAnswer(int aid, string text, int qid = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE answer " +
                                                $"SET answer_text = '{text}'" +
                                                $"WHERE answer_id = {aid}", conn);

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

        public AnswerModel GetAnswerModelToQuestion(int aid, int qid = 0)
        {
            AnswerModel answermodel = new AnswerModel();

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
                        answermodel = new AnswerModel(answer_id, question_id, answer_text, answer_image, vote_number, downvote_number, submission_time);
                    }
                }
            }
            return answermodel;
        }

        public Answer_CommentModel GetCommentModelToAnswer(int cid)
        {
            Answer_CommentModel answercommentmodel = new Answer_CommentModel();

            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand($"SELECT * FROM answer_comment WHERE comment_id = {cid}", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var comment_id = Convert.ToInt32(reader["comment_id"]);
                        var answer_id = Convert.ToInt32(reader["answer_id"]);
                        var submission_time = Convert.ToDateTime(reader["submission_time"]);
                        var comment_text = Convert.ToString(reader["comment_text"]);
                        var edited_number = Convert.ToInt32(reader["edited_number"]);
                        answercommentmodel = new Answer_CommentModel(comment_id, comment_text, answer_id, submission_time, edited_number);
                    }
                }
            }
            return answercommentmodel;
        }


        public Question_CommentModel GetCommentModelToQuestion(int cid)
        {
            Question_CommentModel questioncomment = new Question_CommentModel();

            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand($"SELECT * FROM question_comment WHERE comment_id = {cid}", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var comment_id = Convert.ToInt32(reader["comment_id"]);
                        var question_id = Convert.ToInt32(reader["question_id"]);
                        var submission_time = Convert.ToDateTime(reader["submission_time"]);
                        var comment_text = Convert.ToString(reader["comment_text"]);
                        var edited_number = Convert.ToInt32(reader["edited_number"]);
                        questioncomment = new Question_CommentModel(comment_id, comment_text, question_id, submission_time, edited_number);
                    }
                }
            }
            return questioncomment;
        }


        public void AddCommentToQuestion(int questionID, string message)
        {
            using (var conn = new NpgsqlConnection(connectingString))

            {
                conn.Open();

                var command = new NpgsqlCommand($"INSERT INTO question_comment(question_id, comment_text, submission_time,edited_number) VALUES ({questionID},'{message}','{DateTime.Now}',0)", conn);

                command.ExecuteNonQuery();
            }
        }

      


        public void DeleteCommentFromQuestion(int commentID, int questionID = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"DELETE FROM question_comment " +
                                                $"WHERE comment_id = {commentID}", conn);

                command.ExecuteNonQuery();
            }
        }


        public void AddCommentToAnswer(int answerID, string message, int questionID = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"INSERT INTO answer_comment(answer_id, comment_text, submission_time, edited_number) VALUES ({answerID},'{message}','{DateTime.Now}',0)", conn);

                command.ExecuteNonQuery();
            }
        }


        public void DeleteCommentFromAnswer(int commentID, int questionID = 0, int answerID = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"DELETE FROM answer_comment " +
                                                $"WHERE comment_id = {commentID}", conn);

               

                command.ExecuteNonQuery();
            }
        }


        public void DeleteCommentFromAnswerWithAnswerID(int answerID)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"DELETE FROM answer_comment " +
                                                $"WHERE answer_id = {answerID} ", conn);



                command.ExecuteNonQuery();
            }
        }


        public void EditCommentForQuestion(int commentID, string text, int questionID = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question_comment " +
                                                $"SET comment_text = '{text}'" +
                                                $"WHERE comment_id = {commentID}", conn);

                command.ExecuteNonQuery();
            }
        }

        public void EditCommentForAnswer(string text, int commentID, int answerID = 0, int questionID = 0)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE answer_comment " +
                                                $"SET comment_text = '{text}'" +
                                                $"WHERE comment_id = {commentID}", conn);

                command.ExecuteNonQuery();
            }
        }

        public void PlusViewToQuestion(int qid)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question SET view_number = view_number + 1 WHERE question_id = {qid}", conn);

                command.ExecuteNonQuery();
            }

        }

        public void PlusEditedForAnswerComment(int cid)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE answer_comment SET edited_number = edited_number + 1 WHERE comment_id = {cid}", conn);

                command.ExecuteNonQuery();
            }

        }

        public void PlusEditedForQuestionComment(int cid)
        {
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                var command = new NpgsqlCommand($"UPDATE question_comment SET edited_number = edited_number + 1 WHERE comment_id = {cid}", conn);

                command.ExecuteNonQuery();
            }

        }


    }
}
