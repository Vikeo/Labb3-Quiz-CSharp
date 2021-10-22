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
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //TODO kanske ska instansiera Question och Quiz här, tomma. Och ett ObservableCollection<string> objekt?
            
            //Här ska man instansiera allt som man vill kunna visa. Flera olika view, om de ska leva över applikationens livstid.

            //TODO Är det här rätt? Borde JAG göra såhär?


            var questions = new ObservableCollection<Question>();

            //TODO Behöver nog inte ta in data in i MainViewModel.
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_allQuizzes, _quiz)
            };

            MainWindow.Show();

            
        }
    }
}
