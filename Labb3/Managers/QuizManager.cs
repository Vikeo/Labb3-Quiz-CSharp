using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Labb3.Models;

namespace Labb3.Managers
{
    //Singelton
    public class QuizManager
    {
        public ObservableCollection<Quiz> AllQuizzes = new ObservableCollection<Quiz>();
        public ObservableCollection<Quiz> Quizzes
        {
            get => AllQuizzes;
            set => AllQuizzes = value;
        }

        [JsonConstructor]
        public QuizManager()
        {
        }

        public void CreateNewQuiz(string title, List<Question> questions)
        {
            Quiz newQuiz = new Quiz(title, questions);
            AllQuizzes.Add(newQuiz);
        }
        public void RemoveQuiz(Quiz quiz)
        {
            AllQuizzes.Remove(quiz);
        }

        public ObservableCollection<string> GetUniqueThemes(Quiz quiz)
        {
            return new ObservableCollection<string>(quiz.Questions.Select(q => q.Theme.ThemeName).Distinct().ToList());
        }
    }
}
