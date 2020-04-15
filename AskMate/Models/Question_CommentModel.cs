using System;

namespace AskMate.Models
{
    public class Question_CommentModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public int QuestionID { get; set; }
        public int Edited { get; set; }
        public DateTime SubmissionTime { get; set; }
        public int UserID { get; set; }

        public Question_CommentModel(int id, string text, int questionID, DateTime submission, int edited, int UserID)
        {
            ID = id;
            Text = text;
            Edited = edited;
            QuestionID = questionID;
            SubmissionTime = submission;
            this.UserID = UserID;

        }

        public Question_CommentModel()
        {

        }
    }
}