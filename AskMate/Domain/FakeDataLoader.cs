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
            int nexdId;
            if (ListOfQuestions.Count == 0)
            {
                nexdId = 0;
            }
            else
            {
                nexdId = ListOfQuestions.Select(q => q.ID).Max() + 1;
            }
            ListOfQuestions.Add(new Question(title,text));
            return nexdId;

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

    }
}
