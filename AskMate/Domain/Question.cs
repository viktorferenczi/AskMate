using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class Question
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<Answer> ListOfAnswers;
        public List<Comment> ListOfComments;
        public string Image { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public int NumOfMessages { get; set; }
        public int NumOfViews { get; set; }
        public DateTime PostedDate { get; set; }


        



        public Question(int id, string title, string text, string image, DateTime postedDate)
        {
            ID = id;
            Title = title;
            Text = text;
            ListOfAnswers = new List<Answer>();

            ListOfComments = new List<Comment>();

            if (image != null)
            {
                Image = image;
            }
            PostedDate = postedDate;
        }
        public Question(int id, string title, string text, string image, int like, int dislike, int numOfMessages, int numOfViews)
        {
            ID = id;
            Title = title;
            Text = text;
            ListOfAnswers = new List<Answer>();
            ListOfComments = new List<Comment>();
            if (image != null)
            {
                Image = image;
            }
            Like = like;
            Dislike = dislike;
            NumOfMessages = numOfMessages;
            NumOfViews = numOfViews;
        }
        public Question()
        {

        }
    }
}
