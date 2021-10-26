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
using Labb3.Stores;
using Labb3.ViewModels;
using Labb3.Views;

namespace Labb3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly NavigationStore _navigationStore;
        private readonly QuizManager _quizManager;

        //private readonly Quiz _quiz = new Quiz("Big quiz", new List<Question>());
        //private readonly QuizManager _allQuizzes = new QuizManager();

        public App()
        {
            ObservableCollection<Question> questions = new ObservableCollection<Question>();
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore);

            base.OnStartup(e);

            QuizManager._allQuizzes = QuizManager.LoadQuizzes();

            //TODO kanske ska instansiera Question och Quiz här, tomma. Och ett ObservableCollection<string> objekt?
            //Här ska man instansiera allt som man vill kunna visa. Flera olika view, om de ska leva över applikationens livstid.
            //TODO Är det här rätt? Borde JAG göra såhär?

            var questions = new ObservableCollection<Question>();

            //TODO Tog bort in-datan till MainViewModel(_allQuizzes, _quiz). Är det OK utan eller måsta man ha med dem? Tror att det är lugnt utan.
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore, _quizManager)
            };

            MainWindow.Show();

        }
    }
}
