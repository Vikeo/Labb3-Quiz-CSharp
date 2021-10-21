using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Labb3.Managers;
using Labb3.Models;
using Labb3.ViewModels;

namespace Labb3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly Quiz _quiz = new Quiz("Big quiz", new List<Question>());
        private readonly QuizManager _allQuizzes = new QuizManager();

        public App()
        {
            ObservableCollection<Question> questions = new ObservableCollection<Question>();

            questions.Add(new Question("lol1?", "pog", "log", "dog", "Dogs", "dog"));
            questions.Add(new Question("lol2?", "pog", "log", "dog", "Twitch", "pog"));
            questions.Add(new Question("lol3?", "pog", "log", "dog", "Nature", "log"));

            _quiz = new Quiz("Big quiz1", questions);
            _allQuizzes.Quizzes.Add(_quiz);

            Quiz quiz2 = new Quiz("Big quiz2", questions);
            _allQuizzes.Quizzes.Add(quiz2);
    
            Quiz quiz3 = new Quiz("Big quiz3", questions);
            _allQuizzes.Quizzes.Add(quiz3);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //TODO kanske ska instansiera Question och Quiz här, tomma. Och ett ObservableCollection<string> objekt?
            
            //Här ska man instansiera allt som man vill kunna visa. Flera olika view, om de ska leva över applikationens livstid.

            //TODO Är det här rätt? Borde JAG göra såhär?


            var questions = new ObservableCollection<Question>();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_allQuizzes, _quiz)
            };

            MainWindow.Show();

            
        }
    }
}
