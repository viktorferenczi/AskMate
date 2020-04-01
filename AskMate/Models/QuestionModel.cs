using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Models
{
    public class QuestionModel
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
        public List<AnswerModel> AnswerModels;
        public List<CommentModel> CommentModels;





        public QuestionModel(int id, string title, string text, string image, DateTime postedDate)
        {
            ID = id;
            Title = title;
            Text = text;




            if (image != null)
            {
                Image = image;
            }
            PostedDate = postedDate;
        }

        public QuestionModel(int id, string title, string text, string image, int like, int dislike, int numOfViews, DateTime posted)
        {
            ID = id;
            Title = title;
            Text = text;
            AnswerModels = new List<AnswerModel>();
            CommentModels = new List<CommentModel>();

            if (image != null)
            {
                Image = image;
            }
            Like = like;
            Dislike = dislike;
            NumOfViews = numOfViews;
            PostedDate = posted;
        }
        public QuestionModel()
        {

        }
    }
}
