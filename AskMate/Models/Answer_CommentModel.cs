using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Models
{
    public class Answer_CommentModel
    {

        public int ID { get; set; }
        public string Text { get; set; }
        public int AnswerID { get; set; }
        public int Edited { get; set; }
        public DateTime SubmissionTime { get; set; }

        //Question comment constructor
        public Answer_CommentModel(int id, string text, int questionID, DateTime submission)//int edited
        {
            ID = id;
            Text = text;
            //this.Edited = edited
            SubmissionTime = submission;
        }
        //Answer comment constructor
        public Answer_CommentModel(int answerID, int id, string text, DateTime submission)//int edited
        {
            ID = id;
            Text = text;
            //this.Edited = edited;
            AnswerID = answerID;
            SubmissionTime = submission;
        }
        public Answer_CommentModel()
        {

        }

    }


}
