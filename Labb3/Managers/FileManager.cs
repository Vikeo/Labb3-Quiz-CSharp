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
    public class FileManager
    {
        public string _savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public string _fileName = "QuizGameQuizList.json";

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
                await outputFile.WriteAsync(JsonSerializer.Serialize(allQuizzes));
            }
        }

        //TODO Fixa Async plz!
        public ObservableCollection<Quiz> LoadQuizzes()
        {
            using (var sr = new StreamReader(Path.Combine(_savePath, _fileName)))
            {
                var text = sr.ReadToEnd();

                ObservableCollection<Quiz> quizList = JsonSerializer.Deserialize<ObservableCollection<Quiz>>(text);
                return quizList;
            }
        }
    }
}
