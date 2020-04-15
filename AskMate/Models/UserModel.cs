using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<QuestionModel> UserQuestions;
        public List<AnswerModel> UserAnswers;
        public List<Question_CommentModel> UserQuestionComments;
        public List<Answer_CommentModel> UserAnswerComments;

        public UserModel(int id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public UserModel() { }
    }
}
