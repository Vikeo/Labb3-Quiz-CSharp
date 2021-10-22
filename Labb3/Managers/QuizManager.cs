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
        public static ObservableCollection<Quiz> _allQuizzes = new ObservableCollection<Quiz>();

        public static string _savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string _fileName = "QuizGameQuizList.json";

        public ObservableCollection<Quiz> Quizzes
        {
            get { return _allQuizzes; }
            set { _allQuizzes = value; }
        }

        public QuizManager()
        {
        }

        public static async void SaveQuizzes(ObservableCollection<Quiz> allQuizzes)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_savePath, _fileName)))
            {
                await outputFile.WriteAsync(JsonSerializer.Serialize(_allQuizzes));
            }
        }

        public List<Quiz> LoadQuizzes()
        {
            using (var sr = new StreamReader(Path.Combine(_savePath, _fileName)))
            {
                var text = sr.ReadToEnd();
                List<Quiz> quizList = JsonSerializer.Deserialize<List<Quiz>>(text);
                return quizList;
            }
        }
    }
}
