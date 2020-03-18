using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public class FakeDataLoader : IDataLoader
    {
        private List<Question> ListOfQuestions = new List<Question>();

        public int AddQuestion(string title, string text)
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
            ListOfQuestions.Add(new Question(nextID,title, text));
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
                    q.ListOfAnswers.Add(new Answer(nextID, message));
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
    }
}
