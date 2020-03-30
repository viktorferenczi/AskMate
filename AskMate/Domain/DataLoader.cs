using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class DataLoader : IDataLoader
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
            ListOfQuestions.Add(new Question(nextID,title, text, image, DateTime.Now));
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



        public int AddAnswer(int questionID,string message,string image)
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
                    nextID = q.ListOfAnswers.Select(aq => q.ID).Max() + 1;
                }
            }


            foreach (var q in ListOfQuestions)
            {
                if (q.ID.Equals(questionID))
                {
                    q.ListOfAnswers.Add(new Answer(nextID, message,questionID,image,DateTime.Now));
                    q.NumOfMessages++;
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

        public void DeleteAnswer(int ID)
        {

            foreach (var q in ListOfQuestions)
            {
                foreach (var answer in q.ListOfAnswers)
                {
                    if (answer.ID == ID)
                    {
                        q.ListOfAnswers.Remove(answer);
                        q.NumOfMessages--;
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


        public void EditAnswer(int qid,  int aid, string text)
        {
            foreach (var q in ListOfQuestions)
            {
                if(qid == q.ID)
                {
                    foreach (var ans in q.ListOfAnswers)
                    {
                        if (ans.ID == aid)
                        {
                            if (text != ans.Text & text != null)
                            {
                                ans.Text = text;
                            }
                        }
                    }
                }
            }
        }

        public Answer GetAnswerToQuestion(int qid, int aid)
        {
            Answer ansz = new Answer();
            foreach (var q in ListOfQuestions)
            {
                if(qid == q.ID)
                {
                    foreach (var ans in q.ListOfAnswers)
                    {
                        if (ans.ID == aid)
                            ansz = ans;
                    }
                }
            }
            return ansz;
        }


        //public void WriteQuestionToCSV()
        //{
        //    string questionDatabase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QuestionDatabase.csv");
        //    using (var w = new StreamWriter(questionDatabase))
        //    {
        //        for (int i = 0; i < ListOfQuestions.Count; i++)
        //        {
        //            var q = ListOfQuestions[i];
        //            if (string.IsNullOrEmpty(q.Image))
        //            {
        //                q.Image = "";
        //            }

        //            w.WriteLine($"{q.ID},{q.Title},{q.Text},{q.Image},{q.Like},{q.Dislike},{q.NumOfMessages},{q.NumOfViews},{q.PostedDate}");
        //            w.Flush();
        //        }
        //    }
        //}

        //public void WriteAnswerToCSV()
        //{
        //    string questionDatabase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AnswerDatabase.csv");
        //    using (var w = new StreamWriter(questionDatabase))
        //    {
        //        foreach (var q in ListOfQuestions)
        //        {
        //            foreach (var a in q.ListOfAnswers)
        //            {
        //                if(string.IsNullOrEmpty(a.Image))
        //                {
        //                    a.Image = "";
        //                }
        //                w.WriteLine($"{q.ID},{a.ID},{a.Text},{a.Image},{a.UpVotes},{a.DownVotes},{a.PostedDate}");
        //                w.Flush();
        //            }

        //        }
        //    }
        //}


        //public List<Question> LoadQuestion()
        //{
        //    string questionDatabase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QuestionDatabase.csv");
        //    string answerDatabase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AnswerDatabase.csv");
        //    string[] lines = File.ReadAllLines(questionDatabase);
        //    string[] anslines = File.ReadAllLines(answerDatabase);
        //    Regex CSVParser = new Regex(",");
        //    Regex AnsParser = new Regex(",");
        //    int rowcount = 0;
        //    foreach (var row in lines)
        //    {

        //        Question q = new Question();
        //        String[] Fields = CSVParser.Split(row);
        //        for (int i = 0; i < Fields.Length; i++)
        //        {
        //            Fields[i] = Fields[i].TrimStart(' ', '"');
        //            Fields[i] = Fields[i].TrimEnd('"');
        //        }
        //        q.ID = int.Parse(Fields[0]);
        //        q.Title = Fields[1];
        //        q.Text = Fields[2];
        //        if(Fields[3]!= null)
        //        {
        //            q.Image = Fields[3];
        //        }
        //        else
        //        {
        //            q.Image = "";
        //        }

        //        q.Like = int.Parse(Fields[4]);
        //        q.Dislike = int.Parse(Fields[5]);
        //        q.NumOfMessages = int.Parse(Fields[6]);
        //        q.NumOfViews = int.Parse(Fields[7]);
        //        q.PostedDate = DateTime.Parse(Fields[8]);
        //        ListOfQuestions.Add(q);
        //        ListOfQuestions[rowcount].ListOfAnswers = new List<Answer>();
        //        foreach (var ansrow in anslines)
        //        {
        //            Answer ans = new Answer();
        //            String[] ansFields = AnsParser.Split(ansrow);
        //            for (int j = 0; j < ansFields.Length; j++)
        //            {
        //                ansFields[j] = ansFields[j].TrimStart(' ', '"');
        //                ansFields[j] = ansFields[j].TrimEnd('"');
        //            }
        //            if (int.Parse(ansFields[0]) == q.ID)
        //            {
        //                ans.ID = int.Parse(ansFields[1]);
        //                ans.Text = ansFields[2];
        //                if (Fields[3] != null)
        //                {
        //                    ans.Image = ansFields[3];
        //                }
        //                else
        //                {
        //                    ans.Image = "";
        //                }
        //                ans.UpVotes = int.Parse(ansFields[4]);
        //                ans.DownVotes = int.Parse(ansFields[5]);
        //                ans.PostedDate = DateTime.Parse(ansFields[6]);
        //                ListOfQuestions[rowcount].ListOfAnswers.Add(ans);
        //            }
        //        }
        //       rowcount++;
        //    }
        //    return ListOfQuestions;
        //}


        public int AddCommentToQuestion(int questionID, string message)
        {
            int nextID = 0;
            foreach (var q in ListOfQuestions)
            {
                if (q.ListOfComments.Count == 0)
                {
                    nextID = 1;
                }
                else
                {
                    nextID = q.ListOfComments.Select(aq => q.ID).Max() + 1;
                }
            }


            foreach (var q in ListOfQuestions)
            {
                if (q.ID.Equals(questionID))
                {
                    q.ListOfComments.Add(new Comment(nextID, message, questionID));
                    return nextID;
                }
            }
            return nextID;
        }

        public void DeleteCommentFromQuestion(int ID, int commentID)
        {

            foreach (var q in ListOfQuestions)
            {
                if (q.ID.Equals(ID))
                {
                    foreach (var comment in q.ListOfComments)
                    {
                        if (comment.ID == commentID)
                        {
                            q.ListOfComments.Remove(comment);
                            return;
                        }
                    }
                }
            }
        }


        public int AddCommentToAnswer(int questionID, int answerID, string message)
        {
            int nextID = 0;
            foreach (var q in ListOfQuestions)
            {
                if (q.ListOfComments.Count == 0)
                {
                    nextID = 1;
                }
                else
                {
                    nextID = q.ListOfComments.Select(aq => q.ID).Max() + 1;
                }
            }

            foreach (var q in ListOfQuestions)
            {
                if (q.ID.Equals(questionID))
                {
                    foreach (var answer in q.ListOfAnswers)
                    {
                        if (answer.ID.Equals(answerID))
                        {
                            answer.ListOfComments.Add(new Comment(nextID, message, questionID));
                            return nextID;
                        }
                    }
                }
            }
            return nextID;
        }


        public void DeleteCommentFromAnswer(int questionID,int answerID, int commentID)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID.Equals(questionID))
                {
                    foreach (var answer in q.ListOfAnswers)
                    {
                        if (answer.ID.Equals(answerID))
                        {
                            foreach (var comment in answer.ListOfComments)
                            {
                                if (comment.ID.Equals(commentID))
                                {
                                    answer.ListOfComments.Remove(comment);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
          
        }


        public void EditCommentForQuestion(int qid, int commentID, string text)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID == qid)
                {
                    foreach (var comment in q.ListOfComments)
                    {
                        if (comment.ID.Equals(commentID))
                        {
                            comment.Text = text;
                        }
                    }
                }
            }
        }

        public void EditCommentForAnswer(int qid, int commentID, int answerID, string text)
        {
            foreach (var q in ListOfQuestions)
            {
                if (q.ID == qid)
                {
                    foreach (var answer in q.ListOfAnswers)
                    {
                        if (answer.ID.Equals(answerID))
                        {
                            foreach (var comment in answer.ListOfComments)
                            {
                                if (comment.ID.Equals(commentID))
                                {
                                    comment.Text = text;
                                }
                            }
                        }
                    }
                }
            }
        }


    }
}
