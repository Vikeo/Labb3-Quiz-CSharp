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

        public string _savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public string _fileName = "QuizGameQuizList.json";

        public ObservableCollection<Quiz> Quizzes
        {
            get { return _allQuizzes; }
            set { _allQuizzes = value; }
        }

        public QuizManager()
        {
        }

        //TODO Måste jag returnera Task? Går bra med void????
        //Ska Serialize vara SerializeAsync
        public async Task SaveQuizzes(ObservableCollection<Quiz> allQuizzes)
        {
            //ASYNC
            //using FileStream createStream = File.Create(_fileName);
            //await JsonSerializer.SerializeAsync(createStream, allQuizzes);
            //await createStream.DisposeAsync();

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_savePath, _fileName)))
            {
                await outputFile.WriteAsync(JsonSerializer.Serialize(_allQuizzes));
            }
        }

        //TODO Fixa Async!
        public ObservableCollection<Quiz> LoadQuizzes()
        {
            using (var sr = new StreamReader(Path.Combine(_savePath, _fileName)))
            {
                var text = sr.ReadToEnd();

                ObservableCollection<Quiz> quizList = JsonSerializer.Deserialize<ObservableCollection<Quiz>>(text);
                return quizList;
            }
        }

        public static ObservableCollection<string> GetUniqueThemes(Quiz quiz)
        {
            return new ObservableCollection<string>(quiz.Questions.Select(q => q.Theme).Distinct().ToList());
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
