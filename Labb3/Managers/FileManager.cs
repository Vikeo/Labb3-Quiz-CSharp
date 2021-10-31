using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using Labb3.Models;

namespace Labb3.Managers
{
    public class FileManager
    {
        private string _savePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private string _fileName = "QuizGameQuizListAsync.json";

        //TODO NIKLAS: Måste jag returnera Task? Går bra med void????
        public async Task SaveAllQuizzes(ObservableCollection<Quiz> allQuizzes)
        {
            //ASYNC
            using FileStream createStream = File.Create(Path.Combine(_savePath, _fileName));
            await JsonSerializer.SerializeAsync(createStream, allQuizzes);
            await createStream.DisposeAsync();
        }

        
        public async Task<ObservableCollection<Quiz>> LoadAllQuizzes()
        {
            //Async?!
            using FileStream openStream = File.OpenRead(Path.Combine(_savePath, _fileName));
            ObservableCollection<Quiz> quizList =
                await JsonSerializer.DeserializeAsync<ObservableCollection<Quiz>>(openStream);
            return quizList;
        }
    }
}
