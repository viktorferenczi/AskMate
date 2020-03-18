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
        int AddQuestion(string title, string text,string image);
        int CountAnswers(int questionId);

        int AddComment(int questionId, string message);

    }
}
