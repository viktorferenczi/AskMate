using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class FakeDataLoader : IDataLoader
    {
        private List<Question> ListOfQuestions = new List<Question>();

        public int AddQuestion(string title, string text, string image)
        {
            int nextID;
            if (ListOfQuestions.Count == 0)
            {
                nextID = 1;
            }
            else
            {
                nextID = ListOfQuestions.Select(q => q.ID).Max() + 1;
            }
            ListOfQuestions.Add(new Question(nextID,title, text, image));
            return nextID;
        }

        public int CountAnswers(int questionId)
        {
            foreach (var question in ListOfQuestions)
            {
                if(question.ID.Equals(questionId))
                {
                    return question.ListOfAnswers.Count;
                }
            }
            return 0;
        }

        public Question GetQuestion(int questionId)
        {
            foreach (var question in ListOfQuestions)
            {
                if (question.ID.Equals(questionId))
                {
                    return question;
                }
            }
            return null;
        }

        public List<Question> GetQuestions()
        {
            return ListOfQuestions;
        }



        public int AddComment(int questionID,string message)
        {
            int nextID = 0;
            foreach (var q in ListOfQuestions)
            {
                if (q.ListOfAnswers.Count == 0)
                {
                    nextID = 1;
                }
                else
                {
                    nextID = q.ListOfAnswers.Select(q => q.ID).Max() + 1;
                }
            }


            foreach (var q in ListOfQuestions)
            {
                if (q.ID.Equals(questionID))
                {
                    q.ListOfAnswers.Add(new Answer(nextID, message,questionID));
                    return nextID;
                }
            }
            throw new Exception("There is no such ID");
        }

        public void DeleteQuestion(int ID)
        {
            for (int i = 0; i < ListOfQuestions.Count; i++)
            {
                if (ListOfQuestions[i].ID == ID)
                {
                    ListOfQuestions.Remove(ListOfQuestions[i]);
                }
            }
           
        }

        public void DeleteComment(int ID)
        {

            foreach (var q in ListOfQuestions)
            {
                foreach (var answer in q.ListOfAnswers)
                {
                    if (answer.ID == ID)
                    {
                        q.ListOfAnswers.Remove(answer);
                        return;
                    }
                }
            }
           
        }

        public void EditQuestion(int qid,string title, string text)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID == qid)
                {
                    if (title != q.Title & title != null)
                    {
                        q.Title = title;
                    }

                    if (text != q.Text & text != null)
                    {
                        q.Text = text;
                    }
                }
            }
        }
        public void Like(int qid)
        {
            foreach (var q in ListOfQuestions)
            {
                if(q.ID == qid)
                {
                    q.Like++;
                }
            }
        }
        public void Dislike(int qid)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID == qid)
                {
                    q.Dislike++;
                }
            }
        }

        public void LikeAnswer(int aid,int qid)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID == qid)
                {
                    foreach (var an in q.ListOfAnswers)
                    {
                        if (aid == an.ID)
                        {
                            an.UpVotes++;
                        }
                    }
                }
            }
        }
        public void DislikeAnswer(int aid,int qid)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID == qid)
                {
                    foreach (var an in q.ListOfAnswers)
                    {
                        if (aid == an.ID)
                        {
                            an.DownVotes++;
                        }
                    }
                }
            }
        }

        public void WriteQuestionToCSV()
        {
            string questionDatabase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QuestionDatabase.csv");
            using (var w = new StreamWriter(questionDatabase))
            {
                for (int i = 0; i < ListOfQuestions.Count; i++)
                {
                    var q = ListOfQuestions[i];
                    w.WriteLine($"{q.ID},{q.Title},{q.Text},{q.Like},{q.Dislike},{q.Image}");
                    w.Flush();
                }
            }
        }

        public void WriteAnswerToCSV()
        {
            string questionDatabase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AnswerDatabase.csv");
            using (var w = new StreamWriter(questionDatabase))
            {
                foreach (var q in ListOfQuestions)
                {
                    w.WriteLine(q.ID);
                    foreach (var a in q.ListOfAnswers)
                    {
                        w.WriteLine($"{a.ID},{a.Text},{a.UpVotes},{a.DownVotes},{a.Imageurl}");
                        w.Flush();
                    }

                }
            }
        }
    }
}
