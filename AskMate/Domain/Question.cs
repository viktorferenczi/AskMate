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
        public List<Answer> ListOfAnswers { get; set; }
        //public string Image { get; set; }

        public Question(int id, string title, string text)
        {
            ID = id;
            Title = title;
            Text = text;
        }
    }
}
