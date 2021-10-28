using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Labb3.Models;

namespace Labb3.Managers
{
    //Singelton
    public class QuizManager
    {
        public ObservableCollection<Quiz> _allQuizzes = new ObservableCollection<Quiz>();

        

        public ObservableCollection<Quiz> Quizzes
        {
            get { return _allQuizzes; }
            set { _allQuizzes = value; }
        }

        public QuizManager()
        {
        }

        public void CreateNewQuiz(string title, List<Question> questions)
        {
            Quiz newQuiz = new Quiz(title, questions);
            _allQuizzes.Add(newQuiz);
        }
        public void RemoveQuiz(Quiz quiz)
        {
            _allQuizzes.Remove(quiz);
        }

        public static ObservableCollection<string> GetUniqueThemes(Quiz quiz)
        {
            return new ObservableCollection<string>(quiz.Questions.Select(q => q.Theme.ThemeName).Distinct().ToList());
        }

        //public static async Task<ObservableCollection<Quiz>> LoadQuizzes()
        //{
        //    string fileName = "WeatherForecast.json";
        //    using (FileStream openStream = File.OpenRead(Path.Combine(_savePath, fileName)))
        //    {

        //        await JsonSerializer.DeserializeAsync<ObservableCollection<Quiz>>(openStream);

        //        ObservableCollection<Quiz> result = await JsonSerializer.DeserializeAsync<ObservableCollection<Quiz>>(openStream);

        //        return result;
        //    }
        //}
    }
}
