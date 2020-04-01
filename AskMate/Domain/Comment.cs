using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class Comment
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
        //public int Edited { get; set; }
   
        //Question comment constructor
        public Comment(int id, string text, int questionID)//int edited
        {
            ID = id;
            Text = text;
            //this.Edited = edited;
            QuestionID = questionID;
        }
        //Answer comment constructor
        public Comment(int answerID, int id, string text)//int edited
        {
            ID = id;
            Text = text;
            //this.Edited = edited;
            AnswerID = answerID;
        }
    }
}
