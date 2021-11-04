using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Labb3.Models;
using Microsoft.Win32;

namespace Labb3.Managers
{
    public class FileManager
    {
        private string _baseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ViktorsQuizGame");
        private string _fileName = "AllQuizzes.json";

        //TODO Gör så det är en mapp i solution istället:
        private string _imageFolderPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ViktorsQuizGame"), "images");
        public string ImageFolderPath
        {
            get { return _imageFolderPath; }
            set { _imageFolderPath = value; }
        }
        public FileManager()
        {
        }

        //TODO Skriv bilder till en mapp i solutionen??.
        public async Task SaveAllQuizzes(ObservableCollection<Quiz> allQuizzes)
        {
            CreateNewDirectorys();
            using FileStream createStream = File.Create(Path.Combine(_baseFolderPath, _fileName));
            JsonSerializerOptions options = new JsonSerializerOptions() {WriteIndented = true};
            await JsonSerializer.SerializeAsync(createStream, allQuizzes, options);
            await createStream.DisposeAsync();
        }

        //TODO Kan inte exportera BILDER (bara ImagePath)
        public async Task ExportQuiz(Quiz quiz)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Export Quiz as .qff";
            saveFileDialog1.DefaultExt = "qff";
            saveFileDialog1.Filter = "qff files (*.qff)|*.qff|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.FileName = $"{quiz.Title}";

            if (saveFileDialog1.ShowDialog() == true)
            {
                //Samma som SaveAllQuizzes
                using FileStream createStream = File.Create(saveFileDialog1.FileName);
                JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
                await JsonSerializer.SerializeAsync(createStream, quiz, options);
                await createStream.DisposeAsync();
            }
        }

        //TODO Gör en check som kollar om alla grejer i konstruktorn kunde fylles i korrekt. Säg till att det inte gick annars och skriv ut hur den ska vara formaterad.
        //TODO Kan inte importera BILDER
        public async Task<Quiz> ImportQuiz(ObservableCollection<Quiz> allQuizzes)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Import '.qff' quiz";
            openFileDialog1.Filter = "qff files (*.qff)|*.qff|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            Quiz importedQuiz = new Quiz();

            if (openFileDialog1.ShowDialog() == true)
            {
                using FileStream openStream = File.OpenRead(openFileDialog1.FileName);

                //Så fort som något Deserializesas så returneras något, men vet inte vad.. 

                importedQuiz = JsonSerializer.DeserializeAsync<Quiz>(openStream).Result;

                //Ser till så att filnamet och quizzens titel är samma.

                importedQuiz.Title = Path.GetFileNameWithoutExtension(openFileDialog1.SafeFileName);

                if (allQuizzes.Any(q => q.Title == Path.GetFileNameWithoutExtension(openFileDialog1.SafeFileName) ||
                                        allQuizzes.Any(q => q.Title == importedQuiz.Title)))
                {
                    MessageBox.Show($"Det finns redan ett quiz med namnet '{Path.GetFileNameWithoutExtension(openFileDialog1.SafeFileName)}', kunde ej importera quiz.", "Quiz finns", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                
                return importedQuiz;
            }
            
            return null;
        }

        public async Task<ObservableCollection<Quiz>> LoadAllQuizzes()
        {
            //Async?!
            if (File.Exists(Path.Combine(_baseFolderPath, _fileName)))
            {
                using FileStream openStream = File.OpenRead(Path.Combine(_baseFolderPath, _fileName));
                ObservableCollection<Quiz> quizList =
                    await JsonSerializer.DeserializeAsync<ObservableCollection<Quiz>>(openStream);
                return quizList;
            }

            return new ObservableCollection<Quiz>();
        }

        public void CreateNewDirectorys()
        {
            Directory.CreateDirectory(Path.Combine(_baseFolderPath, "images"));
        }
    }
}
