﻿using System;
using System.Collections.Generic;

namespace AskMate.Models
{
    public class AnswerModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public DateTime PostedDate { get; set; }
        public int QuestionID { get; set; }
        public List<Answer_CommentModel> CommentModels;
        public int UserID { get; set; }

        public AnswerModel(int id, int qid, string message, string image, int vote, int downvote, DateTime date, int UserID)
        {
            ID = id;
            Text = message;
            Image = image;
            UpVotes = vote;
            DownVotes = downvote;
            PostedDate = date;
            QuestionID = qid;
            CommentModels = new List<Answer_CommentModel>();
            this.UserID = UserID;

        }

        public AnswerModel(int id, string text, int QaID, string image, DateTime postedDate, int UserID)
        {
            ID = id;
            Text = text;
            this.UserID = UserID;
            CommentModels = new List<Answer_CommentModel>();

            if (image != null)
            {
                Image = image;
            }
            PostedDate = postedDate;
        }

        public AnswerModel(int id, string text, string image, int upVotes, int downVotes, int UserID)
        {
            ID = id;
            Text = text;
            this.UserID = UserID;

            if (image != null)
            {
                Image = image;
            }
            UpVotes = upVotes;
            DownVotes = downVotes;
            CommentModels = new List<Answer_CommentModel>();
        }
        public AnswerModel()
        {

        }
    }
}