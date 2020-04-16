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
            UserQuestions = new List<QuestionModel>();
            UserAnswers = new List<AnswerModel>();
            UserQuestionComments = new List<Question_CommentModel>();
            UserAnswerComments = new List<Answer_CommentModel>();

        }

        public UserModel() { }

        public string ToHiddenPassword(string password)
        {
           string HiddenPassword = "";
            foreach (var c in password)
            {
                HiddenPassword += "*";
            }
            return HiddenPassword;
        }

        public string ToHiddenEmail(string email)
        {
            bool isDomain = true;
            string resultEmail = "";
            for (int i = 0; i < email.Length; i++)
            {
                if (i == 0)
                {
                    resultEmail += email[i];
                }
                if (email[i] == '@')
                {
                    isDomain = false;
                }
                if (isDomain)
                {
                    resultEmail += '*';
                }
                else
                {
                    resultEmail += email[i];
                }
            }

            return resultEmail;

        }
    }
}
