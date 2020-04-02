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

      
        public Answer_CommentModel(int commmentid, string text, int answerID,  DateTime submission, int Edited)
        {
            ID = commmentid;
            Text = text;
            AnswerID = answerID; 
            SubmissionTime = submission;
            this.Edited = Edited; 
        }

    
        public Answer_CommentModel()
        {

        }

    }


}
