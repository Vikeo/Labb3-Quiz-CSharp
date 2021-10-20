using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Models;

namespace Labb3.Managers
{
    public class QuizManager
    {
        public static List<Quiz> _allQuizzes;

        public List<Quiz> Quizzes
        {
            get { return _allQuizzes; }
            set { _allQuizzes = value; }
        }

        public QuizManager(Quiz quiz)
        {
            _allQuizzes.Add(quiz);
        }
    }
}
