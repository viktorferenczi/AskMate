﻿using System;
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
        public string Image { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public int NumOfMessages { get; set; }
        public int NumOfViews { get; set; }
        public DateTime PostedDate { get; set; }

        public int MessageNumber { get; set; }


        



        public Question(int id, string title, string text, string image, DateTime postedDate, int messagenumber)
        {
            ID = id;
            Title = title;
            Text = text;

            MessageNumber = messagenumber;



            if (image != null)
            {
                Image = image;
            }
            PostedDate = postedDate;
        }
        public Question(int id, string title, string text, string image, int like, int dislike, int numOfViews, DateTime posted, int messagenumber)
        {
            ID = id;
            Title = title;
            Text = text;
            MessageNumber = messagenumber;
            if (image != null)
            {
                Image = image;
            }
            Like = like;
            Dislike = dislike;
            NumOfViews = numOfViews;
            PostedDate = posted;
        }
        public Question()
        {

        }
    }
}
