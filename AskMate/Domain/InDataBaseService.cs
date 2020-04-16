using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AskMate.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Npgsql;

namespace AskMate.Domain
{
    public class InDataBaseService : IUserService
    {
        private List<User> _users = new List<User>();

        private static readonly string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string dbPass = Environment.GetEnvironmentVariable("DB_PASS");
        private static readonly string dbName = Environment.GetEnvironmentVariable("DB_NAME");
        public static readonly string connectingString = $"Host={dbHost};Username={dbUser};Password={dbPass};Database={dbName}";


       

        public List<UserModel> GetAll()
        {
    
            List<UserModel> users = new List<UserModel>();
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
                        var user_id = Convert.ToInt32(reader["userid"]);

                        questionModel = new QuestionModel(question_id, question_title, question_text, question_image, vote_number, downvote_number, view_number, submission_time, question_messagenumber, user_id);
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
                        var user_id = Convert.ToInt32(readerans["userid"]);
                        answer = new AnswerModel(answer_id, aquestion_id, answer_text, answer_image, avote_number, adownvote_number, asubmission_time, user_id);
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
                        var user_id = Convert.ToInt32(readercomm["userid"]);
                        var comment_edited = Convert.ToInt32(readercomm["edited_number"]);

                        comment = new Question_CommentModel(comment_id, comment_text, cquestion_id, csubmission_time, comment_edited, user_id);
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
                        var user_id = Convert.ToInt32(readercomm["userid"]);
                        var comment_edited = Convert.ToInt32(readercomm["edited_number"]);

                        comment = new Answer_CommentModel(comment_id, comment_text, canswer_id, csubmission_time, comment_edited, user_id);
                        answercomments.Add(comment);
                    }
                }

                conn.Close();

                conn.Open();
                using (var command = new NpgsqlCommand($"SELECT * FROM users", conn))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UserModel user = new UserModel();
                        var user_id = Convert.ToInt32(reader["userid"]);
                        var user_email = Convert.ToString(reader["user_email"]);
                        var user_pass = Convert.ToString(reader["user_password"]);

                        user = new UserModel(user_id, user_email, user_pass);
                        users.Add(user);
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


                foreach (var user in users)
                {
                    foreach (var question in questions)
                    {
                        if (question.UserID == user.Id)
                        {
                            user.UserQuestions.Add(question);
                        }
                    }
                }


                foreach (var user in users)
                {
                    foreach (var answer in answers)
                    {
                        if (answer.UserID == user.Id)
                        {
                            user.UserAnswers.Add(answer);
                        }
                    }
                }

                foreach (var user in users)
                {
                    foreach (var answercomment in answercomments)
                    {
                        if (answercomment.UserID == user.Id)
                        {
                            user.UserAnswerComments.Add(answercomment);
                        }
                    }
                }

                foreach (var user in users)
                {
                    foreach (var questioncomment in questioncomments)
                    {
                        if (questioncomment.UserID == user.Id)
                        {
                            user.UserQuestionComments.Add(questioncomment);
                        }
                    }
                }




            }


            return users; 
        }


        public UserModel GetOne(string email,string password)
        {
            
            UserModel user = new UserModel();
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM users WHERE user_email = @useremail AND user_password = @userpassword", conn))
                {
                    command.Parameters.AddWithValue("useremail", email);
                    command.Parameters.AddWithValue("userpassword", password);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                       
                        var user_id = Convert.ToInt32(reader["userid"]);
                        var user_email = Convert.ToString(reader["user_email"]);
                        var user_pass = Convert.ToString(reader["user_password"]);
                        user = new UserModel(user_id, user_email, user_pass);
                    }
                }
            }
            return user;
           
        }

        public UserModel GetOne(string email)
        {

            UserModel user = new UserModel();
            using (var conn = new NpgsqlConnection(connectingString))
            {
                conn.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM users WHERE user_email = @useremail", conn))
                {
                    command.Parameters.AddWithValue("useremail", email);
                   
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        var user_id = Convert.ToInt32(reader["userid"]);
                        var user_email = Convert.ToString(reader["user_email"]);
                        var user_pass = Convert.ToString(reader["user_password"]);
                        user = new UserModel(user_id, user_email, user_pass);
                    }
                }
            }
            return user;

        }

        public UserModel Login(string email, string password)
        {
            var user = GetOne(email, password);
            if(user == null)
            {
                return null;
            }
          
            return user;
        }

        public void RegisterIntoDatabase(string email, string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
             password: password,
             salt: salt,
             prf: KeyDerivationPrf.HMACSHA1,
             iterationCount: 10000,
             numBytesRequested: 256 / 8));

            var conn = new NpgsqlConnection(connectingString);

            conn.Open();

            var command = new NpgsqlCommand($"INSERT INTO users (user_email,user_password) VALUES (@email,@pass)", conn);
            command.Parameters.AddWithValue("email", email);
            command.Parameters.AddWithValue("pass", password);
            command.ExecuteNonQuery();
            conn.Close();

        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public User Register(string email, string password)
        {
            throw new NotImplementedException();
        }

        List<User> IUserService.GetAll()
        {
            throw new NotImplementedException();
        }

        User IUserService.GetOne(string email)
        {
            throw new NotImplementedException();
        }

        User IUserService.Login(string email, string password)
        {
            throw new NotImplementedException();
        }
    }

}
