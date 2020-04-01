using System;

namespace AskMate.Models
{
    public class CommentModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
        public int Edited { get; set; }
        public DateTime SubmissionTime { get; set; }

        //Question comment constructor
        public CommentModel(int id, string text, int questionID, DateTime submission)//int edited
        {
            ID = id;
            Text = text;
            //this.Edited = edited;
            QuestionID = questionID;
            SubmissionTime = submission;
        }
        //Answer comment constructor
        public CommentModel(int answerID, int id, string text, DateTime submission)//int edited
        {
            ID = id;
            Text = text;
            //this.Edited = edited;
            AnswerID = answerID;
            SubmissionTime = submission;
        }
        public CommentModel()
        {

        }
    }
}