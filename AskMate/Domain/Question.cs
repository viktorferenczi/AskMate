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
        public string Image { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public int NumOfMessages { get; set; }
        public int NumOfViews { get; set; }
        public DateTime PostedDate { get; set; }

        public Question(int id, string title, string text, string image )
        {
            ID = id;
            Title = title;
            Text = text;
            ListOfAnswers = new List<Answer>();

            if (image != null)
            {
                Image = image;
            }
           
        }
        public Question()
        {

        }
    }
}
