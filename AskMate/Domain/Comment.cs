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

        public int Edited { get; set; }
   

        public Comment(int id, string text, int QaID)
        {
            ID = id;
            Text = text;
        }
    }
}
