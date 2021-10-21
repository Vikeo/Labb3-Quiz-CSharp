using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Models;

namespace Labb3.Managers
{
    public class QuizManager
    {
        public static ObservableCollection<Quiz> _allQuizzes = new ObservableCollection<Quiz>();

        public ObservableCollection<Quiz> Quizzes
        {
            get { return _allQuizzes; }
            set { _allQuizzes = value; }
        }

        public QuizManager()
        {
        }
    }
}
