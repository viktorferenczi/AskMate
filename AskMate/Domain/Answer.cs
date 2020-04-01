using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class Answer
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public DateTime PostedDate { get; set; }
        public int QuestionID { get; set; }

        public Answer(int id, int qid, string message, string image, int vote, int downvote, DateTime date )
        {
            ID = id;
            Text = message; 
            Image = image;
            UpVotes = vote;
            DownVotes = downvote;
            PostedDate = date;
            QuestionID = qid;

        }

        public Answer(int id, string text, int QaID, string image, DateTime postedDate)
        {
            ID = id;
            Text = text;

           
            if (image != null)
            {
                Image = image;
            }
            PostedDate = postedDate;
        }

        public Answer(int id, string text, string image, int upVotes, int downVotes)
        {
            ID = id;
            Text = text;
         
            if (image != null)
            {
                Image = image;
            }
            UpVotes = upVotes;
            DownVotes = downVotes;

        }
        public Answer()
        {

        }
    }

    
}
