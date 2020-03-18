﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class Answer
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Imageurl { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }

        public Answer(int id, string text)
        {
            ID = id;
            Text = text;
        }
    }

    
}