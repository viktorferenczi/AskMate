﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class DataBaseLoader : IDataLoader
    {

        private List<Question> ListOfQuestions = new List<Question>();
        private static readonly string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string dbPass = Environment.GetEnvironmentVariable("DB_PASS");
        private static readonly string dbName = Environment.GetEnvironmentVariable("DB_NAME");
        public static readonly string connectingString = $"Host={dbHost};Username={dbUser};Password={dbPass};Database={dbName}";

       
        // --------------------------------------------------------------------------------- 1

        public void AddQuestion(string title, string text, string image)
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
            
           // return nextID;

            using (var conn = new NpgsqlConnection(connectingString))

            {
                conn.Open();
                
                var command = new NpgsqlCommand($"INSERT INTO question (question_title,question_text,question_image,submission_time) VALUES ({title},{text},{image},{DateTime.Now})", conn);
                
                command.ExecuteNonQuery();
            }
            //ListOfQuestions.Add(new Question(nextID, title, text, image, DateTime.Now));
        }

        public int CountAnswers(int questionId)
        {
            int counter = 0;

            using (var conn = new NpgsqlConnection(connectingString))

            {
                conn.Open();

                using (var command = new NpgsqlCommand($"SELECT COUNT(answer_id) as f FROM answer WHERE question_id = {questionId}", conn)) 
                {
                    var reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        var fasz = reader["f"];
                        counter = Convert.ToInt32(fasz);
                        return counter;
                    }
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

        public int AddAnswer(int questionID, string message, string image)
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
                    q.ListOfAnswers.Add(new Answer(nextID, message, questionID, image, DateTime.Now));
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

        // --------------------------------------------------------------------------------- 2




        public void EditQuestion(int qid, string title, string text)
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
                if (q.ID == qid)
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

        public void LikeAnswer(int aid, int qid)
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

        public void DislikeAnswer(int aid, int qid)
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

        public void EditAnswer(int qid, int aid, string text)
        {
            foreach (var q in ListOfQuestions)
            {
                if (qid == q.ID)
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


            foreach (var q in ListOfQuestions)
            {
                if (qid == q.ID)
                {
                    foreach (var ans in q.ListOfAnswers)
                    {
                        if (ans.ID == aid)
                        {
                            ans.Question = q;
                            return ans;
                        }

                    }

                }
            }
            return null;

        }


        // --------------------------------------------------------------------------------- 3




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


        public void DeleteCommentFromAnswer(int questionID, int answerID, int commentID)
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
