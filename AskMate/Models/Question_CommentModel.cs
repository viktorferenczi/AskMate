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

        //Question comment constructor
        public Question_CommentModel(int id, string text, int questionID, DateTime submission)//int edited
        {
            ID = id;
            Text = text;
            //this.Edited = edited;
            QuestionID = questionID;
            SubmissionTime = submission;
        }
        //Answer comment constructor
        public Question_CommentModel(int answerID, int id, string text, DateTime submission)//int edited
        {
            ID = id;
            Text = text;
            //this.Edited = edited;
            SubmissionTime = submission;
        }
        public Question_CommentModel()
        {

        }
    }
}