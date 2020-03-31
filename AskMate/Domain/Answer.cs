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
        public List<Comment> ListOfComments;
        public Question Question { get; set; }

        public Answer(int id, string text, int QaID, string image, DateTime postedDate)
        {
            ID = id;
            Text = text;

            ListOfComments = new List<Comment>();
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
            ListOfComments = new List<Comment>();
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
