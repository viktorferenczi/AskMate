using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Domain
{
    public interface IDataLoader
    {
        List<Question> GetQuestions();
        Question GetQuestion(int questionId);
        void AddQuestion(string title, string text);
        int CountAnswers(int questionId);
    }
}
